using System;
using System.IO;
using System.Reflection;
using Microsoft.Extensions.PlatformAbstractions;
using Template.CrossCutting.ExtensionMethods;

namespace Template.Services.Web.Api.Swagger.Attibutes
{
    /// <summary>
    /// Attribute to swagger response examples.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class SwaggerResponseExamplesAttribute : Attribute
    {
        #region Constructors | Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SwaggerResponseExamplesAttribute"/> class.
        /// </summary>
        /// <param name="responseType">Type of response.</param>
        /// <param name="mockPath">Mock path.</param>
        public SwaggerResponseExamplesAttribute(Type responseType, string mockPath)
        {
            MockPath = mockPath;
            ResponseType = responseType;

            try
            {
                Init();

                var method = typeof(StringExtensions).GetMethod("JsonToObject");
                var generic = method.MakeGenericMethod(responseType);
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
        /// Type of response.
        /// </summary>
        public Type ResponseType { get; set; }

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