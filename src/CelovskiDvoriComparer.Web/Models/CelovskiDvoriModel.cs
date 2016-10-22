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
        /// Creates the main model of an application.
        /// </summary>
        /// <returns></returns>
        public static async Task<IEnumerable<CelovskiDvoriModel>> GetModels()
        {
            var frontpageItems = await FrontPageScraper.GetBasicData();
            //var tasks = frontpageItems.Select(async item => await A(item));
            //var models = await Task.WhenAll(tasks);
            //return models;
            return await frontpageItems.ForEachAsync(GetModel);

        }

        private static async Task<CelovskiDvoriModel> GetModel(BasicDescriptionModel basic)
        {
            var detail = await DetailPageScraper.GetDetailData(basic.DetailUri);
            var model = new CelovskiDvoriModel
            {
                BasicDescription = basic,
                Detail = detail
            };
            return model;
        }
    }
}
