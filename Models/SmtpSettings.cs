namespace BusinessManagement.Models
{
    #region SmtpSettings
    public class SmtpSettings
    {
        public string Server { get; set; }
        public int Port { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string SenderEmail { get; set; }
        public string SenderName { get; set; }
    }

    #endregion SmtpSettings

}
