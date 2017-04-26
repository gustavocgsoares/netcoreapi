using System;
using System.Collections.Generic;
using System.Linq;

namespace Template.CrossCutting.Exceptions.Base
{
    public abstract class BusinessRuleException : BaseException
    {
        #region Constructors | Destructors
        public BusinessRuleException(string code, string message)
            : base(Resources.Exceptions.Base.Messages.BusinessRuleException, message)
        {
            Code = code;
            Messages = new List<string>();
            Messages.Add(message);
        }

        public BusinessRuleException(string code, string message, params string[] args)
            : base(string.Format(Resources.Exceptions.Base.Messages.BusinessRuleException, message), args)
        {
            Code = code;
            Messages.Add(message);
        }

        public BusinessRuleException(string code, IEnumerable<string> messages)
            : base(Resources.Exceptions.Base.Messages.BusinessRuleException, string.Join(Environment.NewLine, (messages ?? new List<string>()).ToArray()))
        {
            Code = code;
            Messages.AddRange(messages);
        }
        #endregion

        #region Properties
        public List<string> Messages { get; private set; } = new List<string>();
        #endregion
    }
}
