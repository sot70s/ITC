using ITC.Models;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Linq;
using System.Web.Mvc;

namespace ITC.Controllers
{
    public class AccountController : Controller
    {
        public ActionResult ChangePassword(string id)
        {
            AccountJoinEmployee query = QueryAccount.ListAccountMeyer().Where(w => w.Id == id).FirstOrDefault();
            ViewBag.BindEmployeeName = query.EMPLOYEE_NAME;
            return View();
        }

        [HttpPost]
        public JsonResult SetPassword(ParameterAccount cc)
        {
            bool status = false;
            var msg = string.Empty;
            MILAuthContext _db = new MILAuthContext();
            PasswordHasher hasher = new PasswordHasher();
            Accounts query = _db.Accounts.Where(s => s.Id == cc.Id).FirstOrDefault();
            if (cc.Password == cc.ConfirmPassword)
            {
                var _pwd = hasher.GenerateIdentityV3Hash(cc.ConfirmPassword, KeyDerivationPrf.HMACSHA1, 10000, 16);

                status = true;
                msg = "Successful";
                query.PasswordHash = _pwd;
                _db.SaveChanges();
            }
            else {
                status = false;
                msg = "Please check your password";
            }

            return Json(new { success = status, message = msg });
        }
    }
}