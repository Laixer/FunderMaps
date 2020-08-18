using FunderMaps.Core.Entities;
using FunderMaps.Core.Exceptions;
using FunderMaps.Core.Helpers;
using FunderMaps.Core.Interfaces;
using FunderMaps.Core.Interfaces.Repositories;
using FunderMaps.Core.Types;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace FunderMaps.Core.UseCases
{
    /// <summary>
    ///     Inquiry use case.
    /// </summary>
    public class InquiryUseCase
    {
        private readonly INotificationService _notificationService;
        private readonly IFileStorageService _fileStorageService;
        private readonly IInquiryRepository _inquiryRepository;
        private readonly IInquirySampleRepository _inquirySampleRepository;

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public InquiryUseCase(
            INotificationService notificationService,
            IFileStorageService fileStorageService,
            IInquiryRepository inquiryRepository,
            IInquirySampleRepository inquirySampleRepository)
        {
            _notificationService = notificationService ?? throw new ArgumentNullException(nameof(notificationService));
            _fileStorageService = fileStorageService ?? throw new ArgumentNullException(nameof(fileStorageService));
            _inquiryRepository = inquiryRepository ?? throw new ArgumentNullException(nameof(inquiryRepository));
            _inquirySampleRepository = inquirySampleRepository ?? throw new ArgumentNullException(nameof(inquirySampleRepository));
        }

        #region Inquiry

        /// <summary>
        ///     Get inquiry.
        /// </summary>
        /// <param name="id">Entity id.</param>
        public virtual async ValueTask<Inquiry> GetAsync(int id)
        {
            // FUTURE:
            //inquiry.AttributionNavigation = ...
            return await _inquiryRepository.GetByIdAsync(id);
        }

        // TODO: Remove stream.
        public async ValueTask StoreDocumentAsync(FileWrapper file, Stream stream)
        {
            if (file == null)
            {
                throw new ArgumentNullException(nameof(file));
            }

            await _fileStorageService.StoreFileAsync("somestore", file, stream);
        }

        /// <summary>
        ///     Create new inquiry.
        /// </summary>
        /// <param name="inquiry">Entity object.</param>
        public virtual async ValueTask<Inquiry> CreateAsync(Inquiry inquiry)
        {
            if (inquiry == null)
            {
                throw new ArgumentNullException(nameof(inquiry));
            }

            inquiry.InitializeDefaults();
            inquiry.Attribution = 1; // TODO: Remove
            inquiry.Validate();

            var id = await _inquiryRepository.AddAsync(inquiry);
            return await _inquiryRepository.GetByIdAsync(id);
        }

        /// <summary>
        ///     Retrieve all inquiries.
        /// </summary>
        /// <param name="navigation">Recordset nagivation.</param>
        public virtual IAsyncEnumerable<Inquiry> GetAllAsync(INavigation navigation)
            => _inquiryRepository.ListAllAsync(navigation);

        /// <summary>
        ///     Update inquiry.
        /// </summary>
        /// <param name="inquiry">Entity object.</param>
        public virtual async ValueTask UpdateAsync(Inquiry inquiry)
        {
            if (inquiry == null)
            {
                throw new ArgumentNullException(nameof(inquiry));
            }

            inquiry.InitializeDefaults(await _inquiryRepository.GetByIdAsync(inquiry.Id));
            inquiry.Validate();

            if (!inquiry.AllowWrite)
            {
                throw new EntityReadOnlyException();
            }

            await _inquiryRepository.UpdateAsync(inquiry);
        }

        /// <summary>
        ///     Update inquiry.
        /// </summary>
        /// <param name="id">Entity id.</param>
        /// <param name="status">New entity status.</param>
        /// <param name="message">Status change message.</param>
        public virtual async ValueTask UpdateStatusAsync(int id, AuditStatus status, string message)
        {
            // FUTURE: Abstract this away.
            Inquiry inquiry = await _inquiryRepository.GetByIdAsync(id);

            Func<ValueTask> postUpdateEvent = () => new ValueTask();

            switch (status)
            {
                case AuditStatus.Pending:
                    inquiry.TransitionToPending();
                    break;
                case AuditStatus.Done:
                    inquiry.TransitionToDone();
                    break;
                case AuditStatus.Discarded:
                    inquiry.TransitionToDiscarded();
                    break;
                case AuditStatus.PendingReview:
                    inquiry.TransitionToReview();

                    // TODO: Reviewer receives notification
                    postUpdateEvent = () => _notificationService.NotifyByEmailAsync(new string[] { "info@example.com" }, message);
                    break;
                case AuditStatus.Rejected:
                    inquiry.TransitionToRejected();

                    // TODO: Creator receives notification + message
                    postUpdateEvent = () => _notificationService.NotifyByEmailAsync(new string[] { "info@example.com" }, message);
                    break;
                default:
                    throw new StateTransitionException(inquiry.AuditStatus, status);
            }

            await _inquiryRepository.UpdateAsync(inquiry);
            await postUpdateEvent();
        }

        /// <summary>
        ///     Delete inquiry.
        /// </summary>
        /// <param name="id">Entity id.</param>
        public virtual async ValueTask DeleteAsync(int id)
        {
            await _inquiryRepository.DeleteAsync(id);
        }

        #endregion

        #region Inquiry Sample

        /// <summary>
        ///     Get inquiry sample.
        /// </summary>
        /// <param name="id">Entity sample id.</param>
        public virtual async ValueTask<InquirySample> GetSampleAsync(int id)
        {
            var inquirySample = await _inquirySampleRepository.GetByIdAsync(id);
            inquirySample.InquiryNavigation = await _inquiryRepository.GetByIdAsync(inquirySample.Inquiry);
            return inquirySample;
        }

        /// <summary>
        ///     Create new inquiry sample.
        /// </summary>
        /// <param name="inquirySample">Entity object.</param>
        public virtual async ValueTask<InquirySample> CreateSampleAsync(InquirySample inquirySample)
        {
            if (inquirySample == null)
            {
                throw new ArgumentNullException(nameof(inquirySample));
            }

            inquirySample.InitializeDefaults();
            inquirySample.Validate();

            // FUTURE: Too much logic
            var inquiry = await _inquiryRepository.GetByIdAsync(inquirySample.Inquiry);

            if (!inquiry.AllowWrite)
            {
                throw new EntityReadOnlyException();
            }

            var id = await _inquirySampleRepository.AddAsync(inquirySample);
            inquirySample = await _inquirySampleRepository.GetByIdAsync(id);

            inquiry.TransitionToPending();
            await _inquiryRepository.UpdateAsync(inquiry);

            inquirySample.InquiryNavigation = inquiry;
            return inquirySample;
        }

        /// <summary>
        ///     Retrieve all inquiry samples.
        /// </summary>
        /// <param name="navigation">Recordset nagivation.</param>
        public virtual async IAsyncEnumerable<InquirySample> GetAllSampleAsync(INavigation navigation)
        {
            await foreach (var inquirySample in _inquirySampleRepository.ListAllAsync(navigation))
            {
                // FUTURE: This is working, but not efficient
                inquirySample.InquiryNavigation = await _inquiryRepository.GetByIdAsync(inquirySample.Inquiry);
                yield return inquirySample;
            }
        }

        /// <summary>
        ///     Delete inquiry sample.
        /// </summary>
        /// <param name="id">Entity id.</param>
        public virtual async ValueTask DeleteSampleAsync(int id)
        {
            // FUTURE: Too much logic
            var inquirySample = await _inquirySampleRepository.GetByIdAsync(id);
            var inquiry = await _inquiryRepository.GetByIdAsync(inquirySample.Inquiry);

            if (!inquiry.AllowWrite)
            {
                throw new EntityReadOnlyException();
            }

            await _inquirySampleRepository.DeleteAsync(id);
            var itemCount = await _inquiryRepository.CountAsync(); // FUTURE: Should only select inquiry
            if (itemCount == 0)
            {
                inquiry.TransitionToTodo();
                await _inquiryRepository.UpdateAsync(inquiry);
            }
        }

        /// <summary>
        ///     Update inquiry sample.
        /// </summary>
        /// <param name="inquirySample">Entity object.</param>
        public virtual async ValueTask UpdateSampleAsync(InquirySample inquirySample)
        {
            if (inquirySample == null)
            {
                throw new ArgumentNullException(nameof(inquirySample));
            }

            inquirySample.BaseMeasurementLevel = BaseMeasurementLevel.NAP;
            inquirySample.Validate();

            // FUTURE: Too much logic
            var inquiry = await _inquiryRepository.GetByIdAsync(inquirySample.Inquiry);

            if (!inquiry.AllowWrite)
            {
                throw new EntityReadOnlyException();
            }

            await _inquirySampleRepository.UpdateAsync(inquirySample);

            inquiry.TransitionToPending();
            await _inquiryRepository.UpdateAsync(inquiry);
        }

        #endregion
    }
}
