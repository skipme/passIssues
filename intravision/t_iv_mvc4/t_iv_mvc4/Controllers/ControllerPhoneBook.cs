using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace t_iv_mvc4.Controllers
{
    public class PhoneBookController : Controller
    {
        PhoneBook.PhoneBookAccess access = new PhoneBook.PhoneBookAccess();
        //
        // GET: /ControllerPhoneBook/

        public ActionResult Index(string filter = null)
        {
            if (filter == null)
                ViewBag.List = access.GetData();
            else ViewBag.List = access.GetDataFilter(filter);
            return View();
        }
        [HttpGet]
        public ActionResult EditAdd(int recid = -1)
        {
            if (recid > 0)
            {
                ViewBag.record = access.TakeRecord(recid);
            }
            return View();
        }

        public ActionResult CSV()
        {
            return File(access.GetCsvData(), "text/csv", string.Format("phonebook_{0}.csv", DateTime.Now.ToShortDateString()));
        }
        [HttpPost]
        public ActionResult EditAdd(string fam_name, string name, string tel, int recid = -1)
        {
            if (recid > 0)
            {
                access.ModifyRecord(recid, name, fam_name, tel);
            }
            else
            {
                recid = access.AddRecord(name, fam_name, tel);
                return Redirect(Url.Action("EditAdd", new { recid = recid }));
            }
            return Redirect(Url.Action("Index"));
        }
    }
}
