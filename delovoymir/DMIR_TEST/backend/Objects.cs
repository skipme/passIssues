using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace backend
{
    public partial class Objects
    {
        public class User
        {
            public string Login { get; set; }
            public string Password { get; set; }

            public string Email { get; set; }
            public int Id { get; private set; }

            internal static User FromEntity(dynamic usr)
            {
                if (usr == null)
                    return usr;

                return new User() { Login = usr.ACLU_LOGIN, Password = usr.ACLU_PASSWORD, Id = usr.ACL_USER_ID, Email = usr.ACLU_EMAIL };
            }

            public static User Get(int id)
            {
                var q = new Entities.User();

                var u = q.All(where: "WHERE ACL_USER_ID = @0", args: id).FirstOrDefault();

                return FromEntity(u);
            }
            public static User Get(string login, string password)
            {
                var q = new Entities.User();

                var u = q.All(where: "WHERE ACLU_LOGIN = @0 AND ACLU_PASSWORD = @1", args: new object[] { login, password }).FirstOrDefault();

                return FromEntity(u);
            }
            public static User GetByEmail(string email, string password)
            {
                var q = new Entities.User();

                var u = q.All(where: "WHERE ACLU_EMAIL = @0 AND ACLU_PASSWORD = @1", args: new object[] { email, password }).FirstOrDefault();

                return FromEntity(u);
            }
            public static User GetByEmail(string email)
            {
                var q = new Entities.User();

                var u = q.All(where: "WHERE ACLU_EMAIL = @0", args: email).FirstOrDefault();

                return FromEntity(u);
            }
            public static User Get(string login)
            {
                var q = new Entities.User();

                var u = q.All(where: "WHERE ACLU_LOGIN = @0 ", args: login).FirstOrDefault();

                return FromEntity(u);
            }
            public static User Insert(User u)
            {
                var q = new Entities.User();
                q.Insert(new { ACLU_LOGIN = u.Login, ACLU_PASSWORD = u.Password, ACLU_EMAIL = u.Email });
                return Get(u.Login);
            }
            public static bool Update(User u)
            {
                var q = new Entities.User();
                var usr = q.All(where: "WHERE ACL_USER_ID = @0", args: u.Id).FirstOrDefault();

                if (usr == null)
                    return false;

                q.Update(new { ACLU_LOGIN = u.Login, ACLU_PASSWORD = u.Password, ACLU_EMAIL = u.Email }, u.Id);
                return true;
            }
        }

        public class Employee
        {

            public string Name { get; set; }
            public string FamilyName { get; set; }
            public string LastName { get; set; }

            public string Department { get; private set; }
            public string Position { get; private set; }
            public decimal Salary { get; private set; }
            public string Email { get; set; }

            public int Id { get; private set; }
            public int UserId { get; private set; }

            internal static Employee FromEntity(dynamic emp)
            {
                if (emp == null)
                    return emp;

                return new Employee() { Name = emp.HRE_FIRST_NAME, FamilyName = emp.HRE_FAMILY_NAME, LastName = emp.HRE_LAST_NAME, Department = emp.HRD_NAME, Position = emp.HRP_NAME, Salary = emp.HRS_SUM, Id = emp.HRE_ID, UserId = emp.ACL_USER_ID, Email = emp.ACLU_EMAIL };
            }
            public static Employee[] Get(User user)
            {
                var q = new Entities.Employee();

                var u = q.Query(@"select e.HRE_FIRST_NAME, e.HRE_FAMILY_NAME, e.HRE_LAST_NAME, d.HRD_NAME, p.HRP_NAME, s.HRS_SUM, e.HRE_ID, e.ACL_USER_ID, u.ACLU_EMAIL from 
ACL_USER u, HR_EMPLOYEE e, HR_POSITION p, HR_DEPARTMENT d, HR_SALARY s
where u.ACL_USER_ID = @0 and e.ACL_USER_ID = u.ACL_USER_ID and p.HRP_ID = e.HRP_ID and d.HRD_ID = e.HRD_ID and s.HRD_ID = e.HRD_ID and s.HRP_ID = e.HRP_ID 
", args: user.Id);
                return (from d in u select (Employee)FromEntity(d)).ToArray();
            }
            public static Employee Get(int id)
            {
                var q = new Entities.Employee();

                var u = q.Query(@"select e.HRE_FIRST_NAME, e.HRE_FAMILY_NAME, e.HRE_LAST_NAME, d.HRD_NAME, p.HRP_NAME, s.HRS_SUM, e.HRE_ID, e.ACL_USER_ID, u.ACLU_EMAIL from 
HR_EMPLOYEE e, HR_POSITION p, HR_DEPARTMENT d, HR_SALARY s, ACL_USER u
where e.HRE_ID = @0 and p.HRP_ID = e.HRP_ID and d.HRD_ID = e.HRD_ID and s.HRD_ID = e.HRD_ID and s.HRP_ID = e.HRP_ID and u.ACL_USER_ID = e.ACL_USER_ID
", args: id).FirstOrDefault();
                return FromEntity(u);
            }
            public static Employee[] Get(string login)
            {
                var q = new Entities.Employee();

                var u = q.Query(@"select e.HRE_FIRST_NAME, e.HRE_FAMILY_NAME, e.HRE_LAST_NAME, d.HRD_NAME, p.HRP_NAME, s.HRS_SUM, e.HRE_ID, e.ACL_USER_ID, u.ACLU_EMAIL from 
ACL_USER u, HR_EMPLOYEE e, HR_POSITION p, HR_DEPARTMENT d, HR_SALARY s
where u.ACLU_LOGIN = @0 and e.ACL_USER_ID = u.ACL_USER_ID and p.HRP_ID = e.HRP_ID and d.HRD_ID = e.HRD_ID and s.HRD_ID = e.HRD_ID and s.HRP_ID = e.HRP_ID 
", args: login);

                return (from d in u select (Employee)FromEntity(d)).ToArray();
            }
            public static IEnumerable<Employee> All()
            {
                var q = new Entities.Employee();

                var u = q.Query(@"select e.HRE_FIRST_NAME, e.HRE_FAMILY_NAME, e.HRE_LAST_NAME, d.HRD_NAME, p.HRP_NAME, s.HRS_SUM, e.HRE_ID, e.ACL_USER_ID, u.ACLU_EMAIL from 
HR_EMPLOYEE e, HR_POSITION p, HR_DEPARTMENT d, HR_SALARY s, ACL_USER u
where p.HRP_ID = e.HRP_ID and d.HRD_ID = e.HRD_ID and s.HRD_ID = e.HRD_ID and s.HRP_ID = e.HRP_ID and u.ACL_USER_ID = e.ACL_USER_ID
");
                return (from d in u select (Employee)FromEntity(d)).AsEnumerable<Employee>();
            }
            public static long CountAll()
            {
                var q = new Entities.Employee();

                var u = q.Scalar(@"select count(*) from 
HR_EMPLOYEE e, HR_POSITION p, HR_DEPARTMENT d, HR_SALARY s, ACL_USER u
where p.HRP_ID = e.HRP_ID and d.HRD_ID = e.HRD_ID and s.HRD_ID = e.HRD_ID and s.HRP_ID = e.HRP_ID and u.ACL_USER_ID = e.ACL_USER_ID
");
                return (long)u;
            }
            public static IEnumerable<Employee> Page(int now, out int pages, int max = 10)
            {
                var q = new Entities.Employee();

                var u = q.Paged(sql: @"select e.HRE_FIRST_NAME, e.HRE_FAMILY_NAME, e.HRE_LAST_NAME, d.HRD_NAME, p.HRP_NAME, s.HRS_SUM, e.HRE_ID, e.ACL_USER_ID, u.ACLU_EMAIL from 
HR_EMPLOYEE e, HR_POSITION p, HR_DEPARTMENT d, HR_SALARY s, ACL_USER u
where p.HRP_ID = e.HRP_ID and d.HRD_ID = e.HRD_ID and s.HRD_ID = e.HRD_ID and s.HRP_ID = e.HRP_ID and u.ACL_USER_ID = e.ACL_USER_ID
", primaryKey: "HRE_FAMILY_NAME", pageSize: max, currentPage: now);

                pages = u.TotalPages;
                return (from d in (IEnumerable<dynamic>)u.Items select (Employee)FromEntity(d)).AsEnumerable<Employee>();
            }
            public static IEnumerable<Employee> PageSearchName(string nameStartsWith, string departmentStartsWith, int now, out int pages, int max = 10)
            {
                var q = new Entities.Employee();
                string sql = @"select e.HRE_FIRST_NAME, e.HRE_FAMILY_NAME, e.HRE_LAST_NAME, d.HRD_NAME, p.HRP_NAME, s.HRS_SUM, e.HRE_ID, e.ACL_USER_ID, u.ACLU_EMAIL from 
HR_EMPLOYEE e, HR_POSITION p, HR_DEPARTMENT d, HR_SALARY s, ACL_USER u
where $inc1
 p.HRP_ID = e.HRP_ID and d.HRD_ID = e.HRD_ID  $inc2 and s.HRD_ID = e.HRD_ID and s.HRP_ID = e.HRP_ID and u.ACL_USER_ID = e.ACL_USER_ID
";
                string inc1 = "(e.HRE_FIRST_NAME like @0 or e.HRE_FAMILY_NAME like @0 or e.HRE_LAST_NAME like @0 ) and ";
                string inc2 = "and ( d.HRD_NAME like @1 )";
                sql = sql.Replace("$inc1", string.IsNullOrWhiteSpace(nameStartsWith) ? "" : inc1);
                sql = sql.Replace("$inc2", string.IsNullOrWhiteSpace(departmentStartsWith) ? "" : inc2);

                var u = q.Paged(sql: sql, primaryKey: "HRE_FAMILY_NAME", pageSize: max, currentPage: now, args: new object[] { nameStartsWith + "%", departmentStartsWith + "%" });


                pages = u.TotalPages;
                return (from d in (IEnumerable<dynamic>)u.Items select (Employee)FromEntity(d)).AsEnumerable<Employee>();
            }
            public static bool Update(Employee u)
            {
                User usr = User.Get(u.UserId);
                usr.Email = u.Email;

                if (!User.Update(usr))
                    return false;

                var q = new Entities.Employee();
                var emp = q.All(where: "WHERE HRE_ID = @0", args: u.Id).FirstOrDefault();

                if (emp == null)
                    return false;

                q.Update(new { HRE_FIRST_NAME = u.Name, HRE_FAMILY_NAME = u.FamilyName, HRE_LAST_NAME = u.LastName }, u.Id);
                return true;
            }

            public static Employee Insert(Employee u)
            {
                var q = new Entities.Employee();
                q.Insert(new { HRE_FIRST_NAME = u.Name, HRE_FAMILY_NAME = u.FamilyName, HRE_LAST_NAME = u.LastName, ACL_USER_ID = u.UserId, HRP_ID = 1, HRD_ID = 1 });
                return Get(User.Get(u.UserId)).FirstOrDefault();
            }
            public static Employee Insert(User user, Employee u)
            {
                var q = new Entities.Employee();
                q.Insert(new { HRE_FIRST_NAME = u.Name, HRE_FAMILY_NAME = u.FamilyName, HRE_LAST_NAME = u.LastName, ACL_USER_ID = user.Id, HRP_ID = 1, HRD_ID = 1 });
                return Get(user).FirstOrDefault();
            }
        }
    }
}
