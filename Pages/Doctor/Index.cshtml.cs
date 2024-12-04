using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Patient_Management_System.Pages.Doctor
{
    public class IndexModel : PageModel
    {
        public List<Treatment> listTreatments = new List<Treatment>();

        public void OnGet()
        {
            listTreatments.Clear();

            try
            {
                String conString = "Data Source=PIERRE-KASANANI\\SQLEXPRESS;Initial Catalog=projectDB;Integrated Security=True";
                using (SqlConnection con = new SqlConnection(conString))
                {
                    con.Open();
                    string sqlQuery = "SELECT T.id, T.patientCode, S.type AS serviceType, " +
                                    "T.examDesc, T.result FROM Treatment T JOIN Services S ON T.serviceId = S.id";
                    using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Treatment treatment = new Treatment();
                                treatment.id = reader.GetInt32(0).ToString();
                                treatment.patient = reader.GetString(1);
                                treatment.serviceType = reader.GetString(2);
                                treatment.examDescription = reader.GetString(3);

                                
                                // Handle byte[] data appropriately
                                if (!reader.IsDBNull(4))
                                {
                                    byte[] imageData = (byte[])reader.GetValue(4);
                                    treatment.result = Convert.ToBase64String(imageData); // Convert to base64 for storage or display
                                }
                                else
                                {
                                    treatment.result = string.Empty; // or null, depending on your needs
                                }

                                listTreatments.Add(treatment);
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

        public class Treatment
        {
            public string? id;
            public string? patient;
            public string? serviceType; // New property for service type
            public string? examDescription;
            public string? result;
        }
    }
}
