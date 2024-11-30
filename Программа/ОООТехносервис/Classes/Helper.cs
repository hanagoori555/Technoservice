using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ОООТехносервис.Classes
{
    //Класс с глобальными для всего проекта величинами
    public class Helper
    {
        //Связка с БД
        public  static Model.ConnectDBRequest  DBRequest { get; set; }
        //Пользователь, вошедший в систему
        public static Model.User user;
    }
}
