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
            var imageUri = TryParseSketchImage(doc);
            var descriptionTable = ParseDescriptionTable(doc).Where(x=>!string.IsNullOrWhiteSpace(x.Item2)); // get only those that have value in last column
            var usableArea = descriptionTable.First(x => x.Item1.Contains("Uporabna")).Item2;
            var completeArea = descriptionTable.First(x => x.Item1.Contains("Neto")).Item2;

            return new DetailModel
            {
                SketchImageUri = imageUri,
                UsableAreaSquares = usableArea,
                CompleteArea = completeArea,
                Characteristics = descriptionTable
            };
        }

        private static IEnumerable<Tuple<string, string>> ParseDescriptionTable(HtmlDocument doc)
        {
            var rows = doc.GetElementbyId("nepTable") // table with the details
                .Descendants("tr")
                .Where(tr => tr.Attributes
                    .Any(x => HasAttributeWithValue(x, "class", "nepSUM") || HasAttributeWithValue(x, "class", "nepTR0") || HasAttributeWithValue(x, "class", "nepTR1"))) // grey rows (sums for m2 and all the rest with content)
                .Select(row =>
                {
                    var tds = row.Descendants("td").ToArray();
                    return new Tuple<string, string>(tds[1].InnerText.Trim(), tds[2].InnerText.Trim());
                });

            return rows;
        }

        private static Uri TryParseSketchImage(HtmlDocument doc)
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
