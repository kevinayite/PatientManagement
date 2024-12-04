using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace FinalProject.Pages
{
    public class IndexModel : PageModel
    {

        private readonly IHttpContextAccessor _httpContextAccessor;
        public Account accountInfo = new Account();
        public string errorMessage = "";
        public string successMessage = "";
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }


        public class Account
        {
            public string username { get; set; }
            public string password { get; set; }
            public string role { get; set; }
        }
        public void OnGet()
        {

        }

        public IActionResult OnPost()
        {
            accountInfo.username = Request.Form["username"];
            accountInfo.password = Request.Form["password"];



            try
            {
                String conString = "Data Source=PIERRE-KASANANI\\SQLEXPRESS;Initial Catalog=projectDB;Integrated Security=True";
                using (SqlConnection con = new SqlConnection(conString))
                {
                    con.Open();
                    string sqlQuery = "SELECT * FROM Staff WHERE phoneNumber = @v_phoneNumber AND pwd = @v_pwd";
                    using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
                    {
                        cmd.Parameters.AddWithValue("@v_phoneNumber", accountInfo.username);
                        cmd.Parameters.AddWithValue("@v_pwd", accountInfo.password);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // Authentication successful, populate role
                                accountInfo.role = reader.GetString(reader.GetOrdinal("serviceProvided"));

                                // Store role in session
                                _httpContextAccessor.HttpContext.Session.SetString("UserRole", accountInfo.role);

                                // Redirect based on role
                                switch (accountInfo.role)
                                {
                                    case "Doctor":
                                        return RedirectToPage("/Employee/Index"); // Redirect to doctor page
                                    case "Receptionist":
                                        return RedirectToPage("/Receptionist/Index"); // Redirect to receptionist page
                                    // Add more cases for other roles as needed
                                    case "Manager":
                                        return RedirectToPage("/Services/Displaying");
                                    default:
                                        errorMessage = "Invalid role";
                                        return Page(); // Stay on the login page
                                }
                            }
                            else
                            {
                                errorMessage = "Invalid username or password";
                                return Page(); // Stay on the login page
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return Page(); // Stay on the login page
            }
        }
    }
}
