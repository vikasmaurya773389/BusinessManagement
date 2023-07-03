using BusinessManagement.Models;
using MessagePack;
using System.ComponentModel.DataAnnotations;

namespace BusinessManagement.Models
{
    #region User
    public class User
    {
        
        public int UserId { get; set; }
        [Required(ErrorMessage = "The Name field is required.")]
        public string Name { get; set; }
        [Required(ErrorMessage = "The User Name field is required.")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "The Password field is required.")]
        public string Password { get; set; }
        [Required(ErrorMessage = "The Contact Number field is required.")]
        public string ContactNumber { get; set; }
        [Required(ErrorMessage = "The Designation field is required.")]
        public string Designation { get; set; }
        [Required(ErrorMessage = "The Email Id field is required.")]
        public string EmailId { get; set; }
        
    }

    #endregion User

}

