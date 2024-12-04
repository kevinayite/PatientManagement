using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace GroupFourDotNet.Pages.Employee
{
    public class EditModel : PageModel
    {
        public EmployeeInfo employeeInfo = new EmployeeInfo();
        public String errorMessage = "";
        public String successMessage = "";
        public void OnGet()
        {
            String phoneNumber = Request.Query["phoneNumber"];
            try
            {
                String conString = "Data Source=PIERRE-KASANANI\\SQLEXPRESS;Initial Catalog=projectDB;Integrated Security=True";
                using (SqlConnection con = new SqlConnection(conString))
                {
                    con.Open();
                    string sqlQuery = "SELECT * FROM Staff WHERE phoneNumber=@id";
                    using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                EmployeeInfo employeeInfo = new EmployeeInfo();
                                employeeInfo.number = "" + reader.GetInt32(0);
                                employeeInfo.names = reader.GetString(1);
                                employeeInfo.serviceId = "" + reader.GetInt32(2);
                                employeeInfo.phoneNumber = reader.GetString(3);
                                employeeInfo.pwd = reader.GetString(4);
                                employeeInfo.serviceProvided = reader.GetString(5);

                                
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
            employeeInfo.phoneNumber = Request.Query["phoneNumber"];
            employeeInfo.names = Request.Form["names"];
            employeeInfo.pwd = Request.Form["pwd"];
            /*employeeInfo.phoneNumber = Request.Form["phone"];*/
            employeeInfo.serviceId = Request.Form["serviceId"];
            employeeInfo.serviceProvided = Request.Form["serviceProvided"];

            if (employeeInfo.names.Length == 0 || employeeInfo.serviceId.Length == 0
                || employeeInfo.pwd.Length == 0 || employeeInfo.serviceProvided.Length == 0)
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
                    string sqlQuery = "UPDATE Staff SET names=@v_names, serviceId= @v_serviceId,pwd= @v_pwd,serviceProvided = @v_serviceProvided WHERE phoneNumber=@v_phoneNumber";
                    using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
                    {

                        cmd.Parameters.AddWithValue("@v_phoneNumber", employeeInfo.phoneNumber);
                        cmd.Parameters.AddWithValue("@v_names", employeeInfo.names);
                        cmd.Parameters.AddWithValue("@v_serviceId", employeeInfo.serviceId);
                        /*cmd.Parameters.AddWithValue("@v_phoneNumber", employeeInfo.phoneNumber);*/
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
            employeeInfo.pwd = "";
            employeeInfo.serviceId = "";
            employeeInfo.serviceProvided = "";

            successMessage = "Employee updated with success";
            Response.Redirect("/Employee/Index");

        }
    }
}
