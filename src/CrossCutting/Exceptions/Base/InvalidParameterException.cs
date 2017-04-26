using System.Collections.Generic;
using Template.CrossCutting.Resources.Exceptions.Base;

namespace Template.CrossCutting.Exceptions.Base
{
    public class InvalidParameterException : BaseException
    {
        #region Constructors | Destructors
        public InvalidParameterException(string validation)
            : base(Messages.InvalidParameterException)
        {
            Code = "00001";
            Validations.Add(validation);
        }

        public InvalidParameterException(IEnumerable<string> validations)
            : base(Messages.InvalidParameterException)
        {
            Code = "00001";
            Validations.AddRange(validations);
        }
        #endregion

        #region Properties
        public List<string> Validations { get; private set; } = new List<string>();
        #endregion
    }
}