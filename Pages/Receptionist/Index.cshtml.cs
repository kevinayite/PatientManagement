using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace FinalProject.Pages.Receptionist
{
    public class IndexModel : PageModel
    {
        public List<PatientInfo> listPatients = new List<PatientInfo>();

        public void OnGet()
        {
            listPatients.Clear();
            try
            {
                String conString = "Data Source=PIERRE-KASANANI\\SQLEXPRESS;Initial Catalog=projectDB;Integrated Security=True";
                using (SqlConnection con = new SqlConnection(conString))
                {
                    con.Open();
                    string sqlQuery = "SELECT * FROM Patients";
                    using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                PatientInfo patientInfo = new PatientInfo();
                                patientInfo.phoneNumber = reader.IsDBNull(0) ? null : reader.GetString(0);
                                patientInfo.names = reader.IsDBNull(1) ? null : reader.GetString(1);
                                patientInfo.dob = reader.IsDBNull(2) ? null : reader.GetDateTime(2).ToString("yyyy-MM-dd");
                                patientInfo.address = reader.IsDBNull(3) ? null : reader.GetString(3);
                                patientInfo.payment = reader.IsDBNull(4) ? null : reader.GetString(4);

                                listPatients.Add(patientInfo);
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
        public class PatientInfo
        {
            public string phoneNumber;
            public string names;
            public string dob;//DateTime
            public string address;
            public string payment;
        }
    }
}
