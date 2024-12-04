using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using static FinalProject.Pages.Treatments.updateModel;
using System.Data.SqlClient;
using static FinalProject.Pages.Receptionist.IndexModel;

namespace FinalProject.Pages.Receptionist
{
    public class CreateModel : PageModel
    {
        public PatientInfo patientInfo = new PatientInfo();
        public string errorMessage = "";
        public string successMessage = "";
        public void OnGet()
        {
        }
        public void OnPost()
        {
            patientInfo.phoneNumber = Request.Form["phoneNumber"];
            patientInfo.names = Request.Form["names"];
            patientInfo.dob = Request.Form["dob"];
            patientInfo.address = Request.Form["address"];
            


            try
            {
                String conString = "Data Source=PIERRE-KASANANI\\SQLEXPRESS;Initial Catalog=projectDB;Integrated Security=True";
                using (SqlConnection con = new SqlConnection(conString))
                {
                    con.Open();
                    string sqlQuery = "INSERT INTO Patients (phoneNumber,names,dob,address) VALUES (@phoneNumber,@names,@dob,@address)";
                    using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
                    {
                        // Add parameters with their values
                        cmd.Parameters.AddWithValue("@phoneNumber", patientInfo.phoneNumber);
                        cmd.Parameters.AddWithValue("@names", patientInfo.names);
                        cmd.Parameters.AddWithValue("@dob", patientInfo.dob);
                        cmd.Parameters.AddWithValue("@address", patientInfo.address);
                        

                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            successMessage = "Patient added";
                        }
                        else
                        {
                            errorMessage = "Insertion failed";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }
        }
    }
}
