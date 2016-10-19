using CelovskiDvoriComparer.Web.Models;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CelovskiDvoriComparer.Web.Scrapers
{
    public static class DetailPageScraper
    {
        public static async Task<DetailModel> GetDetailData(Uri detailUri)
        {
            var webGet = new HtmlWeb();
            var doc = await webGet.LoadFromWebAsync(detailUri.ToString());
            var imageUri = TryGetSketchImage(doc);
            var detailsDictionary = ParseDescriptionTable(doc);

            return new DetailModel
            {
                SketchImageUri = imageUri,
                Details = detailsDictionary
            };
        }

        private static IDictionary<string, string> ParseDescriptionTable(HtmlDocument doc)
        {
            var sumRows = doc.GetElementbyId("nepTable") // table with the details
                .Descendants("tr")
                .Where(tr => tr.Attributes.Any(x => HasAttributeWithValue(x, "class", "nepSUM"))) // only grey rows
                .Select(row =>
                {
                    var tds = row.Descendants("td").ToArray();
                    return new Tuple<string, string>(tds[1].InnerText.Trim(), tds[2].InnerText.Trim());
                })
                .ToDictionary(x => x.Item1, x => x.Item2);

            return sumRows;
        }

        private static Uri TryGetSketchImage(HtmlDocument doc)
        {
            var sketchImage = doc.DocumentNode
                .Descendants("img")
                .FirstOrDefault(x => x.Attributes.Any(a => HasAttributeWithValue(a, "class", "first_image")));

            if (sketchImage == null)
            {
                return null;
            }

            var src = sketchImage.Attributes["src"].Value;
            return new Uri(src);
        }

        private static bool HasAttributeWithValue(HtmlAttribute a, string name, string value)
        {
            return a != null && a.Name == name && a.Value.Split(' ').Any(x => x.Trim().Equals(value, StringComparison.OrdinalIgnoreCase));
        }
    }
}
