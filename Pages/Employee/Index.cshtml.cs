using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace GroupFourDotNet.Pages.Employee
{
    public class IndexModel : PageModel
    {
        public List<EmployeeInfo> listEmployee = new List<EmployeeInfo>();
        public List<Services> servicesList = new List<Services>();
        public void OnGet()
        {
            listEmployee.Clear();
            try
            {
                String conString = "Data Source=PIERRE-KASANANI\\SQLEXPRESS;Initial Catalog=projectDB;Integrated Security=True";
                using (SqlConnection con = new SqlConnection(conString))
                {
                    con.Open();
                    string sqlQuery = "SELECT T.number, T.names, S.type AS serviceType," +
                        " T.phoneNumber, T.pwd, T.serviceProvided FROM" +
                        " Staff T JOIN Services S ON T.serviceId = S.id";
                    using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                EmployeeInfo patientInfo = new EmployeeInfo();
                                patientInfo.number = "" + reader.GetInt32(0);
                                patientInfo.names = reader.GetString(1);
                                patientInfo.serviceType = "" + reader.GetString(2);
                                patientInfo.phoneNumber = reader.GetString(3);
                                patientInfo.pwd = reader.GetString(4);
                                patientInfo.serviceProvided = reader.GetString(5);

                                listEmployee.Add(patientInfo);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
            }
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

    }
  

    public class EmployeeInfo
    {
        public string number;
        public string names;
        public string serviceId;
        public string serviceType;
        public string phoneNumber;
        public string pwd;
        public string serviceProvided;
    }

    public class Services
    {
        public int? id { get; set; }
        public string type { get; set; }
    }
}
