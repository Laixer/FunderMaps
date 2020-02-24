using System.Collections.Generic;

namespace FunderMaps.ViewModels
{
    // FUTURE: MVC should not handle any errors

    /// <summary>
    /// Error structure.
    /// </summary>
    public class ErrorDetail
    {
        /// <summary>
        /// Error code.
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// Error message.
        /// </summary>
        public string Message { get; set; }
    }

    /// <summary>
    /// Display error to the client.
    /// </summary>
    public class ErrorOutputModel
    {
        /// <summary>
        /// Collection of errors.
        /// </summary>
        public IList<ErrorDetail> Errors { get; private set; }

        /// <summary>
        /// Create new instance.
        /// </summary>
        public ErrorOutputModel() { }

        /// <summary>
        /// Create new instance.
        /// </summary>
        /// <param name="code">Error code.</param>
        /// <param name="message">Error message.</param>
        public ErrorOutputModel(int code, string message)
        {
            AddError(code, message);
        }

        /// <summary>
        /// Add error by code and message.
        /// </summary>
        /// <param name="code">Error code.</param>
        /// <param name="message">Error message.</param>
        public void AddError(int code, string message)
        {
            if (Errors == null)
            {
                Errors = new List<ErrorDetail>();
            }

            Errors.Add(new ErrorDetail { Code = code, Message = message });
        }

        /// <summary>
        /// Add error by code.
        /// </summary>
        /// <param name="code">Error code.</param>
        public void AddError(int code) => AddError(code, null);
    }
}
