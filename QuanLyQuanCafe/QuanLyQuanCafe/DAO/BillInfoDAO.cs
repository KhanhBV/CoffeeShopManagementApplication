using QuanLyQuanCafe.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyQuanCafe.DAO
{
    public class BillInfoDAO
    {
        private static BillInfoDAO instance;

        public static BillInfoDAO Insatnace
        {
            get { if (instance == null) instance = new BillInfoDAO(); return instance; }
           private set { instance = value; }
        }

        private BillInfoDAO() { }

        public List<Menu> GetListBillInfo(int id)
        {
            List<Menu> listBillInfo = new List<Menu>();
            DataTable data = DataProvider.Instance.ExecuteQuery("Select * from BillInfo where idBill = " + id);
            foreach (DataRow item in data.Rows)
            {
                Menu info = new Menu(item);
                listBillInfo.Add(info);
            }
            return listBillInfo;
        }

        public void InsertBillInfo(int idBill, int idFood, int count)
        {
            DataProvider.Instance.ExecuteNonQuery("USP_InsertBillInfo @idBill="+ idBill +", @idFood= "+idFood+", @count="+count);
        }
        //xoa cac thuc an muon xoa co trong bill info 
        public void DeleteBillInfoByFoodID(int id)
        {
            DataProvider.Instance.ExecuteQuery("Delete from BillInfo Where idFood = " + id);
        }
    }
}
