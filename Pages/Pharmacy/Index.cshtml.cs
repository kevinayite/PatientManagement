using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using static FinalProject.Pages.Treatments.updateModel;
using System.Data.SqlClient;
using System.IO;

namespace FinalProject.Pages.Shared.Pharmacy
{
    public class IndexModel : PageModel
    {
         public List<PharmacyInfo> listOrdonnance = new List<PharmacyInfo>();

        public void OnGet()
        {
            listOrdonnance.Clear();
            try
            {
                String conString = "Data Source=PIERRE-KASANANI\\SQLEXPRESS;Initial Catalog=projectDB;Integrated Security=True";
                using (SqlConnection con = new SqlConnection(conString))
                {
                    con.Open();
                    string sqlQuery = "SELECT * FROM pharmacy";
                    using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                PharmacyInfo pharmacyInfo = new PharmacyInfo();
                                /* pharmacyInfo.OrdonnanceId = "" + reader.GetInt32(0);
                                 pharmacyInfo.treatmentId = "" + reader.GetInt32(1); ;
                                 pharmacyInfo.patientCode =  reader.GetString(2);
                                 pharmacyInfo.drugList = reader.GetString(3);
                                 pharmacyInfo.description = reader.GetString(4);
                                 pharmacyInfo.drugPrice = reader.GetDouble(5) ?? 0.0;*/


                                pharmacyInfo.ordonnanceId = reader.IsDBNull(0) ? null : reader.GetInt32(0).ToString();
                                pharmacyInfo.treatmentId = reader.IsDBNull(1) ? null : reader.GetInt32(1).ToString();
                                pharmacyInfo.patientCode = reader.IsDBNull(2) ? null : reader.GetString(2);
                                pharmacyInfo.drugList = reader.IsDBNull(3) ? null : reader.GetString(3);
                                pharmacyInfo.description = reader.IsDBNull(4) ? null : reader.GetString(4);
                                pharmacyInfo.drugPrice = reader.IsDBNull(5) ? 0.0 : reader.GetDouble(5);




                                listOrdonnance.Add(pharmacyInfo);
                            }
                            foreach (var item in listOrdonnance)
                            {
                                Console.WriteLine($"OrdonnanceId: {item.ordonnanceId}, TreatmentId: {item.treatmentId}, PatientCode: {item.patientCode}");
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

        public IActionResult GenerateTextFile(string ordonnanceId, string treatmentId, string patientCode, string drugList, string description, string drugPrice)
        {
            // Construct the text content
            string textContent = $"Ordonnance Id: {ordonnanceId}\n" +
                                 $"Treatment Id: {treatmentId}\n" +
                                 $"Patient Code: {patientCode}\n" +
                                 $"Drug List: {drugList}\n" +
                                 $"Description: {description}\n" +
                                 $"Drug Price: {drugPrice}\n";

            // Set the file name and path
            string fileName = "PharmacyData.txt";
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", fileName);

            // Write the text content to the file
            System.IO.File.WriteAllText(filePath, textContent);

            // Return a FileResult to download the text file
            return File(filePath, "text/plain", fileName);
        }

        public class PharmacyInfo
        {
            public string ordonnanceId;
            public string treatmentId;
            public string patientCode;
            public string drugList;
            public string description;
            public double drugPrice;
        }
    }
}

