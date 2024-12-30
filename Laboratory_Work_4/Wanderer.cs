using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Animation;
using static System.Net.Mime.MediaTypeNames;

namespace Лаба3
{
    public partial class Wanderer : IReactable //Странник
    {
        public event Action<string> OnMessage; //Событие передачи сообщения на интерфейс
        string answerToTheRiddle = "Время"; //Ответ на загадку
        protected IUIUpdater uiUpdater;
        public Wanderer(IUIUpdater uiUpdater)
        {
            this.uiUpdater = uiUpdater;
        }
        public void GivePotion() // Дает зелье и увеличивает здоровье
        {
            string message = "Странник протягивает вам зелье и говорит: «Соблюдай осторожность\n и используй это жизненное благо с умом» и исчезает.";
            PlayerStats.Health.AddHealth(35); // Увеличиваем здоровье
            OnMessage?.Invoke(message);
            uiUpdater.UpdateMessage(message);
        }

        public void GiveMap() // Дает карту, требуя ответ на загадку
        {
            string message = "Странник говорит: «Но просто так я тебе ее не отдам! Сначала нужно дать ответ на мой вопрос.\n" + 
           "Уничтожает все кругом:Цветы, зверей, высокий дом\nСжует железо, сталь сожрет. И скалы в порошок сотрет,\nМощь городов, власть королей.Его могущества слабей.»";
            OnMessage?.Invoke(message);
            uiUpdater.UpdateMessage(message);

        }


        public bool HandleRiddle(int maxAttempts, string playerAnswer, int attempts) // Обрабатывает загадку
        {
            // Проверяем ответ
            if (playerAnswer.Equals(answerToTheRiddle, StringComparison.OrdinalIgnoreCase))
            {
                string message = "Это правильный ответ! Странник говорит:\n«Я еще не встречал таких мудрых людей...» и протягивает вам карту." +
                                 " Вы не знаете, что с ней делать,\nно развернув ее, вы находите ключ!";
                PlayerStats.Inventory.AddKeys(1); // Добавляем ключ
                OnMessage?.Invoke(message);
                uiUpdater.UpdateMessage(message);
                return true; // Ответ верный
            }
            
                else // Если попытки исчерпаны
                {
                    string message = "Вы исчерпали все попытки! Странник говорит: Кажется я переоценил тебя и насылает на вас заклятье.";
                    PlayerStats.Health.RemoveHealth(25);
                    OnMessage?.Invoke(message);
                    uiUpdater.UpdateMessage(message);
                    uiUpdater.UpdateUI(9);
                }
            

            return false; // Ответ неверный
        }

        public void ReactToAction(PlayerAction action) // Обрабатывает действие игрока
        {
            switch (action) // Выполняем действие
            {
                case PlayerAction.TakePotion:
                    GivePotion(); // Даем зелье
                    break;
                case PlayerAction.TakeTreasureMap:
                    GiveMap(); // Даем карту
                    break;
                default:
                     uiUpdater.UpdateMessage("Странник не понимает вашего действия."); // Действие непонятно
                    break;
            }
        }
    }
}
