# CSharp入門勉強会

## ハイライト

### 2020/8/19:CollectionTest.cs

        [TestMethod]
        public void CollectionTest()
        {
            void AddB(List<string> list)
            {
                list.Add("b");
            }
            var array = new string[] { "a" };
            AddB(array.ToList());
            Assert.AreEqual(1, array.Count(), "arrayをToListした時点で新しいコレクションに生まれ変わっているので、arrayの数は増えない");
        }

### 2020/8/18:StringBuilderTest.cs

            int numberOfItem = 4000;
            Debug.WriteLine($"{numberOfItem}個"); // 先頭に$をつけた文字列には変数の値を直接埋め込める
            Debug.WriteLine($"{numberOfItem:#,##0}個"); // 書式を指定することもできる