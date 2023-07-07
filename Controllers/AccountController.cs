using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;
using Microsoft.Extensions.Configuration;
using BusinessManagement.Models;
using Microsoft.AspNetCore.Mvc;
using BusinessManagement.ADO;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication;
using System.Data.SqlClient;
using System.Data;
using System.Security.Claims;

namespace BusinessManagement.Controllers
{
    public class AccountController : Controller
    {
        AccountLogin account = new AccountLogin();


        private readonly IConfiguration _configuration;

        public AccountController(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        #region DashBoard
        // Action method for the dashboard page
        public IActionResult Index()
        {
            try
            {
                if (HttpContext.Session.GetInt32("UserId") == null)
                {
                    TempData["errorMessage"] = "Your session has expired. Please login again.";
                    return RedirectToAction("Login", "Account"); // Redirect to the login page
                }
                var UserId = HttpContext.Session.GetInt32("UserId");
                var modal = new Total();
                modal = account.GetTotal(Convert.ToInt32(UserId));
                return View(modal);
            }
            catch (Exception ex)
            {

                TempData["errorMessage"] = ex.Message;
                return View();
            }
        }
        #endregion DashBoard

        #region LoginLogout
        // GET action method for the login page

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // POST action method for the login page
        [HttpPost]
        //public IActionResult Login(Account model)
        public IActionResult Login(Account model, string userEmail)
        {

            try
            {
                if (model.UserName == null)
                {
                    TempData["errorMessage"] = "Please Enter UserName";
                    return View();
                }
                else if (model.Password == null)
                {
                    TempData["errorMessage"] = "Please Enter Password";
                    return View();
                }

                bool result = account.LogIn(model,userEmail);
                if (!result)
                {
                    TempData["errorMessage"] = "Invalid Username Or Password!";
                    return View();
                }
                else if (result == true)
                {
                    HttpContext.Session.SetInt32("UserId", model.UserId);
                    HttpContext.Session.SetString("Name", model.Name);
                    HttpContext.Session.SetString("UserName", model.UserName);
                    HttpContext.Session.SetString("UserTypeId", model.UserTypeId);
                    var UID = HttpContext.Session.GetInt32("UserId");
                    var Name = HttpContext.Session.GetString("Name");
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return View();

            }
        }

        // Action method for logging out
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("UserId");
            HttpContext.Session.Remove("Name");
            HttpContext.Session.Remove("UserEmail");
            var UID = HttpContext.Session.GetInt32("UserId");
            var Name = HttpContext.Session.GetString("Name");
            return View("Login");
        }

        #endregion LoginLogout

        #region ChangePasswordForgotPassword
        // GET action method for the change password page
        [HttpGet]
        public IActionResult ChangePassword()
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                // User is not logged in or session has expired
                return RedirectToAction("Login", "Account"); // Redirect to the login page
            }
            return View();
        }

        // POST action method for the change password page
        [HttpPost]
        public IActionResult ChangePassword(ChangePasswordModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string UserName = HttpContext.Session.GetString("UserName");

                    bool result = account.ChangePassword(model, UserName);

                    if (!result)
                    {
                        TempData["errorMessage"] = "Invalid CurrentPassword!";
                        return View();
                    }
                    else if (result == true)
                    {
                        TempData["successMessage"] = "Password changed successfully!";
                        return RedirectToAction("Index");
                    }


                }


