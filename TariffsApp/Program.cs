using System;
using System.Windows.Forms;

namespace TariffsApp
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new LoginForm());
             //Application.Run(new Form1()); // для отладки в обход авторизации
        }
    }
}
