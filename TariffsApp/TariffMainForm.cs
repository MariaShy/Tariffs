using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace TariffsApp
{   
    public partial class TariffMainForm : Form
    {
        //SqlConnection sqlConnect = new SqlConnection(LoginForm.ConnectConst);
        public static string tariff_id;
        
        public string flag = ""; // флаг для контроля нажатия кнопки

        public TariffMainForm()
        {
            InitializeComponent();

            if (LoginForm.user_role == "Administrator")
            {
                пользователиToolStripMenuItem.Visible = true;
                button1.Visible = false;
                button2.Visible = false;
                button3.Visible = false;
                button4.Visible = false;
                button5.Visible = false;
                button9.Visible = false;
                button10.Visible = false;
                button12.Visible = false;
                button14.Visible = false;
                button15.Visible = false;                 
            }
            
            else if (LoginForm.user_role == "Manager")
            {
                пользователиToolStripMenuItem.Visible = false;
                button1.Visible = false;
                button2.Visible = false;
                button3.Visible = false;
                button4.Visible = false;
                button5.Visible = false;
                button9.Visible = false;
                button10.Visible = false;
                button14.Visible = true;
                button15.Visible = true;
                button12.Visible = true;
            }
            else // иначе - роль "Constructor"
            {
                пользователиToolStripMenuItem.Visible = false;
                button12.Visible = false;                                  
            }

            this.FormClosing += new FormClosingEventHandler(Form1Closing);
        }

        // при закрытии этой формы закрываем приложение (в т.ч. скрытую форму авторизации):
        private void Form1Closing(object sender, System.EventArgs e)
        {
            Application.Exit();
            LoginForm.sqlConnect.Close();
        }

        public void disp_data()
        {
            //if (LoginForm.sqlConnect != null)
            //    LoginForm.sqlConnect.Close(); 

            //sqlConnect.Open();

            SqlCommand cmd = LoginForm.sqlConnect.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select * from Tariffs";

            cmd.ExecuteNonQuery();

            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            dataGridView1.DataSource = dt;

            //sqlConnect.Close();
            flag = "";
            
        } // - просмотр таблицы Тарифы

        public string new_fee_check(string fee_name)
        {
            string existing_fee_id = "";
            
            //sqlConnect.Open();
            SqlCommand cmd = LoginForm.sqlConnect.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select Id from Fees where FeeName =N'" + fee_name + "'";
                       
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                existing_fee_id = reader[0].ToString();                
            }
           reader.Close();
           //sqlConnect.Close();

           return existing_fee_id;

           /*if (existing_fee_id == "") -удалить
            {
                return false;
            }
            else
            {
                return true;
            }*/

        } // - проверка на уникальность платы

        // ============Функционал главной формы===========:

        // 1) главное меню:
        private void обновитьБДToolStripMenuItem_Click(object sender, EventArgs e)
        {
            disp_data();
        }

        private void сменаПароляToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChangePassForm PassChangeForm = new ChangePassForm(this);
            PassChangeForm.ShowDialog();
        }

        private void оПрограммеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox1 AboutForm = new AboutBox1(this);
            AboutForm.ShowDialog();
        }

        private void пользователиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddUserForm UserChangeForm = new AddUserForm(this);
            UserChangeForm.ShowDialog();
        }
                
        // 2) закладки:
        //создать тариф: ВНИМАНИЕ не хватает проверки на уникальность!!!
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                //sqlConnect.Open();
                SqlCommand cmd = LoginForm.sqlConnect.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "insert into Tariffs values(N'" + comboBox1.Text + "',N'" + textBox1.Text + "',N'" + textBox2.Text + "',N'" + comboBox2.Text + "','',N'В разработке','06.20.2018')";

                cmd.ExecuteNonQuery();
                MessageBox.Show("Создана новая запись в таблице Тарифов.", "Обратите внимание!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //sqlConnect.Close();

                disp_data();
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
        }
        
        // (дополнительно) при изменении услуги заполнить текущие параметры по ID:
        private void button16_Click(object sender, EventArgs e)
        {
            if (textBox9.Text == "")// проверка на заполнение полей
            {
                MessageBox.Show("Заполните поле Идентификатора!", "Обратите внимание!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                //sqlConnect.Open();
                SqlCommand cmd = LoginForm.sqlConnect.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "select Operation,Parameter  from Tariffs where  IdTariff=N'" + textBox9.Text + "'";

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    textBox18.Text = reader[0].ToString();
                    textBox19.Text = reader[1].ToString();
                }
                reader.Close();
                //sqlConnect.Close();
            }
        }

        //изменить услугу: ВНИМАНИЕ не хватает проверки на уникальность!!!
        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 1) // надо удалить!!!
            {
                try
                {
                    //sqlConnect.Open();
                    SqlCommand cmd = LoginForm.sqlConnect.CreateCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "update Tariffs set Operation =N'" + textBox3.Text + "', Parameter=N'" + textBox4.Text + "' where IdTariff =N'" + textBox9.Text + "'";

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Обновлена услуга в таблице Тарифов.", "Обратите внимание!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //sqlConnect.Close();

                    disp_data();
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
            }
            else
            {
                MessageBox.Show("Выберите услугу для изменения", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //создать или изменить плату: 
        private void button3_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 1) // надо удалить!!!
            {
                try
                {
                    string new_fee;
                    string existing_fee;
                    // Проверки на логику внесения платы:
                    if (checkBox1.Checked == true)//если плата без значения
                    {
                        MessageBox.Show("Вами был выбран режим БЕЗ числового значения. Плата не взимается. ", "Обратите внимание!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        //textBox8.ReadOnly = false; - удалить
                        textBox5.Text = "";
                        textBox6.Text = "";
                        textBox7.Text = "";
                        comboBox3.Text = "";
                        comboBox4.Text = "";
                        
                        new_fee = textBox8.Text;
                    }
                    else //иначе
                    {
                        if (textBox6.Text == "" && textBox7.Text == "") // % от суммы - если не заполнены мин.и макс.
                        {
                            if (textBox5.Text == "")
                            {
                                MessageBox.Show("Заполните % от суммы!" , "Обратите внимание!", MessageBoxButtons.OK, MessageBoxIcon.Information); 
                                return;
                            }
                            else
                            {
                                textBox8.Text = "";                                
                                comboBox3.Text = "";
                                //comboBox4.Text = ""; ?????
                                new_fee = textBox5.Text + "%" + comboBox4.Text;
                            }
                        }
                        else if (textBox5.Text == "") // фикс. плата - если не заполнен %
                        {
                            if (textBox6.Text == "" || comboBox3.Text == "")
                            {
                                MessageBox.Show("Заполните минимальную сумму и валюту!", "Обратите внимание!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                return;
                            }
                            else
                            {
                                textBox8.Text = "";
                                textBox7.Text = "";

                                new_fee = textBox6.Text + comboBox3.Text + comboBox4.Text;
                            }
                        }
                        else if (textBox7.Text != "") // диапазон - если заполнен макс.
                        {
                            if (textBox5.Text == "" || textBox6.Text == "" || comboBox3.Text == "")
                            {
                                MessageBox.Show("Заполните % от суммы, минимальную сумму и валюту!", "Обратите внимание!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                return;
                            }
                            else
                            {
                                textBox8.Text = "";                                
                                new_fee = textBox5.Text + "% мин."+ textBox6.Text + comboBox3.Text + " макс."+ textBox7.Text +comboBox3.Text+ comboBox4.Text;
                            }
                        }
                        else // иначе 
                        {
                            if (textBox6.Text == "" || comboBox3.Text == "")
                            {
                                MessageBox.Show("Заполните минимальную сумму и валюту!", "Обратите внимание!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                return;
                            }
                            else
                            {
                                textBox8.Text = "";
                                if (checkBox2.Checked == true)//если выбран "+"
                                {
                                    new_fee = textBox5.Text + "% +" + textBox6.Text + comboBox3.Text + comboBox4.Text;
                                }
                                else // иначе
                                {
                                    new_fee = textBox5.Text + "% мин." + textBox6.Text + comboBox3.Text + comboBox4.Text;
                                }
                            }
                        }
                    }
                    
                    existing_fee = new_fee_check(new_fee);

                    //sqlConnect.Open();
                    SqlCommand cmd = LoginForm.sqlConnect.CreateCommand();
                    cmd.CommandType = CommandType.Text;                    
                    // если FeeName раньше не встречалось, то дополнительно создать новый ID в Fees
                    if (existing_fee == "")
                    {                        
                        cmd.CommandText = "update Tariffs set Fee =N'" + new_fee + "' where IdTariff =N'" + textBox10.Text + "';" +
                        "insert into Fees values(N'" + new_fee + "',N'" + textBox5.Text + "',N'" + textBox6.Text + "',N'" + textBox7.Text + "',N'" + comboBox3.Text + "',N'" + comboBox4.Text + "',N'" + textBox8.Text +"')";
                        MessageBox.Show("Создана новая запись в таблице Плат: " + new_fee, "Обратите внимание!", MessageBoxButtons.OK, MessageBoxIcon.Information);                        
                    }
                    else // иначе только обновить Tariffs
                    {
                        cmd.CommandText = "update Tariffs set Fee =N'" + new_fee + "' where IdTariff =N'" + textBox10.Text + "'";
                        MessageBox.Show("Обновлена плата в таблице Тарифов.", "Обратите внимание!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                    /* -удалить
                     IF object_id('Persons') IS NULL CREATE TABLE Persons(INN INT NOT NULL PRIMARY KEY," + "FIO NVARCHAR(150), Address NVARCHAR(200), Education NVARCHAR(200), Date_of_birth DATE);" +
                                      "IF object_id('Event') IS NULL CREATE TABLE Event(ID INT PRIMARY KEY IDENTITY, INN INT FOREIGN KEY REFERENCES Persons(INN) ON DELETE CASCADE," +
                                      "Event NVARCHAR(250))                     
                     */
                    //cmd.CommandText = "insert into Fees values(N'" + new_fee + "',N'" + textBox5.Text + "',N'" + textBox6.Text + "',N'" + textBox7.Text + "',N'" + comboBox3.Text + "',N'" + comboBox4.Text + "','')";

                    cmd.ExecuteNonQuery();

                    //sqlConnect.Close();
                            
                    disp_data();
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
            }
            else
            {
                MessageBox.Show("Выберите тариф для изменения платы", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // копировать тариф: (детали - см.форму 4)
        private void button4_Click(object sender, EventArgs e)
        {
            CopyTariffForm TariffCopyForm = new CopyTariffForm(this);
            TariffCopyForm.ShowDialog();
            tariff_id = textBox12.Text;
        }
       
        // показать тарифы в разработке для удаления:
        private void button7_Click(object sender, EventArgs e)
        {
            //sqlConnect.Open();
            SqlCommand cmd = LoginForm.sqlConnect.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select * from Tariffs where Status = N'В разработке'";

            cmd.ExecuteNonQuery();

            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            dataGridView1.DataSource = dt;

            //sqlConnect.Close();
            
            flag = "del";
            /*textBox11.Visible = true;
            button5.Visible = true;*/
        }

        // удалить тариф: ВНИМАНИЕ не хватает сообщения об успехе!!!
        private void button5_Click(object sender, EventArgs e)
        {
            //if (dataGridView1.SelectedRows.Count == 1) // удалить????
            if (flag == "del") // проверка флага
            {
                try
                {
                    string status = "В разработке"; // контроль на статус
                    //sqlConnect.Open();
                    SqlCommand cmd = LoginForm.sqlConnect.CreateCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "delete from Tariffs where IdTariff=N'" + textBox11.Text + "' and Status=N'" +status+ "'";

                    cmd.ExecuteNonQuery();
                    //MessageBox.Show("Удалена запись в таблице Тарифов, ID= " + textBox11.Text, "Обратите внимание!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //sqlConnect.Close();

                    disp_data();
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
            }
            else
            {
                MessageBox.Show("Для удаления выберите тариф в разработке", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        // показать тарифы в разработке для перевода в действие:
        private void button8_Click(object sender, EventArgs e)
        {
            //sqlConnect.Open();
            SqlCommand cmd = LoginForm.sqlConnect.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select * from Tariffs where Status = N'В разработке'";

            cmd.ExecuteNonQuery();

            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            dataGridView1.DataSource = dt;

            //sqlConnect.Close();
                        
            /*button9.Visible = true;*/
        }

        // перевод в действие:
        private void button9_Click(object sender, EventArgs e)
        {
            try
            {
                //sqlConnect.Open();
                SqlCommand cmd = LoginForm.sqlConnect.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "update Tariffs set Status =N'На согласовании', Date='" + dateTimePicker1.Text + "'  where Status =N'В разработке'";

                cmd.ExecuteNonQuery();

                //sqlConnect.Close();
                MessageBox.Show("Тарифы в разработке были переведены на согласование руководителя.", "Обратите внимание!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                disp_data();
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
            
        }

        // показать действующие тарифы для перевода в архив:
        private void button11_Click(object sender, EventArgs e)
        {
            //sqlConnect.Open();
            SqlCommand cmd = LoginForm.sqlConnect.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select * from Tariffs where Status = N'Действует'";

            cmd.ExecuteNonQuery();

            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            dataGridView1.DataSource = dt;

            //sqlConnect.Close();

        }

        // перевод в архив конкретного тарифа:
        private void button10_Click(object sender, EventArgs e)
        {
            try
            {
                //sqlConnect.Open();
                SqlCommand cmd = LoginForm.sqlConnect.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "update Tariffs set Status =N'На согласовании. Действует', Date='" + dateTimePicker1.Text + "'  where Status =N'Действует'and IdTariff='"+textBox13.Text+"'";

                cmd.ExecuteNonQuery();

                //sqlConnect.Close();
                MessageBox.Show("Тарифы в действии были переведены на согласование руководителя.", "Обратите внимание!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                disp_data();
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
        }

        // показать тарифы на согласовании:
        private void button13_Click(object sender, EventArgs e)
        {
            //sqlConnect.Open();
            SqlCommand cmd = LoginForm.sqlConnect.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select * from Tariffs where Status like N'На согласовании%'";

            cmd.ExecuteNonQuery();

            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            dataGridView1.DataSource = dt;

            //sqlConnect.Close();

        }

        // согласовать все:
        private void button12_Click(object sender, EventArgs e)
        {
            try
            {
                //sqlConnect.Open();
                SqlCommand cmd = LoginForm.sqlConnect.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "update Tariffs set Status =N'Действует' where Status =N'На согласовании'" +
                    "update Tariffs set Status =N'Архивный' where Status =N'На согласовании. Действует'";

                cmd.ExecuteNonQuery();

                //sqlConnect.Close();
                MessageBox.Show("Все тарифы, находящиеся на согласовании, были утверждены.", "Обратите внимание!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                disp_data();
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
        }

        // отклонить все:
        private void button14_Click(object sender, EventArgs e)
        {
            try
            {
                //sqlConnect.Open();
                SqlCommand cmd = LoginForm.sqlConnect.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "update Tariffs set Status =N'В разработке' where Status =N'На согласовании'" +
                    "update Tariffs set Status =N'Действует' where Status =N'На согласовании. Действует'";

                cmd.ExecuteNonQuery();

                //sqlConnect.Close();
                MessageBox.Show("Все тарифы, находящиеся на согласовании, были отклонены.", "Обратите внимание!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                disp_data();
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
        }

        // отклонить конкретный тариф:
        private void button15_Click(object sender, EventArgs e)
        {
            try
            {
                //sqlConnect.Open();
                SqlCommand cmd = LoginForm.sqlConnect.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "update Tariffs set Status =N'В разработке' where Status =N'На согласовании' and IdTariff='"+textBox14.Text+"'" +
                    "update Tariffs set Status =N'Действует' where Status =N'На согласовании. Действует'and IdTariff='"+textBox14.Text+"'";

                cmd.ExecuteNonQuery();

                //sqlConnect.Close();
                MessageBox.Show("Тариф, ID =" + textBox14.Text + ", находящийся на согласовании, был отклонен.", "Обратите внимание!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                disp_data();
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
        }
        
        //поиск: ВНИМАНИЕ! не хватает множественности посика по нескольким параметрам (2, 3... но не все)
        private void button6_Click(object sender, EventArgs e)
        {
            if (textBoxIDt.Text == "" && textBoxOperation.Text == "" && textBoxParameter.Text == "" && comboBoxChanal.Text == "" && comboBoxProduct.Text == "" && comboBoxStatus.Text == "")
            {
                MessageBox.Show("Не заполнено ни одно поле для поиска!", "Обратите внимание!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                //sqlConnect.Open();
                SqlCommand cmd = LoginForm.sqlConnect.CreateCommand();
                cmd.CommandType = CommandType.Text;
                // показать все возможные варианты задания поиска:

                if (textBoxIDt.Text != "" && textBoxOperation.Text != "" && textBoxParameter.Text != "" && comboBoxChanal.Text != "" && comboBoxProduct.Text != "" && comboBoxStatus.Text != "")
                {
                    cmd.CommandText = "select * from Tariffs where IdTariff=N'" + textBoxIDt.Text +
                    "' and Product=N'" + comboBoxProduct.Text +
                    "' and Operation=N'" + textBoxOperation.Text +
                    "' and Parameter=N'" + textBoxParameter.Text +
                    "' and Canal=N'" + comboBoxChanal.Text +
                    "' and Status=N'" + comboBoxStatus.Text +
                    "'";// самый полный запрос
                }
                else if (textBoxIDt.Text == "" && comboBoxChanal.Text == "" && comboBoxStatus.Text == "" && comboBoxProduct.Text != "" && textBoxOperation.Text !="") 
                {
                    cmd.CommandText = "select * from Tariffs where Product=N'" + comboBoxProduct.Text +
                    "' and Operation=N'" + textBoxOperation.Text + 
                    "'";// запрос только по продукту и операции
                    MessageBox.Show("Произведен поиск по продукту и операции", "Обратите внимание!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (textBoxIDt.Text != "") // запрос включает ИД, остальное обнуляем
                {
                    cmd.CommandText = "select * from Tariffs where IdTariff=N'" + textBoxIDt.Text +
                    "'";// запрос только по ИД

                    comboBoxChanal.Text = "";
                    comboBoxProduct.Text = "";
                    comboBoxStatus.Text = "";
                    textBoxParameter.Text = "";
                    textBoxOperation.Text = "";

                    MessageBox.Show("Произведен поиск по ID= " + textBoxIDt.Text, "Обратите внимание!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (comboBoxStatus.Text != "") // запрос включает Статус, остальное обнуляем
                {
                    cmd.CommandText = "select * from Tariffs where Status=N'" + comboBoxStatus.Text +
                    "'";// запрос только по статусу

                    textBoxIDt.Text = "";
                    comboBoxProduct.Text = "";
                    comboBoxChanal.Text = "";
                    textBoxParameter.Text = "";
                    textBoxOperation.Text = "";

                    MessageBox.Show("Произведен поиск по Статусу: " + comboBoxStatus.Text, "Обратите внимание!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (comboBoxChanal.Text != "") // запрос включает Канал, остальное обнуляем
                {
                    cmd.CommandText = "select * from Tariffs where Canal=N'" + comboBoxChanal.Text +
                    "'";// запрос только по каналу

                    textBoxIDt.Text = "";
                    comboBoxProduct.Text = "";
                    comboBoxStatus.Text = "";
                    textBoxParameter.Text = "";
                    textBoxOperation.Text = "";

                    MessageBox.Show("Произведен поиск по Каналу: " + comboBoxChanal.Text, "Обратите внимание!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (comboBoxProduct.Text != "") // запрос включает Продукт, остальное обнуляем
                {
                    cmd.CommandText = "select * from Tariffs where Product=N'" + comboBoxProduct.Text +
                    "'";// запрос только по продукту

                    textBoxIDt.Text = "";
                    comboBoxStatus.Text = "";
                    comboBoxChanal.Text = "";
                    textBoxParameter.Text = "";
                    textBoxOperation.Text = "";

                    MessageBox.Show("Произведен поиск по Продукту: " + comboBoxProduct.Text, "Обратите внимание!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    cmd.CommandText = "select * from Tariffs";
                    MessageBox.Show("Заполните все поля для поиска!", "Обратите внимание!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                cmd.ExecuteNonQuery();

                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                dataGridView1.DataSource = dt;

                //sqlConnect.Close();
            }
        }
 
        // организация ролевых доступов к элементам формы 
        private void Form1_Load(object sender, EventArgs e)
        {                  
            
            if (LoginForm.user_role == "Manager")
            {
                пользователиToolStripMenuItem.Visible = false;
                button1.Visible = false;
                button2.Visible = false;
                button3.Visible = false;
                button4.Visible = false;
                button5.Visible = false;
                button9.Visible = false;
                button10.Visible = false;
                button14.Visible = true;
                button15.Visible = true;
                button12.Visible = true;
            }           
            else if (LoginForm.user_role == "Constructor")
            {
                пользователиToolStripMenuItem.Visible = false;
                button12.Visible = false;
            }
            else // иначе - роль "Administrator"
            {
                пользователиToolStripMenuItem.Visible = true;
                button1.Visible = false;
                button2.Visible = false;
                button3.Visible = false;
                button4.Visible = false;
                button5.Visible = false;
                button9.Visible = false;
                button10.Visible = false;
                button12.Visible = false;
                button14.Visible = false;
                button15.Visible = false;                
            }
        }

    }
}
