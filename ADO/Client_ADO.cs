using BusinessManagement.Models;
using System.Data.SqlClient;

namespace BusinessManagement.ADO
{
    public class Client_ADO
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

        #region GetAllClient
        public List<Clients> GetAll()
        {
            List<Clients> clientsList = new List<Clients>();
            using (connection = new SqlConnection(GetConnectionString()))
            {
                cmd = connection.CreateCommand();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "USP_GetClients";
                connection.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    Clients clients = new Clients();
                    clients.ClientId = Convert.ToInt32(dr["ClientId"]);
                    clients.Name = dr["Name"].ToString();
                    clients.Country = dr["Country"].ToString();
                    clients.TimeZone = dr["TimeZone"].ToString();
                    clients.Email = dr["Email"].ToString();
                    clientsList.Add(clients);
                }
                connection.Close();
            }
            return clientsList;
        }
        #endregion GetAllClent

        #region InsertUpdateDeleteClient
        public bool Insert(Clients model,int UserId)
        {
            if (model.Email == null)
            {
                model.Email = string.Empty;
            }
            int id = 0;
            using (connection = new SqlConnection(GetConnectionString()))
            {
                cmd = connection.CreateCommand();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "USP_InsertClients";
                cmd.Parameters.AddWithValue("@Name", model.Name);
                cmd.Parameters.AddWithValue("@Country", model.Country);
                cmd.Parameters.AddWithValue("@Address", model.Address);
                cmd.Parameters.AddWithValue("@TimeZone", model.TimeZone);
                cmd.Parameters.AddWithValue("@Email", model.Email);
                cmd.Parameters.AddWithValue("@Notes", model.Notes);
                cmd.Parameters.AddWithValue("@UserId", UserId);

                connection.Open();
                id = cmd.ExecuteNonQuery();
                connection.Close();
            }
            return id > 0 ? true : false;
        }
        public Clients GetByID(int ID)
        {
            Clients clients = new Clients();
            using (connection = new SqlConnection(GetConnectionString()))
            {
                cmd = connection.CreateCommand();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "USP_GetClientByID";
                cmd.Parameters.AddWithValue("@ID", ID);

                connection.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {

                    clients.ClientId = Convert.ToInt32(dr["ClientId"]);
                    clients.Name = dr["Name"].ToString();
                    clients.Country = dr["Country"].ToString();
                    clients.Address = dr["Address"].ToString();
                    clients.Email = dr["Email"].ToString();
                    clients.TimeZone = dr["TimeZone"].ToString();
                    clients.Notes = dr["Notes"].ToString();
                }
                connection.Close();
            }
            return clients ;
        }
        public bool Update(Clients model)
        {
            if(model.Email==null)
            {
                model.Email = string.Empty;
            }
            int id = 0;
            using (connection = new SqlConnection(GetConnectionString()))
            {
                cmd = connection.CreateCommand();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "USP_UpdateClient";
                cmd.Parameters.AddWithValue("@ClientId", model.ClientId);
                cmd.Parameters.AddWithValue("@Name", model.Name);
                cmd.Parameters.AddWithValue("@Country", model.Country);
                cmd.Parameters.AddWithValue("@Email", model.Email);
                cmd.Parameters.AddWithValue("@Address", model.Address);
                cmd.Parameters.AddWithValue("@TimeZone", model.TimeZone);
                cmd.Parameters.AddWithValue("@Notes", model.Notes);
                connection.Open();
                id = cmd.ExecuteNonQuery();
                connection.Close();
            }
            return id > 0 ? true : false;
        }
        public bool Delete(int ID,int UserId)
        {
            int id = 0;
            using (connection = new SqlConnection(GetConnectionString()))
            {
                cmd = connection.CreateCommand();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "USP_Deleteclient";
                cmd.Parameters.AddWithValue("@ID", ID);
                cmd.Parameters.AddWithValue("@UserID", UserId);

                connection.Open();
                id = cmd.ExecuteNonQuery();
                connection.Close();
            }
            return id > 0 ? true : false;
        }
        #endregion InsertUpdateDeleteClient
    }
}
