using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace GroupFourDotNet.Pages.Employee
{
    public class CreateModel : PageModel
    {

        public EmployeeInfo employeeInfo = new EmployeeInfo();
        
        public List<Services> servicesList = new List<Services>();
        public String errorMessage = "";
        public String successMessage = "";
        public void OnGet()
        {
            RetrieveServiceData();
        }

        public void OnPost()
        {
            employeeInfo.names = Request.Form["names"];
            employeeInfo.serviceId = Request.Form["serviceId"];
            employeeInfo.phoneNumber = Request.Form["phone"];
            employeeInfo.pwd = Request.Form["pwd"];
            employeeInfo.serviceProvided = Request.Form["serviceProvided"];

            if (employeeInfo.names.Length == 0 || employeeInfo.serviceId.Length == 0
                || employeeInfo.phoneNumber.Length == 0 || employeeInfo.pwd.Length == 0
                || employeeInfo.serviceProvided.Length == 0)
            {
                errorMessage = "All fields are required";
                return;
            }

            try
            {
                String conString = "Data Source=PIERRE-KASANANI\\SQLEXPRESS;Initial Catalog=projectDB;Integrated Security=True";
                using (SqlConnection con = new SqlConnection(conString))
                {
                    con.Open();
                    string sqlQuery = "INSERT INTO Staff (names,serviceId,phoneNumber,pwd, serviceProvided) VALUES (@v_names,@v_serviceId,@v_phoneNumber,@v_pwd, @v_serviceProvided)";
                    using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
                    {

                        cmd.Parameters.AddWithValue("@v_names", employeeInfo.names);
                        cmd.Parameters.AddWithValue("@v_serviceId", employeeInfo.serviceId);
                        cmd.Parameters.AddWithValue("@v_phoneNumber", employeeInfo.phoneNumber);
                        cmd.Parameters.AddWithValue("@v_pwd", employeeInfo.pwd);
                        cmd.Parameters.AddWithValue("@v_serviceProvided", employeeInfo.serviceProvided);

                        cmd.ExecuteNonQuery();

                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }
            employeeInfo.names = "";
            employeeInfo.serviceId = "";
            employeeInfo.phoneNumber = "";
            employeeInfo.pwd = "";
            employeeInfo.serviceProvided = "";

            successMessage = "New Employee added with success";
            RetrieveServiceData();
        }

        private void RetrieveServiceData()
        {
            servicesList.Clear();
            String conString = "Data Source=PIERRE-KASANANI\\SQLEXPRESS;Initial Catalog=projectDB;Integrated Security=True";
            using (SqlConnection con = new SqlConnection(conString))
            {
                string qry = "SELECT id, type FROM Services";
                con.Open();

                using (SqlCommand cmd = new SqlCommand(qry, con))
                {
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Services services = new Services
                        {
                            id = Convert.ToInt32(reader["id"]),
                            type = reader["type"].ToString()
                        };

                        servicesList.Add(services);
                    }
                }
            }
        }
        public class Services
        {
            public int? id { get; set; }
            public string type { get; set; }
        }
    }
}
