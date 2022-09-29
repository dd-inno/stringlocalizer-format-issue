using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;

namespace FormatIssue
{
    internal static class Program
    {
        public static void Main()
        {
            const string input = "Formatted string: {0},{1}.";
            IStringLocalizer localizer = new Localizer();
            int? formatArg0 = null;
            int? formatArg1 = 1;

            // Works
            string a = string.Format(localizer[input], formatArg0, formatArg1);
            Console.WriteLine(string.Format(a, formatArg0, formatArg1));

            // Works too, but produces warning CS8604
            string b =  localizer[input, formatArg0, formatArg1];
            Console.WriteLine(b);
        }

        private class Localizer : IStringLocalizer
        {
            private readonly Dictionary<string, string> _localizedStrings = new()
            {
                { "Formatted string: {0},{1}.", "Formatierte ZeichenFolge: {0},{1}." }
            };

            public LocalizedString this[string name]
            {
                get
                {
                    string? localizedName = GetStringSafely(name);
                    return new LocalizedString(name, localizedName ?? name, resourceNotFound: localizedName == null);
                }
            }

            public LocalizedString this[string name, params object[] arguments]
            {
                get
                {
                    string? localizedName = GetStringSafely(name);
                    string formattedName = string.Format(localizedName ?? name, arguments);
                    return new LocalizedString(name, formattedName, resourceNotFound: localizedName == null);
                }
            }

            private string? GetStringSafely(string name)
            {
                return _localizedStrings.TryGetValue(name, out string? localizedName)
                    ? localizedName
                    : null;
            }

            public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
            {
                throw new System.NotImplementedException();
            }
        }
    }
}