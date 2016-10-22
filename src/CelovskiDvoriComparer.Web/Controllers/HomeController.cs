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
        private static Task<IEnumerable<CelovskiDvoriModel>> _model = CelovskiDvoriModel.GetModelsFromWeb(); // poorman's caching

        public async Task<IActionResult> Index()
        {
            var model = (await _model)
                .Where(x => x.BasicDescription.NrRooms.StartsWith("tr"))
                .OrderBy(x => x.BasicDescription.Price)
                .ToList();

            return View(model);
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
