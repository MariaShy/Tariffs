using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;


namespace TariffsApp
{
    // форма смены пароля
    public partial class ChangePassForm : Form
    {
        TariffMainForm OwnerForm;
       // SqlConnection sqlConnect = new SqlConnection(LoginForm.ConnectConst);
 
        public ChangePassForm(TariffMainForm ownerForm)
        {
            this.OwnerForm = ownerForm; 
            InitializeComponent();
            //this.FormClosing += new FormClosingEventHandler(Form2Closing); -удалить
        }
                
        /* -удалить . обновление главной формы  при закрытии окна этой формы
        private void Form2Closing(object sender, System.EventArgs e)
        {
            //MessageBox.Show("Данные успешно сохранены!", "Обратите внимание!", MessageBoxButtons.OK, MessageBoxIcon.Information);

            if (Form1.full_name_of_image != "\0")
            {
                Form1.FromPixelToBitmap();
                FromBitmapToScreen();
            }
        }*/

        public string pass_check(string id_user)
        {
            string user_pass = "";

            //sqlConnect.Open();
            SqlCommand cmd = LoginForm.sqlConnect.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select Password from Users where IdUser =N'" + id_user + "'";

            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                user_pass = reader[0].ToString();
            }
            reader.Close();
            //sqlConnect.Close();

            return user_pass;           

        } // - проверка соответствия ИД и пароля

        // сохранить:
        private void button1_Click(object sender, EventArgs e)
        {
            // удостовериться, что такой пароль этого пользователя существует:            
            //if (textBox1.Text == pass_check(LoginForm.user_id))
            {
                if (textBox1.Text == "" || textBox2.Text == "" || textBox3.Text == "")// проверка на заполнение всех полей
                {
                    MessageBox.Show("Все поля обязательны для заполнения!", "Обратите внимание!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    if (textBox1.Text == textBox2.Text)// сравнить старый и новый пароли, чтобы отличались
                    {
                        MessageBox.Show("Старый и новый пароли должны отличаться!", "Обратите внимание!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        if (textBox2.Text != textBox3.Text)//сравнить  новый пароль и его повтор, чтобы совпадали
                        {
                            MessageBox.Show("Пароли не совпадают!", "Обратите внимание!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else // все проверки пройдены, сохранить:
                        {
                            // в БД:
                            try
                            {                               
                                //sqlConnect.Open();
                                SqlCommand cmd = LoginForm.sqlConnect.CreateCommand();
                                cmd.CommandType = CommandType.Text;
                                cmd.CommandText = "update Users set Password =N'" + LoginForm.CalculateMD5Hash(textBox3.Text) +
                                    "' where IdUser =N'" + LoginForm.user_id + "'";

                                cmd.ExecuteNonQuery();

                                //sqlConnect.Close();

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

                            MessageBox.Show("Данные успешно изменены!", "Обратите внимание!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            this.Close();
                        }
                    }
                }
            }
            /*else
            {
                MessageBox.Show("Вы неверно ввели старый пароль!", "Обратите внимание!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }*/
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        
    }
}
