using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyQuanCafe.DTO
{
    public class BillInfo
    {
        public BillInfo(int id, int iDBill, int iDFood, int count)
        {
            this.ID = id;
            this.IDBill = iDBill;
            this.IDFood = iDFood;
            this.Count = count;
        }

        public BillInfo(DataRow row)
        {
            this.ID = (int)row["id"];
            this.IDBill = (int)row["iDBill"];
            this.IDFood = (int)row["iDFood"];
            this.Count = (int)row["count"];
        }

        private int count;

        public int Count
        {
            get { return count; }
            set { count = value; }
        }


        private int iDFood;

        public int IDFood
        {
            get { return iDFood; }
            set { iDFood = value; }
        }


        private int iDBill;

        public int IDBill        {
            get { return iDBill; }
            set { iDBill = value; }
        }


        private int iD;

        public int ID
        {
            get { return iD; }
            set { iD = value; }
        }

    }
}
