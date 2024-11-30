using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ОООТехносервис.Classes;
using ОООТехносервис.Model;

namespace ОООТехносервис.View
{
    //Глобальная переданная заявка или null для новой заявки
    public partial class WorkRequest : Form
    {
        //Глобальная переданная заявка или null для новой заявки
        Model.Request Request;

        public WorkRequest()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Конструктор с параметром
        /// </summary>
        /// <param name="id">Номер выбранной заявки или 0</param>
        public WorkRequest(int id)
        {
            InitializeComponent();
            if (id > 0)         //Режим редактирование выбранной заявки
            {
                //Получить всю информацию о заявке, номер которой передался в форму
                Request = Helper.DBRequest.Request.Where(r => r.RequestId == id).FirstOrDefault();
            }
            else               //Режим добавления, выбранной заявки нет
            { 
                Request = null;
            }
        }

        private void buttonExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //При закрузке формы подготовить все элементы интерфейса
        private void WorkRequest_Load(object sender, EventArgs e)
        {
            //Настройка всех ComboBox не зависимо от режима окна
            comboBoxEquipment.DataSource = Helper.DBRequest.Device.ToList();
            comboBoxEquipment.DisplayMember = "DeviceName";
            comboBoxEquipment.ValueMember = "DeviceId";

            comboBoxProblem.DataSource = Helper.DBRequest.Defect.ToList();
            comboBoxProblem.DisplayMember = "DefectName";
            comboBoxProblem.ValueMember = "DefectId";

            comboBoxClient.DataSource = Helper.DBRequest.User.Where(u=>u.UserRoleId==1).ToList();
            comboBoxClient.DisplayMember = "UserFullName";
            comboBoxClient.ValueMember = "UserId";

            comboBoxStatus.DataSource = Helper.DBRequest.Status.ToList();
            comboBoxStatus.DisplayMember = "StatusName";
            comboBoxStatus.ValueMember = "StatusId";

            comboBoxMaster.DataSource = Helper.DBRequest.User.Where(u => u.UserRoleId == 2).ToList();
            comboBoxMaster.DisplayMember = "UserFullName";
            comboBoxMaster.ValueMember = "UserId";

            comboPriority.DataSource = Helper.DBRequest.Priory.ToList();
            comboPriority.DisplayMember = "PrioryName";
            comboPriority.ValueMember = "PrioryId";

            comboBoxStage.DataSource = Helper.DBRequest.Stage.ToList();
            comboBoxStage.DisplayMember = "StageName";
            comboBoxStage.ValueMember = "StageId";
            
            //Для случая новой заявки
            if (Request == null)
            {
                //Заполняем заблокированные элемениты значениями по умолчанию
                textBoxDate.Text = DateTime.Now.ToShortDateString();    //Текущая дата
                comboBoxStatus.SelectedValue = 1;   //Статус "В ожидании"
                comboBoxStage.SelectedValue = 3;    //Этап "Не выполнено"
                textBoxTime.Text = "0";             //Время еще не затрачено
            }
            else         //Случай выбранной заявки
            {
                //Заполняем все элементы интерфейса значениями выбранной заявки
                //Для ComboBox - это свойство SelectValue,
                //для него настроено свойство ValueMember нужным полем 
                comboBoxEquipment.SelectedValue = Request.RequestDeviceId;
                comboBoxProblem.SelectedValue = Request.RequestDefectId;
                comboBoxClient.SelectedValue = Request.RequestClientId;
                comboBoxMaster.SelectedValue = Request.RequestMasterId;
                comboPriority.SelectedValue = Request.RequestPrioryId;
                comboBoxStatus.SelectedValue = Request.RequestStatusId;
                comboBoxStage.SelectedValue = Request.RequestStatusId;
                //Значение берем из полей выбранной заявки
                textBoxDate.Text = Request.RequestDate.ToShortDateString();
                textBoxTime.Text = Request.RequestTime.ToString();
            }
            
            //В зависимости от роли пользователя включить необходимые элементы интерфейса
            if (Helper.user.UserRoleId == 3)    //Оператор
            {
                //Оператор в режиме Добавление и Редактирование может менять поле Мастера
                comboBoxMaster.Enabled = true;
                //Включаем элементы доступные оператору в режиме Добавления 
                if (Request == null)
                {
                    comboBoxEquipment.Enabled = comboBoxProblem.Enabled = comboBoxClient.Enabled = true;
                    textBoxDescription.Enabled = comboPriority.Enabled = true;
                }
            }
            if (Helper.user.UserRoleId == 2)    //Оператор только в режиме Редактирования
            {
                //Может менять только время обслуживания заявки, ее этап и комментарий
                textBoxTime.Enabled = comboBoxStage.Enabled = textBoxComment.Enabled = true;
            }
        }

        private void buttonFixed_Click(object sender, EventArgs e)
        {
            if (Request == null)
            {
                Request = new Request(); //1.
                //2
                Request.RequestDate = DateTime.Parse(this.textBoxDate.Text);
                Request.RequestDeviceId = (int)comboBoxEquipment.SelectedValue;
                Request.RequestDefectId = (int)comboBoxProblem.SelectedValue;
                Request.RequestClientId = (int)comboBoxClient.SelectedValue;
                Request.RequestDescription = textBoxDescription.Text;
                Request.RequestStatusId = (int)comboBoxStatus.SelectedValue;
                Request.RequestMasterId = (int)comboBoxMaster.SelectedValue;
                Request.RequestTime = int.Parse(textBoxTime.Text);
                Request.RequestPrioryId = (int)comboPriority.SelectedValue;
                Request.RequestStageId = (int)comboBoxStage.SelectedValue;
                Request.RequestComment = textBoxComment.Text;
                //3
                Helper.DBRequest.Request.Add(Request);
                //4
                try
                {
                    Helper.DBRequest.SaveChanges();
                    MessageBox.Show("Данные успешно добавлены");

                }
                catch (Exception)
                {
                    MessageBox.Show("Данные не добавленн");
                    return;
                }
                this.Close();
            }
            else
            {

            }
        }
    }
}
