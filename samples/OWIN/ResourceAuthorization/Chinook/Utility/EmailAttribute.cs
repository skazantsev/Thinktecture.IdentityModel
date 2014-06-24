using System.ComponentModel.DataAnnotations;

namespace Chinook.Utility
{
    public class EmailAttribute : RegularExpressionAttribute
    {
        public EmailAttribute()
            : base(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*")
        {
            base.ErrorMessage = "The field {0} must be a valid email format.";
        }
    }
}
