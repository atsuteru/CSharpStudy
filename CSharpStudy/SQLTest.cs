using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

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
        #endregion
    }
}
