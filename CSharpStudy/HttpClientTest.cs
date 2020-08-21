using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CSharpStudy
{
    [TestClass]
    public class HttpClientTest
    {
        class GetDeltaBody
        {
            [JsonProperty(PropertyName = "versionNo")]
            public int VersionNo { get; set; }

            [JsonProperty(PropertyName = "deltaData")]
            public string[] DeltaData { get; set; }
        }

        [TestMethod]
        public void SimpleHttpClientTest()
        {
            const string URL = "http://storeserver:5003/api/DataSync/GetDelta?SellingLocationID=1001&RealMaintenanceVersionNo=0";

            var client = new HttpClient();
            var getTask = client.GetAsync(URL);
            var result = getTask.Result;

            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
            var readBodyTask = result.Content.ReadAsStringAsync();
            var bodyData = readBodyTask.Result;

            var body = JsonConvert.DeserializeObject<GetDeltaBody>(bodyData);
            Assert.IsTrue(body.VersionNo > 0);
            Assert.IsTrue(body.DeltaData.Length > 0);
        }

        class Cake
        {
            public string Sponge { get; set; }
            public string Cream { get; set; }
        }

        [TestMethod]
        public void TaskTest()
        {
            var bakeTheCakeTask = Task.Run(() =>
            {
                var cake = new Cake();
                Thread.Sleep(1000);
                cake.Sponge = "Biscuit dough";
                return cake;
            });

            var cake = bakeTheCakeTask.Result;
            Assert.AreEqual("Biscuit dough", cake.Sponge);
        }

        [TestMethod]
        public void TaskTest2()
        {
            Task<Cake> BakeTheCakeAsync()
            {
                return Task.Run(() =>
                {
                    var cake = new Cake();
                    Thread.Sleep(1000);
                    cake.Sponge = "Biscuit dough";
                    return cake;
                });
            }

            var bakeTheCakeTask = BakeTheCakeAsync();
            var cake = bakeTheCakeTask.Result;
            Assert.AreEqual("Biscuit dough", cake.Sponge);
        }

        [TestMethod]
        public void TaskWaitTest()
        {
            var bakeTheSpongeTask = Task.Run(() =>
            {
                Thread.Sleep(1000);
                return "Biscuit dough";
            });

            var makeTheCreamTask = Task.Run(() =>
            {
                Thread.Sleep(500);
                return "Creamy vanilla";
            });

            var makePartsTask = Task.WhenAll(bakeTheSpongeTask, makeTheCreamTask);
            makePartsTask.Wait();

            var cake = new Cake()
            {
                Sponge = bakeTheSpongeTask.Result,
                Cream = makeTheCreamTask.Result
            };

            Assert.AreEqual("Biscuit dough", cake.Sponge);
            Assert.AreEqual("Creamy vanilla", cake.Cream);
        }
    }
}
