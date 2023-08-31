﻿using Ajaxcall.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Diagnostics;

namespace Ajaxcall.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult GetUser()
        {
            List<UserModel> UserName = new List<UserModel>();
            var dbconfig = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
            string dbConnectionStr = "";
            try
            {
                string sql = "[dbo].[GetUserProfile]";
                dbConnectionStr = dbconfig["ConnectionStrings:DefaultConnection"];
                using (SqlConnection connection = new SqlConnection(dbConnectionStr))
                {
                    SqlCommand command = new SqlCommand(sql, connection);
                    connection.Open();
                    using (SqlDataReader dataReader = command.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            UserModel User = new UserModel();
                            User.id = Convert.ToInt32(dataReader["ukid"]);
                            User.fname = Convert.ToString(dataReader["fname"]);
                            User.lname = Convert.ToString(dataReader["lname"]);
                            User.ProfilePicture = Convert.ToString(dataReader["profile_picture"]);
                            UserName.Add(User);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return View(UserName);
        }

        [HttpPost]
        public IActionResult CreateUser(UserModel user)
        {
            try
            {
                var dbconfig = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json").Build();

                string dbConnectionStr = dbconfig["ConnectionStrings:DefaultConnection"];
                using (SqlConnection connection = new SqlConnection(dbConnectionStr))
                {
                    string sql = "[dbo].[createuser]";
                    using (SqlCommand cmd = new SqlCommand(sql, connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@fname", user.fname);
                        cmd.Parameters.AddWithValue("@lname", user.lname);
                        cmd.Parameters.AddWithValue("@profile_picture", user.ProfilePicture);

                        connection.Open();
                        cmd.ExecuteNonQuery();
                        connection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return RedirectToAction("GetUser");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
