using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using ProjectBig.Models;

namespace WebAppThird.Controllers
{
    public class AccountController : Controller
    {
        private readonly IConfiguration _config;

        public AccountController(IConfiguration config)
        {
            _config = config;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(UserInfo userInfo)
        {
            string message = "";

            string connectionString = _config.GetConnectionString("DefaultConnection");
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("login", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@username", userInfo.username);
                    command.Parameters.AddWithValue("@password", userInfo.password);

                    connection.Open();
                    using SqlDataReader reader = command.ExecuteReader();
                    if (reader != null)
                    {
                        if (reader.Read())
                        {
                            message = reader["Message"].ToString();
                        }
                    }
                }
            }

            ViewBag.Message = message;

            if (!string.IsNullOrEmpty(message))
            {
                Response.Cookies.Append("username", userInfo.username);
                // Successful login, redirect to Home/Index
                return RedirectToAction("Index", "Home");
            }
            else
            {
                // Show the login view
                return View();
            }
        }

        public IActionResult Register()
        {
            return View();
        }

        //      [HttpPost]
        //public IActionResult RegisterUser(UserInfo userInfo)
        //{
        //	string message;
        //	if (ModelState.IsValid)
        //	{
        //		try
        //		{
        //			using (var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
        //			{
        //				using (var command = new SqlCommand("createuser", connection))
        //				{
        //					command.CommandType = CommandType.StoredProcedure;
        //					command.Parameters.AddWithValue("@fname", userInfo.fname);
        //					command.Parameters.AddWithValue("@lname", userInfo.lname);
        //					command.Parameters.AddWithValue("@username", userInfo.username);
        //					command.Parameters.AddWithValue("@email", userInfo.email);
        //					command.Parameters.AddWithValue("@age", userInfo.age);
        //					command.Parameters.AddWithValue("@userpass", userInfo.password);
        //					var messageParam = new SqlParameter("@message", SqlDbType.NVarChar, 50);
        //					messageParam.Direction = ParameterDirection.Output;
        //					command.Parameters.Add(messageParam);
        //					connection.Open();
        //					command.ExecuteNonQuery();
        //					message = command.Parameters["@message"].Value.ToString();
        //				}
        //			}
        //			ViewBag.Message = message;
        //			return View();
        //		}
        //		catch (SqlException ex)
        //		{
        //			// Log or handle the SQL exception
        //			Console.WriteLine(ex.ToString());
        //			throw; // Rethrow the exception to halt the execution and display the error page
        //		}
        //	}
        //	else
        //	{
        //		return View(userInfo);
        //	}
        //}
        [HttpPost]
        public IActionResult UpdateProfilePicture(string username, string profileLink)
        {
            try
            {
                // Check if a profile picture link is provided
                if (!string.IsNullOrEmpty(profileLink))
                {
                    // Update the profile picture in the database using the provided username and profileLink
                    using (var connection = new SqlConnection("DefaultConnection"))
                    {
                        connection.Open();

                        var command = new SqlCommand("uploadprofilepic", connection);
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@username", username);
                        command.Parameters.AddWithValue("@profile_picture", profileLink);

                        var messageParam = command.Parameters.Add("@message", SqlDbType.NVarChar, 50);
                        messageParam.Direction = ParameterDirection.Output;

                        command.ExecuteNonQuery();

                        var message = messageParam.Value.ToString();
                    }
                }

                // Redirect to the profile page or perform any additional actions
                return RedirectToAction("UpdateProfile");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while updating the profile picture.";
                // Handle the exception or display an error message as needed
                return RedirectToAction("UpdateProfile");
            }
        }





        public IActionResult MyAction()
        {
            if (Request.Cookies["username"] != null)
            {
                // The "user" cookie exists
            }
            else
            {
                // The "user" cookie does not exist
            }

            return View();
        }
        [HttpPost]
        public IActionResult Logout()
        {
            // Remove the "user" cookie to log out the user
            Response.Cookies.Delete("username");

            // Redirect the user to the login page or any other desired page
            return RedirectToAction("Index", "Account");
        }







