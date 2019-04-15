using QuanLyQuanCafe.DAO;
using QuanLyQuanCafe.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyQuanCafe
{
    public partial class frmAdmin : Form
    {
        //datasource dung chung de binding
        //neu khong se bi doi datasource
        BindingSource foodList = new BindingSource();
        BindingSource listCategory = new BindingSource();

        BindingSource accountList = new BindingSource();
        BindingSource tableList = new BindingSource();
        public Account loginAccount;
        public frmAdmin()
        {
            InitializeComponent();
            Load();
        }

        #region methods
        public void Load()
        {
            //datasoure cua griview bang datasource foodList
            dgvFood.DataSource = foodList;
            dgvCategory.DataSource = listCategory;
            dgvAccount.DataSource = accountList;
            dgvTable.DataSource = tableList;

            LoadListBillByDate(dtpkFromDate.Value, dtpkToDate.Value);
            loadDateTimePickerBill();
            LoadListFood();
            LoadListCategory();
            LoadAccount();
            LoadListTableByState();
            AddCategoryBinding();
            AddFoodBinding();
            AddTableBinding();
            LoadCategoryIntoCombobox(cbFoodCategory);
            AddAccountBinding();
        }

        public void AddAccountBinding()
        {
            txtUsername.DataBindings.Add("Text", dgvAccount.DataSource, "UserName", true, DataSourceUpdateMode.Never);
            txtAccountName.DataBindings.Add("Text", dgvAccount.DataSource, "DisplayName", true, DataSourceUpdateMode.Never);
            nbdTypeAccount.DataBindings.Add("Value", dgvAccount.DataSource, "Type", true, DataSourceUpdateMode.Never);
        }

        //load account
        public void LoadAccount()
        {
            accountList.DataSource = AccountDAO.Instance.GetListAccount();
        }

        //load doanh thu
            public void LoadListBillByDate(DateTime checkIn, DateTime checkOut)
        {
            dgvBill.DataSource = BillDAO.Insatnce.GetBillByDate(checkIn, checkOut);
        }

        public void loadDateTimePickerBill()
        {
            DateTime today = DateTime.Now;
            dtpkFromDate.Value = new DateTime(today.Year, today.Month, 1);
            dtpkToDate.Value = dtpkFromDate.Value.AddMonths(1).AddDays(-1);
            dgvBill.DataSource = BillDAO.Insatnce.GetBillByDate(dtpkFromDate.Value, dtpkToDate.Value);
        }

        //load category
        public void LoadListCategory()
        {
            listCategory.DataSource = CategoryDAO.Instance.GetListCategory();
        }

        public void LoadListFood()
        {
            foodList.DataSource = FoodDAO.Instance.GetListFood();
        }

        //Load list table
        public void LoadListTableByState()
        {
            tableList.DataSource = TableDAO.Instance.LoadTableListByState();
        }

        //load category de binding
        public void LoadCategoryIntoCombobox(ComboBox cb)
        {
            cb.DataSource = CategoryDAO.Instance.GetListCategory();
            cb.DisplayMember = "Name";
        }
        public void AddFoodBinding()
        {
            txtFoodID.DataBindings.Add(new Binding("Text", dgvFood.DataSource, "ID", true, DataSourceUpdateMode.Never));// true la tu dong ep kieu, UpdateMode.Never griview se khong thay doi khi thay doi textBox
            txtFoodName.DataBindings.Add(new Binding("Text", dgvFood.DataSource, "Name", true, DataSourceUpdateMode.Never));
            nmFoodPrice.DataBindings.Add(new Binding("Value", dgvFood.DataSource, "Price" , true, DataSourceUpdateMode.Never));
        }
        public List<Food> searchFoodByName(string name)
        {
            List<Food> listFood = FoodDAO.Instance.SearchFoodByName(name);
            return listFood;
        }

        //add table binding
        public void AddTableBinding()
        {
            txtTableID.DataBindings.Add(new Binding("Text", dgvTable.DataSource, "ID", true, DataSourceUpdateMode.Never));
            txtTableName.DataBindings.Add(new Binding("Text", dgvTable.DataSource, "Name", true, DataSourceUpdateMode.Never));
            txtStatus.DataBindings.Add(new Binding("Text", dgvTable.DataSource, "status", true, DataSourceUpdateMode.Never));
            nbStatusTable.DataBindings.Add(new Binding("Value", dgvTable.DataSource, "stateTable", true, DataSourceUpdateMode.Never));
        }


        //add category binding
        public void AddCategoryBinding()
        {
            txtCategoryID.DataBindings.Add(new Binding("Text", dgvCategory.DataSource, "ID", true, DataSourceUpdateMode.Never));// true la tu dong ep kieu, UpdateMode.Never griview se khong thay doi khi thay doi textBox
            txtCategoryName.DataBindings.Add(new Binding("Text", dgvCategory.DataSource, "Name", true, DataSourceUpdateMode.Never));
            
        }

        //Them Account 
        public void AddAccount(string username, string displayName, int type)
        {
            if (AccountDAO.Instance.InsertAccount(username, displayName, type))
            {
                MessageBox.Show("Thêm tài khoản thành công!!");
            }
            else
            {
                MessageBox.Show("Thêm tài khoản thất bại!!");
            }
            LoadAccount();
        }

        //Update account
        public void UpdateAccount(string username, string displayName, int type)
        {
            if (AccountDAO.Instance.UpdateAccount(username, displayName, type))
            {
                MessageBox.Show("Cập nhật tài khoản thành công!!");
            }
            else
            {
                MessageBox.Show("Cập nhật tài khoản thất bại!!");
            }
            LoadAccount();
        }
        //delete account
        public void DeleteAccount(string username)
        {
            if (loginAccount.UserName.Equals(username))
            {
                MessageBox.Show("Tài khoản đang đăng nhập vào hệ thống \n Không thể xóa tài khoản!!");
                return;
            }
            if (AccountDAO.Instance.DeleteAccount(username))
            {
                MessageBox.Show("Xóa tài khoản thành công!!");
            }
            else
            {
                MessageBox.Show("Xóa tài khoản thất bại!!");
            }
            LoadAccount();
        }

        //Reset password
        public void ResetPassword(string username)
        {
            if (AccountDAO.Instance.ResetPassword(username))
            {
                MessageBox.Show("Đặt lại mật khẩu thành công!!");
            }
            else
            {
                MessageBox.Show("Đặt lại mật khẩu thất bại!!");
            }
        }

        //Update category
        public void UpdateCategory(int id, string name)
        {
            if(CategoryDAO.Instance.UpdateCategory(id, name))
            {
                MessageBox.Show("Cập nhật danh mục thành công!!");
                if (updateCategoryEvent != null)
                    updateCategoryEvent(this, new EventArgs());
            }
            else
            {
                MessageBox.Show("Cập nhật danh mục thất bại!!");
                
            }
            LoadListCategory();
            LoadCategoryIntoCombobox(cbFoodCategory);

        }

        //add category
        public void AddCategory(string name)
        {
            if (CategoryDAO.Instance.InsertCategory(name))
            {
                MessageBox.Show("Thêm danh mục thành công!!");
                if (insertCategory != null)
                    insertCategory(this, new EventArgs());
            }
            else
            {
                MessageBox.Show("Thêm danh mục thất bại!!");
            }
            LoadListCategory();
            LoadCategoryIntoCombobox(cbFoodCategory);

        }
        
        //Delete Category
        public void DeleteCategory(int id)
        {
            if (CategoryDAO.Instance.DeleteCategory(id))
            {
                MessageBox.Show("Xóa danh mục thành công!!");
                if (deleteCategoryEvent != null)
                    deleteCategoryEvent(this, new EventArgs());
            }
            else
            {
                MessageBox.Show("Xóa danh mục thất bại!!");
            }
            LoadListCategory();
            LoadCategoryIntoCombobox(cbFoodCategory);
            LoadListFood();
        }

        //insert table food
        public void AddTableFood(string name)
        {
            if (TableDAO.Instance.InsertTableFood(name))
            {
                MessageBox.Show("Thêm bàn thành công!!");
                if (insertTableFoodEvent != null)
                    insertTableFoodEvent(this, new EventArgs());
            }
            else
            {
                MessageBox.Show("Thêm bàn thất bại!!");
            }
        }
         
        public void UpdateTableFood(string name, int state, int id)
        {
            if(TableDAO.Instance.updateTableFood(name, state, id))
            {
                MessageBox.Show("Cập nhật bàn thành công!!");
                if (insertTableFoodEvent != null)
                    insertTableFoodEvent(this, new EventArgs());
            }
            else
            {
                MessageBox.Show("Cập nhật bàn thất bại!!");
            }
            LoadListTableByState();
        }
        #endregion

        #region event
        private void btnDeleteFood_Click(object sender, EventArgs e)
        {
            
            int id = Convert.ToInt32(txtFoodID.Text);

            if (FoodDAO.Instance.DeleteFood(id))
            {
                MessageBox.Show("Xoá món thành công!");
                LoadListFood();
                if (deleteFood != null)
                    deleteFood(this, new EventArgs());
            }
            else
            {
                MessageBox.Show("Có lỗi khi xóa món!!");
            }
        }

        private void btnAddAccount_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text;
            string displayName = txtAccountName.Text;
            int type = (int)nbdTypeAccount.Value;

            AddAccount(username, displayName, type);

        }

        private void btnViewBill_Click(object sender, EventArgs e)
        {
            LoadListBillByDate(dtpkFromDate.Value, dtpkToDate.Value);
        }

        private void btnShowFood_Click(object sender, EventArgs e)
        {
            LoadListFood();
        }

        //binding category on food 
        private void txtFoodID_TextChanged(object sender, EventArgs e)
        {
            try
            {
                //lay tat ca cac o dc chon, sau do lay dong chua o do, sau do lay o co ten categoryID 
                if (dgvFood.SelectedCells.Count > 0)
                {
                    int id = (int)dgvFood.SelectedCells[0].OwningRow.Cells["idCategory"].Value;

                    Category category = CategoryDAO.Instance.GetCategoryByID(id);

                    int index = -1;
                    int i = 0;
                    foreach (Category item in cbFoodCategory.Items)
                    {
                        if (item.ID == category.ID)
                        {
                            index = i;
                            break;
                        }
                        i++;
                    }
                    cbFoodCategory.SelectedIndex = index;
                }
            }
            catch
            {

            }
            
        }

        private void btnAddFood_Click(object sender, EventArgs e)
        {
            string name = txtFoodName.Text;
            int categoryID = (cbFoodCategory.SelectedItem as Category).ID;
            float price = (float)nmFoodPrice.Value;

            if (FoodDAO.Instance.InsertFood(name, categoryID, price))
            {
                MessageBox.Show("Thêm đồ uống thành công!");
                LoadListFood();
                if (insertFood != null)
                    insertFood(this, new EventArgs());
            }
            else
            {
                MessageBox.Show("Có lỗi khi thêm thức ăn!!");
            }
        }

        private void btnEditFood_Click(object sender, EventArgs e)
        {
            string name = txtFoodName.Text;
            int categoryID = (cbFoodCategory.SelectedItem as Category).ID;
            float price = (float)nmFoodPrice.Value;
            int id = Convert.ToInt32(txtFoodID.Text);

            if (FoodDAO.Instance.UpdateFood(id, name, categoryID, price))
            {
                MessageBox.Show("Sửa món thành công!");
                LoadListFood();
                if (updateFood != null)
                    updateFood(this, new EventArgs());
            }
            else
            {
                MessageBox.Show("Có lỗi khi sửa món!!");
            }
        }

        private event EventHandler insertTableFoodEvent;
        public event EventHandler InsertTableFoodEvent
        {
            add { insertTableFoodEvent += value; }
            remove { insertTableFoodEvent -= value; }
        }

        private event EventHandler insertCategory;
        public event EventHandler InsertCategory
        {
            add { insertCategory += value; }
            remove { insertCategory -= value; }
        }

        private event EventHandler updateCategoryEvent;
        public event EventHandler UpdateCategoryEvent
        {
            add { updateCategoryEvent += value; }
            remove { updateCategoryEvent -= value; }
        }

        private event EventHandler deleteCategoryEvent;
        public event EventHandler DeleteCategoryEvent
        {
            add { deleteCategoryEvent += value; }
            remove { deleteCategoryEvent -= value; }
        }


        private event EventHandler insertFood;
        public event EventHandler InsertFood
        {
            add { insertFood += value; }
            remove { insertFood -= value; }
        }

        private event EventHandler deleteFood;
        public event EventHandler DeleteFood
        {
            add { deleteFood += value; }
            remove { deleteFood -= value; }
        }

        private event EventHandler updateFood;
        public event EventHandler UpdateFood
        {
            add { updateFood += value; }
            remove { updateFood -= value; }
        }

        private void btnSearchFood_Click(object sender, EventArgs e)
        {
           
            foodList.DataSource = searchFoodByName(txtSearchFoodName.Text);
        }

        private void btnShowAccount_Click(object sender, EventArgs e)
        {
            LoadAccount();
        }

        private void btnEditAccount_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text;
            string displayName = txtAccountName.Text;
            int type = (int)nbdTypeAccount.Value;

            UpdateAccount(username, displayName, type);
        }

        private void btnDeleteAccount_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text;
        

            DeleteAccount(username);
        }

        private void btnResetPassword_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text;


            ResetPassword(username);
        }

        private void btnShowCategory_Click(object sender, EventArgs e)
        {
            LoadListCategory();
        }

        private void btnUpdateCategory_Click(object sender, EventArgs e)
        {
            int id = int.Parse( txtCategoryID.Text);
            string name = txtCategoryName.Text;
            UpdateCategory(id, name);
        }

        private void btnAddCategory_Click(object sender, EventArgs e)
        {
            string name = txtCategoryName.Text;
            AddCategory(name);
        }

        private void btnDeleteCategory_Click(object sender, EventArgs e)
        {
            int id = int.Parse(txtCategoryID.Text);
            DeleteCategory(id);
        }

        private void btnAddTable_Click(object sender, EventArgs e)
        {
            string name = txtTableName.Text;
            AddTableFood(name);
            LoadListTableByState();
        }

        private void btnEditTable_Click(object sender, EventArgs e)
        {
            string name = txtTableName.Text;
            int state = (int)nbStatusTable.Value;
            int id = int.Parse( txtTableID.Text);
            UpdateTableFood(name, state, id);
        }
    }
    #endregion



}
