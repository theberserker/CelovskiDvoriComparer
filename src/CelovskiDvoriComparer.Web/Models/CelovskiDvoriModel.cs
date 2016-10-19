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
            var bag = new ConcurrentBag<CelovskiDvoriModel>();
            var frontpageItems = await FrontPageScraper.GetBasicData();

            await Task.Run(() => Parallel.ForEach(
                frontpageItems,
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
    }
}
