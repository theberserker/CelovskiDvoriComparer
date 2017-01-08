using System;
using System.Collections.Generic;
using System.Linq;

namespace CelovskiDvoriComparer.Web.Models
{
    /// <summary>
    /// "Osnovni podatki" model
    /// </summary>
    public class BasicInfoModel
    {
        public string Lamela => GetCharacteristic("Lamela");

        public string VrstaObjekta => GetCharacteristic("Vrsta");

        public string Nadstropje => GetCharacteristic("Nadstropje");

        /// <summary>
        /// Bag of all values that were possible to be parsed in the section.
        /// </summary>
        public IEnumerable<Tuple<string, string>> Characteristics { get; set; }


        private string GetCharacteristic(string s)
        {
            if (s == null)
            {
                throw new ArgumentException("Provide the string to match!", "s");
            }
            
            return Characteristics
                ?.FirstOrDefault(x => x.Item1.TrimStart().StartsWith(s, StringComparison.OrdinalIgnoreCase))
                ?.Item2;
        }
    }
}