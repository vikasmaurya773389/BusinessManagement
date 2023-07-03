using BusinessManagement.Models;
using Microsoft.AspNetCore.Http;
using System.Data;
using System.Data.SqlClient;

namespace BusinessManagement.ADO
{
    public class AccountLogin
    {

        SqlConnection connection = null;
        SqlCommand cmd = null;

        public static IConfiguration Configuration { get; set; }

        #region GetConnectionString
        private string GetConnectionString()
        {
            // Read the connection string from appsettings.json file
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");
            Configuration = builder.Build();
            return Configuration.GetConnectionString("DefaultConnection");
        }

        #endregion GetConnectionString

        #region Login
        public bool LogIn(Account model,string userEmail)
        {
            int id = 0;

            if (model.Password == null)
            { model.Password = ""; }
                
            var pass = Common.PasswordToVarbinary(model.Password);
            using (connection = new SqlConnection(GetConnectionString()))
            {
                cmd = connection.CreateCommand();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "USP_LoginUser";
                cmd.Parameters.AddWithValue("@UserName", model.UserName);
                cmd.Parameters.AddWithValue("@Password", pass);
                cmd.Parameters.AddWithValue("@Email", userEmail);
                connection.Open();


                // Execute the stored procedure
                id = cmd.ExecuteNonQuery();

                // Read the result using SqlDataReader
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    // Populate the Account model with the retrieved data
                    model.UserId = Convert.ToInt32(dr["UserId"]);
                    model.Name = dr["Name"].ToString();
                    model.UserName = dr["UserName"].ToString();
                    model.UserTypeId = dr["UserTypeId"].ToString();
                }
                connection.Close();
                
            }
            return id > 0 ? true : false;
        }

        #endregion Login

        #region GetTotal
        public Total GetTotal(int UserId)
        {
            Total total = new Total();
            using (connection = new SqlConnection(GetConnectionString()))
            {
                cmd = connection.CreateCommand();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "USP_GetTotal";
                cmd.Parameters.AddWithValue("@UserID", UserId);
               

                connection.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    // Populate the Total object with the retrieved data

                    total.TotalBids = dr["TotalBids"].ToString();
                   total.TotalClients = dr["TotalClients"].ToString();
                   total.TotalProjects= dr["TotalProjects"].ToString();
                   total.TotalUsers= dr["TotalUsers"].ToString();
                   


                }
                connection.Close();
            }
            return total;
        }

        #endregion GetTotal

        #region ChangePassword
        public bool ChangePassword(ChangePasswordModel model,string UserName)
        {
            int id = 0;
            var cpass = Common.PasswordToVarbinary(model.CurrentPassword);
            var Npass=Common.PasswordToVarbinary(model.NewPassword);
            using (connection = new SqlConnection(GetConnectionString()))
            {
                cmd = connection.CreateCommand();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "USP_ChangePassword";
                cmd.Parameters.AddWithValue("@UserName", UserName);
                cmd.Parameters.AddWithValue("@Password", cpass);
                cmd.Parameters.AddWithValue("@NewPassword", Npass);
                connection.Open();

                // Execute the stored procedure
                id = cmd.ExecuteNonQuery();
               
                connection.Close();

            }
            return id > 0 ? true : false;
        }

        #endregion ChangePassword

        #region ForgotPassword
        public ForgotPasswordViewModel ForgotPassword(ForgotPasswordViewModel model)
        {
            var modal = new ForgotPasswordViewModel();
            using (connection = new SqlConnection(GetConnectionString()))
            {
                cmd = connection.CreateCommand();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "USP_ForgotPassword";
                cmd.Parameters.AddWithValue("@UserName", model.UserName);
                connection.Open();
               
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    // Populate the ForgotPasswordViewModel with the retrieved data
                    modal.Password = dr["Password"].ToString();
                    modal.Email = dr["EmailId"].ToString();
                    modal.UserName = dr["UserName"].ToString();
                    modal.Name = dr["Name"].ToString();
                }
                connection.Close();

            }
            return modal;
        }

        #endregion ForgotPassword
    }
}
