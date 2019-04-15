using QuanLyQuanCafe.DAO;
using QuanLyQuanCafe.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyQuanCafe
{
    public partial class frmTableManager : Form
    {
        
        private Account loginAccount;
        public string DisplayName { get; set; }
        public Account LoginAccount
        {
            get { return loginAccount; }
            set { loginAccount = value; changeAccount(loginAccount.Type); }
        }
        public frmTableManager(Account acc)
        {
            InitializeComponent();
            
            this.LoginAccount = acc;

            LoadTable();
            LoadCategory();
            loadComboboxTable(cbSwitchTable);
            Console.WriteLine("Helllo");
        }
        #region Method
        
        public void changeAccount(int type)
        {
            adminToolStripMenuItem.Enabled = type == 1;
            DisplayName = loginAccount.DisplayName;
            thongTinTàiKhoảnToolStripMenuItem.Text += " (" + DisplayName + ")";
        }

        public void changeDisplayName(string displayName)
        {
            thongTinTàiKhoảnToolStripMenuItem.Text = thongTinTàiKhoảnToolStripMenuItem.Text.Substring(0,19);
            DisplayName = displayName;
            thongTinTàiKhoảnToolStripMenuItem.Text += " (" + DisplayName + ")";
            loginAccount.DisplayName = displayName;
        }

        private void LoadTable()
        {
            flpTable.Controls.Clear();
            List<Table> tableList = TableDAO.Instance.LoadTableList();
            foreach (Table item in tableList)
            {
                Button btn = new Button() { Width = TableDAO.TableWidth, Height = TableDAO.TableHeight };
                btn.Text = item.Name + "\n" + item.Status;
                btn.Click += btn_Click;
                btn.Tag = item; //luu cai ban dc click vao de sau nay lay thong tin cua no
                switch (item.Status)
                {
                    case ("Trống"):
                        btn.BackColor = Color.Azure;
                        break;
                    default:
                        btn.BackColor = Color.Yellow;
                        break;
                }
                flpTable.Controls.Add(btn);
            }
        }

        //ham nay dung de show bill len list Bill
        private void ShowBill(int id)
        {
            
            lsvBill.Items.Clear();
            List<DTO.Menu> listBillInfo = MenuDAO.Instance.GetListMenuByTable(id);
            float totalPrice = 0;
            foreach (DTO.Menu item in listBillInfo)
            {
                
                ListViewItem lsvItem = new ListViewItem(item.FoodName.ToString());
                lsvItem.SubItems.Add(item.Count.ToString());//thang dau tien la new listView item
                lsvItem.SubItems.Add(item.Price.ToString());//thnag thu hai tro di phai addSubItem
                lsvItem.SubItems.Add(item.TotalPrice.ToString());
                lsvBill.Items.Add(lsvItem);
                totalPrice += item.TotalPrice;
                
            }
            CultureInfo culture = new CultureInfo("vi-vn"); //thay doi curtureInfo ve dinh dang tien viet nam
            txtTotalPrice.Text = totalPrice.ToString("c",culture);
            
        }

        void LoadCategory()
        {
            List<Category> listCategory = CategoryDAO.Instance.GetListCategory();
            cbCategory.DataSource = listCategory;
            cbCategory.DisplayMember = "Name";
        }

        void LoadFoodListByCategoryID(int id)
        {
            List<Food> listFood = FoodDAO.Instance.GetListFoodByCategoryID(id);
            cbFood.DataSource = listFood;
            cbFood.DisplayMember = "Name";
        }
         public void loadComboboxTable(ComboBox cb)
        {
            cb.DataSource = TableDAO.Instance.LoadTableList();
            cb.DisplayMember = "Name";
        }
        public void ShowForm()
        {
            fLogin frm = new fLogin();
            Application.Run(frm);
        }
        #endregion

        #region Event
        private void btn_Click(object sender, EventArgs e)
        {
            int tableID = ((sender as Button).Tag as Table).ID; //ep kieu thnag sender ve kieu button de .Tag lay thang object ra ep kieu ve table
            lsvBill.Tag = (sender as Button).Tag; //luu thang table vo thang tag cua listview
            ShowBill(tableID);//lay id ra de show bill
        }
        private void đăngXuấtToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            this.Hide();
            Thread t = new Thread(new ThreadStart(ShowForm));
            t.Start();
            this.Close();
        }

        private void thôngTinCáNhânToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAccountProfile f = new frmAccountProfile(loginAccount, this);
            f.ShowDialog();
        }

        private void cbCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            int id = 0;

            ComboBox cb = sender as ComboBox;

            if (cb.SelectedItem == null)
                return;

            Category selected = cb.SelectedItem as Category;
            id = selected.ID;

            LoadFoodListByCategoryID(id);
        }

        private void adminToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAdmin f = new frmAdmin();
            f.loginAccount = loginAccount;
            f.InsertFood += F_InsertFood;
            f.UpdateFood += F_UpdateFood;
            f.DeleteFood += F_DeleteFood;
            f.InsertCategory += F_InsertCategory;
            f.UpdateCategoryEvent += F_UpdateCategoryEvent;
            f.DeleteCategoryEvent += F_DeleteCategoryEvent;
            f.InsertTableFoodEvent += F_InsertTableFoodEvent;
            f.ShowDialog();
        }

        private void F_InsertTableFoodEvent(object sender, EventArgs e)
        {
            LoadTable();
        }

        private void F_DeleteCategoryEvent(object sender, EventArgs e)
        {
            LoadCategory();
        }

        private void F_UpdateCategoryEvent(object sender, EventArgs e)
        {
            LoadCategory();
        }

        private void F_InsertCategory(object sender, EventArgs e)
        {
            LoadCategory();
            
        }

        private void F_DeleteFood(object sender, EventArgs e)
        {
            LoadFoodListByCategoryID((cbCategory.SelectedItem as Category).ID);
            if (lsvBill.Tag != null)
                ShowBill((lsvBill.Tag as Table).ID);
            LoadTable();

        }
        //khi update food thi ben form manager cung phai doi theo
        private void F_UpdateFood(object sender, EventArgs e)
        {
            LoadFoodListByCategoryID((cbCategory.SelectedItem as Category).ID);
            if (lsvBill.Tag != null)
                ShowBill((lsvBill.Tag as Table).ID);
        }

        private void F_InsertFood(object sender, EventArgs e)
        {
            LoadFoodListByCategoryID((cbCategory.SelectedItem as Category).ID);
            if (lsvBill.Tag != null) //kiểm tra xem đã có bàn muốn thêm chưa
                ShowBill((lsvBill.Tag as Table).ID);
        }

        private void frmTableManager_FormClosing(object sender, FormClosingEventArgs e)
        {
            

        }

        private void cbCategory_SelectedValueChanged(object sender, EventArgs e)
        {

        }

        private void btnAddFood_Click(object sender, EventArgs e)
        {
            
            Table table = lsvBill.Tag as Table; //lay cai ban ra tu listView.Tag roi ep kieu sang Table
            if (table == null)
            {
                MessageBox.Show("Bạn phải chọn bàn!!");
            }
            if (table == null)
            {
                return;
            }
            int idBill = BillDAO.Insatnce.GetUncheckBillIDTableID(table.ID); //lay id cua bill cua ban vua chon
            int idFood = (cbFood.SelectedItem as Food).ID; //lay id cua mon an ma muon dua vao bill
            int count = (int)nmFoodCount.Value;//lay so luong cua 1 mon an de them vao

            if (idBill == -1)
            {
                BillDAO.Insatnce.InsertBill(table.ID);//insert bill moi vao ban
                BillInfoDAO.Insatnace.InsertBillInfo(BillDAO.Insatnce.GetMaxIDBill(), idFood, count);

            }
            else
            {
                BillInfoDAO.Insatnace.InsertBillInfo(idBill, idFood, count);
            }
            ShowBill(table.ID);
            LoadTable();
        }


        //ham nay dung de thanh toan mot bill info 
        private void btnCheckOut_Click(object sender, EventArgs e)
        {
            Table table = lsvBill.Tag as Table;
            if (table == null)
                MessageBox.Show("Bạn phải chọn bàn muốn thanh toán!!");
            else
            {
                int discount = (int)nmDiscount.Value;//lay gia tri discount
                                                     //phai lay dc id bill cua ban muon thanh toan
                int idBill = BillDAO.Insatnce.GetUncheckBillIDTableID(table.ID);

                float totalPrice = Convert.ToSingle(txtTotalPrice.Text.Split(',')[0]) * 1000;
                float finalTotalPrice = totalPrice - (totalPrice / 100) * discount;

                if (idBill != -1)
                {
                    if (MessageBox.Show(string.Format("Bạn có muốn thanh toán hóa đơn cho bàn {0}\n Tổng tiền:{1}\n " +
                        "Giảm giá: {2}%\n Thành tiền: {3}", table.Name, totalPrice, discount, finalTotalPrice), "Thông báo", MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.OK)
                    {
                        BillDAO.Insatnce.checkOut(idBill, discount, finalTotalPrice);
                        ShowBill(table.ID);
                    }
                }
                LoadTable();
            }
            

        }

        private void btnSwitchTable_Click(object sender, EventArgs e)
        {
            
            if(lsvBill.Tag as Table == null)
            {
                MessageBox.Show("Bạn phải chọn bàn muốn chuyển!");
            }
            else
            {
                int id1 = (lsvBill.Tag as Table).ID;
                int id2 = (cbSwitchTable.SelectedItem as Table).ID;


                if (MessageBox.Show(string.Format("Bạn có muốn chuyển bàn {0} sang bàn {1}?", (lsvBill.Tag as Table).Name, (cbSwitchTable.SelectedItem as Table).Name),
                    "Thông báo", MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.OK)
                {

                    TableDAO.Instance.Switchtable(id1, id2);

                    LoadTable();
                }
            }
            
            


        }

        #endregion

        private void thongTinTàiKhoảnToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}
