using BusinessManagement.ADO;
using BusinessManagement.Models;
using Microsoft.AspNetCore.Mvc;

namespace BusinessManagement.Controllers
{
    public class UserController : Controller
    {

        User_ADO User_ADO=new User_ADO();

        #region ShowUser
        // GET action method for the Index page
        public IActionResult Index()
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                // User is not logged in or session has expired
                return RedirectToAction("Login", "Account"); // Redirect to the login page
            }
           
            List<User> users = new List<User>();
            try
            {
                users = User_ADO.GetAll();
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
            }

            return View(users);
        }
        #endregion ShowUser


        #region CreateUser
        // GET action method for the Create page
        [HttpGet]
        public IActionResult Create()
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                // User is not logged in or session has expired
                return RedirectToAction("Login", "Account"); // Redirect to the login page
            }

            var model = new User();
           
            return View(model);

        }

        // POST action method for the Create page
        [HttpPost]
        public IActionResult Create(User model)
        {
            try
            {
                var UID = HttpContext.Session.GetInt32("UserId");

                if (!ModelState.IsValid)
                {
                    TempData["errorMessage"] = "Model Data in Invalid";
                }
                bool result = User_ADO.Insert(model);
                if (!result)
                {
                    TempData["errorMessage"] = "Unable to save User.";
                }
                TempData["successMessage"] = "User Detail Saved.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {

                TempData["errorMessage"] = ex.Message;
                return View();
            }
        }
        #endregion CreateUser

        #region EditUser
        // GET action method for the Edit page
        [HttpGet]
        public IActionResult Edit(int ID)
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                // User is not logged in or session has expired
                return RedirectToAction("Login", "Account"); // Redirect to the login page
            }
            try
            {
                var model = new User();
                model = User_ADO.GetByID(ID);
               

                if (model.UserId == 0)
                {
                    TempData["errorMessage"] = $"User Detail not Found with id :{ID}";
                    return RedirectToAction("Index");
                }
                return View(model);
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return View();
            }

        }

        // POST action method for the Edit page
        [HttpPost]
        public IActionResult Edit(User model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    TempData["errorMessage"] = "Model Data in Invalid";

                }
                bool result = User_ADO.Update(model);
                if (!result)
                {
                    TempData["errorMessage"] = "Unable to Update Data ";
                    return View();
                }
                TempData["successMessage"] = "User Detail Updated";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return View();
            }

        }

        #endregion EditUser

        #region DeleteUser
        // GET action method for the Delete page
        [HttpGet]
        public IActionResult ConfirmDelete(int ID)
        {
            try
            {
                bool result = User_ADO.Delete(ID);
                if (!result)
                {
                    TempData["errorMessage"] = "Unable to delete User.";
                    return RedirectToAction("Index");
                }
                TempData["successMessage"] = "User deleted successfully!";
               
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");

        }

        #endregion DeleteUser

        #region ViewUser
        // GET action method for the View page
        [HttpGet]
        public IActionResult View(int ID)
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                // User is not logged in or session has expired
                return RedirectToAction("Login", "Account"); // Redirect to the login page
            }
            try
            {
                var model = new User();
                model = User_ADO.GetByID(ID);
              

                if (model.UserId == 0)
                {
                    TempData["errorMessage"] = $"User Detail not Found with id :{ID}";
                    return RedirectToAction("Index");
                }
                return View(model);
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return View();
            }

        }

        #endregion ViewUser

    }
}
