//using GroupFourDotNet.Pages.Employee;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Data.SqlClient;

namespace GroupFourDotNet.Pages.Services
{
    public class UpdateServiceModel : PageModel
    {
        public ServiceInfo serviceInfo = new ServiceInfo();
        public string errorMessage = "";
        public string successMessage = "";

        public void OnGet()
        {
            try
            {
                string id = Request.Query["id"];
                String conString = "Data Source=PIERRE-KASANANI\\SQLEXPRESS;Initial Catalog=projectDB;Integrated Security=True";
                using (SqlConnection con = new SqlConnection(conString))
                {
                    con.Open();
                    string sqlQuery = "SELECT * FROM Services WHERE id=@v_id";
                    using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
                    {
                        cmd.Parameters.AddWithValue("@v_id", id);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                serviceInfo.id = reader.GetInt32(0).ToString();
                                serviceInfo.type = reader.GetString(1);
                                serviceInfo.price = reader.IsDBNull(2) ? 0.0 : reader.GetDouble(2);
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

        public void OnPost()
        {
            serviceInfo.type = Request.Form["type"];
            serviceInfo.id = Request.Form["id"];

            if (float.TryParse(Request.Form["price"], out float price))
            {
                serviceInfo.price = price;
            }

            if (string.IsNullOrEmpty(serviceInfo.type) || serviceInfo.price == 0)
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
                    string sqlQuery = "UPDATE Services SET price=@v_price, type = @v_type WHERE id=@v_id";
                    using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
                    {
                        cmd.Parameters.AddWithValue("@v_id", serviceInfo.id);
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

            successMessage = "Service Information has been updated successfully";
            Response.Redirect("/Services/Displaying");
        }
    }
}
