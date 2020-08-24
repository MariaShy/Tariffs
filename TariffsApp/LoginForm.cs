using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Windows.Forms;
using System.Security.Cryptography;

namespace TariffsApp
{
    public partial class LoginForm : Form
    {
        public static string user_id;
        public static string user_role;
        public static string ConnectConst = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=D:\Tariffs\TariffsApp\Database1.mdf;Integrated Security=True";

        public static SqlConnection sqlConnect = new SqlConnection(ConnectConst);
        
        //считает хеш для пароля
        public static string CalculateMD5Hash(string input)
        {
            // 1, посчитать MD5 hash из ввода
            MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
            byte[] hash = md5.ComputeHash(inputBytes);
            // 2, сконвертировать byte array to string
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }
            return sb.ToString();
        }

        public LoginForm()
        {
            InitializeComponent();
            sqlConnect.Open();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            user_id = ""; 
            user_role = "";
            
            SqlCommand cmd = sqlConnect.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select IDUser, Role from Users where UserName = N'" + 
                textBox1.Text + "' and Password = N'" + 
                CalculateMD5Hash(textBox2.Text) + "'";
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                user_id = reader[0].ToString();
                user_role = reader[1].ToString();
                MessageBox.Show("Ваша роль: "+user_role, "Права доступа", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
           reader.Close();
          

           if (user_role == "Administrator" || user_role == "Constructor" || user_role == "Manager")            
            {
            // если проверка прошла успешно:
            TariffMainForm MainForm = new TariffMainForm();
            this.Hide();
            MainForm.ShowDialog();            
            }            
            else
            {
                MessageBox.Show("Проверьте правильность введенного логина или пароля!", "Некорректные данные!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
            sqlConnect.Close();
        }
    }
}
