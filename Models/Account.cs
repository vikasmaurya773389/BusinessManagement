using System.ComponentModel.DataAnnotations;

namespace BusinessManagement.Models
{
    #region Account
    public class Account
    {
        [Key]
        public int UserId { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string ContactNumber { get; set; }
        public string Designation { get; set; }
        public string EmailId { get; set; }
        public string UserTypeId { get; set; }
        
       
    }
    #endregion Account

    #region Total
    public class Total
    {
        public string TotalBids { get; set; }
        public string TotalClients { get; set; }
        public string TotalProjects { get; set; }
        public string TotalUsers { get; set; }
    }

    #endregion Total

    #region ChangePasswordModel
    public class ChangePasswordModel
    {
       

        [Required]
        public string CurrentPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        
        public string ConfirmPassword { get; set; }
    }

    #endregion ChangePasswordModel

    #region ForgotPasswordViewModel

    public class ForgotPasswordViewModel
    {
        [Required]
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
    }

    #endregion ForgotPasswordViewModel


}
