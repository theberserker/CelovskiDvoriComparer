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
using CelovskiDvoriComparer.Web.Extensions;

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
        private static Task<IEnumerable<DetailModel>> _model2 = null; // poorman's caching v2...
        private static readonly object syncLock2 = new object();

        /// <summary>
        /// The controller for the all follow-up appartments. Scraped before they were published, but URLs were found.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            if (_model2 == null)
            {
                lock (syncLock2)
                {
                    if (_model2 == null)
                    {
                        string jsonPath = Path.Combine(_hostingEnvironment.WebRootPath, @"data\model_all.json");
                        var json = System.IO.File.ReadAllText(jsonPath);
                        var obj = JsonConvert.DeserializeObject<IEnumerable<DetailModel>>(json);
                        _model2 = Task.FromResult(obj);
                    }
                }
            }

            var model = (await _model2)
                .Where(m => !m.BasicInfo.Lamela.Equals("L1", StringComparison.OrdinalIgnoreCase)
                                && m.BasicInfo.VrstaObjekta.TrimStart().StartsWith("trisob", StringComparison.OrdinalIgnoreCase));

            return View(model);
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
