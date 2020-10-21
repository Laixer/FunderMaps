using AutoMapper;
using FunderMaps.AspNetCore.DataTransferObjects;
using FunderMaps.Core.Entities;
using FunderMaps.Core.Exceptions;
using FunderMaps.Core.Interfaces.Repositories;
using FunderMaps.WebApi.DataTransferObjects;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#pragma warning disable CA1062 // Validate arguments of public methods
namespace FunderMaps.WebApi.Controllers.Report
{
    /// <summary>
    ///     Endpoint controller for inquiry sample operations.
    /// </summary>
    [Route("inquiry/{inquiryId}/sample")]
    public class InquirySampleController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly Core.AppContext _appContext;
        private readonly IInquiryRepository _inquiryRepository;
        private readonly IInquirySampleRepository _inquirySampleRepository;

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public InquirySampleController(
            IMapper mapper,
            Core.AppContext appContext,
            IInquiryRepository inquiryRepository,
            IInquirySampleRepository inquirySampleRepository)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _appContext = appContext ?? throw new ArgumentNullException(nameof(appContext));
            _inquiryRepository = inquiryRepository ?? throw new ArgumentNullException(nameof(inquiryRepository));
            _inquirySampleRepository = inquirySampleRepository ?? throw new ArgumentNullException(nameof(inquirySampleRepository));
        }

        // GET: api/inquiry/{id}/sample/{id}
        /// <summary>
        ///     Return inquiry sample by id.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(int id)
        {
            // Act.
            var inquirySample = await _inquirySampleRepository.GetByIdAsync(id);

            // Map.
            var output = _mapper.Map<InquirySampleDto>(inquirySample);

            // Return.
            return Ok(output);
        }

        // GET: api/inquiry/{id}/sample
        /// <summary>
        ///     Return all inquiry samples.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAllAsync(int inquiryId, [FromQuery] PaginationDto pagination)
        {
            // Act.
            IAsyncEnumerable<InquirySample> inquirySampleList = _inquirySampleRepository.ListAllAsync(inquiryId, pagination.Navigation);

            // Map.
            var output = await _mapper.MapAsync<IList<InquirySampleDto>, InquirySample>(inquirySampleList);

            // Return.
            return Ok(output);
        }

        // POST: api/inquiry/{id}/sample/{id}
        /// <summary>
        ///     Create inquiry sample.
        /// </summary>
        /// <remarks>
        ///     Transition <see cref="Inquiry"/> into state pending if a <see cref="InquirySample"/>
        ///     was successfully created within this <see cref="Inquiry"/>.
        /// </remarks>
        [HttpPost]
        public async Task<IActionResult> CreateAsync(int inquiryId, [FromBody] InquirySampleDto input)
        {
            // Map.
            var inquirySample = _mapper.Map<InquirySample>(input);
            inquirySample.Inquiry = inquiryId;

            // Act.
            // FUTURE: Too much logic
            var inquiry = await _inquiryRepository.GetByIdAsync(inquirySample.Inquiry);
            if (!inquiry.State.AllowWrite)
            {
                throw new EntityReadOnlyException();
            }

            inquirySample = await _inquirySampleRepository.AddGetAsync(inquirySample);

            inquiry.State.TransitionToPending();
            await _inquiryRepository.SetAuditStatusAsync(inquiry.Id, inquiry);

            // Map.
            var output = _mapper.Map<InquirySampleDto>(inquirySample);

            // Return.
            return Ok(output);
        }

        // PUT: api/inquiry/{id}/sample/{id}
        /// <summary>
        ///     Update inquiry by id.
        /// </summary>
        /// <remarks>
        ///     Transition <see cref="Inquiry"/> into state pending if a <see cref="InquirySample"/>
        ///     was successfully updated within this <see cref="Inquiry"/>.
        /// </remarks>
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateAsync(int inquiryId, int id, [FromBody] InquirySampleDto input)
        {
            // Map.
            var inquirySample = _mapper.Map<InquirySample>(input);
            inquirySample.Id = id;
            inquirySample.Inquiry = inquiryId;

            // Act.
            var inquiry = await _inquiryRepository.GetByIdAsync(inquirySample.Inquiry);
            if (!inquiry.State.AllowWrite)
            {
                throw new EntityReadOnlyException();
            }

            await _inquirySampleRepository.UpdateAsync(inquirySample);

            inquiry.State.TransitionToPending();
            await _inquiryRepository.SetAuditStatusAsync(inquiry.Id, inquiry);

            // Return.
            return NoContent();
        }

        // DELETE: api/inquiry/{id}/sample/{id}
        /// <summary>
        ///     Delete inquiry sample by id.
        /// </summary>
        /// <remarks>
        ///     Transition <see cref="Inquiry"/> into state todo if all <see cref="InquirySample"/>
        ///     within this <see cref="Inquiry"/> are deleted.
        /// </remarks>
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            // Act.
            var inquirySample = await _inquirySampleRepository.GetByIdAsync(id);
            var inquiry = await _inquiryRepository.GetByIdAsync(inquirySample.Inquiry);
            if (!inquiry.State.AllowWrite)
            {
                throw new EntityReadOnlyException();
            }

            await _inquirySampleRepository.DeleteAsync(id);

            // FUTURE: Should only select inquiry
            if (await _inquirySampleRepository.CountAsync() == 0)
            {
                inquiry.State.TransitionToTodo();
                await _inquiryRepository.SetAuditStatusAsync(inquiry.Id, inquiry);
            }

            // Return.
            return NoContent();
        }
    }
}
#pragma warning restore CA1062 // Validate arguments of public methods
