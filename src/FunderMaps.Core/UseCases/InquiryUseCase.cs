using AutoMapper;
using FunderMaps.Core.Entities;
using FunderMaps.Core.Exceptions;
using FunderMaps.Core.Interfaces;
using FunderMaps.Core.Interfaces.Repositories;
using FunderMaps.Core.Types;
using FunderMaps.Core.Types.Control;
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
            var config = new MapperConfiguration(cfg => cfg.CreateMap<InquiryFull, Inquiry>());
            var mapper = config.CreateMapper();

            return mapper.Map<Inquiry>(await _inquiryRepository.GetByIdAsync(id));
        }

        /// <summary>
        ///     Get inquiry state.
        /// </summary>
        /// <param name="id">Entity id.</param>
        public virtual async ValueTask<AuditStatus> GetStateAsync(int id)
        {
            var inquiry = await _inquiryRepository.GetByIdAsync(id);
            return inquiry.State.AuditStatus;
        }

        /// <summary>
        ///     Get inquiry reviewer.
        /// </summary>
        /// <param name="id">Entity id.</param>
        public virtual async ValueTask<Guid?> GetReviewerAsync(int id)
        {
            var inquiry = await _inquiryRepository.GetByIdAsync(id);
            return inquiry.Attribution.Reviewer;
        }

        /// <summary>
        ///     Get inquiry contractor.
        /// </summary>
        /// <param name="id">Entity id.</param>
        public virtual async ValueTask<Guid> GetContractorAsync(int id)
        {
            var inquiry = await _inquiryRepository.GetByIdAsync(id);
            return inquiry.Attribution.Contractor;
        }

        /// <summary>
        ///     Get record create date.
        /// </summary>
        /// <param name="id">Entity id.</param>
        public virtual async ValueTask<DateTime> GetRecordCreateDateAsync(int id)
        {
            var inquiry = await _inquiryRepository.GetByIdAsync(id);
            return inquiry.Record.CreateDate;
        }

        /// <summary>
        ///     Get record update date.
        /// </summary>
        /// <param name="id">Entity id.</param>
        public virtual async ValueTask<DateTime?> GetRecordUpdateDateAsync(int id)
        {
            var inquiry = await _inquiryRepository.GetByIdAsync(id);
            return inquiry.Record.UpdateDate;
        }

        /// <summary>
        ///     Get inquiry access policy.
        /// </summary>
        /// <param name="id">Entity id.</param>
        public virtual async ValueTask<AccessPolicy> GetAccessPolicyAsync(int id)
        {
            var inquiry = await _inquiryRepository.GetByIdAsync(id);
            return inquiry.Access.AccessPolicy;
        }

        /// <summary>
        ///     Store document and return new name.
        /// </summary>
        /// <param name="stream">FIle stream.</param>
        /// <param name="fileName">Original file name.</param>
        /// <param name="contentType">Original file content type.</param>
        /// <returns></returns>
        public async ValueTask<string> StoreDocumentAsync(Stream stream, string fileName, string contentType)
        {
            string newFileName = IO.Path.GetUniqueName(fileName);
            await _fileStorageService.StoreFileAsync("inquiry-report", newFileName, contentType, stream); // TODO: store?
            return newFileName;
        }

        /// <summary>
        ///     Create new inquiry.
        /// </summary>
        /// <param name="inquiry">Entity object.</param>
        /// <param name="attribution">Entity attribution.</param>
        public virtual async ValueTask<Inquiry> CreateAsync(AttributionControl attribution, Inquiry inquiry)
        {
            if (inquiry == null)
            {
                throw new ArgumentNullException(nameof(inquiry));
            }

            inquiry.InitializeDefaults();
            inquiry.Validate();

            var config = new MapperConfiguration(cfg => cfg.CreateMap<Inquiry, InquiryFull>());
            var mapper = config.CreateMapper();

            var inquiryFull = mapper.Map<InquiryFull>(inquiry);
            inquiryFull.Attribution = attribution;
            inquiryFull.State = new StateControl { AuditStatus = AuditStatus.Todo };
            inquiryFull.Access = new AccessControl { AccessPolicy = AccessPolicy.Private };
            inquiryFull.Record = new RecordControl { };

            var id = await _inquiryRepository.AddAsync(inquiryFull);
            return await GetAsync(id);
        }

        /// <summary>
        ///     Retrieve all inquiries.
        /// </summary>
        /// <param name="navigation">Recordset nagivation.</param>
        public virtual async IAsyncEnumerable<Inquiry> GetAllAsync(INavigation navigation)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<InquiryFull, Inquiry>());
            var mapper = config.CreateMapper();

            await foreach (var inquiry in _inquiryRepository.ListAllAsync(navigation))
            {
                yield return mapper.Map<Inquiry>(inquiry);
            }
        }

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

            //inquiry.InitializeDefaults(await _inquiryRepository.GetByIdAsync(inquiry.Id));
            inquiry.Validate();

            var config = new MapperConfiguration(cfg => cfg.CreateMap<Inquiry, InquiryFull>());
            var mapper = config.CreateMapper();

            var inquiryFull = mapper.Map<InquiryFull>(inquiry);
            var inquiry2 = await _inquiryRepository.GetByIdAsync(inquiry.Id);

            inquiryFull.Attribution = inquiry2.Attribution;
            inquiryFull.State = inquiry2.State;
            inquiryFull.Access = inquiry2.Access;
            inquiryFull.Record = inquiry2.Record;

            if (!inquiryFull.State.AllowWrite)
            {
                throw new EntityReadOnlyException();
            }

            await _inquiryRepository.UpdateAsync(inquiryFull);
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
            InquiryFull inquiry = await _inquiryRepository.GetByIdAsync(id);

            Func<ValueTask> postUpdateEvent = () => new ValueTask();

            switch (status)
            {
                case AuditStatus.Pending:
                    inquiry.State.TransitionToPending();
                    break;
                case AuditStatus.Done:
                    inquiry.State.TransitionToDone();
                    break;
                case AuditStatus.Discarded:
                    inquiry.State.TransitionToDiscarded();
                    break;
                case AuditStatus.PendingReview:
                    inquiry.State.TransitionToReview();

                    // TODO: Reviewer receives notification
                    postUpdateEvent = () => _notificationService.NotifyByEmailAsync(new string[] { "info@example.com" }, message);
                    break;
                case AuditStatus.Rejected:
                    inquiry.State.TransitionToRejected();

                    // TODO: Creator receives notification + message
                    postUpdateEvent = () => _notificationService.NotifyByEmailAsync(new string[] { "info@example.com" }, message);
                    break;
                default:
                    throw new StateTransitionException(inquiry.State.AuditStatus, status);
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
            return await _inquirySampleRepository.GetByIdAsync(id);
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

            if (!inquiry.State.AllowWrite)
            {
                throw new EntityReadOnlyException();
            }

            var id = await _inquirySampleRepository.AddAsync(inquirySample);
            inquirySample = await _inquirySampleRepository.GetByIdAsync(id);

            inquiry.State.TransitionToPending();
            await _inquiryRepository.UpdateAsync(inquiry);

            return inquirySample;
        }

        /// <summary>
        ///     Retrieve all inquiry samples.
        /// </summary>
        /// <param name="navigation">Recordset nagivation.</param>
        public virtual IAsyncEnumerable<InquirySample> GetAllSampleAsync(INavigation navigation)
        {
            return _inquirySampleRepository.ListAllAsync(navigation);
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

            if (!inquiry.State.AllowWrite)
            {
                throw new EntityReadOnlyException();
            }

            await _inquirySampleRepository.DeleteAsync(id);
            var itemCount = await _inquiryRepository.CountAsync(); // FUTURE: Should only select inquiry
            if (itemCount == 0)
            {
                inquiry.State.TransitionToTodo();
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

            if (!inquiry.State.AllowWrite)
            {
                throw new EntityReadOnlyException();
            }

            await _inquirySampleRepository.UpdateAsync(inquirySample);

            inquiry.State.TransitionToPending();
            await _inquiryRepository.UpdateAsync(inquiry);
        }

        #endregion
    }
}
