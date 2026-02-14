using System.Text;
using System.Text.RegularExpressions;

namespace PCInventory.Utils
{
    public static class PCNameValidator
    {
        private static readonly Regex SafeHostRegex = new(
            @"^[A-Za-z0-9][A-Za-z0-9._:-]{0,254}$",
            RegexOptions.Compiled | RegexOptions.CultureInvariant);

        /// <summary>
        /// Sanitizes a PC name by removing whitespace and optionally validating against a pattern.
        /// </summary>
        /// <param name="input">The input string to sanitize</param>
        /// <param name="pattern">Pattern to validate (A=letter, #=digit). Empty to skip validation.</param>
        /// <param name="enableValidation">Whether to apply pattern validation</param>
        /// <returns>Sanitized PC name, or empty string if validation fails</returns>
        public static string SanitizePCName(string input, string pattern, bool enableValidation)
        {
            if (string.IsNullOrWhiteSpace(input))
                return string.Empty;

            // Step 1: Remove all whitespace and convert to uppercase
            var sanitized = new string(input.Where(c => !char.IsWhiteSpace(c)).ToArray()).ToUpper();

            // Step 2: Always enforce a safe host/IP character set.
            if (!IsSafeHost(sanitized))
                return string.Empty;

            // Step 3: If validation is disabled, just return the sanitized name
            if (!enableValidation || string.IsNullOrWhiteSpace(pattern))
                return sanitized;

            // Step 4: Convert pattern to regex using StringBuilder for better performance
            // A = Letter (A-Z), # = Digit (0-9)
            var regexPattern = new StringBuilder("^(");
            foreach (char c in pattern)
            {
                if (c == 'A' || c == 'a')
                    regexPattern.Append("[A-Z]");
                else if (c == '#')
                    regexPattern.Append("\\d");
                else
                    regexPattern.Append(Regex.Escape(c.ToString()));
            }
            regexPattern.Append(")$");

            // Step 5: Try to extract the matching pattern
            try
            {
                var regex = new Regex(regexPattern.ToString());
                var match = regex.Match(sanitized);

                if (match.Success)
                {
                    return match.Groups[1].Value;
                }
                else
                {
                    return string.Empty; // No match found
                }
            }
            catch
            {
                // If regex fails, return empty
                return string.Empty;
            }
        }

        public static bool IsSafeHost(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return false;

            return SafeHostRegex.IsMatch(value.Trim());
        }
    }
}
