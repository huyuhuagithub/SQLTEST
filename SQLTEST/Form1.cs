using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Configuration;

namespace SQLTEST
{
    public partial class Form1 : Form
    {
        static string connstrings = ConfigurationManager.ConnectionStrings["connstring"].ConnectionString;
        public Form1()
        {
            InitializeComponent();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            safeInvoke(get);
        }

        private void get()
        {
            //users users = Getq<users>(textBox1.Text);
            //users users = SQLHelper.GetT<users>(int.Parse(textBox1.Text));

            //textBox2.Text = users.id.ToString();
            //textBox3.Text = users.Account;
            //textBox4.Text = users.Password;
            //textBox5.Text = users.Permission.ToString();

            //customers customerss = SQLHelper.GetT<customers>(int.Parse(textBox1.Text));
            //textBox2.Text = customerss.id.ToString();
            //textBox3.Text = customerss.Name;
            //textBox4.Text = customerss.ProjectName;

            //users users = SQLHelper.GetT<users>(1);

            //var customerss = SQLHelper.GetEntitylist<customers>();
            //users users1 = new users() { id = 1008, Account = "APTK", Permission = false, Password = "Jason" };


            dataGridView1.Rows.Clear();
            var entityusers = SQLHelper.GetT<users>(int.Parse(textBox2.Text));
            dataGridView1.Rows.Add(entityusers.id, entityusers.Account, entityusers.Password, entityusers.Permission);
            //entityusers.Account = "abtec1111111111";
            //SQLHelper.UpdateEntity(entityusers);

            //SQLHelper.InsertEntity(users1);

            //var userss = SQLHelper.GetEntitylist<users>();

            //SQLHelper.DeleteEntity<users>(1008);
        }


        public void safeInvoke(Action action)
        {
            try
            {
                action?.Invoke();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "提示！");

            }


        }

        private void Button2_Click(object sender, EventArgs e)
        {
            safeInvoke(Insert);
        }

        private void Insert()
        {
            users users1 = new users();
            users1.id = int.Parse(textBox2.Text);
            users1.Account = textBox3.Text;
            users1.Password = textBox4.Text;
            users1.Permission = bool.Parse(textBox5.Text);
            SQLHelper.InsertEntity(users1);
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            safeInvoke(Delete);
        }

        private void Delete()
        {
            SQLHelper.DeleteEntity<users>(int.Parse(textBox2.Text));
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            safeInvoke(Update1);
        }

        private void Update1()
        {
            users users1 = new users();
            users1.id = int.Parse(textBox2.Text);
            users1.Account = textBox3.Text;
            users1.Password = textBox4.Text;
            users1.Permission = bool.Parse(textBox5.Text);
            SQLHelper.UpdateEntity(users1);
        }

        private void Button5_Click(object sender, EventArgs e)
        {
            safeInvoke(QueryList);
        }

        private void QueryList()
        {
            var userss = SQLHelper.GetEntitylist<users>();
            foreach (var item in userss)
            {
                dataGridView1.Rows.Add(item.id, item.Account, item.Password, item.Permission);
            }
        }
    }
}
