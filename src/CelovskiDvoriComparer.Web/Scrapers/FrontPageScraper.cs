using CelovskiDvoriComparer.Web.Models;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CelovskiDvoriComparer.Web.Scrapers
{
    public class FrontPageScraper
    {
        public static string BaseUri = "http://nepremicnine.dutb.eu/cd/Stanovanja";

        public static async Task<IEnumerable<BasicDescriptionModel>> GetBasicData()
        {
            var webGet = new HtmlWeb();
            var doc = await webGet.LoadFromWebAsync(BaseUri);
            var trNodes = doc.GetElementbyId("novoCenik").Descendants("tr");

            return CreateModels(trNodes);
        }

        private static IEnumerable<BasicDescriptionModel> CreateModels(IEnumerable<HtmlNode> trNodes)
        {
            foreach (HtmlNode tr in trNodes)
            {
                Console.WriteLine(tr.OuterHtml);
                var tds = tr.Descendants("td").ToArray();
                if (tds.Count() < 5) { continue; }

                var basic = new BasicDescriptionModel
                {
                    Code = tds[0].InnerText.Trim(),
                    NrRooms = tds[1].InnerText.Trim(),
                    Floor = tds[2].InnerText.Trim(),
                    Squares = tds[3].InnerText.Trim(),
                    Price = tds[4].InnerText.Trim(),
                };

                basic.DetailUri = ParseUri(tr);

                yield return basic;
            }
        }

        private static Uri ParseUri(HtmlNode tr)
        {
            var onClickAttr = tr.Attributes.Where(x => x.Name == "onclick").Single();

            int iStart = onClickAttr.Value.IndexOf("('") + 3;
            int iLast = onClickAttr.Value.LastIndexOf("')");
            var uri = "http://nepremicnine.dutb.eu/" + onClickAttr.Value.Substring(iStart, iLast - iStart);
            return new Uri(uri);
        }
    }
}
