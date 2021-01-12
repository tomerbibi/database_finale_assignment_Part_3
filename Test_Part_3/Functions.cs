using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Test_Part_3
{
    static class Functions
    {
        static private readonly log4net.ILog my_logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        static private readonly string connection_string;
        static private SqlConnection con = new SqlConnection(connection_string);
        static Functions()
        {
            var reader = File.OpenText("Test_Part_3.config.json");
            string connection_string = reader.ReadToEnd();
        }
        static public void ExecuteNonQuery(string query)
        {
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.CommandType = CommandType.Text;
                cmd.ExecuteScalar();
                con.Close();
            }
            catch(Exception)
            {
                my_logger.Error("failed to connect to databace");
            }
        }
        static public List<Store> GetStores()
        {
            List<Store> stores = new List<Store>();
            con.Open();
            SqlCommand cmd = new SqlCommand("select * from Stores", con);
            cmd.CommandType = CommandType.Text;
            SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.Default);
            while (reader.Read() == true)
            {
                Store s = new Store((int)reader.GetValue(0), (string)reader.GetValue(1), (int)reader.GetValue(2), (long)reader.GetValue(3));
                stores.Add(s);
            }
            con.Close();
            return stores;
        }
        static public Store GetStoreById(int id)
        {
            con.Open();
            Store s = new Store();
            SqlCommand cmd = new SqlCommand($"select * from Stores where ID = {id}", con);
            cmd.CommandType = CommandType.Text;
            SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.Default);
            while (reader.Read() == true)
            {
                s = new Store((int)reader.GetValue(0), (string)reader.GetValue(1), (int)reader.GetValue(2), (long)reader.GetValue(3));
                return s;
            }
            con.Close();
            return s;
        }
        static public void AddStore(string name, int floor, long categories_id)
        {
            try
            {
                ExecuteNonQuery($"insert into Stores(Name_, Floor_, Categories_ID) values('{name}', {floor}, {categories_id})");
                my_logger.Info("store was added to stores");
            }
            catch(Exception)
            {
                my_logger.Error("failed to add a store")
            }
        }
        static public void UpdateStore(int id, string name, int floor, long categories_id)
        {
            ExecuteNonQuery($"update Stores set Name_ = '{name}', Floor_ = {floor}, Categories_ID = {categories_id} where ID = {id}");
        }
        static public void DeleteStore(int id)
        {
            try
            {
                ExecuteNonQuery($"delete from Stores where ID = {id}");
                my_logger.Info("store was deleted from stores");
            }
            catch
            {
                my_logger.Error("failed to delete store");
            }
        }
        static public List<Categories> GetCategories()
        {
            List<Categories> categories = new List<Categories>();
            con.Open();
            SqlCommand cmd = new SqlCommand("select * from Categories", con);
            cmd.CommandType = CommandType.Text;
            SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.Default);
            while (reader.Read() == true)
            {
                Categories c = new Categories((long)reader.GetValue(0), (string)reader.GetValue(1));
                categories.Add(c);
            }
            con.Close();
            return categories;
        }
        static public Categories GetCategoryById(int id)
        {
            con.Open();
            Categories c = new Categories();
            SqlCommand cmd = new SqlCommand($"select * from Categories where ID = {id}", con);
            cmd.CommandType = CommandType.Text;
            SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.Default);
            while (reader.Read() == true)
            {
                c = new Categories((long)reader.GetValue(0), (string)reader.GetValue(1));
                return c;
            }
            con.Close();
            return c;
        }
        static public void AddCategory(string name)
        {
            try
            {
                ExecuteNonQuery($"insert into Categories(Name_) values('{name}')");
                my_logger.Info("category was added to categoreis");
            }
            catch(Exception)
            {
                my_logger.Error("failed to add category");
            }
        }
        static public void UpdateCategory(int id, string name)
        {
            ExecuteNonQuery($"update Categories set Name_ = '{name}' where ID = {id}");
        }
        static public void DeleteCategory(int id)
        {
            try
            {
                ExecuteNonQuery($"delete from Categories where ID = {id}");
                my_logger.Info("category was deletet from categoreis");
            }
            catch(Exception)
            {
                my_logger.Error("failed to delete category");
            }
        }
        static public List<Store> GetStoresByCategoryAndFloor(int floor, long Category_id)
        {
            List<Store> stores = new List<Store>();
            con.Open();
            SqlCommand cmd = new SqlCommand($"select * from Stores where Floor_ = {floor} and Categories_ID = {Category_id}", con);
            cmd.CommandType = CommandType.Text;
            SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.Default);
            while (reader.Read() == true)
            {
                Store s = new Store((int)reader.GetValue(0), (string)reader.GetValue(1), (int)reader.GetValue(2), (long)reader.GetValue(3));
                stores.Add(s);
            }
            con.Close();
            return stores;
        }
        static public void GetCategoryWithMostStores()
        {
            long category_id = 0;
            con.Open();
            SqlCommand cmd = new SqlCommand("select top 1 s.Categories_ID from (SELECT COUNT(ID) amount_of_stores, Categories_ID" +
            " FROM Stores GROUP BY Categories_ID) s order by s.amount_of_stores desc", con);
            cmd.CommandType = CommandType.Text;
            SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.Default);
            while (reader.Read() == true)
            {
                category_id = (long)reader.GetValue(0);
            }
            con.Close();

            con.Open();
            SqlCommand cmd2 = new SqlCommand($"select * from Categories where ID = {category_id}", con);
            cmd2.CommandType = CommandType.Text;
            SqlDataReader reader2 = cmd2.ExecuteReader(CommandBehavior.Default);
            while (reader2.Read() == true)
            {
                Console.WriteLine($"ID: {reader2.GetValue(0)}, Name: {reader2.GetValue(1)}");
            }
            con.Close();
        }
    }
}
