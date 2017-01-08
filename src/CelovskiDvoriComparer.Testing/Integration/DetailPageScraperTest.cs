using CelovskiDvoriComparer.Web.Scrapers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace CelovskiDvoriComparer.Testing.Integration
{
    public class DetailPageScraperTest
    {
        [Fact]
        public async Task Test()
        {
            var uri = new Uri("http://nepremicnine.dutb.eu/cd/oglas/oglas3041722.html");
            var model = await DetailPageScraper.GetDetailData(uri);

            Assert.Equal(model.SketchImageUri.ToString(), "//slike.nepremicnine.si21.com/images/201502/6473_3041722_0_b.jpg");
        }
    }
}