        //[HttpPost]
        //public IActionResult UpdateProfile(UserInfo userInfo, IFormFile profilePicture)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            var dbConnectionStr = _config.GetConnectionString("DefaultConnection");
        //            using (SqlConnection connection = new SqlConnection(dbConnectionStr))
        //            {
        //                string sql = "updateprofile";
        //                using (SqlCommand cmd = new SqlCommand(sql, connection))
        //                {
        //                    cmd.CommandType = CommandType.StoredProcedure;
        //                    cmd.Parameters.AddWithValue("@Ukid", userInfo.Id);
        //                    cmd.Parameters.AddWithValue("@fname", userInfo.fname);
        //                    cmd.Parameters.AddWithValue("@lname", userInfo.lname);
        //                    cmd.Parameters.AddWithValue("@email", userInfo.email);
        //                    cmd.Parameters.AddWithValue("@age", userInfo.age);
        //                    cmd.Parameters.AddWithValue("@phoneno", userInfo.phoneno);
        //                    var profilePictureParam = new SqlParameter("@profile_picture", SqlDbType.NVarChar, 2000);
        //                    profilePictureParam.Value = GetProfilePictureData(profilePicture); // Helper method to get profile picture data from IFormFile
        //                    cmd.Parameters.Add(profilePictureParam);
        //                    var messageParam = new SqlParameter("@message", SqlDbType.NVarChar, 50);
        //                    messageParam.Direction = ParameterDirection.Output;
        //                    cmd.Parameters.Add(messageParam);

        //                    connection.Open();
        //                    cmd.ExecuteNonQuery();

        //                    string message = messageParam.Value.ToString();
        //                    TempData["Message"] = message;
        //                }
        //            }

        //            return RedirectToAction("Profile");
        //        }
        //        catch (Exception ex)
        //        {
        //            TempData["ErrorMessage"] = "An error occurred while updating the profile.";

        //            return RedirectToAction("Profile");
        //        }
        //    }

        //    return View(userInfo);
        //}

        //private byte[] GetProfilePictureData(IFormFile profilePicture)
        //{
        //    using (var memoryStream = new MemoryStream())
        //    {
        //        profilePicture.CopyTo(memoryStream);
        //        return memoryStream.ToArray();
        //    }
        //}

        //        [HttpPost]
        //        public IActionResult UploadProfilePicture(int Ukid, IFormFile profilePicture)
        //        {
        //            try
        //            {
        //                var dbConnectionStr = _config.GetConnectionString("DefaultConnection");
        //                using (SqlConnection connection = new SqlConnection(dbConnectionStr))
        //                {
        //                    string sql = "uploadprofilepic";
        //                    using (SqlCommand cmd = new SqlCommand(sql, connection))
        //                    {
        //                        cmd.CommandType = CommandType.StoredProcedure;
        //                        cmd.Parameters.AddWithValue("@Ukid", Ukid);

        //                        var profilePictureParam = new SqlParameter("@profile_picture", SqlDbType.NVarChar, 2000);
        //                        profilePictureParam.Value = GetProfilePictureData(profilePicture); // Helper method to get profile picture data from IFormFile
        //                        cmd.Parameters.Add(profilePictureParam);

        //                        var messageParam = new SqlParameter("@message", SqlDbType.NVarChar, 50);
        //                        messageParam.Direction = ParameterDirection.Output;
        //                        cmd.Parameters.Add(messageParam);

        //                        connection.Open();
        //                        cmd.ExecuteNonQuery();

        //                        string message = messageParam.Value.ToString();
        //                        TempData["Message"] = message;
        //                    }
        //                }

        //                return RedirectToAction("Index");
        //            }
        //            catch (Exception ex)
        //            {
        //                TempData["ErrorMessage"] = "An error occurred while uploading the profile picture.";

        //                return RedirectToAction("Profile");
        //            }
        //        }
    }
}

//public async Task<string> AuthenticateUser(string email, string password)
//{
//    var userNameParameter = new SqlParameter("@UserName", SqlDbType.NVarChar, 200)
//    {
//        Direction = ParameterDirection.Output
//    };

//    await _context.Database.ExecuteSqlRawAsync("EXEC [dbo].[AuthenticateUser] @Email, @Password, @UserName OUT",
//        new SqlParameter("@Email", email),
//