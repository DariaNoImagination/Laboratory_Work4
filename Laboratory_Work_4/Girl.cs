using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Лаба3
{
    public partial class Girl : IReactable //Девочка
    {
        public event Action<string> OnMessage;
        protected IUIUpdater uiUpdater;
        int answerToTheQuestion = 52; // Правильный ответ на вопрос
        string playerAnswer; // Ответ игрока
        public Girl(IUIUpdater uiUpdater) { this.uiUpdater = uiUpdater; 
            OnMessage += uiUpdater.UpdateMessage;

        }
        public void AskQuestion() // Метод задать вопрос
        {
            string message = "Она смеясь, повторяет таблицу умножения: «4 на 6 хахаха 24.. 5 на 8 ахах 40!»\nЗаметив вас она серьезно поворачивается и спрашивает «2 плюс 5 на 10..?»";
            OnMessage.Invoke(message);  
            uiUpdater.UpdateMessage(message);
        }  //Передаем сообщение на интерфейс
        public void HandleAnswer(string answer) {   
            playerAnswer = answer;
            if (int.TryParse(playerAnswer, out int playerAnswerOnQuestion) && answerToTheQuestion == playerAnswerOnQuestion) // Проверяем ответ
            {
                string message = "Это правильный ответ!\nДевочка расплывается в улыбке и хохоча уходит, обронив ключ.\nВы поднимаете ключ и идете дальше.";
                OnMessage.Invoke(message);
                uiUpdater.UpdateMessage(message);
                PlayerStats.Inventory.AddKeys(1); // Добавляем ключ
            }
            else
            {
                string message = "Ответ неверный!\nДевочка смотрит на вас с недовольным выражением лица и уходит ничего не сказав.";
                OnMessage.Invoke(message);
                uiUpdater.UpdateMessage(message);
            }
        }

        public void ReactionOnIgnore() // Реакция на игнорирование
        {
            string message = "Девочка злится, что вы ее проигнорировали.\nОна догоняет вас и дает вам пинка.\nУдар был слабым,но все же неприятным.";
            PlayerStats.Health.RemoveHealth(10); // Уменьшаем здоровье
            OnMessage.Invoke(message);
            uiUpdater.UpdateMessage(message);
        }

        public void ReactToAction(PlayerAction action) // Реакция на действие
        {
            switch (action) // Проверяем действие
            {
                case PlayerAction.Greet:
                    AskQuestion(); // Задаем вопрос
                    break;
                case PlayerAction.Ignore:
                    ReactionOnIgnore(); // Реакция на игнорирование
                    break;
                default:
                    uiUpdater.UpdateMessage("Девочка не понимает вашего действия."); // Если действие неопределенно
                    break;
            }
        }
    }
}
