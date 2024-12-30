using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Лаба3
{
    public partial class Serpent : IReactable //Змей
    {
        public event Action<string> OnMessage;
        protected IUIUpdater uiUpdater;
        public Serpent(IUIUpdater uiUpdater)  // Конструктор
        {
            OnMessage += uiUpdater.UpdateMessage;
            this.uiUpdater = uiUpdater;
            

        }
        public void Fight() // Метод для действия "сражаться"
        {
            if (PlayerStats.Inventory.Objects.Contains("меч")) // Если есть меч
            {
               string message = "Вы решаетесь атаковать змея!Сражение кажется бесконечным,\nно вы все же выходите из него победителем."
                + "Вы замечаете,что на поверженном змее висит ключ и забираете его.";
                PlayerStats.Inventory.AddKeys(1); // Добавляем ключ
                OnMessage.Invoke(message);
                uiUpdater.UpdateMessage(message);
            }
            else // Если нет меча
            {
                string message = "Вы бросаетесь на змея с кулаками.\nКажется,вы совсем не кажетесь ему опасным.\nОн ударяет вас своим огромным хвостом!"
                + "От такого удара оправиться не получится...";
                OnMessage.Invoke(message);
                uiUpdater.UpdateMessage(message);
                int health = PlayerStats.Health.Health;
                PlayerStats.Health.RemoveHealth(health); // У игрока отнимается все здоровье
            }
        }

        public void ReactionToEscape() // Метод для реакции на побег игрока
        {
            string message = "Вы решаете не рисковать и пройти мимо змея.";
            switch (PlayerStats.Progression.ChoosedPath) // Зависит от выбранного пути
            {
                case "Путь стратега": // Если выбран путь стратега
                    message+="Вы тщательно обдумываете все свои шаги,\nи вам удается обойти змея, оставшись незамеченным.";
                    OnMessage.Invoke(message);
                    uiUpdater.UpdateMessage(message);
                    break;
                case "Путь воина": // Если выбран путь воина
                    message += "Вы оббегаете змея.\nУ самого выхода он замечает вас и задевает своим хвостом!";
                    OnMessage.Invoke(message);
                    uiUpdater.UpdateMessage(message);
                    PlayerStats.Health.RemoveHealth(50); // Уменьшаем здоровье
                    break;
            }
        }

        public void ReactToAction(PlayerAction action) // Метод для реакции на действие игрока
        {
            switch (action) 
            {
                case PlayerAction.Hit:
                    Fight(); // Вызываем действие "сражаться"
                    break;
                case PlayerAction.Run:
                    ReactionToEscape(); // Вызываем действие "реакция на побег"
                    break;
                default:
                    uiUpdater.UpdateMessage("Змей не понимает вашего действия."); // Действие неопределенно
                    break;
            }
        }
    }
}


