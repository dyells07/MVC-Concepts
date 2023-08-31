
using Calling.Hubs;
using Calling.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Calling.Controllers
{
    public class ProductController : Controller
    {

        //private readonly IHubContext<ItemHub> _itemHubContext;

        //public ProductController(IHubContext<ItemHub> itemHubContext)
        //{
        //    _itemHubContext = itemHubContext;
        //}
        public IActionResult Index()
        {
            List<ProductModel> productList = new List<ProductModel>();
            var dbconfig = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
            string dbConnectionStr = "";
            try
            {
                string sql = "[dbo].[USP_Get_ProductList]";
                dbConnectionStr = dbconfig["ConnectionStrings:DefaultConnection"];
                using (SqlConnection connection = new SqlConnection(dbConnectionStr))
                {
                    SqlCommand command = new SqlCommand(sql, connection);
                    connection.Open();
                    using (SqlDataReader dataReader = command.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            ProductModel product = new ProductModel();
                            product.Id = Convert.ToInt32(dataReader["Id"]);
                            product.ProductName = Convert.ToString(dataReader["ProductName"]);
                            product.ProductDescription = Convert.ToString(dataReader["ProductDescription"]);
                            product.ProductCost = Convert.ToDecimal(dataReader["ProductCost"]);
                            product.Stock = Convert.ToInt32(dataReader["Stock"]);
                            productList.Add(product);



                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return View(productList);

        }
        //[dbo].[SP_Get_Product_By_Id]
        public IActionResult ProductCreate()
        {
            return View();
        }

        public IActionResult Count()
        {
            return View();
        }



        [HttpPost]
        public IActionResult CreateProduct(ProductModel product)
        {
            try

            {
                
                    var dbconfig = new ConfigurationBuilder()
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.json").Build();

                    string dbConnectionStr = "";
                    if (!string.IsNullOrEmpty(dbconfig.ToString()))
                    {
                        dbConnectionStr = dbconfig["ConnectionStrings:DefaultConnection"];
                        using (SqlConnection connection = new SqlConnection(dbConnectionStr))
                        {
                            string sql = "USP_Add_New_Product";
                            using (SqlCommand cmd = new SqlCommand(sql, connection))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue("@ProductName", product.ProductName);
                                cmd.Parameters.AddWithValue("@ProductDescription", product.ProductDescription);
                                cmd.Parameters.AddWithValue("@ProductCost", product.ProductCost);
                                cmd.Parameters.AddWithValue("@Stock", product.Stock);
                                connection.Open();
                                cmd.ExecuteNonQuery();
                                connection.Close();
                            }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return RedirectToAction("Index");
        }



        [HttpPost]
        public IActionResult ProductDelete(int id)
        {
            var dbconfig = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
            var dbconnectionStr = dbconfig["ConnectionStrings:DefaultConnection"];

            using (SqlConnection connection = new SqlConnection(dbconnectionStr))
            {
                connection.Open();

                string sql = "USP_Delete_Product_By_Id";

                using (SqlCommand cmd = new SqlCommand(sql, connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id", id);

                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (SqlException ex)
                    {
                        throw;
                    }
                }

                connection.Close();
            }

            return RedirectToAction("Index");
        }
        private readonly string _dbconnectionStr;

        public ProductController(IConfiguration configuration)
        {
            _dbconnectionStr = configuration.GetConnectionString("DefaultConnection");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            using (SqlConnection con = new SqlConnection(_dbconnectionStr))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM [dbo].[Product_Master] WHERE Id = @id", con))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    try
                    {
                        con.Open();
                        using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                        {
                            DataTable dataTable = new DataTable();
                            adapter.Fill(dataTable);
                            if (dataTable.Rows.Count > 0)
                            {
                                ProductModel product = new ProductModel();
                                //product.Id = (int)dataTable.Rows[0]["Id"];
                                product.ProductName = (string)dataTable.Rows[0]["product_name"];
                                product.ProductDescription = (string)dataTable.Rows[0]["product_desc"];
                                product.ProductCost = (decimal)dataTable.Rows[0]["cost"];
                                product.Stock = (int)dataTable.Rows[0]["stock"];
                                return View(product);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw;
                    }
                }
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Edit(int id, ProductModel product)
        {
            using (SqlConnection con = new SqlConnection(_dbconnectionStr))
            {
                using (SqlCommand cmd = new SqlCommand("USP_Update_Product", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.Parameters.AddWithValue("@ProductName", product.ProductName);
                    cmd.Parameters.AddWithValue("@ProductDescription", product.ProductDescription);
                    cmd.Parameters.AddWithValue("@ProductCost", product.ProductCost);
                    cmd.Parameters.AddWithValue("@Stock", product.Stock);
                    try
                    {
                        con.Open();
                        cmd.ExecuteNonQuery();
                        return RedirectToAction("Index");
                    }
                    catch (Exception ex)
                    {
                        throw;
                    }
                }
            }
            return View(product);
        }
    }
}
