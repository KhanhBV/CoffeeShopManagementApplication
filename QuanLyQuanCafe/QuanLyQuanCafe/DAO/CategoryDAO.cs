using QuanLyQuanCafe.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyQuanCafe.DAO
{
    public class CategoryDAO
    {
        private static CategoryDAO instance;

        public static CategoryDAO Instance {
            get { if (instance == null) instance = new CategoryDAO(); return instance; }
            private set { instance = value; }
        }
        private CategoryDAO() { }

        public List<Category> GetListCategory()
        {
            List<Category> list = new List<Category>();

            string query = "Select * from FoodCategory";

            DataTable data = DataProvider.Instance.ExecuteQuery(query);

            foreach (DataRow item in data.Rows)
            {
                Category category = new Category(item);

                list.Add(category);
            }

            return list;
        }

        public Category GetCategoryByID(int id)
        {
            Category category = null;
            string query = "Select * from FoodCategory Where ID = " + id;

            DataTable data = DataProvider.Instance.ExecuteQuery(query);

            foreach (DataRow item in data.Rows)
            {
                category = new Category(item);

                return category;
            }
            return category;
        }

        //update category
        public bool UpdateCategory(int idCategory, string name)
        {
            string sql = string.Format("Update FoodCategory set Name = N'{0}' where id = {1}", name, idCategory);
            int result = DataProvider.Instance.ExecuteNonQuery(sql);

            return result > 0;
        }
        //add category
        public bool InsertCategory(string name)
        {
            string query = string.Format("Insert into FoodCategory Values(N'{0}')", name);
            int result = DataProvider.Instance.ExecuteNonQuery(query);
            return result > 0;


        }

        //Delete Category

        public bool DeleteCategory(int id)
        {
            List<Food> listFoodOfThisCategory;
            listFoodOfThisCategory = FoodDAO.Instance.GetListFoodByCategoryID(id);
            string query = string.Format("Delete FoodCategory where id = N'{0}'", id);
            int result = 0;
            if (listFoodOfThisCategory == null)
            {
                result = DataProvider.Instance.ExecuteNonQuery(query);
            }
            else
            {
                foreach (Food item in listFoodOfThisCategory)
                {
                    FoodDAO.Instance.DeleteFood(item.ID);
                }
                result = DataProvider.Instance.ExecuteNonQuery(query);
            }

            
           

            return result > 0;
        }
    }
}
