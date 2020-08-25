using CSharpStudy.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace CSharpStudy
{
    [Microsoft.VisualStudio.TestTools.UnitTesting.TestClass]
    public class SQLTest
    {
        #region DDL

        [TestMethod]
        public void CreateTable()
        {
            using (var dbInstance = new DbContext(
                new DbContextOptionsBuilder()
                    .UseSqlServer(@"Server=(LocalDb)\MSSQLLocalDB;Database=master;Trusted_Connection=True")
                    .Options))
            {
                dbInstance.Database.OpenConnection();
                dbInstance.Database.ExecuteSqlRaw(
                    $"CREATE TABLE EMPlOYEE_{DateTime.Now:yyyyMMdd_HHmmss}"
                    + "( EmployeeId UNIQUEIDENTIFIER PRIMARY KEY"
                    + ", EmployeeName nvarchar(255)"
                    + ", EmployeeAge int"
                    + ", DivisionCode int"
                    + ")"
                    );
            }
        }

        #endregion

        #region DML
        [TestMethod]
        public void InsertWithNoBind()
        {
            var inputName = "kami''s";
            var inputAge = 39;
            var inputDivision = 113;

            using (var dbInstance = new DbContext(
                new DbContextOptionsBuilder()
                    .UseSqlServer(@"Server=(LocalDb)\MSSQLLocalDB;Database=master;Trusted_Connection=True")
                    .Options))
            {
                dbInstance.Database.OpenConnection();
                dbInstance.Database.ExecuteSqlRaw(
                    "INSERT INTO EMPLOYEE"
                    + $" VALUES( '{Guid.NewGuid().ToString()}'"
                    + $", '{inputName}'"
                    + $", {inputAge}"
                    + $", {inputDivision}"
                    + ")"
                    );
            }
        }

        [TestMethod]
        public void InsertWithBind()
        {
            var inputName = "kami's";
            var inputAge = 39;
            var inputDivision = 113;


            using (var dbInstance = new DbContext(
                new DbContextOptionsBuilder()
                    .UseSqlServer(@"Server=(LocalDb)\MSSQLLocalDB;Database=master;Trusted_Connection=True")
                    .Options))
            {
                dbInstance.Database.OpenConnection();
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

        [TestMethod]
        public void InsertWithEntity()
        {
            var inputName = "kami's";
            var inputAge = 39;
            var inputDivision = 113;

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


        [TestMethod]
        public void SelectWithEntity()
        {
            using (var dbInstance = new EmployeeDbContext(
                new DbContextOptionsBuilder()
                    .UseSqlServer(@"Server=(LocalDb)\MSSQLLocalDB;Database=master;Trusted_Connection=True")
                    .Options))
            {
                dbInstance.Database.OpenConnection();

                var employeeList = dbInstance.Employees
                    .Where(x => x.EmployeeAge > 25)
                    .ToList();
                //var dbSet = dbInstance.Employees;
                //var querable = dbSet.Where(x => x.EmployeeAge > 25);
                //var employeeList = querable.ToList();
            }
        }

        [TestMethod]
        public void SelectWithLinqToSQL()
        {
            using (var dbInstance = new EmployeeDbContext(
                new DbContextOptionsBuilder()
                    .UseSqlServer(@"Server=(LocalDb)\MSSQLLocalDB;Database=master;Trusted_Connection=True")
                    .Options))
            {
                dbInstance.Database.OpenConnection();
                var querable = 
                    from emp in dbInstance.Employees
                    where emp.EmployeeAge > 25
                    select emp;
                var employeeList = querable.ToList();
            }
        }

        [TestMethod]
        public void Select()
        {
            using (var dbInstance = new EmployeeDbContext(
                new DbContextOptionsBuilder()
                    .UseSqlServer(@"Server=(LocalDb)\MSSQLLocalDB;Database=master;Trusted_Connection=True")
                    .Options))
            {
                dbInstance.Database.OpenConnection();
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
                var employeeList = querable.ToList();
            }
        }

        #endregion


    }
}
