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
    public partial class frmAccountProfile : Form
    {
        private frmTableManager frmTableManager;
        private Account loginAccount;
        public string DisplayName { get; set; }
        public Account LoginAccount
        {
            get { return loginAccount; }
            set { loginAccount = value; ChangeAccount(loginAccount);}
        }
        public frmAccountProfile(Account acc, frmTableManager frm)
        {
            InitializeComponent();
            this.frmTableManager = frm;
            LoginAccount = acc;
        }

        void ChangeAccount(Account acc)
        {
            txtUsername.Text = LoginAccount.UserName;
            txtDisplayName.Text = LoginAccount.DisplayName;
        }
        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public void updateAccount()
        {
            String displayName = txtDisplayName.Text;
            String password = txtPassword.Text;
            String newpass = txtNewPassword.Text;
            String reenterPass = txtRenEnterPass.Text;
            String username = txtUsername.Text;

            if (!newpass.Equals(reenterPass))
            {
                MessageBox.Show("Vui lòng nhập lại mật khẩu đúng với mật khẩu mới!");
                
            }
            else
            {
                if (AccountDAO.Instance.updateAccount(username, displayName, password, newpass))
                {
                    frmTableManager.DisplayName = displayName;
                    frmTableManager.changeDisplayName(displayName);
                    MessageBox.Show("Cập nhật thành công");
                    
                }
                else
                {
                    MessageBox.Show("Vui lòng điền đúng mật khấu");
                }
            }

        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            updateAccount();
        }
    }
}
