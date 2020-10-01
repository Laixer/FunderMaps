using System;

namespace FunderMaps.WebApi.DataTransferObjects
{
    /// <summary>
    ///     DTO for downloading an inquiry.
    /// </summary>
    public sealed class InquiryDownloadDto
    {
        /// <summary>
        ///     Unique identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        ///     Document download uri.
        /// </summary>
        public Uri DownloadUri { get; set; }
    }
}
