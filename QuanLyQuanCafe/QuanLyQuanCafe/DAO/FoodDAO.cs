using QuanLyQuanCafe.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyQuanCafe.DAO
{
    public class FoodDAO
    {
        private static FoodDAO instane;

        public static FoodDAO Instance
        {
            get { if (instane == null) instane = new FoodDAO(); return instane; }
            private set { instane = value; }
        }

        private FoodDAO() { }

        public List<Food> GetListFoodByCategoryID(int id)
        {
            List<Food> list = new List<Food>();

            string query = "Select * from Food where idCategory = " + id;

            DataTable data = DataProvider.Instance.ExecuteQuery(query);

            foreach (DataRow item in data.Rows)
            {
                Food f = new Food(item);
                list.Add(f);
            }
            return list;
        }

        public List<Food> GetListFood()
        {
            List<Food> list = new List<Food>();

            string query = "Select * from Food" ;

            DataTable data = DataProvider.Instance.ExecuteQuery(query);

            foreach (DataRow item in data.Rows)
            {
                Food f = new Food(item);
                list.Add(f);
            }
            return list;
        }

        public bool InsertFood(string name, int id, float price)
        {
            string query = string.Format("Insert into Food Values(N'{0}', {1}, {2})", name, id, price);
            int result = DataProvider.Instance.ExecuteNonQuery(query);
            return result > 0;

           
        }
        public bool UpdateFood(int idFood, string name, int id, float price)
        {
            string sql = string.Format("Update Food set Name = N'{0}', idCategory = {1}, price = {2} where id = {3}", name, id, price, idFood);
            int result = DataProvider.Instance.ExecuteNonQuery(sql);

            return result > 0;
        }
        //xoa thuc an
        public bool DeleteFood(int idFood)
        {
            //xoa cac thuc an muon xoa trong billinfo
            BillInfoDAO.Insatnace.DeleteBillInfoByFoodID(idFood);

            string sql = string.Format("Delete Food Where id = {0}", idFood);
            int result = DataProvider.Instance.ExecuteNonQuery(sql);

            return result > 0;
        }

        //search thuc an theo ten
        public List<Food> SearchFoodByName(string name)
        {
            List<Food> list = new List<Food>();

            string query = string.Format("Select * from Food where name Like N'%{0}%'" ,name );

            DataTable data = DataProvider.Instance.ExecuteQuery(query);

            foreach (DataRow item in data.Rows)
            {
                Food f = new Food(item);
                list.Add(f);
            }
            return list;
        }
        public List<Food> GetListFoodToShow()
        {
            List<Food> list = new List<Food>();

            string query = "Select Name, Price from Food";

            DataTable data = DataProvider.Instance.ExecuteQuery(query);

            foreach (DataRow item in data.Rows)
            {
                Food f = new Food(item);
                list.Add(f);
            }
            return list;
        }
    }
}
