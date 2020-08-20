# CSharp入門勉強会

## ハイライト

### 2020/8/20:DictionaryTest.cs

        class Person
        {
            public string Address { get; set; }
            public int Age { get; set; }
        }

        [TestMethod]
        public void DictionaryTest()
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
            map.Add("tachi3", map["tachi"]);

            void HappyBirthday(List<Person> persons)
            {
                persons.ForEach(person => person.Age += 1);
            }

            Debug.WriteLine($"Before: {JsonConvert.SerializeObject(map)}");
            // Before: { "kami":{ "Address":"Osaka","Age":39},"tachi":{ "Address":"Tokyo","Age":23},"tachi2":{ "Address":"Tokyo","Age":23},"tachi3":{ "Address":"Tokyo","Age":23} }
            Assert.AreEqual(39, map["kami"].Age);
            Assert.AreEqual(23, map["tachi"].Age);
            Assert.AreEqual(23, map["tachi2"].Age);
            Assert.AreEqual(23, map["tachi3"].Age);
            Assert.IsTrue(ReferenceEquals(map["tachi"], map["tachi3"]));

            HappyBirthday(map.Values.ToList());

            Debug.WriteLine($"After: {JsonConvert.SerializeObject(map)}");
            // After: { "kami":{ "Address":"Osaka","Age":40},"tachi":{ "Address":"Tokyo","Age":25},"tachi2":{ "Address":"Tokyo","Age":24},"tachi3":{ "Address":"Tokyo","Age":25} }
            Assert.AreEqual(40, map["kami"].Age);
            Assert.AreEqual(25, map["tachi"].Age);
            Assert.AreEqual(24, map["tachi2"].Age);
            Assert.AreEqual(25, map["tachi3"].Age);

            // map.ValuesをToListすると"器"はnewされるが、その中身であるPersonはnewされない(mapとlistが同じ参照を共有する)
            // tachiとtachi3は同じPersonの参照を共有しているので、どちらかの年齢が加算されるともう一方も同時に加算される
        }

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