using BusinessManagement.Models;
using System.Data.SqlClient;

namespace BusinessManagement.ADO
{
    public class Project_ADO
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

        #region GetAllProject
        public List<Project> GetAll()
        {
            List<Project> projectList = new List<Project>();
            using (connection = new SqlConnection(GetConnectionString()))
            {
                cmd = connection.CreateCommand();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "USP_GetProject";
                connection.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    Project project = new Project();
                    project.ProjectId = Convert.ToInt32(dr["ProjectId"]);
                    project.Title = dr["Title"].ToString();
                    project.Url = dr["Url"].ToString();
                    project.ClientId = dr["ClientId"].ToString();
                   
                    projectList.Add(project);
                }
                connection.Close();
            }
            return projectList;
        }

        #endregion GetAllProject
    }
}
