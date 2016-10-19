﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CelovskiDvoriComparer.Web.Models
{
    public class DetailModel
    {
        public Uri SketchImageUri { get; set; }
        public IDictionary<string, string> Details { get; set; }
        public string UsableAreaSquares { get; internal set; }
        public string CompleteArea { get; internal set; }
    }
}
