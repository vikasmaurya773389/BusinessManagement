using Microsoft.AspNetCore.Mvc;
using BusinessManagement.ADO;
using BusinessManagement.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using NuGet.Packaging.Signing;
using System.Reflection;
using Microsoft.CodeAnalysis.Scripting;

namespace BusinessManagement.Controllers
{
    public class BidsController : Controller
    {
        Bids_ADO bids_ADO = new Bids_ADO();

        #region ShowBids
        // GET action method for the Index page
        [HttpGet]
        public IActionResult Index()
        {
            try
            {
                // Check if the user is logged in or if the session has expired
                if (HttpContext.Session.GetInt32("UserId") == null)
                {
                    // User is not logged in or session has expired
                    return RedirectToAction("Login", "Account"); // Redirect to the login page
                }
                var UserId = HttpContext.Session.GetInt32("UserId");
                var model = new all();
               
                
                try
                {
                    // Get all bids and bidders from the data access object (ADO)
                    model.bids = bids_ADO.GetAll(Convert.ToInt32(UserId));
                    model.biders= bids_ADO.GetBider();
                }
                catch (Exception ex)
                {
                    TempData["errorMessage"] = ex.Message;
                    return View();
                }

                return View(model);
            }
            catch (Exception ex)
            {

                TempData["errorMessage"] = ex.Message;
                return View();
            }
        }

        #endregion ShowBids

        #region CreateBid
        // GET action method for the Create page
        [HttpGet]
        public IActionResult Create()
        {
            try
            {
                // Check if the user is logged in or if the session has expired
                if (HttpContext.Session.GetInt32("UserId") == null)
                {
                    // User is not logged in or session has expired
                    return RedirectToAction("Login", "Account"); // Redirect to the login page
                }

                var model = new Bids();
                model.Clients = bids_ADO.GetClients();
                model.Response = bids_ADO.GetResponse();
                return View(model);
            }
            catch (Exception ex)
            {

                TempData["errorMessage"] = ex.Message;
                return View();
            }

        }
        // POST action method for the Create page
        [HttpPost]
        public IActionResult Create(Bids model)
        {
            try
            {
                var UID = HttpContext.Session.GetInt32("UserId");
                if (model.ClientId == "0")
                {
                    TempData["errorMessage"] = "Please Select Client";
                }
                if (!ModelState.IsValid)
                {
                    TempData["errorMessage"] = "Model Data in Invalid";
                }
                bool result = bids_ADO.Insert(model, Convert.ToInt32(UID));
                if (!result)
                {
                    TempData["errorMessage"] = "Unable to save Bids.";
                }
                TempData["successMessage"] = "Bids Detail Saved.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {

                TempData["errorMessage"] = ex.Message;
                return View();
            }
        }
        #endregion CreateBid

        #region EditBid
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
                var model = new Bids();
                model = bids_ADO.GetByID(ID);
                model.Clients = bids_ADO.GetClients();
                model.Response = bids_ADO.GetResponse();
               
                if (model.BidId == 0)
                {
                    TempData["errorMessage"] = $"Bids Detail not Found with id :{ID}";
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
        public IActionResult Edit(Bids model)
        {
            try
            {
                
                model.Clients = bids_ADO.GetClients();
                model.Response = bids_ADO.GetResponse();
                if (model.ClientId == "0")
                {
                    TempData["errorMessage"] = "Please Select Client";
                    return View(model);
                }
                if (model.BidResponseId == "0")
                {
                    TempData["errorMessage"] = "Please Select Bid Response";
                    return View(model);
                }
                if (!ModelState.IsValid)
                {
                    TempData["errorMessage"] = "Model Data in Invalid";
                   
                }
                bool result = bids_ADO.Update(model);
                if (!result)
                {
                    TempData["errorMessage"] = "Unable to Update Data ";
                }
                TempData["successMessage"] = "Bids Detail Updated";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return View();
            }

        }

        #endregion EditBid

        #region DeleteBid
        // GET action method for the Delete page
        [HttpGet]
        public IActionResult ConfirmDelete(int ID)
        {
            try
            {
                bool result = bids_ADO.Delete(ID);
                if (!result)
                {
                    TempData["errorMessage"] = "Unable to delete bid.";
                }
                TempData["successMessage"] = "Bid deleted successfully!";
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }

        #endregion DeleteBid

        #region ViewBid
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
                var model = new Bids();
                model = bids_ADO.GetByID(ID);
                model.Clients = bids_ADO.GetClients();
                model.Response = bids_ADO.GetResponse();

                if (model.BidId == 0)
                {
                    TempData["errorMessage"] = $"Bids Detail not Found with id :{ID}";
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

        #endregion ViewBid

    }
}
