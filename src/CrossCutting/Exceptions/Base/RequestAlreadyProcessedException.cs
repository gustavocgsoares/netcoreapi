using Template.CrossCutting.Resources.Exceptions.Base;

namespace Template.CrossCutting.Exceptions.Base
{
    public class RequestAlreadyProcessedException : BaseException
    {
        #region Constructors | Destructors
        public RequestAlreadyProcessedException(string customMessage)
            : base(Messages.RequestAlreadyProcessedException, customMessage)
        {
        }
        #endregion
    }
}