using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace GroupFourDotNet.Pages.Services
{
    public class DisplayingModel : PageModel
    {
        public List<ServiceInfo> listService = new List<ServiceInfo>();
        public void OnGet()
        {
            listService.Clear();
            try
            {
                String conString = "Data Source=PIERRE-KASANANI\\SQLEXPRESS;Initial Catalog=projectDB;Integrated Security=True";
                using (SqlConnection con = new SqlConnection(conString))
                {
                    con.Open();
                    string sqlQuery = "SELECT * FROM Services";
                    using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ServiceInfo servicesInfo = new ServiceInfo();
                                servicesInfo.id = "" + reader.GetInt32(0);
                                servicesInfo.type = reader.GetString(1);
                                /*servicesInfo.price = "" + reader.GetFloat(2);*/
                                servicesInfo.price = reader.IsDBNull(2) ? 0.0 : reader.GetDouble(2);



                                listService.Add(servicesInfo);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
            }
        }
    }

    public class ServiceInfo
    {
        public string id;
        public string type;
        public double price;

    }
}
