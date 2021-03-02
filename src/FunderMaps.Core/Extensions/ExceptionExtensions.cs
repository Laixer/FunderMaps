using System;
using System.Collections.Generic;
using System.Linq;

namespace FunderMaps.Core.Extensions
{
    /// <summary>
    ///     Exception extension methods.
    /// </summary>
    public static class ExceptionExtensions
    {
        /// <summary>
        ///     Retrieve the exception message concatenated with all the messages from the nested innerExceptions.
        /// </summary>
        public static string GetMessageWithInner(this Exception ex) =>
            string.Join($";{ Environment.NewLine }caused by: ", GetInnerExceptions(ex).Select(e => $"{ e.Message }"));

        /// <summary>
        ///     Retrieve all nested innerExceptions as an enumerable.
        /// </summary>
        public static IEnumerable<Exception> GetInnerExceptions(this Exception _exception)
        {
            while (_exception is not null)
            {
                yield return _exception;
                _exception = _exception.InnerException;
            }
        }
    }
}
