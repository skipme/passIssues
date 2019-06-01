using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DMIR_TEST.Areas.TEST.Controllers
{
    [Authorize]
    public class EmployeeController : Controller
    {
        //
        // GET: /TEST/Employee/

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Edit()
        {
            backend.Objects.Employee model = backend.Objects.Employee.Get(DUser).FirstOrDefault();

            if (model == null)
                return Content("Error");

            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(int id, string FirstName, string Email, string FamilyName, string LastName)
        {
            backend.Objects.Employee modelx = backend.Objects.Employee.Get(DUser).FirstOrDefault();

            if (modelx == null || id != modelx.Id)
                return Content("Error");

            modelx.FamilyName = FamilyName;
            modelx.Name = FirstName;
            modelx.LastName = LastName;
            modelx.Email = Email;

            backend.Objects.Employee.Update(modelx);

            return RedirectToAction("Edit");
        }

        [HttpGet]
        public ActionResult List()
        {
            //var model = backend.Objects.Employee.All();

            //if (model == null)
            //    return Content("Error");

            return View();
        }

        public ActionResult Fetch(int page, string srch_name, string srch_dep)
        {
            ArrayList rows = new ArrayList();
            int pages = 0;
            IEnumerable<backend.Objects.Employee> dpage;
            if (string.IsNullOrWhiteSpace(srch_name) && string.IsNullOrWhiteSpace(srch_dep))
            {
                dpage = backend.Objects.Employee.Page(page, out pages);
            }
            else
            {
                dpage = backend.Objects.Employee.PageSearchName(srch_name, srch_dep, page, out pages);
            }

            foreach (var item in dpage)
            {
                rows.Add(new { id = item.Id, cell = new object[] { item.Name, item.FamilyName, item.LastName, item.Department, item.Position, item.Salary.ToString("### ### ##0.00", System.Globalization.CultureInfo.InvariantCulture) + "р." } });
            }
            return new JsonResult() { Data = new { page = page, total = pages, records = "1", rows = rows }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }

        public new backend.Objects.User DUser
        {
            get
            {
                return backend.Objects.User.Get(User.Identity.Name);
            }
        }
    }
}
