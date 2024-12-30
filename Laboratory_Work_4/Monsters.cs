using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace Лаба3
{

    public abstract partial class Monster : IReactable// Абстрактный базовый класс для монстров
    {
        protected IUIUpdater uiUpdater; //Интерфейс для взаимодействия с пользовательским интерфейсом
      
        public Monster(IUIUpdater uIUpdater) //Конструктор 
        {
            this.uiUpdater = uIUpdater;
        }

        public abstract void Hit(); // Действие удара
        public abstract void Greetings(); // Действие приветствия
        public abstract void ReactToAction(PlayerAction action); // Реакция на действие
       
    }

    public partial class GoodMonster : Monster // Класс доброго монстра
    {
     public event Action<string> OnMessage // Событие для передачи сообщений
       ;
      
        public GoodMonster(IUIUpdater uiUpdater) : base(uiUpdater) // Конструктор
        {
            OnMessage += uiUpdater.UpdateMessage;
            
        }

        public override void Hit() // Реализация удара
        {
            string message = "Вы ударили монстра! " +
                             "Он не ударил в ответ и убежал. Кажется, вы видели слезы в его глазах...\n" +
                             "Вы чувствуете себя виноватым.";

            if (PlayerStats.Inventory.Objects.Contains("меч")) // Если есть меч
            {
                message += "\nМеч был изъят!";
                PlayerStats.Inventory.Objects.Remove("меч"); // Удаляем меч
            }
            else if (PlayerStats.Inventory.Objects.Contains("коробка")) // Если нет меча, но есть коробка
            {
                message += "\nЗагадочная коробка была изъята!";
                PlayerStats.Inventory.Objects.Remove("коробка"); // Удаляем коробку
            }
           
            OnMessage.Invoke(message);
            uiUpdater.UpdateMessage(message);// Обновляем интерфейс

        }

        public override void Greetings() // Реализация приветствия
        {
            string message = "Монстр рад, что вы не напали. Он задает вам вопрос: " +
                             "Ты за луну или за солнце? " +
                             "\nВы думаете: достаточно странный вопрос. Но решаете ответить:";
            
            OnMessage?.Invoke(message);
            uiUpdater.UpdateMessage(message); // Обновляем интерфейс
            

        }
        public void Question(int choice) { //Реализация обработки ответа игрока на вопрос
            string message;
            switch (choice) // Действие в зависимости от выбора
            {
                case 1:
                    message = "Монстру явно не понравился ваш ответ. Он стукнул вас и ушел. Здоровье уменьшилось.";
                    PlayerStats.Health.RemoveHealth(25); // Уменьшаем здоровье
                    break;
                case 2:
                    message = "Монстр улыбнулся - ему явно понравился ваш ответ. Он протягивает вам что-то.\n";
                    PlayerStats.Inventory.AddKeys(1); // Добавляем ключ
                    break;
                default:
                    message = "Неверный выбор.";
                    break;
            }

            OnMessage?.Invoke(message);
            uiUpdater.UpdateMessage(message);
           
          
        }

        public override void ReactToAction(PlayerAction action) // Реализация реакции на действие
        {
            switch (action) // Выполнение действия
            {
                case PlayerAction.Hit:
                    Hit(); // Действие удара
                    break;
                case PlayerAction.Greet:
                    Greetings(); // Действие приветствия
                    break;
                default:
                   uiUpdater.UpdateMessage("Монстр не понимает вашего действия."); // Неопределенное действие
                    break;
            }
        }
    }

    public partial class BadMonster : Monster // Класс плохого монстра
    {
        public event Action<string> OnMessage; // Событие для передачи сообщений
        public BadMonster(IUIUpdater uiUpdater) : base(uiUpdater) // Конструктор
        {
            OnMessage += uiUpdater.UpdateMessage;
        }
        public override void Hit() // Реализация действия удара
        {

            switch (PlayerStats.Progression.ChoosedPath) // Зависит от выбора пути
            {
                case "Путь воина": // Если выбран путь воина
                    string message = "Вы чувствуете прилив сил и атакуете монстра мечом!\n" +
                    "После долгого сражения вы побеждаете монстра.\nЕго огромная лапа разжимается и вы видите в ней ключ\n";
                    PlayerStats.Inventory.AddKeys(1); // Добавляем ключ
                    OnMessage.Invoke(message);
                    uiUpdater.UpdateMessage(message);
                    ;

                    break;

                case "Путь стратега": // Если выбран путь стратега
                    message = "Вы ударяете монстра!" +
                    " Кажется удар только больше разозлил его.\n" +
                    " Вы шарите в карманах, находите " +
                    "коробку и решаете бросить ее в монстра." + 
                    "Она попадает ему прямо в голову.\nНа миг он замирает,но все же успевает нанести удар.";
                    PlayerStats.Health.RemoveHealth(25); // Уменьшаем здоровье
                    PlayerStats.Inventory.Objects.Remove("коробка"); // Убираем коробку
                    OnMessage.Invoke(message);
                    uiUpdater.UpdateMessage(message);
                   
                    break;
            }

        }

        public override void Greetings() // Реализация действия приветствия
        {
            string message = "Вам кажется что монстр не так уж и страшен.Вы решаете заговорить с ним\nВы не успеваеете сказать и слова как монстра наносит удар!";
            OnMessage.Invoke(message);
            uiUpdater.UpdateMessage(message);
             PlayerStats.Health.RemoveHealth(50);
            
        }

        public void Run() // Реализация действия бегства
        {
            string message = "Вы не думаете ни о чем и просто бежите. Монстр даже не успевает ничего понять.\n" + 
            "Вы убежали от монстра!";
            if (PlayerStats.Progression.ChoosedPath == "Путь стратега") // Если выбран путь стратега
            {
                message+="По пути вы видите ключ и поднимаете его.";
                PlayerStats.Inventory.AddKeys(1); // Добавляем ключ
            }
              OnMessage .Invoke(message);
            uiUpdater.UpdateMessage(message);
           
        }
        
        public override void ReactToAction(PlayerAction action) // Реализация реакции на действие
        {
            switch (action) // Выбор действия
            {
                case PlayerAction.Hit:
                    Hit(); // Выполняем действие удара
                    break;
                case PlayerAction.Greet:
                    Greetings(); // Выполняем действие приветствия
                    break;
                case PlayerAction.Run:
                    Run(); // Выполняем действие бегства
                    break;
                default:
                    uiUpdater.UpdateMessage("Монстр не понимает вашего действия."); // Неопределенное действие
                    break;
            }
        }
    }
}