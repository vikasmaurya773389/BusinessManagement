using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace BusinessManagement.Models
{
    #region Clients
    public class Clients
    {
        [Key]
        public int ClientId { get; set; }
        [Required(ErrorMessage = "The Client Name field is required.")]
        public string Name { get; set; }
        [Required(ErrorMessage = "The Address field is required.")]
        public string Address { get; set; }
        
        public string Email { get; set; }
        [Required]
        public string Notes { get; set; }
        [Required(ErrorMessage = "The Time Zone field is required.")]

        public string TimeZone { get; set; }
        [Required]
        public string Country { get; set; }
        public string UserId { get; set; }
    }

    #endregion Clients

}
