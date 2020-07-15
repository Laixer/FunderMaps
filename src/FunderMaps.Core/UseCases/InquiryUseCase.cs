using FunderMaps.Core.Entities;
using FunderMaps.Core.Exceptions;
using FunderMaps.Core.Interfaces;
using FunderMaps.Core.Interfaces.Repositories;
using FunderMaps.Core.Types;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace FunderMaps.Core.UseCases
{
    /// <summary>
    ///     Inquiry use case.
    /// </summary>
    public class InquiryUseCase
    {
        private readonly INotificationService _notificationService;
        private readonly IInquiryRepository _inquiryRepository;
        private readonly IInquirySampleRepository _inquirySampleRepository;

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public InquiryUseCase(INotificationService notificationService, IInquiryRepository inquiryRepository, IInquirySampleRepository inquirySampleRepository)
        {
            _notificationService = notificationService ?? throw new ArgumentNullException(nameof(notificationService));
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
            try
            {
                // TODO:
                //inquiry.AttributionNavigation = ...
                return await _inquiryRepository.GetByIdAsync(id).ConfigureAwait(false);
            }
            catch (RepositoryException)
            {
                // FUTURE: We *assume* repository exceptions are non existing entities.
                throw new EntityNotFoundException();
            }
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

            try
            {
                inquiry.InstantiateDefaults();
                inquiry.Attribution = 1; // TODO: Remove

                Validator.ValidateObject(inquiry, new ValidationContext(inquiry), true);

                var id = await _inquiryRepository.AddAsync(inquiry).ConfigureAwait(false);
                return await _inquiryRepository.GetByIdAsync(id).ConfigureAwait(false);
            }
            catch (RepositoryException)
            {
                // FUTURE: We *assume* repository exceptions are non existing entities.
                throw new EntityNotFoundException();
            }
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

            try
            {
                inquiry.InstantiateDefaults(await _inquiryRepository.GetByIdAsync(inquiry.Id).ConfigureAwait(false));

                Validator.ValidateObject(inquiry, new ValidationContext(inquiry), true);

                if (!inquiry.AllowWrite)
                {
                    throw new FunderMapsCoreException("Cannot set status from current state"); // TODO: state exception
                }

                await _inquiryRepository.UpdateAsync(inquiry).ConfigureAwait(false);
            }
            catch (RepositoryException)
            {
                // FUTURE: We *assume* repository exceptions are non existing entities.
                throw new EntityNotFoundException();
            }
        }

        /// <summary>
        ///     Update inquiry.
        /// </summary>
        /// <param name="id">Entity id.</param>
        /// <param name="status">New entity status.</param>
        public virtual async ValueTask UpdateStatusAsync(int id, AuditStatus status)
        {
            try
            {
                // FUTURE: Abstract this away.
                var inquiry = await _inquiryRepository.GetByIdAsync(id).ConfigureAwait(false);

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

                        // TODO: After update
                        // TODO: Reviewer receives notification
                        await _notificationService.NotifyByEmailAsync(new string[] { "info@something.com" }).ConfigureAwait(false);
                        break;
                    case AuditStatus.Rejected:
                        inquiry.TransitionToRejected();

                        // TODO: After update
                        // TODO: Creator receives notification + message
                        await _notificationService.NotifyByEmailAsync(new string[] { "info@something.com" }).ConfigureAwait(false);
                        break;
                    default:
                        throw new FunderMapsCoreException("Cannot set status from current state"); // TODO: state exception
                }

                await _inquiryRepository.UpdateAsync(inquiry).ConfigureAwait(false);
            }
            catch (RepositoryException)
            {
                // FUTURE: We *assume* repository exceptions are non existing entities.
                throw new EntityNotFoundException();
            }
        }

        /// <summary>
        ///     Delete inquiry.
        /// </summary>
        /// <param name="id">Entity id.</param>
        public virtual async ValueTask DeleteAsync(int id)
        {
            try
            {
                await _inquiryRepository.DeleteAsync(id).ConfigureAwait(false);
            }
            catch (RepositoryException)
            {
                // FUTURE: We *assume* repository exceptions are non existing entities.
                throw new EntityNotFoundException();
            }
        }

        #endregion

        #region Inquiry Sample

        /// <summary>
        ///     Get inquiry sample.
        /// </summary>
        /// <param name="id">Entity sample id.</param>
        public virtual async ValueTask<InquirySample> GetSampleAsync(int id)
        {
            try
            {
                var inquirySample = await _inquirySampleRepository.GetByIdAsync(id).ConfigureAwait(false);
                inquirySample.InquiryNavigation = await _inquiryRepository.GetByIdAsync(inquirySample.Inquiry).ConfigureAwait(false);
                return inquirySample;
            }
            catch (RepositoryException)
            {
                // FUTURE: We *assume* repository exceptions are non existing entities.
                throw new EntityNotFoundException();
            }
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

            inquirySample.Id = 0;
            inquirySample.BaseMeasurementLevel = BaseMeasurementLevel.NAP;
            inquirySample.CreateDate = DateTime.MinValue;
            inquirySample.UpdateDate = null;
            inquirySample.DeleteDate = null;

            Validator.ValidateObject(inquirySample, new ValidationContext(inquirySample), true);

            try
            {
                // FUTURE: Too much logic
                var inquiry = await _inquiryRepository.GetByIdAsync(inquirySample.Inquiry).ConfigureAwait(false);
                var id = await _inquirySampleRepository.AddAsync(inquirySample).ConfigureAwait(false);
                inquirySample = await _inquirySampleRepository.GetByIdAsync(id).ConfigureAwait(false);
                if (inquiry.AuditStatus != AuditStatus.Pending)
                {
                    inquiry.AuditStatus = AuditStatus.Pending;
                    await _inquiryRepository.UpdateAsync(inquiry).ConfigureAwait(false);
                }
                inquirySample.InquiryNavigation = inquiry;
                return inquirySample;
            }
            catch (RepositoryException)
            {
                // FUTURE: We *assume* repository exceptions are non existing entities.
                throw new EntityNotFoundException();
            }
        }

        /// <summary>
        ///     Retrieve all inquiry samples.
        /// </summary>
        /// <param name="navigation">Recordset nagivation.</param>
        public virtual async IAsyncEnumerable<InquirySample> GetAllSampleAsync(INavigation navigation)
        {
            await foreach (var inquirySample in _inquirySampleRepository.ListAllAsync(navigation))
            {
                // TODO: This is working, but not efficient
                inquirySample.InquiryNavigation = await _inquiryRepository.GetByIdAsync(inquirySample.Inquiry).ConfigureAwait(false);
                yield return inquirySample;
            }
        }

        /// <summary>
        ///     Delete inquiry sample.
        /// </summary>
        /// <param name="id">Entity id.</param>
        public virtual async ValueTask DeleteSampleAsync(int id)
        {
            try
            {
                // FUTURE: Too much logic
                var inquirySample = await _inquirySampleRepository.GetByIdAsync(id).ConfigureAwait(false);
                await _inquirySampleRepository.DeleteAsync(id).ConfigureAwait(false);
                var itemCount = await _inquiryRepository.CountAsync().ConfigureAwait(false); // TODO: Should only select inquiry
                if (itemCount == 0)
                {
                    var inquiry = await _inquiryRepository.GetByIdAsync(inquirySample.Inquiry).ConfigureAwait(false);
                    inquiry.AuditStatus = AuditStatus.Todo;
                    await _inquiryRepository.UpdateAsync(inquiry).ConfigureAwait(false);
                }
            }
            catch (RepositoryException)
            {
                // FUTURE: We *assume* repository exceptions are non existing entities.
                throw new EntityNotFoundException();
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

            Validator.ValidateObject(inquirySample, new ValidationContext(inquirySample), true);

            try
            {
                // FUTURE: Too much logic
                var inquiry = await _inquiryRepository.GetByIdAsync(inquirySample.Inquiry).ConfigureAwait(false);
                await _inquirySampleRepository.UpdateAsync(inquirySample).ConfigureAwait(false);
                if (inquiry.AuditStatus != AuditStatus.Pending)
                {
                    inquiry.AuditStatus = AuditStatus.Pending;
                    await _inquiryRepository.UpdateAsync(inquiry).ConfigureAwait(false);
                }
            }
            catch (RepositoryException)
            {
                // FUTURE: We *assume* repository exceptions are non existing entities.
                throw new EntityNotFoundException();
            }
        }

        #endregion
    }
}
