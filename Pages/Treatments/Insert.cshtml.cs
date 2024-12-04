using FinalProject.Pages.Receptionist;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using static FinalProject.Pages.Receptionist.IndexModel;
using static FinalProject.Pages.Treatments.updateModel;

namespace FinalProject.Pages.Treatments
{
    public class InsertModel : PageModel
    {
        public TreatmentInfo treatmentInfo = new TreatmentInfo();
        public string errorMessage = "";
        public string successMessage = "";
        public void OnGet()
        {
        }
        public void OnPost()
        {
            treatmentInfo.patientCode = Request.Form["patientCode"];
            treatmentInfo.serviceId = Request.Form["serviceId"];
            treatmentInfo.examDesc = Request.Form["examDesc"];
            

            try
            {
                String conString = "Data Source=PIERRE-KASANANI\\SQLEXPRESS;Initial Catalog=projectDB;Integrated Security=True";
                using (SqlConnection con = new SqlConnection(conString))
                {
                    con.Open();
                    string sqlQuery = "INSERT INTO Treatment (patientCode,serviceId,examDesc) VALUES (@patientCode, @serviceId,@examDesc)";
                    using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
                    {
                        // Add parameters with their values
                        cmd.Parameters.AddWithValue("@patientCode", treatmentInfo.patientCode);
                        cmd.Parameters.AddWithValue("@serviceId", treatmentInfo.serviceId);
                        cmd.Parameters.AddWithValue("@examDesc", treatmentInfo.examDesc);

                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            successMessage = "Treatment added";
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
