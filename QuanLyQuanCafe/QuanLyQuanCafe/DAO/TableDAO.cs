using QuanLyQuanCafe.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyQuanCafe.DAO
{
    class TableDAO
    {
        private static TableDAO instance;

        internal static TableDAO Instance
        {
            get { if (instance == null) instance = new TableDAO(); return instance; }
            private set { TableDAO.instance = value; }
        }

        private TableDAO() { }

        public static int TableWidth = 90;
        public static int TableHeight = 90;
        //ham nay de show ra danh sach ban con hoat dong
        public List<Table> LoadTableList()
        {
            List<Table> tableList = new List<Table>();

            DataTable data = DataProvider.Instance.ExecuteQuery("exec USP_GetTableListOfState");
            foreach (DataRow item in data.Rows)
            {
                Table table = new Table(item);
                tableList.Add(table);
            }

            return tableList;
        }

        public List<Table> LoadTableListByState()
        {
            List<Table> tableList = new List<Table>();

            DataTable data = DataProvider.Instance.ExecuteQuery("exec USP_GetTableList");
            foreach (DataRow item in data.Rows)
            {
                Table table = new Table(item);
                tableList.Add(table);
            }

            return tableList;
        }
        public void UpdateStatusTable(int id)
        {
            
                string query = "exec USP_UpdateStatusFood @id";
                DataProvider.Instance.ExecuteNonQuery(query, new object[] { id});
                
        }

        public void Switchtable(int idTable1, int idTable2)
        {
            DataProvider.Instance.ExecuteQuery("USP_SwitchTabel @idTable1 , @idTable2", new object[] { idTable1, idTable2 });
        }

        public bool InsertTableFood(string name)
        {
            string query = string.Format("insert TableFood (name)VALUES  ( N'{0}')",name);
            int result = DataProvider.Instance.ExecuteNonQuery(query);
            return result > 0;
        }
        public bool updateTableFood(string name, int stateTable, int id)
        {
            string query = string.Format("Update TableFood set name=N'{0}', stateTable={1} where id={2}", name, stateTable, id);
            int result = DataProvider.Instance.ExecuteNonQuery(query);
            return result > 0;
        }
            
    }
}
