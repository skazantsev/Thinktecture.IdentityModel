using System;
using System.Text.RegularExpressions;

namespace Chinook
{
    public static class CheckArgument
    {
        private static readonly Regex EmailExpression = new Regex(@"^([0-9a-zA-Z]+[-._+&])*[0-9a-zA-Z]+@([-0-9a-zA-Z]+[.])+[a-zA-Z]{2,6}$", RegexOptions.Singleline | RegexOptions.Compiled);

        public static void IsNotNull(object param, string name)
        {
            if (param == null) throw new ArgumentNullException(name);
        }
        public static void IsNotNullOrEmpty(string param, string name)
        {
            if (String.IsNullOrEmpty(param)) throw new ArgumentException(String.Format("{0} cannot be null or empty", name));
        }

        public static void IsNotNegative(double param, string name)
        {
            if (param < 0) throw new ArgumentOutOfRangeException(name);
        }
        public static void IsNotNegativeOrZero(double param, string name)
        {
            if (param <= 0) throw new ArgumentOutOfRangeException(name);
        }

        public static void IsNotNegative(long param, string name)
        {
            if (param < 0) throw new ArgumentOutOfRangeException(name);
        }
        public static void IsNotNegativeOrZero(long param, string name)
        {
            if (param <= 0) throw new ArgumentOutOfRangeException(name);
        }

        public static void IsNotNegative(decimal param, string name)
        {
            if (param < 0) throw new ArgumentOutOfRangeException(name);
        }
        public static void IsNotNegativeOrZero(decimal param, string name)
        {
            if (param <= 0) throw new ArgumentOutOfRangeException(name);
        }

        public static void IsNotOutOfRange(int param, int min, int max, string name)
        {
            if (param < min || param > max) throw new ArgumentOutOfRangeException(name, String.Format("{0} must be between \"{1}\" and \"{2}\".", name, min, max));
        }

        public static void IsNotNegative(TimeSpan param, string name)
        {
            if (param < TimeSpan.Zero) throw new ArgumentOutOfRangeException(name, String.Format("{0} cannot be negative", name));
        }

        public static void IsNotInThePast(DateTime param, string name)
        {
            if (param < DateTime.UtcNow) throw new ArgumentOutOfRangeException(name, String.Format("{0} cannot be in the past", name));
        }

        public static void IsNotEmpty(Guid param, string name)
        {
            if (param == Guid.Empty) throw new ArgumentException(name, String.Format("{0} cannot be empty", name));
        }

        public static void IsNotInvalidEmail(string param, string name)
        {
            if (!String.IsNullOrEmpty(param) && !EmailExpression.IsMatch(param))
            {
                throw new ArgumentException(String.Format("\"{0}\" is not a valid email address.", name));
            }
        }
    }
}
