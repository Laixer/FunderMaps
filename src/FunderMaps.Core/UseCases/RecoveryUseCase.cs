using FunderMaps.Core.Entities;
using FunderMaps.Core.Exceptions;
using FunderMaps.Core.Interfaces;
using FunderMaps.Core.Interfaces.Repositories;
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
    ///     Recovery use case.
    /// </summary>
    public class RecoveryUseCase
    {
        private readonly IRecoveryRepository _recoveryRepository;
        private readonly IRecoverySampleRepository _recoverySampleRepository;

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public RecoveryUseCase(IRecoveryRepository recoveryRepository, IRecoverySampleRepository recoverySampleRepository)
        {
            _recoveryRepository = recoveryRepository ?? throw new ArgumentNullException(nameof(recoveryRepository));
            _recoverySampleRepository = recoverySampleRepository ?? throw new ArgumentNullException(nameof(recoverySampleRepository));
        }

        /// <summary>
        ///     Get recovery.
        /// </summary>
        /// <param name="id">Entity id.</param>
        public async ValueTask<Recovery> GetAsync(int id)
        {
            var recovery = await _recoveryRepository.GetByIdAsync(id);
            if (recovery == null)
            {
                throw new EntityNotFoundException();
            }

            // FUTURE:
            //recovery.AttributionNavigation = ...

            return recovery;
        }

        /// <summary>
        ///     Create new recovery.
        /// </summary>
        /// <param name="recovery">Entity object.</param>
        public async ValueTask<Recovery> CreateAsync(Recovery recovery)
        {
            if (recovery == null)
            {
                throw new ArgumentNullException(nameof(recovery));
            }

            //recovery.Status = InquiryStatus.Todo;  //?
            recovery.Attribution = 1; // TODO: Remove
            recovery.Validate();

            var id = await _recoveryRepository.AddAsync(recovery);
            return await _recoveryRepository.GetByIdAsync(id);
        }

        /// <summary>
        ///     Retrieve all recoveries.
        /// </summary>
        /// <param name="navigation">Recordset nagivation.</param>
        public IAsyncEnumerable<Recovery> GetAllAsync(INavigation navigation)
            => _recoveryRepository.ListAllAsync(navigation);

        /// <summary>
        ///     Update recoveries.
        /// </summary>
        /// <param name="recovery">Entity object.</param>
        public ValueTask UpdateAsync(Recovery recovery)
            => _recoveryRepository.UpdateAsync(recovery);

        /// <summary>
        ///     Delete recoveries.
        /// </summary>
        /// <param name="id">Entity id.</param>
        public async ValueTask DeleteAsync(int id)
        {
            await _recoveryRepository.DeleteAsync(id);
        }

        /// <summary>
        ///     Get recovery sample.
        /// </summary>
        /// <param name="id">Entity sample id.</param>
        public async ValueTask<RecoverySample> GetSampleAsync(int id)
        {
            var recoverySample = await _recoverySampleRepository.GetByIdAsync(id);
            if (recoverySample == null)
            {
                throw new EntityNotFoundException();
            }

            recoverySample.RecoveryNavigation = await _recoveryRepository.GetByIdAsync(recoverySample.Recovery);
            //inquirySample.AddressNavigation = await _addressRepository.GetByIdAsync(inquirySample.Address);
            return recoverySample;
        }

        /// <summary>
        ///     Create new recovery sample.
        /// </summary>
        /// <param name="recoverySample">Entity object.</param>
        public async ValueTask<RecoverySample> CreateSampleAsync(RecoverySample recoverySample)
        {
            if (recoverySample == null)
            {
                throw new ArgumentNullException(nameof(recoverySample));
            }

            // TODO: Set Inquiry.Status = InquiryStatus.Pending
            recoverySample.InitializeDefaults();
            recoverySample.Validate();

            var id = await _recoverySampleRepository.AddAsync(recoverySample);
            return await _recoverySampleRepository.GetByIdAsync(id);
        }

        /// <summary>
        ///     Retrieve all recovery samples.
        /// </summary>
        /// <param name="navigation">Recordset nagivation.</param>
        public async IAsyncEnumerable<RecoverySample> GetAllSampleAsync(INavigation navigation)
        {
            await foreach (var recoverySample in _recoverySampleRepository.ListAllAsync(navigation))
            {
                // TODO: This is working, but not efficient
                recoverySample.RecoveryNavigation = await _recoveryRepository.GetByIdAsync(recoverySample.Recovery);
                yield return recoverySample;
            }
        }

        /// <summary>
        ///     Delete recovery sample.
        /// </summary>
        /// <param name="id">Entity id.</param>
        public async ValueTask DeleteSampleAsync(int id)
        {
            // TODO: Set Recovery.Status = InquiryStatus.Todo ?

            await _recoverySampleRepository.DeleteAsync(id);
        }

        /// <summary>
        ///     Update recovery sample.
        /// </summary>
        /// <param name="recoverySample">Entity object.</param>
        public ValueTask UpdateSampleAsync(RecoverySample recoverySample)
        {
            if (recoverySample == null)
            {
                throw new ArgumentNullException(nameof(recoverySample));
            }

            // TODO: Set Recovery.Status = InquiryStatus.Todo ?

            return _recoverySampleRepository.UpdateAsync(recoverySample);
        }
    }
}
