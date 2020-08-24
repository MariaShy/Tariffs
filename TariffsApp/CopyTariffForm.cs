using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace TariffsApp
{
    // форма копирования тарифа
    public partial class CopyTariffForm : Form
    {
        TariffMainForm OwnerForm;

        //SqlConnection sqlConnect = new SqlConnection(LoginForm.ConnectConst);

        public CopyTariffForm(TariffMainForm ownerForm)
        {
            this.OwnerForm = ownerForm; 
            InitializeComponent();
        }
        
        /*public string old_tariff_param(string tariff_id)
        {
            string[] old_tariff ;

            sqlConnect.Open();
            SqlCommand cmd = sqlConnect.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select Product,Operation,Parameter,Canal,Fee from Tariffs where IdTariff =N'" + tariff_id + "'";

            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                old_tariff[0] = reader[0].ToString();
                old_tariff[1] = reader[1].ToString();
                MessageBox.Show(old_tariff[0], "Обратите внимание!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            reader.Close();
            sqlConnect.Close();

            return old_tariff[];
        }*/

        // передать в форму все параметры копируемого тарифа:
        private void Form4_Load(object sender, EventArgs e)
        {
            //old_tariff_param(Form1.tariff_id);

            //string[] old_tariff;
            //sqlConnect.Open();
            SqlCommand cmd = LoginForm.sqlConnect.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select Product,Operation,Parameter,Canal,Fee from Tariffs where IdTariff =N'" + TariffMainForm.tariff_id + "'";

            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                comboBox1.Text = reader[0].ToString();
                textBox1.Text = reader[1].ToString();
                textBox2.Text = reader[2].ToString();
                comboBox2.Text = reader[3].ToString();
                textBox3.Text = reader[4].ToString();


                //MessageBox.Show(old_tariff[0], "Обратите внимание!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            reader.Close();
            //sqlConnect.Close();
            
        } 
        
        // сохранить:
        private void button1_Click(object sender, EventArgs e)
        {
            // проверка на уникальность такого тарифа, иначе - поменяйте  параметры!
            // .....

            // + дописать про новые ИД в таблице плат!!!
            // .....

            try
            {
                //sqlConnect.Open();
                SqlCommand cmd = LoginForm.sqlConnect.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "insert into Tariffs values(N'" + comboBox1.Text + "',N'" + textBox1.Text + "',N'" + textBox2.Text + "',N'" + comboBox2.Text + "',N'" + textBox3.Text + "', N'В разработке','01.01.2018')";

                cmd.ExecuteNonQuery();
                //sqlConnect.Close();
                MessageBox.Show("Тариф успешно сохранен в БД.", "Обратите внимание!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), ex.Source.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            //finally
            //{
            //    if (sqlConnect != null)
            //        sqlConnect.Close();
            //}            
            this.Close();
        }

        // отмена:
        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}
