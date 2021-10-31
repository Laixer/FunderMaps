using FunderMaps.Core.Interfaces;
using Scriban;
using Scriban.Runtime;

namespace FunderMaps.Core.Components
{
    /// <summary>
    ///     Template parser.
    /// </summary>
    public class TemplateParser : ITemplateParser
    {
        private readonly AppContext _appContext;

        private const string templatePath = "Template/{0}/{1}.html";

        private Template template;
        private ScriptObject scriptObject = new();
        private TemplateContext templateContext = new();

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public TemplateParser(AppContext appContext)
            => _appContext = appContext ?? throw new ArgumentNullException(nameof(appContext));

        /// <summary>
        ///     Add object with name to the template context.
        /// </summary>
        /// <param name="contextItems">Dictionary with key and value objects.</param>
        public ITemplateParser AddObject(IDictionary<string, object> contextItems)
        {
            foreach (var contextItem in contextItems)
            {
                scriptObject.Add(contextItem.Key, contextItem.Value);
            }
            return this;
        }

        /// <summary>
        ///     Add object with name to the template context.
        /// </summary>
        /// <param name="name">Name of the object when referring from template.</param>
        /// <param name="value">Object to access in template.</param>
        public ITemplateParser AddObject(string name, object value)
        {
            scriptObject.Add(name, value);
            return this;
        }

        /// <summary>
        ///     Register a <see cref="IScriptObject"/> extension in the template context.
        /// </summary>
        /// <param name="scriptObject">Script object to load on rendering.</param>
        public ITemplateParser RegisterExtension(IScriptObject scriptObject)
        {
            templateContext.PushGlobal(scriptObject);
            return this;
        }

        /// <summary>
        ///     Supply the template.
        /// </summary>
        /// <param name="order">Subject order.</param>
        /// <param name="templateName">Template name on disk.</param>
        public ITemplateParser FromTemplateFile(string order, string templateName)
        {
            string body = File.ReadAllText(Path.Combine(_appContext.applicationDirectory, string.Format(templatePath, order, templateName)));
            string header = File.ReadAllText(Path.Combine(_appContext.applicationDirectory, "Template/Email/Header.html"));
            string footer = File.ReadAllText(Path.Combine(_appContext.applicationDirectory, "Template/Email/Footer.html"));

            template = Template.ParseLiquid(header + body + footer);
            return this;
        }

        /// <summary>
        ///     Supply the template.
        /// </summary>
        /// <param name="templateString">Template as string.</param>
        public ITemplateParser FromTemplate(string templateString)
        {
            template = Template.ParseLiquid(templateString);
            return this;
        }

        /// <summary>
        ///     Render the template and return the result as string.
        /// </summary>
        /// <param name="cancellationToken">Gets or sets the cancellation token used for async evaluation.</param>
        /// <returns>Parsed templated as string.</returns>
        public async Task<string> RenderAsync(CancellationToken cancellationToken)
        {
            templateContext.CancellationToken = cancellationToken;
            templateContext.PushGlobal(scriptObject);
            return await template.RenderAsync(templateContext);
        }
    }
}
