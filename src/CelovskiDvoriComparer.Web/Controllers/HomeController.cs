using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CelovskiDvoriComparer.Web.Scrapers;
using CelovskiDvoriComparer.Web.Models;
using System.Collections.Concurrent;
using Microsoft.Extensions.Options;
using System.Dynamic;

namespace CelovskiDvoriComparer.Web.Controllers
{
    public class HomeController : Controller
    {
        private static IEnumerable<CelovskiDvoriModel> _model = CelovskiDvoriModel.GetModels().Result; // poorman's caching

        public async Task<IActionResult> Index()
        {
            return View(_model.Where(x=>x.BasicDescription.NrRooms.StartsWith("tr")));
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
