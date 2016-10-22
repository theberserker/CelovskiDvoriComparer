using CelovskiDvoriComparer.Web.Extensions;
using CelovskiDvoriComparer.Web.Scrapers;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CelovskiDvoriComparer.Web.Models
{
    public class CelovskiDvoriModel
    {
        public BasicDescriptionModel BasicDescription { get; set; }

        public DetailModel Detail { get; set; }

        /// <summary>
        /// Creates the main model of an application. Scraped from official website.
        /// </summary>
        /// <returns>Appllication root model.</returns>
        public static async Task<IEnumerable<CelovskiDvoriModel>> GetModelsFromWeb()
        {
            var frontpageItems = await FrontPageScraper.GetBasicData();
            var result = await frontpageItems.ForEachAsync(async basic => {
                var detail = await DetailPageScraper.GetDetailData(basic.DetailUri);
                var model = new CelovskiDvoriModel
                {
                    BasicDescription = basic,
                    Detail = detail
                };
                return model;
            });
            return result;
        }
    }
}
