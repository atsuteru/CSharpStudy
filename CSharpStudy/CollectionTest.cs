using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace CSharpStudy
{
    [TestClass]
    public class CollectionTest
    {
        [TestMethod]
        public void CollectionConvertTest()
        {
            // リスト：作業中のリスト
            var list = new List<string>();
            list.Add("a");
            list.Add("b");
            Assert.AreEqual(2, list.Count);
            var listArray = list.ToArray();
            Assert.AreEqual(2, listArray.Length);

            // 配列：完成したリスト
            var array = new string[] 
            {
                "a",
                "b"
            };
            Assert.AreEqual(2, array.Length);
            var arrayList = array.ToList();
            arrayList.Add("c");
            Assert.AreEqual(3, arrayList.Count);
        }

        [TestMethod]
        public void CollectionCastTest()
        {
            var listString = new List<string>();
            listString.Add("a");
            foreach (var item in listString)
            {
                Assert.IsTrue(item is string);
            }

            var listObject = new List<object>();
            listObject.Add(9);
            foreach (var item in listObject)
            {
                try
                {
                    var itemString = (string)item;
                    Assert.Fail("ここに来たらテスト失敗");
                }
                catch (InvalidCastException)
                {
                }
            }

            foreach (var item in listObject)
            {
                var itemString = item as string;
                Assert.IsNull(itemString);
            }

            List<int> listInt = new List<int>();
            foreach (var item in listObject)
            {
                listInt.Add((int)item);
            }
            Assert.AreEqual(1, listInt.Count);
            Assert.AreEqual(9, listInt[0]);

            List<int> listIntByLinq = listObject.Select(item => (int)item).ToList();
            Assert.AreEqual(1, listIntByLinq.Count());
            Assert.AreEqual(9, listIntByLinq.First());
        }

        [TestMethod]
        public void CollectionArgumentTest()
        {
            void AddB(List<string> list)
            {
                list.Add("b");
            }

            var list = new List<string>();
            list.Add("a");
            AddB(list);

            Assert.AreEqual(2, list.Count());

            void ChangeFirstToB(List<string> list)
            {
                list[0] = "b";
            }

            ChangeFirstToB(list);
            Assert.AreEqual(2, list.Count());
            Assert.AreEqual("b", list.First());

            var array = new string[] {"a"};
            AddB(array.ToList());
            //var listFromArray = array.ToList();
            //var listFromArray = new List<string>();
            //foreach (var item in array)
            //{
            //    listFromArray.Add(item);
            //}
            //AddB(listFromArray);
            Assert.AreEqual(1, array.Count());
        }
    }
}
