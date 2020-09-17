using FunderMaps.Core.Types;

namespace FunderMaps.Core.Interfaces
{
    /// <summary>
    ///     Contract for mapping exceptions to an error message.
    /// </summary>
    /// <typeparam name="TException"></typeparam>
    public interface IExceptionMapper<TException>
    {
        /// <summary>
        ///     Maps an <see cref="System.Exception"/> to the 
        ///     corresponding <see cref="ErrorMessage"/>.
        /// </summary>
        /// <param name="exception"><see cref="System.Exception"/></param>
        /// <returns><see cref="ErrorMessage"/></returns>
        ErrorMessage Map(TException exception);
    }
}
