using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CelovskiDvoriComparer.Web.Models
{

    public class BasicDescriptionModel
    {
        public string Code { get; set; }
        public string NrRooms { get; set; }
        public string Floor { get; set; }
        public string Squares { get; set; }
        public string Price { get; set; }
        public Uri DetailUri { get; internal set; }
    }
}
