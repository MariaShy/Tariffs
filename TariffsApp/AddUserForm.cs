using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TariffsApp
{
    // форма добавления/изменения пользователей
    public partial class AddUserForm : Form
    {
        TariffMainForm OwnerForm;

        SqlConnection sqlConnect = new SqlConnection(LoginForm.ConnectConst);

        public AddUserForm(TariffMainForm ownerForm)
        {
            this.OwnerForm = ownerForm; 
            InitializeComponent();
            this.FormClosing += new FormClosingEventHandler(Form3Closing);
        }
        //обновление главной формы  при закрытии окна этой формы
        private void Form3Closing(object sender, System.EventArgs e)
        {
            // если нужно будет добавить видимость в обновленную роль, но не точно
        }
        // проверка на уникальность имени:
        public string name_check(string user_name)
        {
            string user_id = "";

            sqlConnect.Open();
            SqlCommand cmd = sqlConnect.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select IdUser from Users where UserName =N'" + user_name + "'";

            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                user_id = reader[0].ToString();                
            }
            reader.Close();
            sqlConnect.Close();

            return user_id;
        } 
        
        private void button1_Click(object sender, EventArgs e)
        {
            // удостовериться, что такого имени пользователя еще нет в БД:            
            if (name_check(textBox1.Text) == "")
            {    // добавление в БД:
                if (textBox1.Text == "" || textBox2.Text == "" || comboBox1.Text == "")// проверка на заполнение всех полей
                {
                    MessageBox.Show("Заполните поля!", "Обратите внимание!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    try
                    {
                        sqlConnect.Open();
                        SqlCommand cmd = sqlConnect.CreateCommand();
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "insert into Users values(N'" + textBox1.Text + "',N'" +
                            textBox2.Text + "',N'" + comboBox1.Text + "')";

                        cmd.ExecuteNonQuery();

                        sqlConnect.Close();

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message.ToString(), ex.Source.ToString(), 
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    finally
                    {
                        if (sqlConnect != null)
                            sqlConnect.Close();
                    }

                    MessageBox.Show("Пользователь успешно добавлен!", "Обратите внимание!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
            }
            else
            {
                MessageBox.Show("Пользователь с таким именем уже существует!", "Обратите внимание!", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (textBox4.Text == "")// проверка на заполнение полей
            {
                MessageBox.Show("Заполните поле Идентификатора!", "Обратите внимание!", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                // удостовериться, что ДРУГОГО такого имени пользователя еще нет в БД:            
                if (name_check(textBox1.Text) == textBox4.Text)
                {// изменения в БД:
                    try
                    {
                        sqlConnect.Open();
                        SqlCommand cmd = sqlConnect.CreateCommand();
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "update Users set UserName =N'" + textBox1.Text + 
                            "', Password =N'" + textBox2.Text + "', Role =N'" + comboBox1.Text + 
                            "' where IdUser=N'" + textBox4.Text + "'";

                        cmd.ExecuteNonQuery();

                        sqlConnect.Close();

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message.ToString(), ex.Source.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    finally
                    {
                        if (sqlConnect != null)
                            sqlConnect.Close();
                    }

                    MessageBox.Show("Данные успешно изменены!", "Обратите внимание!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Пользователь с таким именем уже существует!", "Обратите внимание!", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // заполнить данные по идентификатору:
        private void button4_Click(object sender, EventArgs e)
        {
            if (textBox4.Text == "")// проверка на заполнение полей
            {
                MessageBox.Show("Заполните поле Идентификатора!", "Обратите внимание!", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                sqlConnect.Open();
                SqlCommand cmd = sqlConnect.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "select UserName,Password,Role  from Users where  IdUser=N'" + 
                    textBox4.Text + "'";

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    textBox1.Text = reader[0].ToString();
                    textBox2.Text = reader[1].ToString();
                    comboBox1.Text = reader[2].ToString();
                }
                reader.Close();
                sqlConnect.Close();
            }
        }
    }
}
