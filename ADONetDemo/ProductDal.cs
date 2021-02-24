using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADONetDemo
{
    public class ProductDal
    {
        SqlConnection _connection = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB; initial Catalog = Etrade;
                                                        integrated security = true");
        public List<Product> GetAll()
        {
            ConnectionControl();
            SqlCommand command = new SqlCommand("Select * from ProductTable",_connection);

           SqlDataReader reader =  command.ExecuteReader();

            List<Product> products = new List<Product>();
            while (reader.Read())
            {
                Product product = new Product
                {
                    Id = Convert.ToInt32(reader["Id"]),
                    ProductName = reader["ProductName"].ToString(),
                    UnitPrice = Convert.ToDecimal(reader["UnitPrice"]),
                    StockAmount = Convert.ToInt32(reader["StockAmount"])
                };
                products.Add(product);
            }

            reader.Close();
            _connection.Close();
            return products;
        }
        private void ConnectionControl()
        {
            if (_connection.State == System.Data.ConnectionState.Closed)
            {
                _connection.Open();
            }
        }
        public DataTable GetAll2()
        {
            if (_connection.State == System.Data.ConnectionState.Closed)
            {
                _connection.Open();
            }
            SqlCommand command = new SqlCommand("Select * from ProductTable", _connection);

            SqlDataReader reader = command.ExecuteReader();

            DataTable dataTable = new DataTable();
            dataTable.Load(reader);
            reader.Close();
            _connection.Close();
            return dataTable;

        }

        public void Add(Product product)
        {
            ConnectionControl();
            SqlCommand command = new SqlCommand("Insert into ProductTable values(@name,@unitPrice,@stockAmount)",_connection);
            command.Parameters.AddWithValue("@name",product.ProductName);
            command.Parameters.AddWithValue("@unitPrice", product.UnitPrice);
            command.Parameters.AddWithValue("@stockAmount", product.StockAmount);
            command.ExecuteNonQuery();

            _connection.Close();
        }
        public void Update(Product product)
        {
            ConnectionControl();
            SqlCommand command = new SqlCommand("update ProductTable set ProductName=@name, UnitPrice=@unitPrice, " +
                                                "StockAmount = @stockAmount where Id = @id", _connection);
            command.Parameters.AddWithValue("@name", product.ProductName);
            command.Parameters.AddWithValue("@unitPrice", product.UnitPrice);
            command.Parameters.AddWithValue("@stockAmount", product.StockAmount);
            command.Parameters.AddWithValue("@id",product.Id);
            command.ExecuteNonQuery();

            _connection.Close();
        }

        public void Delete(int id)
        {
            ConnectionControl();
            SqlCommand command = new SqlCommand("delete from ProductTable where Id = @id", _connection);
            command.Parameters.AddWithValue("@id",id);
            command.ExecuteNonQuery();
            _connection.Close();
        }
    }
}
