using QuanLyQuanCafe.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyQuanCafe.DAO
{
    public class BillDAO
    {
        private static BillDAO instance;

        public static BillDAO Insatnce
        {
            get { if(instance == null) instance = new BillDAO(); return BillDAO.instance; }
            private set { BillDAO.instance = value; }
        }
        private BillDAO() { }
        //lay id bill thong qua idBill va idTale va status
        public int GetUncheckBillIDTableID(int id)
        {
            DataTable data = DataProvider.Instance.ExecuteQuery("Select * from Bill Where idTable = " + id + " and status = 0");
            
            if(data.Rows.Count > 0)
            {
                Bill bill = new Bill(data.Rows[0]);
                return bill.ID; //bill ID
            }
            return -1; //-1 la ko co thang nao
        }

        public void InsertBill(int id) 
        {
            DataProvider.Instance.ExecuteNonQuery("exec USP_InsertBill @idTable", new object[] {id});
        }
        public int GetMaxIDBill()
        {
            try
            {
                //neu khog co bill nao thi se bi loi
                return (int)DataProvider.Instance.ExecuteScalar("select max(id) from Bill");
            } catch
            {
                return 1;
            }
            
        }
        public void checkOut(int id, int discount, float totalPrice)
        {
            string query = "Update Bill set dateCheckOut = GETDATE(), status = 1,"+" discount="+discount+ " , totalPrice= " +totalPrice +" Where id = " + id;
            DataProvider.Instance.ExecuteNonQuery(query);
        }

        public DataTable GetBillByDate(DateTime checkIn, DateTime checkOut)
        {
            return DataProvider.Instance.ExecuteQuery("exec USP_GetListBillByDate @checkIn , @checkOut", new object[] { checkIn, checkOut });
        }


    }
}
