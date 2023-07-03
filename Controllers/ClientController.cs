using BusinessManagement.ADO;
using BusinessManagement.Models;
using Microsoft.AspNetCore.Mvc;

namespace BusinessManagement.Controllers
{
    public class ClientController : Controller
    {
        Client_ADO Clent_ADO=new Client_ADO();

        #region ShowClient
        // GET action method for the Index page
        [HttpGet]
        public IActionResult Index()
        {
            //Check Session Expired or not
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                // User is not logged in or session has expired
                return RedirectToAction("Login", "Account"); // Redirect to the login page
            }
            //Create A list to store All Clients
            List<Clients> clients = new List<Clients>();
            try
            {
                // Get all Clients from the data access object (ADO)
                clients = Clent_ADO.GetAll();
            }
            catch (Exception ex)
            {

                TempData["errorMseeage"] = ex.Message;
                return View();
            }

            return View(clients);
        }
        #endregion ShowClient

        #region CreateClient
        // GET action method for the Create page
        [HttpGet]
        public IActionResult Create()
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                // User is not logged in or session has expired
                return RedirectToAction("Login", "Account"); // Redirect to the login page
            }

            return View();

        }

        // POST action method for the Create page
        [HttpPost]
        public IActionResult Create(Clients model)
        {
            try
            {
                if (model.Name == null)
                {
                    TempData["errorMessage"] = "Please Enter Client Name.!";
                    return View();
                }
                if (model.Country == null)
                {
                    TempData["errorMessage"] = "Please Enter Country.!";
                    return View();
                }
                if (model.Address == null)
                {
                    TempData["errorMessage"] = "Please Enter Address.!";
                    return View();
                }
                if (model.Notes == null)
                {
                    TempData["errorMessage"] = "Please Leave Some Notes.!";
                    return View();
                }
                if (model.TimeZone == null)
                {
                    TempData["errorMessage"] = "Please Enter Time Zone.!";
                    return View();
                }
                if (!ModelState.IsValid)
                {
                    TempData["errorMessage"] = "Model Data in Invalid";
                }
                //Get UserId from Session 
                var UserId = HttpContext.Session.GetInt32("UserId");
                //Insert All data in to Data Base
                bool result = Clent_ADO.Insert(model, Convert.ToInt32(UserId));
                if (!result)
                {
                    TempData["errorMessage"] = "Unable to save Client.";
                }
                TempData["successMessage"] = "Client Detail Saved.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return View();
            }
        }
        #endregion CreateClient

        #region EditClient
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
                //Create Variable Model And Get Data by Id for Edit
                var model = new Clients();
                model = Clent_ADO.GetByID(ID);
              

                if (model.ClientId == 0)
                {
                    TempData["errorMessage"] = $"Client Detail not Found with id :{ID}";
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
        public IActionResult Edit(Clients model)
        {
            try
            {
                if(model.Name== null)
                {
                    TempData["errorMessage"] = "Please Enter Client Name.!";
                    return View();
                }
                if (model.Country == null)
                {
                    TempData["errorMessage"] = "Please Enter Country.!";
                    return View();
                }
                if (model.Address == null)
                {
                    TempData["errorMessage"] = "Please Enter Address.!";
                    return View();
                }
                if (model.Notes == null)
                {
                    TempData["errorMessage"] = "Please Leave Some Notes.!";
                    return View();
                }
                if (model.TimeZone == null)
                {
                    TempData["errorMessage"] = "Please Enter Time Zone.!";
                    return View();
                }
                if (!ModelState.IsValid)
                {
                    TempData["errorMessage"] = "Model Data in Invalid";

                }
                bool result = Clent_ADO.Update(model);
                if (!result)
                {
                    TempData["errorMessage"] = "Unable to Update Data ";
                    return View();
                }
                TempData["successMessage"] = "Client Detail Updated";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return View();
            }

        }
        #endregion EditClient

        #region DeleteClient
        // GET action method for the Delete page
        [HttpGet]
        public IActionResult ConfirmDelete(int ID)
        {
            try
            {
                var UserId = HttpContext.Session.GetInt32("UserId");


                bool result = Clent_ADO.Delete(ID, Convert.ToInt32(UserId));
                if (!result)
                {
                    TempData["errorMessage"] = "You have not permission to delete this client.";
                    return RedirectToAction("Index");
                }
                TempData["successMessage"] = "Client deleted successfully!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return RedirectToAction("Index");
            }

           
        }
        #endregion DeleteClient

        #region ViewClient
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
                var model = new Clients();
                model = Clent_ADO.GetByID(ID);
               

                if (model.ClientId == 0)
                {
                    TempData["errorMessage"] = $"Client Detail not Found with id :{ID}";
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

        #endregion ViewClient
    }
}
