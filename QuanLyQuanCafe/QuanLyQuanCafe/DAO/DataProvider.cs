using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyQuanCafe.DAO
{
    public class DataProvider
    {
        private string connectionStr = @"Data Source=KHANHBVSE63379\SQLEXPRESS;Initial Catalog=QuanLyQuanCafe;Integrated Security=True";
        private static DataProvider instance; // Ctrl + R + E

        public static DataProvider Instance
        {
            get { if (instance == null) instance = new DataProvider(); return DataProvider.instance; }
            private set { DataProvider.instance = value; }
        }

        private DataProvider() { } //ben ngoai khong the tac dong vao duoc

        public DataTable ExecuteQuery(string query, object[] parameter = null)
        {
            DataTable dt = null;
            SqlConnection con = new SqlConnection(connectionStr);
            try
            {
                if(con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                SqlCommand command = new SqlCommand(query, con);

                if (parameter != null)
                {
                    string[] listPara = query.Split(' ');
                    int i = 0;
                    foreach (string item in listPara)
                    {
                        if (item.Contains('@'))
                        {
                            command.Parameters.AddWithValue(item, parameter[i]);
                            i++;
                        }
                    }
                }

                dt = new DataTable();
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(dt);

            } catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                con.Close();
            }
            return dt;
        }

        public int ExecuteNonQuery(string query, object[] parameter = null)
        {
            int data = 0;

            using (SqlConnection connection = new SqlConnection(connectionStr))
            {
                connection.Open();

                SqlCommand command = new SqlCommand(query, connection);

                if (parameter != null)
                {
                    string[] listPara = query.Split(' ');
                    int i = 0;
                    foreach (string item in listPara)
                    {
                        if (item.Contains('@'))
                        {
                            command.Parameters.AddWithValue(item, parameter[i]);
                            i++;
                        }
                    }
                }

                data = command.ExecuteNonQuery();

                connection.Close();
            }

            return data;
        }


        public object ExecuteScalar(string query, object[] parameter = null)
        {
            object dt = 0;
            SqlConnection con = new SqlConnection(connectionStr);
            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                SqlCommand command = new SqlCommand(query, con);

                if (parameter != null)
                {
                    string[] listParam = query.Split(' ');
                    foreach (string item in listParam)
                    {
                        int count = 0;
                        if (item.Contains('@'))
                        {
                            command.Parameters.AddWithValue(item, parameter[count]);
                            count++;
                        }
                    }
                }

                dt = command.ExecuteScalar();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                con.Close();
            }
            return dt;
        }
    }
}



