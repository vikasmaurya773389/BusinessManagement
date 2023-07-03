using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace BusinessManagement.Models
{

    #region Bids
    public class Bids
    {
        [Key]
        public int BidId { get; set; }
        [Required (ErrorMessage = "The Project Title field is required.")]
        public string? ProjectTitle { get; set; }
        [Required(ErrorMessage = "The Project Url field is required.")]
        public string? ProjectUrl { get; set; }
        public string UserId { get; set; }
        [Required(ErrorMessage = "The Client field is required.")]
        public string ClientId { get; set; }

       
        [Required(ErrorMessage = "The Bid Date Time field is required.")]
        public DateTime BidDateTime { get; set; }
        [Required(ErrorMessage = "The Connect Spent field is required.")]
        public string ConnectSpent { get; set; }
        [Required(ErrorMessage = "The Bid Response field is required.")]
        public string BidResponseId { get; set; }

        [Required(ErrorMessage = "The Description field is required.")]
        public string Description { get; set; }
        public List<Client> Clients { get; set; }

        public List<Responses> Response { get; set; }
       
        public string date { get; set; }
       

    }

    #endregion Bids

    #region All
    public class all
    {
        public List<Bids> bids { get; set; }

        public List<Bider> biders { get; set; }
    }

    #endregion All

    #region Client
    public class Client
    {
        public int ClientId { get; set; }
        public string Name { get; set; }
    }
    #endregion Client

    #region Bider
    public class Bider
    {
        public string Biders { get; set; }
        
    }

    #endregion Bider

    #region Responses
    public class Responses
    {
        public int BidResponseId { get; set; }
        public string Response { get; set; }
    }

    #endregion Responses
}
