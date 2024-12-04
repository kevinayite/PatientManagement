using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using static FinalProject.Pages.Receptionist.IndexModel;
using System.Data.SqlClient;

using iTextSharp.text;
using iTextSharp.text.pdf;




namespace FinalProject.Pages.Receptionist
{
    public class UpdatePaymentModel : PageModel
    {
        public List<PaymentInfo> listPayment = new List<PaymentInfo>();

        public PaymentInfo paymentInfo = new PaymentInfo();
        public string errorMessage = "";
        public string successMessage = "";
        public string patientName { get; set; }
        public string serviceType { get; set; }
        public string totalPrice { get; set; }
        public string patientid { get; set; }
        //public IFormFile ImageFile { get; set; }

        public FileContentResult GeneratePDFReceipt(string patientName, string serviceType, string price)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                // Create a new PDF document
                Document document = new Document(PageSize.A4, 30, 30, 30, 30); // Set margins (left, right, top, bottom)
                PdfWriter.GetInstance(document, ms);

                // Open the document for writing
                document.Open();

                // Add a title
                Font titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 18);
                Paragraph title = new Paragraph("Receipt", titleFont);
                title.Alignment = Element.ALIGN_CENTER;
                title.SpacingAfter = 20;
                document.Add(title);

                // Add patient and service details
                Font detailsFont = FontFactory.GetFont(FontFactory.HELVETICA, 12);
                Paragraph patientDetails = new Paragraph($"Patient Name: {patientName}", detailsFont);
                patientDetails.SpacingAfter = 10;
                document.Add(patientDetails);

                Paragraph serviceDetails = new Paragraph($"Service Type: {serviceType}", detailsFont);
                serviceDetails.SpacingAfter = 10;
                document.Add(serviceDetails);

                // Add price
                Font priceFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 14);
                Paragraph priceText = new Paragraph($"Price: {price}", priceFont);
                priceText.SpacingBefore = 20;
                document.Add(priceText);

                // Add a horizontal rule
                Paragraph horizontalRule = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.5f, 100, BaseColor.GRAY, Element.ALIGN_CENTER, 1)));
                horizontalRule.SpacingBefore = 20;
                horizontalRule.SpacingAfter = 20;
                document.Add(horizontalRule);

                // Add a footer
                Font footerFont = FontFactory.GetFont(FontFactory.HELVETICA, 10, BaseColor.GRAY);
                Paragraph footerText = new Paragraph("Thank you for your business!", footerFont);
                footerText.Alignment = Element.ALIGN_CENTER;
                document.Add(footerText);

                // Close the document
                document.Close();

                // Return the PDF content as a FileContentResult
                return File(ms.ToArray(), "application/pdf", $"{patientName}_Receipt.pdf");
            }
        }
        public FileContentResult OnGetDownloadReceipt(string patientName, string serviceType, string price)
        {
            return GeneratePDFReceipt(patientName, serviceType, price);
        }
        public void OnGet()
        {
            listPayment.Clear();
            try
            {
                String conString = "Data Source=PIERRE-KASANANI\\SQLEXPRESS;Initial Catalog=projectDB;Integrated Security=True";
                using (SqlConnection con = new SqlConnection(conString))
                {
                    con.Open();
                    string sqlQuery = "SELECT pm.patientid, p.names AS patientName, s.type AS serviceType, pm.price AS price " +
                                      "FROM payment pm " +
                                      "JOIN patients p ON p.phoneNumber = pm.patientid " +
                                      "JOIN services s ON pm.serviceId = s.id";

                    using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string retrievedPatientId = reader["patientid"].ToString();
                                string retrievedPatientName = reader["patientName"].ToString();
                                string retrievedServiceType = reader["serviceType"].ToString();
                                string retrievedPrice = reader["price"].ToString();

                                PaymentInfo paymentInfo = new PaymentInfo();

                                paymentInfo.patientid = retrievedPatientId;
                                paymentInfo.patientName = retrievedPatientName;
                                paymentInfo.serviceType = retrievedServiceType;
                                paymentInfo.price = retrievedPrice;

                                listPayment.Add(paymentInfo);
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

        //public byte[] GeneratePDFReceipt(string patientName, string serviceType, string price)
        //{
        //    using (MemoryStream ms = new MemoryStream())
        //    {
        //        // Create a new PDF document
        //        Document document = new Document();
        //        PdfWriter.GetInstance(document, ms);

        //        // Open the document for writing
        //        document.Open();

        //        // Add content to the PDF
        //        document.Add(new Paragraph($"Patient Name: {patientName}"));
        //        document.Add(new Paragraph($"Service Type: {serviceType}"));
        //        document.Add(new Paragraph($"Price: {price}"));

        //        // Close the document
        //        document.Close();

        //        // Return the PDF content as a byte array
        //        return ms.ToArray();
        //    }
        //}


        //public IActionResult OnGet2(string handler, string patientName)
        //{
        //    if (handler == "DownloadReceipt" && !string.IsNullOrEmpty(patientName))
        //    {
        //        // Find the payment info for the given patientId
        //        var paymentInfo = listPayment.FirstOrDefault(p => p.patientName == patientName);

        //        if (paymentInfo != null)
        //        {
        //            // Generate the PDF content
        //            byte[] pdfContent = GeneratePDFReceipt(paymentInfo.patientName, paymentInfo.serviceType, paymentInfo.price);

        //            // Return the PDF content as a file download
        //            return File(pdfContent, "application/pdf", $"{paymentInfo.patientName}_Receipt.pdf");
        //        }
        //    }

        //    // Handle other cases or return the page
        //    return Page();
        //}
       
        public void OnPost()
        {
            paymentInfo.patientid = Request.Form["patientid"];
            paymentInfo.serviceId = Request.Form["serviceId"];
            paymentInfo.price = Request.Form["price"];

            try
            {
                String conString = "Data Source=PIERRE-KASANANI\\SQLEXPRESS;Initial Catalog=projectDB;Integrated Security=True";
                using (SqlConnection con = new SqlConnection(conString))
                {
                    con.Open();
                    string sqlQuery = "INSERT INTO payment (patientid,serviceId,price) VALUES (@patientid, @serviceId,@price)";
                    using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
                    {
                        // Add parameters with their values
                        cmd.Parameters.AddWithValue("@patientid", paymentInfo.patientid);
                        cmd.Parameters.AddWithValue("@serviceId", paymentInfo.serviceId);
                        cmd.Parameters.AddWithValue("@price", paymentInfo.price);

                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            successMessage = "Don't forget to tell the customer thank you for paying";
                        }
                        else
                        {
                            errorMessage = "Payment failed";
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
    public class PaymentInfo
    {
        public string id;
        public string patientid;
        public string serviceId;
        public string price;

        public string patientName;
        public string serviceType;
        public double totalPrice;


    }


}

