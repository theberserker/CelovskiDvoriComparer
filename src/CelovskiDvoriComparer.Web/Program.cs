using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Nito.AsyncEx;
using Newtonsoft.Json;
using CelovskiDvoriComparer.Web.Models;
using CelovskiDvoriComparer.Web.Scrapers;
using System.Diagnostics;

namespace CelovskiDvoriComparer.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseStartup<Startup>()
                .Build();

            host.Run();

            //AsyncContext.Run(() => MainAsync(args));
            //MainAsync();
        }

        private static void MainAsync()
        {
            // http://nepremicnine.dutb.eu/cd/oglas/oglas3041697.html
            // http://nepremicnine.dutb.eu/cd/oglas/oglas3041919.html
            int startIndex = 3041697;
            string template = "http://nepremicnine.dutb.eu/cd/oglas/oglas{0}.html";
            int endIndex = 3041919;

            var pages = Enumerable.Range(startIndex, endIndex - startIndex + 1)
                .Select(i => string.Format(template, i));

            //var models = await pages.ForEachAsync(async (url) =>
            //{
            //    return await DetailPageScraper.GetDetailData(new Uri(url));
            //});

            var models = pages
                .Select(x => GetModel(x))
                .Where(x => x != null)
                .ToList();

            string json = JsonConvert.SerializeObject(models);
            //string jsonPath = Path.Combine(_hostingEnvironment.WebRootPath, @"data\model-all.json");
            string jsonPath = Path.Combine(@"C:\tmp", "model_all.json");
            System.IO.File.WriteAllText(jsonPath, json);
        }

        public static DetailModel GetModel(string url)
        {
            try
            {
                var model = DetailPageScraper.GetDetailData(new Uri(url)).Result;
                if (model == null)
                {
                    return null;
                }
                model.OriginalUrl = url;
                return model;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Crashed at {url}{Environment.NewLine}{ex}");
                return null;
            }
        }
    }
}
