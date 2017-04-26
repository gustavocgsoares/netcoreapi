using System.Diagnostics.CodeAnalysis;

[module: SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1649:FileHeaderFileNameDocumentationMustMatchTypeName", Justification = "Reviewed.")]

namespace Template.Application.Model.Enums.V1
{
    /// <summary>
    /// Define the suggestion status.
    /// </summary>
    public enum SuggestionStatus
    {
        /// <summary>
        /// Undefined or not informed.
        /// </summary>
        Undefined = 0,

        /// <summary>
        /// New suggestion.
        /// </summary>
        New = 1,

        /// <summary>
        /// Suggestion in analysis by business area.
        /// </summary>
        InAnalysis = 2,

        /// <summary>
        /// Suggestion added in to do list.
        /// </summary>
        ToDo = 3,

        /// <summary>
        /// Suggestion priorized by business area.
        /// </summary>
        Priorized = 4,

        /// <summary>
        /// Suggestion done.
        /// </summary>
        Done = 5,

        /// <summary>
        /// Suggestion by passed.
        /// </summary>
        Bypassed = 6
    }

    /// <summary>
    /// Extensions to convert <see cref="SuggestionStatus"/>.
    /// </summary>
    public static class SuggestionStatusExtensions
    {
        /// <summary>
        /// Convert <see cref="Domain.Enums.Support.SuggestionStatus"/> to <see cref="SuggestionStatus"/>.
        /// </summary>
        /// <param name="status">See <see cref="SuggestionStatus"/>.</param>
        /// <param name="domainStatus">See <see cref="Domain.Enums.Support.SuggestionStatus"/>.</param>
        /// <returns>See <see cref="SuggestionStatus"/> returned.</returns>
        public static SuggestionStatus ToModel(this SuggestionStatus status, Domain.Enums.Support.SuggestionStatus domainStatus)
        {
            return (SuggestionStatus)domainStatus;
        }

        /// <summary>
        /// Convert <see cref="SuggestionStatus"/> to <see cref="Domain.Enums.Support.SuggestionStatus"/>.
        /// </summary>
        /// <param name="status">See <see cref="SuggestionStatus"/>.</param>
        /// <returns>See <see cref="Domain.Enums.Support.SuggestionStatus"/>.</returns>
        public static Domain.Enums.Support.SuggestionStatus ToDomain(this SuggestionStatus status)
        {
            return (Domain.Enums.Support.SuggestionStatus)status;
        }
    }
}
