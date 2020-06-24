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

namespace Giuaki_baitap
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            loadcb_khoa();
            loadcb_quequan();
        }
        
       
        private void loadcb_khoa()
        {
            SqlConnection cnn = new SqlConnection();
            cnn.ConnectionString = @"Data Source=MAYTINH-BSL3CDR;Initial Catalog=SinhVien1.2;Integrated Security=True;Connect Timeout=15;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            cnn.Open();
            SqlCommand cmn = new SqlCommand();
            cmn.Connection = cnn;
            cmn.CommandText = "select distinct NameKhoa from Khoa";

            SqlDataReader reader = cmn.ExecuteReader();
            while(reader.Read())
            {
                cb_khoa.Items.Add(reader["NameKhoa"].ToString());
            }
            cnn.Close();
        }
        private void loadcb_quequan()
        {
            SqlConnection cnn = new SqlConnection();
            cnn.ConnectionString = @"Data Source=MAYTINH-BSL3CDR;Initial Catalog=SinhVien1.2;Integrated Security=True;Connect Timeout=15;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            cnn.Open();
            SqlCommand cmn = new SqlCommand();
            cmn.Connection = cnn;
            cmn.CommandText = "select distinct Quequan from SinhVien2";

            SqlDataReader reader = cmn.ExecuteReader();
            while (reader.Read())
            {
                cb_quequan.Items.Add(reader["Quequan"].ToString());
            }
            cnn.Close();
        }     

        private void btshow_click(object sender, EventArgs e)
        {

            SqlConnection cnn = new SqlConnection();
            cnn.ConnectionString = @"Data Source=MAYTINH-BSL3CDR;Initial Catalog=SinhVien1.2;Integrated Security=True;Connect Timeout=15;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            cnn.Open();
            SqlCommand cmn = new SqlCommand();
            cmn.Connection = cnn;
            cmn.CommandText = "select Mssv, NameSV, Birthday, Quequan, Hokhau, Gender, DTB, Khoa.NameKhoa From SinhVien2 INNER JOIN Khoa ON SinhVien2.ID_Khoa = Khoa.ID_Khoa";

            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmn;

            da.Fill(ds);
            dataGridView1.DataSource = ds.Tables[0];

        }

        private void selectoin_changed_Click(object sender, EventArgs e)
        {
            if(dataGridView1.SelectedRows.Count!=0)
            {
                SqlConnection cnn = new SqlConnection();
                cnn.ConnectionString = @"Data Source=MAYTINH-BSL3CDR;Initial Catalog=SinhVien1.2;Integrated Security=True;Connect Timeout=15;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
                cnn.Open();
                SqlCommand cmn = new SqlCommand();
                cmn.Connection = cnn;

                string mssv = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
                cmn.CommandText = "select Mssv, NameSV, Birthday, Quequan, Hokhau, Gender, DTB, Khoa.NameKhoa From SinhVien2 INNER JOIN Khoa ON SinhVien2.ID_Khoa = Khoa.ID_Khoa where Mssv = '" + mssv + "'";
                SqlDataReader reader = cmn.ExecuteReader();

                while(reader.Read())
                {
                    tb_mssv.Text = reader["Mssv"].ToString();
                    tb_name.Text = reader["NameSV"].ToString();
                    tb_dtb.Text = reader["DTB"].ToString();
                    tb_hokhau.Text = reader["Hokhau"].ToString();
                    dtp.Value = Convert.ToDateTime(reader["Birthday"]);

                    // by An 
                    cb_quequan.SelectedIndex = cb_quequan.FindStringExact(reader["Quequan"].ToString());
                    cb_khoa.SelectedIndex = cb_khoa.FindStringExact(reader["NameKhoa"].ToString());

                    bool gd = Convert.ToBoolean(reader["Gender"]);
                    if (gd) rb_Nam.Checked = true;
                    else rb_Nu.Checked = true;
                }
                  
            }
        }
        private bool getGender()
        {
            if (rb_Nam.Checked == true) return true;
            else return false;
        }
        private string getKhoa()
        {
            string khoa = cb_khoa.Text;

            SqlConnection cnn = new SqlConnection();
            cnn.ConnectionString = @"Data Source=MAYTINH-BSL3CDR;Initial Catalog=SinhVien1.2;Integrated Security=True;Connect Timeout=15;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            cnn.Open();
            SqlCommand cmn = new SqlCommand();
            cmn.Connection = cnn;

            cmn.CommandText = "select distinct ID_Khoa, NameKhoa from Khoa where Khoa.NameKhoa = N'"+khoa+"'";
            SqlDataReader reader = cmn.ExecuteReader();
            string re = "";
            while (reader.Read())
            { 
              re = reader["ID_Khoa"].ToString();
            }
            return re ;

        }

        private void Addbt_click(object sender, EventArgs e)
        {
            string mssv = tb_mssv.Text;
            string nameSV = tb_name.Text;
            string quequan = cb_quequan.Text;
            string hokhau = tb_hokhau.Text;
            string khoa = getKhoa();
            string DTB = tb_dtb.Text;
            string gender = getGender().ToString();
            string date = dtp.Value.ToString("MM-dd-yyyy");

            SqlConnection cnn = new SqlConnection();
            cnn.ConnectionString = @"Data Source=MAYTINH-BSL3CDR;Initial Catalog=SinhVien1.2;Integrated Security=True;Connect Timeout=15;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            cnn.Open();
            SqlCommand cmn = new SqlCommand();
            cmn.Connection = cnn;

            cmn.CommandText = "insert SinhVien2 " +
                        "values('" + mssv + "', '" + nameSV + "', '" + date + "', '" + quequan + "', '" + hokhau + "', '" + gender + "', '" + DTB + "', '" + khoa + "')";
            cmn.ExecuteNonQuery();
            cnn.Close();

            btshow_click(sender,e );

        }

        private void Update_click(object sender, EventArgs e)
        {
            string mssv = tb_mssv.Text;
            string nameSV = tb_name.Text;
            string quequan = cb_quequan.Text;
            string hokhau = tb_hokhau.Text;
            string khoa = getKhoa();
            string DTB = tb_dtb.Text;
            string gender = getGender().ToString();
            string date = dtp.Value.ToString("MM-dd-yyyy");

            SqlConnection cnn = new SqlConnection();
            cnn.ConnectionString = @"Data Source=MAYTINH-BSL3CDR;Initial Catalog=SinhVien1.2;Integrated Security=True;Connect Timeout=15;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            cnn.Open();
            SqlCommand cmn = new SqlCommand();
            cmn.Connection = cnn;

            cmn.CommandText = "update SinhVien2 " +
                        "set Mssv = '" + mssv + "', NameSV = '" + nameSV + "', Birthday = '" + date + "', Quequan = '" + quequan + "', Hokhau = '" + hokhau + "', Gender = '" + gender + "', DTB = "+ DTB+" , ID_Khoa = '" + khoa + "'" +
                    "where Mssv = '" + mssv + "'";
            cmn.ExecuteNonQuery();
            cnn.Close();

            btshow_click(sender, e);


        }

        private void Delete_click(object sender, EventArgs e)
        {

            SqlConnection cnn = new SqlConnection();
            cnn.ConnectionString = @"Data Source=MAYTINH-BSL3CDR;Initial Catalog=SinhVien1.2;Integrated Security=True;Connect Timeout=15;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            cnn.Open();
            SqlCommand cmn = new SqlCommand();
            cmn.Connection = cnn;

            string mssv = tb_mssv.Text;

            cmn.CommandText = " delete from SinhVien2 " +
                    "where Mssv = '" + mssv + "'";
            cmn.ExecuteNonQuery();
            cnn.Close();

            btshow_click(sender, e);

        }

        private void search_click(object sender, EventArgs e)
        {
            SqlConnection cnn = new SqlConnection();
            cnn.ConnectionString = @"Data Source=MAYTINH-BSL3CDR;Initial Catalog=SinhVien1.2;Integrated Security=True;Connect Timeout=15;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            cnn.Open();
            SqlCommand cmn = new SqlCommand();
            cmn.Connection = cnn;

            string searchText = tb_search.Text;

            cmn.CommandText = " select * from SinhVien2 " +
                            "where Mssv LIKE '"+searchText+ "' OR NameSV LIKE '" + searchText + "' OR Birthday LIKE '" + searchText + "'OR Quequan LIKE '" + searchText + "'OR Hokhau LIKE '" + searchText + "'OR Gender LIKE '" + searchText + "'OR DTB LIKE '" + searchText + "' ";

            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmn;

            da.Fill(ds);
            dataGridView1.DataSource = ds.Tables[0];
        }
    }
}
