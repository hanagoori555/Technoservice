using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ОООТехносервис.Classes;       //Для сокращения


namespace ОООТехносервис
{
    public partial class Authorization : Form
    {
        /// <summary>
        /// Конструктор окна
        /// </summary>
        public Authorization()
        {
            InitializeComponent();
            //Создали подключение к БД
            //try
            //{
            //    Classes.Helper.DBRequest = new Model.ConnectDBRequest();
            //    MessageBox.Show("К БД подключились");
            //}
            //catch 
            //{
            //    MessageBox.Show("Не удалось подключиться к БД");
            //    Environment.Exit(-1);   //Аварийный выход из приложения
            //}
        }
        /// <summary>
        /// Выод из приложения
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Событие загрузки главного окна
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Authorization_Load(object sender, EventArgs e)
        {
            //Создали подключение к БД
            try
            {
                Helper.DBRequest = new Model.ConnectDBRequest();
                MessageBox.Show("К БД подключились");
            }
            catch
            {
                MessageBox.Show("Не удалось подключиться к БД");
                Environment.Exit(-1);   //Аварийный выход из приложения
            }
        }
        /// <summary>
        /// Вход в систему по логину и паролю
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonInput_Click(object sender, EventArgs e)
        {
            //Получили данные от пользователя из интерфейса
            string login = textBoxLogin.Text;
            string password = textBoxPassword.Text;

            ////////////Результат запроса на получение всех пользователей
            //////////List<Model.User>   users =  Classes.Helper.DBRequest.User.ToList();
            ////////////Один пользователь
            //Model.User user=null;
            //////////Список отфильтраванных пользователей
            ////////List<Model.User> usersWhere = users.Where(u=>u.UserLogin == login && u.UserPassword == password).ToList();
            //////////Первый из отфильтрованных
            ////////user = usersWhere.FirstOrDefault();

            Helper.user = Classes.Helper.DBRequest.User.Where(u=>u.UserLogin==login &&
            u.UserPassword==password).FirstOrDefault();

            ////////////////Перебор всех пользователей
            //////////////foreach (Model.User u in users) 
            //////////////{
            //////////////    if (u.UserLogin == login && u.UserPassword == password) //Нашли
            //////////////    {
            //////////////        user = u;
            //////////////        break;
            //////////////    }
            //////////////}

            if (Helper.user != null)
            {
                MessageBox.Show("Нашли. Вы вошли с ролью " + Helper.user.Role.RoleName);

                //Отобразить окно
                View.ListRequests listRequests = new View.ListRequests();   //Создали дополнительное окно
                this.Hide();    //Временно скрыли окно авторизации this - ссылка на окно, в котором сейчас находимся
                listRequests.ShowDialog();  //Открыть окно списка
                this.Show();        //Показать окно авторизации после закрытия окна списка
            }
            else
            {
                MessageBox.Show("Не нашли");
            }

        }
    }
}
