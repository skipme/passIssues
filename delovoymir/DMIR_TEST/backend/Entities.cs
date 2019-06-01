using Massive;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace backend
{
    public partial class Entities
    {
        const string DB = "test";
       
        public class User : DynamicModel
        {
            public User() : base(DB, "ACL_USER", "ACL_USER_ID") { }
        }

        public class Position : DynamicModel
        {
            public Position() : base(DB, "HR_POSITION", "HRP_ID") { }
        }

        public class Department : DynamicModel
        {
            public Department() : base(DB, "HR_DEPARTMENT", "HRD_ID") { }
        }

        public class Employee : DynamicModel
        {
            public Employee() : base(DB, "HR_EMPLOYEE", "HRE_ID") { }
        }

        public class Salary : DynamicModel
        {
            public Salary() : base(DB, "HR_SALARY") { }
        }
        
    }
}
