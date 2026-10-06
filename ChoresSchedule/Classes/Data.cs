using System.Collections.Generic;

namespace ChoresSchedule.Classes
{
    public static class Data
    {
        /// <summary>
        /// Данные для создания карточек
        /// </summary>
        public static int idCreate {  get; set; }
        public static int cardCount { get; set; }

        public static bool adminMode { get; set; }

        public static string pathUserMan { get; private set; } = "/Resources/UserPic/_UserMan.png";
        public static string pathUserWoman { get; private set; } = "/Resources/UserPic/_UserWoman.png";

        public static string aboutProgramInformation => "\tДанная программы была разработана для участия в НПК \"ПО для семьи\".\nПрограмма составляет график ответственных за уборку, основываясь на данных, \nполученных в результате анкетирования.\n\n\tРазработчик:\n- Учебное заведение: Благовещенский строительный техникум\n- Студент: Кухтин К.В.\n- Группа: 911\n- Дата создания: 09.11.2024\n- Версия: release";
        public static string aboutProgram { get; set; } = aboutProgramInformation;

        public static Dictionary<string, double> ListOfOccupationalStatus { get; private set; } = new Dictionary<string, double>
        {
            { "Низкая", 1 },
            { "Средняя", 0.5 },
            { "Высокая", 0.1 }
        };

        public static Dictionary <string, double> ListOfFamilySituations { get; private set; } = new Dictionary<string, double>
        {
            { "Работает", 50 },
            { "Декрет", 30 },
            { "По дому", 100 },
            { "Учеба", 80 },
            { "Безработный", 90}
        };
    }
}
