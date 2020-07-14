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
    // TODO: 
    // - INotify when status pending -> pending_review
    //      -> reviewer receives notification
    // - INotify when status pending_review -> rejected
    //      -> creator receives notification + message

    /// <summary>
    ///     Inquiry use case.
    /// </summary>
    public class InquiryUseCase
    {
        private readonly IInquiryRepository _inquiryRepository;
        private readonly IInquirySampleRepository _inquirySampleRepository;

        /// <summary>
        /// Create new instance.
        /// </summary>
        public InquiryUseCase(IInquiryRepository inquiryRepository, IInquirySampleRepository inquirySampleRepository)
        {
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

            inquiry.Id = 0;
            inquiry.AuditStatus = AuditStatus.Todo;
            inquiry.Attribution = 1; // TODO: Remove
            inquiry.CreateDate = DateTime.MinValue;
            inquiry.UpdateDate = null;
            inquiry.DeleteDate = null;
            inquiry.AttributionNavigation = null;

            Validator.ValidateObject(inquiry, new ValidationContext(inquiry), true);

            try
            {
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
            Validator.ValidateObject(inquiry, new ValidationContext(inquiry), true);

            try
            {
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
        /// Get inquiry sample.
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
        /// Create new inquiry sample.
        /// </summary>
        /// <param name="inquirySample">Entity object.</param>
        public virtual async ValueTask<InquirySample> CreateSampleAsync(InquirySample inquirySample)
        {
            if (inquirySample == null)
            {
                throw new ArgumentNullException(nameof(inquirySample));
            }

            // TODO: Set Inquiry.Status = InquiryStatus.Pending
            inquirySample.Id = 0;
            inquirySample.BaseMeasurementLevel = BaseMeasurementLevel.NAP;
            inquirySample.CreateDate = DateTime.MinValue;
            inquirySample.UpdateDate = null;
            inquirySample.DeleteDate = null;

            Validator.ValidateObject(inquirySample, new ValidationContext(inquirySample), true);

            try
            {
                var id = await _inquirySampleRepository.AddAsync(inquirySample).ConfigureAwait(false);
                return await _inquirySampleRepository.GetByIdAsync(id).ConfigureAwait(false);
            }
            catch (RepositoryException)
            {
                // FUTURE: We *assume* repository exceptions are non existing entities.
                throw new EntityNotFoundException();
            }
        }

        /// <summary>
        /// Retrieve all inquiry samples.
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
        /// Delete inquiry sample.
        /// </summary>
        /// <param name="id">Entity id.</param>
        public virtual async ValueTask DeleteSampleAsync(int id)
        {
            // TODO: Set Inquiry.Status = InquiryStatus.Todo ?

            try
            {
                await _inquirySampleRepository.DeleteAsync(id).ConfigureAwait(false);
            }
            catch (RepositoryException)
            {
                // FUTURE: We *assume* repository exceptions are non existing entities.
                throw new EntityNotFoundException();
            }
        }

        /// <summary>
        /// Update inquiry sample.
        /// </summary>
        /// <param name="inquirySample">Entity object.</param>
        public virtual async ValueTask UpdateSampleAsync(InquirySample inquirySample)
        {
            if (inquirySample == null)
            {
                throw new ArgumentNullException(nameof(inquirySample));
            }

            // TODO: Set Inquiry.Status = InquiryStatus.Pending

            inquirySample.BaseMeasurementLevel = BaseMeasurementLevel.NAP;

            Validator.ValidateObject(inquirySample, new ValidationContext(inquirySample), true);

            try
            {
                await _inquirySampleRepository.UpdateAsync(inquirySample).ConfigureAwait(false);
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
