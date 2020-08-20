using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace CSharpStudy
{
    [TestClass]
    public class DictionaryTest
    {
        class Person
        {
            public string Address { get; set; }
            public int Age { get; set; }
        }

        [TestMethod]
        public void BasicOperationTest()
        {
            var map = new Dictionary<string, object>();
            map.Add("Name", "kami");
            map.Add("Address", "Osaka");
            map.Add("Age", 39);
            Assert.AreEqual(3, map.Count);
            Assert.AreEqual("Osaka", map["Address"]);

            foreach (var item in map)
            {
                Assert.AreEqual("Name", item.Key);
                Assert.AreEqual("kami", item.Value);
                break;
            }
            foreach (var key in map.Keys)
            {
                Assert.AreEqual("Name", key);
                break;
            }
            foreach (var value in map.Values)
            {
                Assert.AreEqual("kami", value);
                break;
            }
        }

        [TestMethod]
        public void InitializeTest()
        {
            var map = new Dictionary<string, object>()
            {
                { "Name", "kami" },
                { "Address", "Osaka" },
                { "Age", 39 }
            };
            Assert.AreEqual(3, map.Count);
            Assert.AreEqual("Osaka", map["Address"]);

            var kamiInfo = new Person()
            {
                Address = "Osaka",
                Age = 39
            };
            var map2 = new Dictionary<string, object>()
            {
                { "kami", kamiInfo }
            };

            var map3 = new Dictionary<string, Person>()
            {
                { 
                    "kami", new Person()
                    {
                        Address = "Osaka",
                        Age = 39,
                    }
                },
                {
                    "tachi", new Person()
                    {
                        Address = "Tokyo",
                        Age = 23,
                    }
                },
            };
            Assert.AreEqual(2, map3.Count);
            Assert.AreEqual("Tokyo", map3["tachi"].Address);

            // ちなみに、匿名クラスを使うとこうなる
            var map4 = new Dictionary<string, object>()
            {
                { 
                    "kami", new 
                    {
                        Address = "Osaka",
                        Age = 39,
                    }
                },
                { 
                    "tachi", new
                    {
                        Address = "Tokyo",
                        Age = 23,
                    }
                },
            };
            Debug.WriteLine(JsonConvert.SerializeObject(map4));
        }

        [TestMethod]
        public void ReferenceTest()
        {
            var map = new Dictionary<string, Person>()
            {
                {
                    "kami", new Person()
                    {
                        Address = "Osaka",
                        Age = 39,
                    }
                },
                {
                    "tachi", new Person()
                    {
                        Address = "Tokyo",
                        Age = 23,
                    }
                },
                {
                    "tachi2", new Person()
                    {
                        Address = "Tokyo",
                        Age = 23,
                    }
                },
            };

            Assert.IsFalse(ReferenceEquals(map["tachi"], map["tachi2"]));
            Assert.AreNotEqual(map["tachi"].GetHashCode(), map["tachi2"].GetHashCode());

            map.Add("tachi3", map["tachi"]);
            Debug.WriteLine(JsonConvert.SerializeObject(map));
            Assert.IsTrue(ReferenceEquals(map["tachi"], map["tachi3"]));
            Assert.AreEqual(map["tachi"].GetHashCode(), map["tachi3"].GetHashCode());

            map["tachi3"].Age = 24;
            Assert.AreEqual(24, map["tachi3"].Age);
            Assert.AreEqual(24, map["tachi"].Age);
            Assert.AreEqual(23, map["tachi2"].Age);

            var persons = map.Values;
            foreach (var person in persons)
            {
                person.Age = 40;
                break;
            }
            Assert.AreEqual(40, map["kami"].Age);

            var persons2 = map.Values.ToList();
            persons2[0].Age = 41;
            Assert.AreEqual(41, map["kami"].Age);

            var list = new List<Person>();
            foreach (var person in map.Values)
            {
                list.Add(person);
            }
            list[0].Age = 41;
            Assert.AreEqual(41, map["kami"].Age);

            void HappyBirthday(List<Person> persons)
            {
                persons.ForEach(person => person.Age += 1);
            }

            Debug.WriteLine($"Before: {JsonConvert.SerializeObject(map)}");
            Assert.AreEqual(41, map["kami"].Age);
            Assert.AreEqual(24, map["tachi"].Age);
            Assert.AreEqual(23, map["tachi2"].Age);
            Assert.AreEqual(24, map["tachi3"].Age);
            Assert.IsTrue(ReferenceEquals(map["tachi"], map["tachi3"]));

            HappyBirthday(map.Values.ToList());

            Debug.WriteLine($"After: {JsonConvert.SerializeObject(map)}");
            Assert.AreEqual(42, map["kami"].Age);
            Assert.AreEqual(26, map["tachi"].Age);
            Assert.AreEqual(24, map["tachi2"].Age);
            Assert.AreEqual(26, map["tachi3"].Age);
        }
    }
}
