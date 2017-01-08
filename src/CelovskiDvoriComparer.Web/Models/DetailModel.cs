using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CelovskiDvoriComparer.Web.Models
{
    public class DetailModel
    {
        public string Code { get; set; }
        public string OriginalUrl { get; set; }
        public string SketchImageUri { get; set; }
        public BasicInfoModel BasicInfo {get;set;}
        public AreasModel Areas { get; set; }


    }
}
