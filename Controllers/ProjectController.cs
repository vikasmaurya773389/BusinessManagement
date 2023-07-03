using BusinessManagement.ADO;
using BusinessManagement.Models;
using Microsoft.AspNetCore.Mvc;

namespace BusinessManagement.Controllers
{
    public class ProjectController : Controller
    {

        Project_ADO project_ADO = new Project_ADO();

        #region ShowProject
        [HttpGet]
        public IActionResult Index()
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                // User is not logged in or session has expired
                return RedirectToAction("Login", "Account"); // Redirect to the login page
            }
            List<Project> projects = new List<Project>();
            try
            {
                projects = project_ADO.GetAll();
            }
            catch (Exception ex)
            {

                TempData["errorMseeage"] = ex.Message;
                return View();
            }

            return View(projects);
        }
        #endregion ShowProject
    }
}
