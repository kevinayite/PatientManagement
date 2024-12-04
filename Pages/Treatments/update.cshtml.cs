using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System;
using System.IO;

namespace FinalProject.Pages.Treatments
{
    public class updateModel : PageModel
    {
        public TreatmentInfo treatmentInfo = new TreatmentInfo();
        public string errorMessage = "";
        public string successMessage = "";
        public IFormFile ImageFile { get; set; }
        public void OnGet()
        {

        }
        public void OnPost()
        {
            treatmentInfo.id = Request.Query["id"];
            treatmentInfo.patientCode = Request.Form["patientCode"];
            treatmentInfo.serviceId = Request.Form["serviceId"];
            treatmentInfo.examDesc = Request.Form["examDesc"];
            treatmentInfo.result = Request.Form["result"];

            if (ImageFile != null && ImageFile.Length > 0)
            {
                var uploadDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploaded-images");

                if (!Directory.Exists(uploadDirectory))
                {
                    Directory.CreateDirectory(uploadDirectory);
                }

                var uniqueFileName = Guid.NewGuid().ToString() + "_" + ImageFile.FileName;
                var filePath = Path.Combine(uploadDirectory, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    ImageFile.CopyTo(fileStream);
                }

                // Save 'filePath' or the uniqueFileName in the database for later retrieval
                treatmentInfo.ImagePath = "/uploaded-images/" + uniqueFileName; // Update TreatmentInfo class accordingly
            }
            if (ImageFile != null && ImageFile.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    ImageFile.CopyTo(memoryStream);
                    byte[] imageData = memoryStream.ToArray();

                    /*const int MaxImageSizeMegabytes = 100;
                    const long MaxImageSizeBytes = MaxImageSizeMegabytes * 1024 * 1024;
                    if (imageData.Length > MaxImageSizeBytes)
                    {
                        errorMessage = "Image data is too large to save.";
                        return;
                    }*/
                    try
                    {
                        String conString = "Data Source=PIERRE-KASANANI\\SQLEXPRESS;Initial Catalog=projectDB;Integrated Security=True";
                        using (SqlConnection con = new SqlConnection(conString))
                        {
                            con.Open();
                            string sqlQuery = "UPDATE Treatment SET result = @imageData, serviceId = @serviceId, examDesc = @examDesc WHERE patientCode = @patientCode";
                            using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
                            {
                                cmd.Parameters.AddWithValue("@imageData", imageData);
                                cmd.Parameters.AddWithValue("@serviceId", treatmentInfo.serviceId);
                                cmd.Parameters.AddWithValue("@examDesc", treatmentInfo.examDesc);
                                cmd.Parameters.AddWithValue("@patientCode", treatmentInfo.patientCode);

                                int rowsAffected = cmd.ExecuteNonQuery();
                                if (rowsAffected > 0)
                                {
                                    successMessage = "Treatment updated with success";
                                }
                                else
                                {
                                    errorMessage = "Failed to update treatment";
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
            else
            {
                errorMessage = "Please select an image to upload";
                return;
            }
        }
        public class TreatmentInfo
        {
            public string id;
            public string patientCode;
            public string serviceId;
            public string examDesc;
            public string result;
            public string ImagePath;
        }
    }
}
