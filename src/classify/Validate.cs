using System;
using System.Text.RegularExpressions;

namespace Classify
{
    public static class Validate
    {
        public static string NotEmpty(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new FormatException($"Must not be empty.");
            }

            return value;
        }

        public static string Format(string value, string regex, string description)
        {
            try
            {
                if (Regex.IsMatch(
                    value,
                    regex,
                    RegexOptions.IgnoreCase,
                    TimeSpan.FromMilliseconds(250)))
                {
                    return value;
                }
            }
            catch (Exception) {}

            throw RequiredFormatException(description);
        }

        public static FormatException RequiredFormatException(string requiredFormat)
        {
            return new FormatException($"Must be in the format of '{requiredFormat}'.");
        }
    }
}