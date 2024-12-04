//using GroupFourDotNet.Pages.Employee;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace GroupFourDotNet.Pages.Services
{
    public class InsertServiceModel : PageModel
    {
        public ServiceInfo serviceInfo = new ServiceInfo();
        public String errorMessage = "";
        public String successMessage = "";
        public void OnGet()
        {
        }

        public void OnPost()
        {
            serviceInfo.type = Request.Form["type"];
            /*serviceInfo.price = Request.Form["price"];*/
            double price;
if (double.TryParse(Request.Form["price"], out price))
{
    // Parsing successful, assign the value to serviceInfo.price
    serviceInfo.price = price;
}
            
            if (serviceInfo.type.Length == 0 || serviceInfo.price == null)
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
                    string sqlQuery = "INSERT INTO Services (type,price) VALUES (@v_type,@v_price)";
                    using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
                    {

                        cmd.Parameters.AddWithValue("@v_type", serviceInfo.type);
                        cmd.Parameters.AddWithValue("@v_price", serviceInfo.price);
                        

                        cmd.ExecuteNonQuery();

                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }
            serviceInfo.type = "";
            serviceInfo.price = 0;
            

            successMessage = "New Service added with success";
        }
    }
}
