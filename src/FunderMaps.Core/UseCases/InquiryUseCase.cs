using FunderMaps.Core.Entities;
using FunderMaps.Core.Exceptions;
using FunderMaps.Core.Interfaces;
using FunderMaps.Core.Interfaces.Repositories;
using FunderMaps.Core.Types;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FunderMaps.Core.UseCases
{
    // TODO: 
    // - INotify when status pending -> pending_review
    //      -> reviewer receives notification
    // - INotify when status pending_review -> rejected
    //      -> creator receives notification + message

    /// <summary>
    /// Inquiry use case.
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
        /// Get inquiry.
        /// </summary>
        /// <param name="id">Entity id.</param>
        public virtual async ValueTask<Inquiry> GetAsync(int id)
        {
            var inquiry = await _inquiryRepository.GetByIdAsync(id).ConfigureAwait(false);
            if (inquiry == null)
            {
                throw new EntityNotFoundException();
            }

            // TODO:
            //inquiry.AttributionNavigation = ...

            return inquiry;
        }

        /// <summary>
        /// Create new inquiry.
        /// </summary>
        /// <param name="inquiry">Entity object.</param>
        public virtual async ValueTask<Inquiry> CreateAsync(Inquiry inquiry)
        {
            if (inquiry == null)
            {
                throw new ArgumentNullException(nameof(inquiry));
            }

            inquiry.Status = InquiryStatus.Todo;
            inquiry.Attribution = 1; // TODO: Remove

            var id = await _inquiryRepository.AddAsync(inquiry).ConfigureAwait(false);
            return await _inquiryRepository.GetByIdAsync(id).ConfigureAwait(false);
        }

        /// <summary>
        /// Retrieve all inquiries.
        /// </summary>
        /// <param name="navigation">Recordset nagivation.</param>
        public virtual IAsyncEnumerable<Inquiry> GetAllAsync(INavigation navigation)
            => _inquiryRepository.ListAllAsync(navigation);

        /// <summary>
        /// Update inquiry.
        /// </summary>
        /// <param name="inquiry">Entity object.</param>
        public virtual ValueTask UpdateAsync(Inquiry inquiry)
            => _inquiryRepository.UpdateAsync(inquiry);

        /// <summary>
        /// Delete inquiry.
        /// </summary>
        /// <param name="id">Entity id.</param>
        public virtual async ValueTask DeleteAsync(int id)
        {
            await _inquiryRepository.DeleteAsync(id).ConfigureAwait(false);
        }

        #endregion

        #region Inquiry Sample

        /// <summary>
        /// Get inquiry sample.
        /// </summary>
        /// <param name="id">Entity sample id.</param>
        public virtual async ValueTask<InquirySample> GetSampleAsync(int id)
        {
            var inquirySample = await _inquirySampleRepository.GetByIdAsync(id).ConfigureAwait(false);
            if (inquirySample == null)
            {
                throw new EntityNotFoundException();
            }

            inquirySample.InquiryNavigation = await _inquiryRepository.GetByIdAsync(inquirySample.Inquiry).ConfigureAwait(false);
            //inquirySample.AddressNavigation = await _addressRepository.GetByIdAsync(inquirySample.Address).ConfigureAwait(false);
            return inquirySample;
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

            inquirySample.BaseMeasurementLevel = BaseMeasurementLevel.NAP;

            var id = await _inquirySampleRepository.AddAsync(inquirySample).ConfigureAwait(false);
            return await _inquirySampleRepository.GetByIdAsync(id).ConfigureAwait(false);
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

            await _inquirySampleRepository.DeleteAsync(id).ConfigureAwait(false);
        }

        /// <summary>
        /// Update inquiry sample.
        /// </summary>
        /// <param name="inquirySample">Entity object.</param>
        public virtual ValueTask UpdateSampleAsync(InquirySample inquirySample)
        {
            if (inquirySample == null)
            {
                throw new ArgumentNullException(nameof(inquirySample));
            }

            inquirySample.BaseMeasurementLevel = BaseMeasurementLevel.NAP;

            // TODO: Set Inquiry.Status = InquiryStatus.Pending

            return _inquirySampleRepository.UpdateAsync(inquirySample);
        }

        #endregion
    }
}
