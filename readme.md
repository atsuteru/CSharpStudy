# CSharp入門勉強会

## ハイライト

### 2020/8/25:SQLTest.cs - Period 2: Query

        [TestMethod]
        public void Select()
        {
            using (var dbInstance = new EmployeeDbContext(
                new DbContextOptionsBuilder()
                    .UseSqlServer(@"Server=(LocalDb)\MSSQLLocalDB;Database=master;Trusted_Connection=True")
                    .Options))
            {
                dbInstance.Database.OpenConnection();

                // with Entity
                var employeeList = dbInstance.Employees
                    .Where(x => x.EmployeeAge > 25)
                    .ToList();

                // use Linq to SQL
                var querable = 
                    from emp in dbInstance.Employees
                    where emp.EmployeeAge > 25
                    select emp;
                employeeList = querable.ToList();

                // join example of Linq to SQL
                var querable =
                    from emp in dbInstance.Employees
                    join div in dbInstance.Divisions
                    on emp.DivisionCode equals div.DivisionCode 
                    into divJoinCondition from divOuter in divJoinCondition.DefaultIfEmpty()
                    where emp.EmployeeAge > 25
                    select new Employee()
                    {
                        EmployeeId = emp.EmployeeId,
                        EmployeeName = emp.EmployeeName,
                        EmployeeAge = emp.EmployeeAge,
                        DivisionCode = emp.DivisionCode,
                        Division = divOuter
                    };
                employeeList = querable.ToList();
            }
        }

        class EmployeeDbContext: DbContext
        {
            public DbSet<Employee> Employees { get; set; }
            public DbSet<Division> Divisions { get; set; }
            public EmployeeDbContext(DbContextOptions options) : base(options)
            {
            }
            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                base.OnModelCreating(modelBuilder);
                modelBuilder.ApplyConfiguration(new Employee_Mapping());
                modelBuilder.ApplyConfiguration(new Division_Mapping());
            }
        }

### 2020/8/25:SQLTest.cs - Period 1: Insert

        [TestMethod]
        public void Insert()
        {
            var inputName = "kami's";
            var inputAge = 39;
            var inputDivision = 113;

            // by SqlRaw
            using (var dbInstance = new DbContext(
                new DbContextOptionsBuilder()
                    .UseSqlServer(@"Server=(LocalDb)\MSSQLLocalDB;Database=master;Trusted_Connection=True")
                    .Options))
            {
                dbInstance.Database.OpenConnection();

                // no-bind
                dbInstance.Database.ExecuteSqlRaw(
                    "INSERT INTO EMPLOYEE"
                    + $" VALUES( '{Guid.NewGuid().ToString()}'"
                    + $", '{inputName}'" // sql injection error!!
                    + $", {inputAge}"
                    + $", {inputDivision}"
                    + ")"
                    );

                // use Bind
                dbInstance.Database.ExecuteSqlRaw(
                    "INSERT INTO EMPLOYEE"
                    + $" VALUES( @Id"
                    + $", @Name"
                    + $", @Age"
                    + $", @Division"
                    + ")"
                    , new SqlParameter("Id", Guid.NewGuid())
                    , new SqlParameter("Name", inputName)
                    , new SqlParameter("Age", inputAge)
                    , new SqlParameter("Division", inputDivision)
                    );
            }

            // by Entity (Framework Core)
            using (var dbInstance = new EmployeeDbContext(
                new DbContextOptionsBuilder()
                    .UseSqlServer(@"Server=(LocalDb)\MSSQLLocalDB;Database=master;Trusted_Connection=True")
                    .Options))
            {
                dbInstance.Database.OpenConnection();
                dbInstance.Employees.Add(new Employee()
                {
                    EmployeeId = Guid.NewGuid(),
                    EmployeeName = inputName,
                    EmployeeAge = inputAge,
                    DivisionCode = inputDivision
                });
                dbInstance.SaveChanges();
            }
        }

        class EmployeeDbContext: DbContext
        {
            public DbSet<Employee> Employees { get; set; }
            public EmployeeDbContext(DbContextOptions options) : base(options)
            {
            }
            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                base.OnModelCreating(modelBuilder);
                modelBuilder.ApplyConfiguration(new Employee_Mapping());
            }
        }

### 2020/8/21:HttpClientTest.cs

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
            using (var result = getTask.Result)
            {
                Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
                var readBodyTask = result.Content.ReadAsStringAsync();
                var bodyData = readBodyTask.Result;

                var body = JsonConvert.DeserializeObject<GetDeltaBody>(bodyData);
                Assert.IsTrue(body.VersionNo > 0);
                Assert.IsTrue(body.DeltaData.Length > 0);
            }
        }

        // === 以下、Taskを理解するために ===
        class Cake
        {
            public string Sponge { get; set; }
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

            var bakeTheCakeTask = BakeTheCakeAsync(); // Taskの処理が開始されるが、完了は待たない
            var cake = bakeTheCakeTask.Result; // ResultはTaskの処理完了まで待ってくれるプロパティ
            Assert.AreEqual("Biscuit dough", cake.Sponge);
        }


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