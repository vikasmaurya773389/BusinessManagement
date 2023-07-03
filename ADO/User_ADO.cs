using BusinessManagement.Models;
using System.Data.SqlClient;

namespace BusinessManagement.ADO
{
    public class User_ADO
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

        #region GetAll
        public List<User> GetAll()
        {
            List<User> UserList = new List<User>();
            using (connection = new SqlConnection(GetConnectionString()))
            {
                cmd = connection.CreateCommand();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "USP_GetUsers";
                connection.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    User user = new User();
                    user.UserId = Convert.ToInt32(dr["UserId"]);
                    user.Name = dr["Name"].ToString();
                    user.UserName = dr["UserName"].ToString();
                    user.ContactNumber = dr["ContactNumber"].ToString();
                    user.Designation = dr["Designation"].ToString();
                    user.EmailId = dr["EmailId"].ToString();

                    UserList.Add(user);
                }
                connection.Close();
            }
            return UserList;
        }

        #endregion GetAll

        #region InsertUpdateDelete

        public bool Insert(User model)
        {
            int id = 0;
            var pass=Common.PasswordToVarbinary(model.Password);
            using (connection = new SqlConnection(GetConnectionString()))
            {

                cmd = connection.CreateCommand();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "USP_InsertUser";
                cmd.Parameters.AddWithValue("@Name", model.Name);
                cmd.Parameters.AddWithValue("@UserName", model.UserName);
                cmd.Parameters.AddWithValue("@Designation", model.Designation);
                cmd.Parameters.AddWithValue("@ContactNumber", model.ContactNumber);
                cmd.Parameters.AddWithValue("@EmailId", model.EmailId);
                cmd.Parameters.AddWithValue("@Password", pass);
              

                connection.Open();
                id = cmd.ExecuteNonQuery();
                connection.Close();
            }
            return id > 0 ? true : false;
        }


        public User GetByID(int ID)
        {
            User user = new User();
            using (connection = new SqlConnection(GetConnectionString()))
            {
                cmd = connection.CreateCommand();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "USP_GetUserByID";
                cmd.Parameters.AddWithValue("@ID", ID);

                connection.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {

                    user.UserId = Convert.ToInt32(dr["UserId"]);
                    user.Name = dr["Name"].ToString();
                    user.UserName = dr["UserName"].ToString();
                    user.ContactNumber = dr["ContactNumber"].ToString();
                    user.Designation = dr["Designation"].ToString();
                    user.EmailId = dr["EmailId"].ToString();
                    user.Password =Common.VarbinaryToPassword(dr["Password"].ToString());
                }
                connection.Close();
            }
            return user;
        }


        public bool Update(User model)
        {
            
            int id = 0;
            using (connection = new SqlConnection(GetConnectionString()))
            {
                cmd = connection.CreateCommand();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "USP_UpdateUser";
                cmd.Parameters.AddWithValue("@UserId", model.UserId);
                cmd.Parameters.AddWithValue("@Name", model.Name);
                cmd.Parameters.AddWithValue("@UserName", model.UserName);
                cmd.Parameters.AddWithValue("@ContactNumber", model.ContactNumber);
                cmd.Parameters.AddWithValue("@Designation", model.Designation);
                cmd.Parameters.AddWithValue("@EmailId", model.EmailId);
               // cmd.Parameters.AddWithValue("@Password", pass);
                
                connection.Open();
                id = cmd.ExecuteNonQuery();
                connection.Close();
            }
            return id > 0 ? true : false;
        }

        public bool Delete(int ID)
        {
            int id = 0;
            using (connection = new SqlConnection(GetConnectionString()))
            {
                cmd = connection.CreateCommand();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "USP_DeleteUser";
                cmd.Parameters.AddWithValue("@ID", ID);

                connection.Open();
                id = cmd.ExecuteNonQuery();
                connection.Close();
            }
            return id > 0 ? true : false;
        }

        #endregion InsertUpdateDelete

    }
}
