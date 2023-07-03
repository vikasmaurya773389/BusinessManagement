using BusinessManagement.Models;
using System.Data.SqlClient;
using System.Reflection;

namespace BusinessManagement.ADO
{
    public class Bids_ADO
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

        #region GetAllBids
        public List<Bids> GetAll(int UserId)
        {
            List<Bids> BidsList = new List<Bids>();
            using (connection = new SqlConnection(GetConnectionString()))
            {
                cmd = connection.CreateCommand();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "USP_GetBids";
                cmd.Parameters.AddWithValue("@UserID", UserId);
                connection.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    Bids bids = new Bids();
                    bids.BidId = Convert.ToInt32(dr["BidId"]);
                    bids.ProjectTitle = dr["Title"].ToString();
                    bids.ClientId = dr["Client"].ToString();
                    bids.UserId = dr["Bidder"].ToString();
                    bids.BidResponseId = dr["Response"].ToString();
                    bids.date = dr["BidDateTime"].ToString();
                  
                    BidsList.Add(bids);
                }
                connection.Close();
            }
            return BidsList;
        }

        #endregion GetAllBids

        #region GetClient&ResponseDDL


        public List<Bider> GetBider()
        {
            var biders = new List<Bider>();

            using (var connection = new SqlConnection(GetConnectionString()))
            {
                connection.Open();

                var query = "select Name from tbluser where  IsDeleted=0 Order By Name Asc";
                using (var command = new SqlCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var bider = new Bider
                            {
                               
                                Biders = reader["Name"].ToString()
                            };
                            biders.Add(bider);
                        }
                    }
                }
            }

            return biders;
        }



        public List<Client> GetClients()
        {
            var clients = new List<Client>();

            using (var connection = new SqlConnection(GetConnectionString()))
            {
                connection.Open();

                var query = "select ClientId,Name from tblClients where  IsDeleted=0 Order By Name Asc";
                using (var command = new SqlCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var client = new Client
                            {
                                ClientId = Convert.ToInt32(reader["ClientId"]),
                                Name = reader["Name"].ToString()
                        };
                            clients.Add(client);
                        }
                    }
                }
            }

            return clients;
        }
        public List<Responses> GetResponse()
        {
            var Responses = new List<Responses>();

            using (var connection = new SqlConnection(GetConnectionString()))
            {
                connection.Open();

                var query = "select BidResponseId,Response from tblBidResponses where  IsDeleted=0 Order By Response Asc";
                using (var command = new SqlCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var Response = new Responses
                            {
                                BidResponseId = Convert.ToInt32(reader["BidResponseId"]),
                                Response = reader["Response"].ToString()
                            };
                            Responses.Add(Response);
                        }
                    }
                }
            }

            return Responses;
        }

        #endregion GetClient&ResponseDDL

        #region InsertUpdateDeleteBids
        public bool Insert(Bids model,int UID)
        {
            
                int id = 0;
                using (connection = new SqlConnection(GetConnectionString()))
                {

                    cmd = connection.CreateCommand();
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "USP_InsertBids";
                    cmd.Parameters.AddWithValue("@ProjectTitle", model.ProjectTitle);
                    cmd.Parameters.AddWithValue("@ProjectUrl", model.ProjectUrl);
                    cmd.Parameters.AddWithValue("@UserId", UID);
                    cmd.Parameters.AddWithValue("@ClientId", model.ClientId);
                    cmd.Parameters.AddWithValue("@BidDateTime", model.BidDateTime);
                    cmd.Parameters.AddWithValue("@ConnectSpent", model.ConnectSpent);
                    cmd.Parameters.AddWithValue("@BidResponseId", model.BidResponseId);
                    cmd.Parameters.AddWithValue("@Description", model.Description);

                    connection.Open();
                    id = cmd.ExecuteNonQuery();
                    connection.Close();
                }
                return id > 0 ? true : false;
           
        }
        public Bids GetByID(int ID)
        {
            Bids bids = new Bids();
            using (connection = new SqlConnection(GetConnectionString()))
            {
                cmd = connection.CreateCommand();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "USP_GetBidByID";
                cmd.Parameters.AddWithValue("@ID", ID);

                connection.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {

                    bids.BidId = Convert.ToInt32(dr["BidId"]);
                    bids.ProjectTitle = dr["ProjectTitle"].ToString();
                    bids.ProjectUrl = dr["ProjectUrl"].ToString();
                    bids.ClientId = dr["ClientId"].ToString();
                    bids.BidDateTime =  Convert.ToDateTime(dr["BidDateTime"]).Date;
                    bids.ConnectSpent = dr["ConnectSpent"].ToString();
                    bids.BidResponseId = dr["BidResponseId"].ToString();
                    bids.Description = dr["Description"].ToString();


                }
                connection.Close();
            }
            return bids;
        }
        public bool Update(Bids model)
        {
            int id = 0;
            using (connection = new SqlConnection(GetConnectionString()))
            {
                cmd = connection.CreateCommand();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "USP_Updatebids";
                cmd.Parameters.AddWithValue("@BidId", model.BidId);
                cmd.Parameters.AddWithValue("@ProjectTitle", model.ProjectTitle);
                cmd.Parameters.AddWithValue("@ProjectUrl", model.ProjectUrl);
                cmd.Parameters.AddWithValue("@ClientId", model.ClientId);
                cmd.Parameters.AddWithValue("@BidDateTime", model.BidDateTime);
                cmd.Parameters.AddWithValue("@ConnectSpent", model.ConnectSpent);
                cmd.Parameters.AddWithValue("@BidResponseId", model.BidResponseId);
                cmd.Parameters.AddWithValue("@Description", model.Description);
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
                cmd.CommandText = "USP_DeleteBids";
                cmd.Parameters.AddWithValue("@ID", ID);

                connection.Open();
                id = cmd.ExecuteNonQuery();
                connection.Close();
            }
            return id > 0 ? true : false;
        }

        #endregion InsertUpdateDeleteBids
    }
}
