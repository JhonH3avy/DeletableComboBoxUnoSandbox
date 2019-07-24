using System;
using System.Collections.Generic;
using System.Text;

namespace ComboBoxUnoSandbox.Shared.Helpers
{
    public class ValidationResult
    {
        public static readonly ValidationResult Success;

        
        protected ValidationResult(ValidationResult validationResult)
        {
            if (validationResult == null)
                throw new ArgumentNullException(nameof(validationResult));
            this.ErrorMessage = validationResult.ErrorMessage;
            this.MemberNames = validationResult.MemberNames;
        }


        public ValidationResult(string errorMessage) : this(errorMessage, (IEnumerable<string>)null)
        {
        }


        public ValidationResult(string errorMessage, IEnumerable<string> memberNames)
        {
            this.ErrorMessage = errorMessage;
            this.MemberNames = (IEnumerable<string>)((object)memberNames ?? (object)new string[0]);
        }


        public string ErrorMessage { get; set; }

        public IEnumerable<string> MemberNames { get; private set; }


        public override string ToString()
        {
            return this.ErrorMessage ?? base.ToString();
        }
    }
}
