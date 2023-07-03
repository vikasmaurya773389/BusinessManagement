using System.ComponentModel.DataAnnotations;

namespace BusinessManagement.Models
{
    #region Project
    public class Project
    {
        [Key]
        public int ProjectId { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Url { get; set; }
        [Required]
        public string ClientId { get; set; }
        
       
    }

    #endregion Project
}