                return View(model);
            }
            catch (Exception ex)
            {

                TempData["errorMessage"] = ex.Message;
                return View();
            }
        }

        // Action method for the forgot password page
        public ActionResult ForgotPassword()
        {
            return View();
        }

        // POST action method for the forgot password page
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            try
            {

                if (model.UserName == null)
                {
                    TempData["errorMessage"] = "Please Enter Username!";
                    return View();
                }
                var modal = new ForgotPasswordViewModel();
                modal = account.ForgotPassword(model);

                if (modal.UserName == null)
                {
                    TempData["errorMessage"] = "Invalid Username!";
                    return View();
                }
                else if (modal.Email == null)
                {
                    TempData["errorMessage"] = "Your Email is Not Update in Our DataBase Please contact with your Admin!";
                    return View();
                }
                else if (modal != null)
                {
                    var newPassword = Common.VarbinaryToPassword(modal.Password); // Implement the logic to generate a random password


                    // Send the new password to the user's email
                    var emailSubject = "Password Reset for Your Business Management Account";
                    var emailBody = $"Dear {modal.Name},\r\n\r\nWe have received a request to reset your password for your Business Management account. We understand that forgetting" +
                        $" passwords can be frustrating, but don't worry, we're here to help you regain access to your account.\r\n\r\nAs requested, we have Send you, your password " +
                        $"for your account. Please find your  login details below:\r\n\r\nUsername: {modal.UserName} \r\nPassword: {newPassword} \r\n\r\nFor security purposes, we" +
                        $" highly recommend Please change your password after log in.\r\n\r\n Please ensure that your new password is unique and not easily guessable to keep your" +
                        $" account secure.\r\n\r\n If you encounter any issues during the login process or have any further questions, please feel free to contact our support team at" +
                        $" [support@antheminfotech.info]. \r\n\r\n Thank you for choosing Business Management. We appreciate your business and are here to assist you whenever you need" +
                        $" it. \r\n\r\n Best regards,";
                    
                    //await SendEmailAsync(modal.Email, emailSubject, emailBody);

                    try
                    {
                        await SendEmailAsync(modal.Email, emailSubject, emailBody);
                    }
                    catch (RedirectException ex)
                    {
                        TempData["errorMessage"] = ex.Message;
                        return RedirectToAction("ForgotPassword", "Account"); // Redirect to the ForgotPassword action of the Account controller
                    }

                    TempData["successMessage"] = "Password has been sent to your email. Please check your email.";
                    return RedirectToAction("Login");
                }

                return View();
            }
            catch (Exception ex)
            {

                TempData["errorMessage"] = ex.Message;
                return View();
            }
        }

        #region SendEmail

        // Method to send an email
        private async Task SendEmailAsync(string email, string subject, string body)
        {
            try
            {
                var smtpSettings = _configuration.GetSection("SmtpSettings").Get<SmtpSettings>();

                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(smtpSettings.SenderName, smtpSettings.SenderEmail));
                message.To.Add(new MailboxAddress("", email)); // Add recipient email address here
                message.Subject = subject;
                message.Body = new TextPart(TextFormat.Plain) { Text = body };

                using (var client = new SmtpClient())
                {
                    await client.ConnectAsync(smtpSettings.Server, smtpSettings.Port, SecureSocketOptions.StartTls);
                    await client.AuthenticateAsync(smtpSettings.UserName, smtpSettings.Password);
                    await client.SendAsync(message);
                    await client.DisconnectAsync(true);
                }
            }
            catch (Exception ex)
            {

                TempData["errorMessage"] = ex.Message;
                throw new RedirectException(ex.Message);

            }
        }

        #endregion SendEmail

        #endregion ChangePasswordForgotPassword



        #region GoogleLogin

        [HttpGet("/api/account/signin-google")]
        public IActionResult SignInWithGoogle()
        {
            try
            {
                var redirectUrl = Url.Action("GoogleCallback", "Account", null, Request.Scheme);
                return Challenge(new AuthenticationProperties { RedirectUri = redirectUrl }, GoogleDefaults.AuthenticationScheme);
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return View("Login");
            }
        }

        [HttpGet("google-callback")]
        public async Task<IActionResult> GoogleCallback()
        {

            try
            {
                var result = await HttpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);
                if (result.Succeeded)
                {
                    var claimsIdentity = (ClaimsIdentity)result.Principal.Identity;
                    var userEmailClaim = claimsIdentity.FindFirst(ClaimTypes.Email);

                    if (userEmailClaim != null)
                    {
                        var userEmail = userEmailClaim.Value;
                        var connectionString = _configuration.GetConnectionString("DefaultConnection");

                        // Check if the email already exists in the GoogleLogin table
                        bool emailExists;
                        using (var connection = new SqlConnection(connectionString))
                        {
                            connection.Open();
                            using (var command = new SqlCommand("SELECT COUNT(*) FROM GoogleLogin WHERE email = @Email", connection))
                            {
                                command.Parameters.AddWithValue("@Email", userEmail);
                                var count = (int)command.ExecuteScalar();
                                emailExists = count > 0;
                            }
                        }

                        if (!emailExists)
                        {
                            // Save the user's email to the GoogleLogin table using the stored procedure
                            using (var connection = new SqlConnection(connectionString))
                            {
                                connection.Open();
                                using (var command = new SqlCommand("InsertGoogleLogin", connection))
                                {
                                    command.CommandType = CommandType.StoredProcedure;
                                    command.Parameters.AddWithValue("@Email", userEmail);
                                    command.Parameters.AddWithValue("@pass", Common.PasswordToVarbinary(Common.GenerateRandomPassword()));
                                    command.ExecuteNonQuery();
                                }
                            }
                        }

                        // Log in the user (if necessary)
                        // ...

                        // Save the email to the session (if necessary)
                        HttpContext.Session.SetString("UserEmail", userEmail);

                        var UserMail = HttpContext.Session.GetString("UserEmail");

                        // return RedirectToAction("Index", "Account");
                        return RedirectToAction("USEREmail", "Account", new { userEmail = userEmail });
                    }
                    else
                    {
                        // Handle authentication failure
                        return RedirectToAction("login", "Account");
                    }
                }

                // Return a fallback action in case of unexpected scenarios
                return RedirectToAction("Login", "Account");
            }
            catch (Exception ex)
            {

                TempData["errorMessage"] = ex.Message;
                return View("Login");
            }
        }

        [HttpGet]
        public IActionResult USEREmail(Account model, string userEmail)
        {

            try
            {
                if (model.UserName == null && userEmail == null)
                {
                    TempData["errorMessage"] = "Please Enter UserName";
                    return View();
                }
                else if (model.Password == null && userEmail == null)
                {
                    TempData["errorMessage"] = "Please Enter Password";
                    return View();
                }

                bool result = account.LogIn(model,userEmail);
                if (!result)
                {
                    TempData["errorMessage"] = "Invalid Username Or Password!";
                    return View();
                }
                else if (result == true)
                {
                    HttpContext.Session.SetInt32("UserId", model.UserId);
                    HttpContext.Session.SetString("Name", model.Name);
                    HttpContext.Session.SetString("UserName", model.UserName);
                    HttpContext.Session.SetString("UserTypeId", model.UserTypeId);
                    var UID = HttpContext.Session.GetInt32("UserId");
                    var Name = HttpContext.Session.GetString("Name");
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return View("Login");

            }
        }

        #endregion GoogleLogin



    }

    // Custom exception class for redirecting to a specific action
    public class RedirectException : Exception
    {
        public RedirectException(string message) : base(message)
        {
        }
    }
}
