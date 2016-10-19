using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CelovskiDvoriComparer.Web.Scrapers;
using CelovskiDvoriComparer.Web.Models;
using System.Collections.Concurrent;

namespace CelovskiDvoriComparer.Web.Controllers
{
    public class HomeController : Controller
    {
        private static IEnumerable<CelovskiDvoriModel> _model = GetModel().Result; // poorman's caching

        private static async Task<IEnumerable<CelovskiDvoriModel>> GetModel()
        {
            var bag = new ConcurrentBag<CelovskiDvoriModel>();
            var frontpageItems = await FrontPageScraper.GetBasicData();

            await Task.Run(() => Parallel.ForEach(
                
                frontpageItems.Take(5), 
                async basic =>
                {
                    var detail = await DetailPageScraper.GetDetailData(basic.DetailUri);
                    var model = new CelovskiDvoriModel
                    {
                        BasicDescription = basic,
                        Detail = detail
                    };

                    bag.Add(model);
                }));

            return bag;
        }

        public async Task<IActionResult> Index()
        {
            return View(_model);
        }

        public IActionResult About()
        {
            ViewData["Message"] = "This application is here to help comparing appartments sold on http://nepremicnine.dutb.eu/ side by side.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
