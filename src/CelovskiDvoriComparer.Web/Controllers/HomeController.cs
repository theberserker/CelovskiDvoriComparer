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

        public async Task<IActionResult> Index()
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

        public IActionResult Error()
        {
            return View();
        }
    }
}
