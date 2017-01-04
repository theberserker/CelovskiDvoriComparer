using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CelovskiDvoriComparer.Web.Scrapers;
using CelovskiDvoriComparer.Web.Models;
using Newtonsoft.Json;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace CelovskiDvoriComparer.Web.Controllers
{
    public class HomeController : Controller
    {
        IHostingEnvironment _hostingEnvironment;
        public HomeController(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        //private static Task<IEnumerable<CelovskiDvoriModel>> _model = CelovskiDvoriModel.GetModelsFromWeb(); // poorman's caching
        private static Task<IEnumerable<CelovskiDvoriModel>> _model = null; // poorman's caching v2...
        private static readonly object syncLock = new object();

        public async Task<IActionResult> OldIndex()
        {
            if (_model == null)
            {
                lock (syncLock)
                {
                    if (_model == null)
                    {
                        string jsonPath = Path.Combine(_hostingEnvironment.WebRootPath, @"data\model.json");
                        var json = System.IO.File.ReadAllText(jsonPath);
                        var obj = JsonConvert.DeserializeObject<IEnumerable<CelovskiDvoriModel>>(json);
                        _model = Task.FromResult(obj);
                    }
                }
            }

            var model = (await _model)
                .Where(x => x.BasicDescription.NrRooms.StartsWith("tr"))
                .OrderBy(x => x.BasicDescription.Price)
                .ToList();

            //var serialized = JsonConvert.SerializeObject(model);
            //System.IO.File.WriteAllText(@"C:\tmp\model.json", serialized);

            return View(model);
        }

        /// <summary>
        /// The controller for the all follow-up appartments. Scraped before they were published, but URLs were found.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            // http://nepremicnine.dutb.eu/cd/oglas/oglas3041697.html
            // http://nepremicnine.dutb.eu/cd/oglas/oglas3041919.html
            int startIndex = 3041697;
            string template = "http://nepremicnine.dutb.eu/cd/oglas/oglas{0}.html";
            int endIndex = 3041919;
            //for ()
            //{

            //}

            var url = string.Format(template, startIndex);
            var a = await DetailPageScraper.GetDetailData(new Uri(url));

            return Ok(a);
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
