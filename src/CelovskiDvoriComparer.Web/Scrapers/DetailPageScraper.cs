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

            // parses the right part - "Tabela prostorov"
            var imageUri = TryParseSketchImage(doc);
            var descriptionTable = ParseDescriptionTable(doc).Where(x => !string.IsNullOrWhiteSpace(x.Item2)).ToList(); // get only those that have value in last column
            var usableArea = descriptionTable.First(x => x.Item1.Contains("Uporabna")).Item2;
            var completeArea = descriptionTable.First(x => x.Item1.Contains("Neto")).Item2;
            var basicData = ParseBasicData(doc);

            return new DetailModel
            {
                SketchImageUri = imageUri,
                Areas = new AreasModel
                {
                    UsableAreaSquares = usableArea, //ParseDecimal(usableArea),
                    CompleteArea = completeArea, //ParseDecimal(completeArea),
                    Characteristics = descriptionTable
                },
                BasicInfo = new BasicInfoModel
                {
                    Characteristics = basicData
                }
            };
        }

        private static IEnumerable<Tuple<string, string>> ParseBasicData(HtmlDocument doc)
        {
            var divContainer = doc.DocumentNode
                .Descendants("div")
                .FirstOrDefault(div => div.Attributes.Any(attr => HasAttributeWithValue(attr, "class", "osnippet")));

            var trs = divContainer
                .Descendants("table")
                .First()
                //.Descendants("tbody")
                //.Single(); // There is actually the 'tbody' within 'table', but zie prasec Agilty Pack will have directly the 'tr's inside!
                .Descendants("tr");

            var trsFiltered = trs.Select(x => x.Descendants("td").ToArray());
            foreach (var tds in trsFiltered.Where(x => x.Count() > 1))
            {
                yield return new Tuple<string, string>(tds[0].InnerText, tds[1].InnerText);
            }

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

        /// <summary>
        /// Parse out decimal number from a string .
        /// E.g. "73,20                            m²" will be parsed to "73,20"
        /// </summary>
        /// <param name="completeArea"></param>
        /// <returns></returns>
        private static string ParseDecimal(string completeArea)
        {
            if (completeArea == null)
                return null;

            var chars = completeArea
                .SkipWhile(c => !char.IsDigit(c))
                .TakeWhile(c => char.IsDigit(c) || c == ',')
                .ToArray();

            return new string(chars);
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
            return new Uri(src, UriKind.Relative);
        }

        private static bool HasAttributeWithValue(HtmlAttribute a, string name, string value)
        {
            return a != null && a.Name == name && a.Value.Split(' ').Any(x => x.Trim().Equals(value, StringComparison.OrdinalIgnoreCase));
        }
    }
}
