using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CelovskiDvoriComparer.Web.Models
{
    /// <summary>
    /// "Tabela prostorov" model.
    /// </summary>
    public class AreasModel
    {
        public string UsableAreaSquares { get; set; }
        public string CompleteArea { get; set; }

        /// <summary>
        /// Bag of all values that were possible to be parsed in the section.
        /// </summary>
        public IEnumerable<Tuple<string, string>> Characteristics { get; set; }
    }
}
