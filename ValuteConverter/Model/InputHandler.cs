using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ValuteConverter
{
    internal static class InputHandler
    {
        private static readonly Regex regex = new Regex("^[0-9,.]*$");
        
        public static string Unify(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return string.Empty;
            input = input.Replace('.', ',');
            if (input.StartsWith(','))
                return string.Empty;
            if (input.Count(c => c == ',') > 1)
                input = new string(input.Take(input.Length - 1).ToArray());
            if(!regex.IsMatch(input))
                input = new string(input.Take(input.Length - 1).ToArray());
            return input;
        }
    }
}
