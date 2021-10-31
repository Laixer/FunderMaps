namespace FunderMaps.Core.Interfaces
{
    /// <summary>
    ///     Provides an abstraction for the template parser.
    /// </summary>
    public interface ITemplateParser
    {
        /// <summary>
        ///     Add object with name to the template context.
        /// </summary>
        /// <param name="contextItems">Dictionary with key and value objects.</param>
        ITemplateParser AddObject(IDictionary<string, object> contextItems);

        /// <summary>
        ///     Add object with name to the template context.
        /// </summary>
        /// <param name="name">Name of the object when referring from template.</param>
        /// <param name="value">Object to access in template.</param>
        ITemplateParser AddObject(string name, object value);

        /// <summary>
        ///     Supply the template.
        /// </summary>
        /// <param name="order">Subject order.</param>
        /// <param name="templateName">Template name on disk.</param>
        ITemplateParser FromTemplateFile(string order, string templateName);

        /// <summary>
        ///     Supply the template.
        /// </summary>
        /// <param name="templateString">Template as string.</param>
        ITemplateParser FromTemplate(string templateString);

        /// <summary>
        ///     Render the template and return the result as string.
        /// </summary>
        /// <param name="cancellationToken">Gets or sets the cancellation token used for async evaluation.</param>
        /// <returns>Parsed templated as string.</returns>
        Task<string> RenderAsync(CancellationToken cancellationToken);
    }
}
