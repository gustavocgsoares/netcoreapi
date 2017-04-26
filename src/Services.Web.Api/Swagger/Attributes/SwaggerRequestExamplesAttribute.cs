using System;
using System.IO;
using System.Reflection;
using Microsoft.Extensions.PlatformAbstractions;
using Template.CrossCutting.ExtensionMethods;

namespace Template.Services.Web.Api.Swagger.Attibutes
{
    /// <summary>
    /// Attribute to swagger request examples.
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false)]
    public class SwaggerRequestExamplesAttribute : Attribute
    {
        #region Constructors | Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SwaggerRequestExamplesAttribute"/> class.
        /// </summary>
        /// <param name="mockPath">Mock path.</param>
        public SwaggerRequestExamplesAttribute(string mockPath)
        {
            MockPath = mockPath;

            try
            {
                Init();
                MockNotFound = false;
            }
            catch
            {
                MockNotFound = true;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SwaggerRequestExamplesAttribute"/> class.
        /// </summary>
        /// <param name="requestType">Type of request.</param>
        /// <param name="mockPath">Mock path.</param>
        public SwaggerRequestExamplesAttribute(Type requestType, string mockPath)
        {
            MockPath = mockPath;
            RequestType = requestType;

            try
            {
                Init();

                var method = typeof(StringExtensions).GetMethod("JsonToObject");
                var generic = method.MakeGenericMethod(requestType);
                Instance = generic.Invoke(this, new object[] { Example });

                MockNotFound = false;
            }
            catch
            {
                MockNotFound = true;
            }
        }
        #endregion

        #region Properties

        /// <summary>
        /// Type of request.
        /// </summary>
        public Type RequestType { get; set; }

        /// <summary>
        /// Instance of example object.
        /// </summary>
        public object Instance { get; set; }

        /// <summary>
        /// Json example.
        /// </summary>
        public string Example { get; set; }

        /// <summary>
        /// Mock path.
        /// </summary>
        public string MockPath { get; set; }

        /// <summary>
        /// Mock not found.
        /// </summary>
        public bool MockNotFound { get; set; }
        #endregion

        #region Private methods

        /// <summary>
        /// Init process.
        /// </summary>
        private void Init()
        {
            var path = PlatformServices.Default.Application.ApplicationBasePath;
            ////var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase);
            var currentPath = new Uri(path).LocalPath;
            var mockFullPath = Path.Combine(currentPath, MockPath);

            using (var stream = new FileStream(mockFullPath, FileMode.Open))
            {
                using (var reader = new StreamReader(stream))
                {
                    Example = reader.ReadToEnd();
                }
            }
        }
        #endregion
    }
}