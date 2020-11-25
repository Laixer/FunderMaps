using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Scriban;
using Scriban.Runtime;

namespace FunderMaps.Core.Components
{
    /// <summary>
    ///     Template parser.
    /// </summary>
    public class TemplateParser
    {
        private const string templatePath = "Template/{0}/{1}.html";

        private Template template;
        private ScriptObject scriptObject = new();
        private TemplateContext templateContext = new();

        /// <summary>
        ///     Add object with name to the template context.
        /// </summary>
        /// <param name="name">Name of the object when referring from template.</param>
        /// <param name="value">Object to access in template.</param>
        public TemplateParser AddObject(string name, object value)
        {
            scriptObject.Add(name, value);
            return this;
        }

        /// <summary>
        ///     Register a <see cref="IScriptObject"/> extension in the template context.
        /// </summary>
        /// <param name="scriptObject">Script object to load on rendering.</param>
        public TemplateParser RegisterExtension(IScriptObject scriptObject)
        {
            templateContext.PushGlobal(scriptObject);
            return this;
        }

        /// <summary>
        ///     Supply the template.
        /// </summary>
        /// <param name="order">Subject order.</param>
        /// <param name="templateName">Template name on disk.</param>
        public TemplateParser FromTemplateFile(string order, string templateName)
        {
            // TODO: Move to AppContext
            var applicationDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var fullTemplatePath = Path.Combine(applicationDirectory, string.Format(templatePath, order, templateName));
            template = Template.ParseLiquid(File.ReadAllText(fullTemplatePath), fullTemplatePath);
            return this;
        }

        /// <summary>
        ///     Supply the template.
        /// </summary>
        /// <param name="templateString">Template as string.</param>
        public TemplateParser FromTemplate(string templateString)
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
