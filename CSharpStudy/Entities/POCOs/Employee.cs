using System;

namespace CSharpStudy.Entities
{
    public partial class Employee
    {
        public Guid EmployeeId { get; set; }

        public string EmployeeName { get; set; }

        public int? EmployeeAge { get; set; }

        public int? DivisionCode { get; set; }

        public Division Division { get; set; }
    }
    
}
