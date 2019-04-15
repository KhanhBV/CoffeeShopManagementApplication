using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyQuanCafe.DTO
{
    public class Table
    {
        public Table(int id, string name, string status, int stateTable)
        {
            this.ID = id;
            this.Name = name;
            this.Status = status;
            this.StateTable = stateTable;
        }
        public Table(DataRow row)
        {
            this.ID = (int)row["id"];
            this.Name = row["name"].ToString();
            this.Status = row["status"].ToString();
            this.StateTable = (int)row["stateTable"];
        }

        private int stateTable;

        public int StateTable
        {
            get { return stateTable; }
            set { stateTable = value; }
        }


        private string status;

        public string Status
        {
            get { return  status; }
            set {  status = value; }
        }


        private string name;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }


        private int iD;
        
        public int ID
        {
            get { return iD; }
            set { iD = value; }
        }

    }
}
