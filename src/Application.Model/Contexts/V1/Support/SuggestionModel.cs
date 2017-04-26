using System;
using Template.Application.Model.Contexts.Base;
using Template.CrossCutting.ExtensionMethods;
using Template.Domain.Entities.Support;

namespace Template.Application.Model.Contexts.V1.Support
{
    /// <summary>
    /// Define a suggestion send by users.
    /// </summary>
    public class SuggestionModel : BaseModel<SuggestionModel>
    {
        #region Properties

        /// <summary>
        /// Suggestion id.
        /// </summary>
        public virtual string Id { get; set; }

        /// <summary>
        /// User submitting suggestion.
        /// </summary>
        public virtual string User { get; set; }

        /// <summary>
        /// Suggestion title.
        /// </summary>
        public virtual string Title { get; set; }

        /// <summary>
        /// Suggestion message.
        /// </summary>
        public virtual string Message { get; set; }
        #endregion

        #region Converters

        /// <summary>
        /// Convert <see cref="Suggestion"/> to <see cref="SuggestionModel"/>.
        /// </summary>
        /// <param name="entity">See <see cref="Suggestion"/>.</param>
        /// <returns>See <see cref="SuggestionModel"/>.</returns>
        public static SuggestionModel ToModel(Suggestion entity)
        {
            var model = Instance();

            model.Id = entity.Id != Guid.Empty ? entity.Id.ToString() : null;
            model.Title = entity.Title;
            model.User = entity.User;
            model.Message = entity.Message;

            return model;
        }

        /// <summary>
        /// Convert <see cref="SuggestionModel"/> to <see cref="Suggestion"/>.
        /// </summary>
        /// <returns>See <see cref="Suggestion"/>.</returns>
        public Suggestion ToDomain()
        {
            return ToDomain(new Suggestion());
        }

        /// <summary>
        /// Convert <see cref="SuggestionModel"/> to <see cref="Suggestion"/>.
        /// </summary>
        /// <param name="entity">Item <see cref="Suggestion"/> to be completed.</param>
        /// <returns>See <see cref="Suggestion"/>.</returns>
        public Suggestion ToDomain(Suggestion entity)
        {
            entity = entity ?? new Suggestion();

            entity.Id = Id.HasValue() ? Id.To<Guid>() : Guid.Empty;
            entity.Title = Title;
            entity.User = User;
            entity.Message = Message;

            return entity;
        }
        #endregion
    }
}
