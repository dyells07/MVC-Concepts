using Microsoft.AspNetCore.Mvc;

using System.Data;
using System.Data.SqlClient;
using ProjectBig.Models;

namespace WebAppThird.Controllers
{
    public class ProfileController : Controller
    {
        private readonly IConfiguration _configuration;
      
     

        public ProfileController(IConfiguration configuration)
        {
            _configuration = configuration;
            
        }

        public IActionResult GetUserProfile(string username)
        {
            try
            {
                string dbConnectionStr = _configuration.GetConnectionString("DefaultConnection");
                using (SqlConnection connection = new SqlConnection(dbConnectionStr))
                {
                    connection.Open();

                    // Retrieve the id from the database based on the username
                    string getIdQuery = "SELECT Ukid FROM UserReg WHERE username = @username";
                    SqlCommand getIdCommand = new SqlCommand(getIdQuery, connection);
                    getIdCommand.Parameters.AddWithValue("@username", username);
                    int id = (int)getIdCommand.ExecuteScalar();

                    string sql = "GetUserProfile";
                    SqlCommand command = new SqlCommand(sql, connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Ukid", id);

                    // Add output parameters
                    command.Parameters.Add("@fname", SqlDbType.NVarChar, 200).Direction = ParameterDirection.Output;
                    command.Parameters.Add("@lname", SqlDbType.NVarChar, 200).Direction = ParameterDirection.Output;
                    command.Parameters.Add("@username", SqlDbType.NVarChar, 200).Direction = ParameterDirection.Output;
                    command.Parameters.Add("@email", SqlDbType.NVarChar, 200).Direction = ParameterDirection.Output;
                    command.Parameters.Add("@phoneno",SqlDbType.NVarChar, 200).Direction=ParameterDirection.Output;
                    command.Parameters.Add("@profile_picture", SqlDbType.NVarChar, 2000).Direction = ParameterDirection.Output;

                    command.ExecuteNonQuery();

                    // Retrieve values from output parameters
                    string fname = command.Parameters["@fname"].Value.ToString();
                    string lname = command.Parameters["@lname"].Value.ToString();
                    string retrievedUsername = command.Parameters["@username"].Value.ToString();
                    string email = command.Parameters["@email"].Value.ToString();
                    string profilePicture = command.Parameters["@profile_picture"].Value.ToString();

                    UserInfo userInfo = new UserInfo
                    {
                        Id = id,
                        fname = fname,
                        lname = lname,
                        username = retrievedUsername,
                        email = email,
                        ProfilePicture = profilePicture
                    };

                    return View(userInfo);
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while retrieving the user profile.";
                return RedirectToAction("GetUserProfile");
            }
        }

        [HttpGet]
        public IActionResult UpdateProfile(string username)
        {
            try
            {
                string dbConnectionStr = _configuration.GetConnectionString("DefaultConnection");
                using (SqlConnection connection = new SqlConnection(dbConnectionStr))
                {
                    connection.Open();

                    string sql = "SELECT * FROM UserReg WHERE username = @username";
                    SqlCommand command = new SqlCommand(sql, connection);
                    command.Parameters.AddWithValue("@username", username);

                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        UserInfo userInfo = new UserInfo
                        {
                            Id = (int)reader["Ukid"],
                            fname = reader["fname"].ToString(),
                            lname = reader["lname"].ToString(),
                            username = reader["username"].ToString(),
                            email = reader["email"].ToString(),

                            ProfilePicture = reader["profile_picture"].ToString()
                        };

                        return View(userInfo);
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while retrieving the user profile.";
            }

            return RedirectToAction("GetUserProfile");
        }




        [HttpPost]
        public IActionResult EditProfile(UserInfo userInfo)
        {
            try
            {
                string dbConnectionStr = _configuration.GetConnectionString("DefaultConnection");
                using (SqlConnection connection = new SqlConnection(dbConnectionStr))
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand("updateprofile", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Ukid", userInfo.Id);
                    command.Parameters.AddWithValue("@fname", userInfo.fname);
                    command.Parameters.AddWithValue("@lname", userInfo.lname);
                    command.Parameters.AddWithValue("@email", userInfo.email);
                    command.Parameters.AddWithValue("@age", userInfo.age);
                    command.Parameters.AddWithValue("@profile_picture", userInfo.ProfilePicture);

                    SqlParameter messageParam = new SqlParameter("@message", SqlDbType.NVarChar, 50);
                    messageParam.Direction = ParameterDirection.Output;
                    command.Parameters.Add(messageParam);

                    command.ExecuteNonQuery();

                    string message = command.Parameters["@message"].Value.ToString();
                    TempData["SuccessMessage"] = message;

                    return RedirectToAction("GetUserProfile", new { username = userInfo.username });
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while updating the user profile.";
                return RedirectToAction("GetUserProfile", new { username = userInfo.username });
            }
        }

        //[HttpPost]
        //public IActionResult UpdateProfilePicture(UserInfo userInfo)
        //{
        //    try
        //    {
        //        string username = userInfo.username;
        //        string profileLink = HttpContext.Request.Form["profilePicture"];

        //        // Check if a profile picture is provided
        //        if (!string.IsNullOrEmpty(profileLink))
        //        {
        //            // Update the profile picture in the database using the provided username
        //            using (var connection = new SqlConnection("DefaultConnection"))
        //            {
        //                connection.Open();

        //                var command = new SqlCommand("uploadprofilepic", connection);
        //                command.CommandType = CommandType.StoredProcedure;
        //                command.Parameters.AddWithValue("@username", username);
        //                command.Parameters.AddWithValue("@profile_picture", profileLink);

        //                var messageParam = command.Parameters.Add("@message", SqlDbType.NVarChar, 50);
        //                messageParam.Direction = ParameterDirection.Output;

        //                command.ExecuteNonQuery();

        //                var message = messageParam.Value.ToString();
        //            }
        //        }

        //        // Redirect to the profile page or perform any additional actions
        //        return RedirectToAction("UpdateProfile");
        //    }
        //    catch (Exception ex)
        //    {
        //        TempData["ErrorMessage"] = "An error occurred while updating the profile picture.";
        //        // Handle the exception or display an error message as needed
        //    }
        //    return RedirectToAction("UpdateProfile");
        //}














        //[HttpPost]
        //public IActionResult UpdateProfilePicture(string username, string profileLink)
        //{
        //    try
        //    {
        //        // Check if a profile picture link is provided
        //        if (!string.IsNullOrEmpty(profileLink))
        //        {
        //            // Update the profile picture in the database using the provided username and profileLink
        //            using (var connection = new SqlConnection("DefaultConnection"))
        //            {
        //                connection.Open();

        //                var command = new SqlCommand("uploadprofilepic", connection);
        //                command.CommandType = CommandType.StoredProcedure;
        //                command.Parameters.AddWithValue("@username", username);
        //                command.Parameters.AddWithValue("@profile_picture", profileLink);

        //                var messageParam = command.Parameters.Add("@message", SqlDbType.NVarChar, 50);
        //                messageParam.Direction = ParameterDirection.Output;

        //                command.ExecuteNonQuery();

        //                var message = messageParam.Value.ToString();
        //            }
        //        }

        //        // Redirect to the profile page or perform any additional actions
        //        return RedirectToAction("Profile");
        //    }
        //    catch (Exception ex)
        //    {
        //        TempData["ErrorMessage"] = "An error occurred while updating the profile picture.";
        //        // Handle the exception or display an error message as needed
        //        return RedirectToAction("UpdateProfile");
        //    }
        //}







        public IActionResult UpdateProfilePicture(string username, string ProfilePicture)
        {
            string connectionString = _configuration.GetConnectionString("DefaultConnection");
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("uploadprofilepic", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Parameters.AddWithValue("@profile_picture", ProfilePicture);
                    try
                    {
                        connection.Open();
                        cmd.ExecuteNonQuery();
                        return RedirectToAction("UpdateProfile");
                    }
                    catch (Exception ex)
                    {
                        TempData["ErrorMessage"] = "An error occurred while updating the profile picture.";
                        // Handle the exception or display an error message as needed
                        return RedirectToAction("UpdateProfile");
                    }
                }
            }
        }

    }
}
