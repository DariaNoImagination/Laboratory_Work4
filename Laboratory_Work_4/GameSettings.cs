using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using System.Windows.Controls;
namespace Лаба3
{
    public interface IUIUpdater
    {
        // Обновляет пользовательский интерфейс в зависимости от текущего индекса развилки.
        void UpdateUI(int currentForkIndex);

        // Устанавливает состояние, что монстр был встречен.
        void SetMonsterGreeted(bool greeted);

        // Устанавливает состояние, что монстр был атакован.
        void SetMonsterHit(bool hit);

        // Устанавливает состояние, что монстр сбежал.
        void SetMonsterRun(bool run);

        // Устанавливает состояние, что игрок взял карту.
        void SetTakeMap(bool map);

        // Устанавливает состояние, что игрок взял зелье.
        void SetTakePotion(bool potion);

        // Устанавливает состояние, что игрок проигнорировал девочку.
        void SetIgnoreGirl(bool flag);

        // Устанавливает состояние, что игрок поговорил с девочкой.
        void SetTalkWithGirl(bool flag);

        // Устанавливает состояние, что игрок ударил змею.
        void SetHitSerpent(bool flag);

        // Устанавливает состояние, что игрок сбежал от змеи.
        void SetRunBySerpent(bool flag);

        // Устанавливает состояние, что игрок выбрал правую дверь.
        void SetChooseRightDoor(bool flag);

        // Закрывает игру.
        void CloseGame();

        // Обновляет сообщение на экране.
        void UpdateMessage(string message);

        // Устанавливает состояние счастливого окончания игры.
        void SetHappyEnd(bool happyEnd);
    }
    public class Action //Действие игрока
    {
        public string Description { get; set; } //Описание
        public PlayerAction Type { get; set; } //Тип действия

        public Action() { } //Конструктор по умолчанию
        public Action(string description, PlayerAction type) //Конструктор с параметрами
        {
            Description = description;
            Type = type;
        }
    }

    public class Fork //Развилка
    {
        public string Description { get; set; } //Описание
        public List<string> Options { get; set; }  // Список вариантов выбора, которые доступны игроку.
        public List<Action> Actions { get; set; }  // Список действий, связанных с этой развилкой (например, последствия выбора).

        public List<string> ImageNames { get; set; } //Список изображений, связанных с данной развилкой
        public Fork() { } // Конструктор по умолчанию
        public Fork(string description, List<string> options, List<Action> actions, List<string> imageNames) //Конструктор с параметрами
        {
            Description = description;
            Options = options;
            Actions = actions;
            ImageNames = imageNames;
        }
    }

    // Класс, управляющий игровым процессом и развилками.
    public partial class GameManager
    {
        private IUIUpdater uiUpdater;// Интерфейс для обновления пользовательского интерфейса
        public event Action<string> OnMessage; // Событие для передачи сообщений игроку
        public int CurrentForkIndex
        { get; set; } //Текущая развилка
        private List<Fork> forks; //Список всех развилок в игре
        public List<Fork> Forks // Свойство
        {
            get { return forks; }
            set { forks = value; }
        }
       
        private void InitializePlayerStats() //Инициализация начальных параметров игрока
        {
            IHealthStats healthStats = new HealthStats(100);
            IInventory inventory = new Inventory(0);
            IProgressionStats progressionStats = new ProgressionStats();
            IGameFlags gameFlags = new GameFlags();

            // Создаем PlayerStats, передавая ему объекты статистики
            PlayerStats playerStats = new PlayerStats(healthStats, inventory, progressionStats, gameFlags);
        }
        public GameManager(IUIUpdater uiUpdater) // Конструктор
        {
            this.uiUpdater = uiUpdater; // Инициализация интерфейса 
            CurrentForkIndex = 0; // Начальный индекс развилки
            forks = new List<Fork>(); // Создание списка развилок
            InitializePlayerStats(); // Инициализация параметров игрока
            InitializeForks(); // Инициализация развилок
        }


        private void InitializeForks() // Инициализация развилок
        {
            // Добавление развилок
            forks.Add(new Fork(
                "Вы просыпаетесь от громкого звука в узком коридоре.Вокруг темнота, а выхода не видно.",
                new List<string> { "Далее" },
                null, new List<string> { "1.jpg" }
            )
            );
            forks.Add(new Fork(
                "Перед собой вы видите только коридор, меч и странную коробку.\nВы решаете взять что-нибудь с собой.",
                new List<string> { "Меч", "Коробка" },
                null, new List<string> { "3.jpg", "2.jpg" }
            )
            );

            forks.Add(new Fork(
                "Перед вами развилка.",
                new List<string> { "Лево", "Право" },
                null, new List<string> { "1.jpg" }
            ));

            forks.Add(new Fork(
                "Вы идете налево и видите перед собой монстра. Кажется, он агрессивно настроен.",
                new List<string> { "Убежать", "Ударить", "Поприветствовать" },
                new List<Action>
                {
                new Action("Убежать", PlayerAction.Run),
                new Action("Ударить", PlayerAction.Hit),
                new Action("Поприветствовать", PlayerAction.Greet)
                }
                , new List<string> { "4.jpg" }
            ));

            forks.Add(new Fork(
                "Вы идете направо и видите перед собой  монстра. Он не кажется опасным...",
                new List<string> { "Ударить", "Поприветствовать" },
                new List<Action>
                {
                new Action("Ударить", PlayerAction.Hit),
                new Action("Поприветствовать", PlayerAction.Greet)
                }, new List<string> { "5.jpg" }
            ));
            forks.Add(new Fork(
                "Вы идете дальше по темным коридорам лабиринта и видите перед собой два прохода.",
                new List<string> { "Лево", "Право" },
                new List<Action>
                {
                null
                }, new List<string> { "1.jpg" }
            ));
            forks.Add(new Fork(
                "Вам показалось, что только что перед вами пробежал какой-то человек!",
                new List<string> { "Побежать за ним", "Проигнорировать" },
                new List<Action>
                { null}, new List<string> { "6.jpg" }

            ));
            forks.Add(new Fork(
               "Вы идете по направо и внезапно проваливаетесь в глубокую яму заполненную водой!\nВы стремительно идете ко дну.",
               new List<string> { "Попытаться выбраться", "Звать на помощь" },
               new List<Action>
               { null}, new List<string> { "7.jpg" }

           ));
            forks.Add(new Fork(
               "Вы идете дальше.Поворачивая, вы встречаете странника, который предлагает вам выбор:",
               new List<string> { "Зелье", "Карта сокровищ", "Ничего не брать" },
               new List<Action>
               {new Action( "Зелье", PlayerAction.TakePotion),
                new Action("Карта сокровищ", PlayerAction.TakeTreasureMap)}, new List<string> { "8.jpg" }

           ));
            forks.Add(new Fork(
               "Идя дальше в темном коридоре лабиринта, теряется ощущение времени, кажется, что выход уже никогда не найти.\nПеред вами снова распутье: ",
               new List<string> { "Право", "Прямо", "Лево" },
               new List<Action>
               {null}, new List<string> { "1.jpg" }

           ));
            forks.Add(new Fork(
               "Неожиданно, вы видите перед собой маленькую девочку",
               new List<string> { "Заговорить", "Пройти мимо" },
               new List<Action>
               {new Action( "Заговорить", PlayerAction.Greet),
                new Action("Пройти мимо", PlayerAction.Ignore)}, new List<string> { "9.jpg" }

           ));
            forks.Add(new Fork(
               "Сделав пару шагов, вы улавливаете мерзкий запах и видите перед собой огромного змея!",
               new List<string> { "Ударить", "Пройти мимо" },
               new List<Action>
               {new Action( "Заговорить", PlayerAction.Hit),
                new Action("Пройти мимо", PlayerAction.Run)}, new List<string> { "10.jpg" }

           ));
            forks.Add(new Fork(
               "Уже совсем потеряв надежду на спасение, вы оказываетесь перед несколькими дверьми.",
               new List<string> { "Далее" },
               new List<Action>
               { }, new List<string> { "11.jpg" }

           ));
            forks.Add(new Fork(
              "Вы понимаете, что не собрали достаточно ключей, а по другому дверь не открыть.\nПридется выбирать из оставшихся дверей",
              new List<string> { "Правая", "Левая" },
              new List<Action> { }, new List<string> { "11.jpg" }

          ));
            forks.Add(new Fork(
              "\nИгра закончена! Начать сначала?",
              new List<string> { "Да", "Нет" },
              new List<Action> { }, new List<string> { "12.jpg" }

          ));



        }

        public void StartGame() //Запуск игры
        {

            LoadGame("save.json"); // Загружаем игру при старте
            uiUpdater.UpdateUI(CurrentForkIndex); //Обновляем интерфейс
            ProcessCurrentFork();
        }
        private void ResetGame() //Перезапуск игры
        {
            CurrentForkIndex = 0;

            // Удаление файла сохранения
            string saveFilePath = "save.json"; // Путь к файлу сохранения
            if (File.Exists(saveFilePath))
            {
                File.Delete(saveFilePath); // Удаление файла, если он существует

            }
            uiUpdater.SetMonsterGreeted(false); //Устанавливаем начальные состояния
            uiUpdater.SetMonsterHit(false);
            uiUpdater.SetMonsterRun(false);
            uiUpdater.SetTakeMap(false);
            uiUpdater.SetTakePotion(false);
            uiUpdater.SetIgnoreGirl(false);
            uiUpdater.SetTalkWithGirl(false);
            uiUpdater.SetHitSerpent(false);
            uiUpdater.SetRunBySerpent(false);
            uiUpdater.SetChooseRightDoor(false);

            // Пересоздание PlayerStats с начальными значениями
            InitializePlayerStats();

            // Перезапуск обработки развилок с начала
            ProcessCurrentFork();
        }


        private void ProcessCurrentFork() // Обрабатываем текущую развилку, отображая её описание и варианты выбора.
        {
            if (CurrentForkIndex < forks.Count) // Проверяем, не вышли ли за пределы списка развилок
            {
                {
                    var currentFork = forks[CurrentForkIndex]; //Текущая развилка
                  
                }
            }
            else
            {
                OnMessage?.Invoke("\nИгра завершена!"); //Если развилки закончились, завершаем игру
            }
        }

        public void HandlePlayerChoice(int choice) //Обработка выбора игрока
        {
            string message = ""; //Текст,описывающий выбор игрока
          
            // Обработка выбора игрока
            switch (CurrentForkIndex)
            {
                case 0:
                    OnMessage?.Invoke(message);
                    uiUpdater.UpdateMessage(message);
                    if (choice == 1)
                       CurrentForkIndex = 1;
                   

                    break;
                case 1: // Первая развилка
                    
                    switch (choice)
                    {
                        case 1:
                            message = "Вы подбираете меч. Выбран путь воина. В инвентарь добавлен меч.";
                           
                            PlayerStats.Inventory.Objects.Add("меч");
                            PlayerStats.Progression.ChoosedPath = "Путь воина";
                            OnMessage?.Invoke(message);
                            uiUpdater.UpdateMessage(message);

                          
                            break;
                        case 2:
                            message = "Вы подбираете загадочную коробку. Выбран путь стратега.\nВ инвентарь добавлена коробка.";
                            
                            PlayerStats.Inventory.Objects.Add("коробка");
                            OnMessage?.Invoke(message);
                            uiUpdater.UpdateMessage(message);
                            PlayerStats.Progression.ChoosedPath = "Путь стратега";
                            
                           
                            break;

                    }
                    CurrentForkIndex = 2;
                    break;

                case 2: 

                    switch (choice)
                    {
                        case 1:
                            message = "";
                            OnMessage?.Invoke(message);

                            CurrentForkIndex = 3; // Переход к третьей развилке
                            break;
                        case 2:
                            message = "";
                            OnMessage?.Invoke(message);

                            CurrentForkIndex = 4; // Переход к четвертой развилке
                            break;

                    }
                   
                    break;

                case 3: 
                   
                    BadMonster evilMonster = new BadMonster(uiUpdater);
                   
                    switch (choice)
                    {
                        case 1:
                            uiUpdater.SetMonsterRun(true); ////Устанавливаем состояние,что игрок убежал от монстра


                            break;
                        case 2:
                            uiUpdater.SetMonsterHit(true); //Устанавливаем состояние,что монстру был нанесен удар


                            break;
                        case 3:
                            uiUpdater.SetMonsterGreeted(true);//Устанавливаем состояние,что монстр был поприветствован


                            break;

                    }
                  
                    break;

                case 4: 
                    switch (choice)
                    {
                        case 1:
                            uiUpdater.SetMonsterHit(true); //Устанавливаем состояние,что монстру был нанесен удар
                            
                            break;
                        case 2:
                            uiUpdater.SetMonsterGreeted(true); //Устанавливаем состояние,что монстр был поприветствован 
                            PlayerStats.Flags.MeetGoodMonster = true;
                           
                            break;
                    }

                   
                    break;

                case 5: //Пятая развилка

                    switch (choice)
                    {
                        case 1:
                            message = "";
                            OnMessage?.Invoke(message);
                            uiUpdater.UpdateMessage(message);
                            CurrentForkIndex = 6;
                            break;
                        case 2:
                            message = "";
                            OnMessage?.Invoke(message);
                            uiUpdater.UpdateMessage(message);
                            CurrentForkIndex = 7;
                            break;

                    }
                    break;
                case 6: 

                    switch (choice)
                    {
                        case 1:
                            message = "Вы побежали за ним, и увидели перед собой мужчину в потрепанной одежде.\nОн говорит “Я уже долго брожу по этим коридорам. Берегись, не поворачивай налево,\n не повторяй моей ошибки” И уходит.";
                            OnMessage?.Invoke(message);
                            CurrentForkIndex = 8;
                            PlayerStats.Flags.MeetStranger = true;
                            break;
                        case 2:
                            message = "Вы думаете, что вам просто померещилось и идете дальше.";
                            OnMessage?.Invoke(message);
                            CurrentForkIndex = 8;
                            break;

                    }

                    break;
                case 7: 

                    switch (choice)
                    {
                        case 1:
                            if (PlayerStats.Progression.ChoosedPath == "Путь воина")
                            {
                                message = "Вашей выносливости можно позавидовать!\nВы изо всех сил гребёте и выбираетесь на поверхность.";
                                OnMessage?.Invoke(message);
                                CurrentForkIndex = 8;
                            }
                            else { OnMessage?.Invoke("Вы пытались выбраться, но ваши силы быстро иссякли.\n Вы полностью погрузились под воду.\nИгра окончена!"); CurrentForkIndex = 14; } //Плохой конец игры
                            break;
                        case 2:
                            message = "Вы зовете на помощь, хоть и понимаете, что никто не придёт.";
                            OnMessage?.Invoke(message);
                            if (PlayerStats.Flags.MeetGoodMonster == true) //Выход на развилку при определенном условии
                            {
                                message = "Вы слышите громкие шаги и видите монстра, встретившегося вам в самом начале пути!\nОн протягивает свою широкую лапу и вытаскивает вас.\nОн убегает, а вы очень радуетесь, что тогда не проявили враждебности…";
                                OnMessage?.Invoke(message); CurrentForkIndex = 8;
                            }
                            else
                            {
                                message = "Как и ожидалось, никто не пришел...\nВы полностью погрузились под воду.\nИгра окончена!";
                                OnMessage?.Invoke(message); CurrentForkIndex = 14;
                            }
                            break;

                    }
                    break;
                case 8:

                    Wanderer wanderer = new Wanderer(uiUpdater);
                    switch (choice)
                    {
                        case 1:
                            uiUpdater.SetTakePotion(true); //Устанавливаем состояни,что игрок взял зелье у странника
                           

                            break;
                        case 2:
                            uiUpdater.SetTakeMap(true); //Устанавливаем состояни,что игрок взял карту сокровищ у странника


                            break;
                        case 3:
                            message = "Вы решаете,что здесь лучше ничего не брать у незнакомцев и проходите мимо странника.";
                            OnMessage?.Invoke(message);
                            uiUpdater.UpdateMessage(message);   
                            CurrentForkIndex = 9;
                            break;



                    }
                    break;
                case 9:

                    switch (choice)
                    {
                        case 1:
                            message = "Вы идете направо.";
                            OnMessage?.Invoke(message);
                            uiUpdater.UpdateMessage(message);
                            CurrentForkIndex = 10;

                            break;
                        case 2:
                            message = "Вы замечаете, что уже видели похожие детали интерьера, и понимаете, что оказались вначале.\nПридется проходить всё сначала!";
                            OnMessage?.Invoke(message);
                            uiUpdater.UpdateMessage(message);
                            // Восстанавливаем здоровье игрока до 100
                            PlayerStats.Health.Health = 100;

                            // Сбрасываем количество ключей в инвентаре до 0
                            PlayerStats.Inventory.Keys = 0;

                            // Обновляем пользовательский интерфейс,до состояния начала
                            uiUpdater.UpdateUI(0);

                            // Устанавливаем индекс текущей развилки обратно на начало
                            CurrentForkIndex = 0;

                            // Сбрасываем состояния взаимодействия с  элементами игры
                            uiUpdater.SetMonsterGreeted(false);
                            uiUpdater.SetMonsterHit(false);
                            uiUpdater.SetMonsterRun(false);
                            uiUpdater.SetTakeMap(false);
                            uiUpdater.SetTakePotion(false);
                            uiUpdater.SetIgnoreGirl(false);
                            uiUpdater.SetTalkWithGirl(false);
                            uiUpdater.SetHitSerpent(false);
                            uiUpdater.SetRunBySerpent(false);
                            uiUpdater.SetChooseRightDoor(false);
                           

                            break;
                        case 3:
                            message = "Вы идете налево.";
                            OnMessage?.Invoke(message);
                            uiUpdater.UpdateMessage(message);
                            CurrentForkIndex = 11;
                            break;

                    }
                    break;
                case 10:
                    switch (choice)
                    {
                        case 1:
                            uiUpdater.SetTalkWithGirl(true); //Устанавливаем,что разговаривали с девочкой
                            break;
                        case 2:
                            uiUpdater.SetIgnoreGirl(true);//Устанавливаем,что проигнорировали девочку
                            break;

                    }
                    break;

                case 11:
                    if (PlayerStats.Flags.MeetStranger) { message = "Кажется кто-то предупреждал вас о том,что не стоит поворачивать налево...";  OnMessage.Invoke(message);
                        uiUpdater.UpdateMessage(message);
                    }
                    switch (choice)
                    {
                        case 1:
                            uiUpdater.SetHitSerpent(true); //Устанавливаем,что игрок ударил змея
                            if (PlayerStats.Health.Health <= 0)CurrentForkIndex = 14;
                            break;
                        case 2:
                            uiUpdater.SetRunBySerpent(true); //Устанавливаем,что игрок убежал от змея
                            if (PlayerStats.Health.Health <= 0) CurrentForkIndex = 14;
                            break;
                    }
                    break;
                case 12:
                    message = "Вы видите свет, проникающий сквозь щель центральной двери.Кажется там выход.\nНо на ней висит три замка...";
                    OnMessage.Invoke(message);
                    uiUpdater.UpdateMessage(message);

                    if (PlayerStats.Inventory.Keys == 3)
                    {
                        message+="В памяти всплывает, что вы как раз собрали три ключа,блуждая по лабиринту.\nС их помощью, замки поддаются, и вы, наконец, выходите на свободу!\nХороший конец.\n\n";
                        uiUpdater.SetHappyEnd(true);
                        OnMessage.Invoke(message);
                        uiUpdater.UpdateMessage(message);
                        CurrentForkIndex = 14;

                    }
                    else
                    {
                        CurrentForkIndex = 13;
                    }
                   
                    break;
                case 13:

                    switch (choice)
                    {
                        case 1:
                            uiUpdater.SetChooseRightDoor(true); //Устанавливаем,что игро выбрал правую дверь


                            break;
                        case 2:
                            message = "Вы решаете выбрать левую дверь. Вы оказываетесь в комнате,в которой видите еще одну дверь, ведущую на свободу\n" +
                            "Но комната кишит монстрами. И чтобы добраться до  двери вам надо уничтожить их всех.";
                            
                            if (PlayerStats.Inventory.Objects.Contains("меч") && PlayerStats.Health.Health >= 110)
                            {
                                message += "\nВы храбро сражались с монстрами и одержали победу!Путь к двери свободен и вы выходите из лабиринта.\nХороший конец.\n\n";
                                uiUpdater.SetHappyEnd(true);
                                CurrentForkIndex = 14;
                            }
                            else
                            {
                                message += "\nК сожалению, вы оказались бессильны перед таким количеством монстров...\nПлохой конец.\n";
                                CurrentForkIndex = 14;
                            }
                            OnMessage.Invoke(message);

                            uiUpdater.UpdateMessage(message);
                            break;

                    }
                    break;
                case 14:

                    switch (choice)
                    {
                        case 1:
                            OnMessage?.Invoke("Вы решили переиграть. Начинаем игру заново!\n");
                            uiUpdater.UpdateMessage(message);
                            ResetGame(); //Перезапуск игры
                            break;
                        case 2:
                            uiUpdater.CloseGame(); //Закрываем игру
                            break;

                    }
                    break;
            }
            

            ProcessCurrentFork(); // Обработка следующей развилки
            uiUpdater.UpdateUI(CurrentForkIndex); //Обновление интерфейса

        }
        



        public void SaveGame(string filePath) //Сохранение игры
        {
            // Создаем объект GameState для сохранения текущего состояния игры
            var gameState = new GameState
            {
                CurrentForkIndex = CurrentForkIndex,
                Forks = forks
            };
            //Сериализуем объект в JSON формат
            string json = JsonConvert.SerializeObject(gameState, Formatting.Indented);
            File.WriteAllText(filePath, json);
            OnMessage?.Invoke("Игра была сохранена"); // Выводим сообщение об успешном сохранении
        }



        public void LoadGame(string filePath) //Загрузка состояния игры 
        {
            // Проверяем, существует ли файл сохранения и не пуст ли он
            if (File.Exists(filePath) && new FileInfo(filePath).Length > 0)
            {
                // Читаем JSON из файла и десериализуем в объект GameState
                string json = File.ReadAllText(filePath);
                var gameState = JsonConvert.DeserializeObject<GameState>(json);

                // Восстанавливаем состояние игры из объекта GameState
                CurrentForkIndex = gameState.CurrentForkIndex; // Восстанавливаем индекс текущей развилки
                forks = gameState.Forks; // Восстанавливаем список развилок

                OnMessage?.Invoke("Игра загружена."); // Выводим сообщение об успешной загрузке
            }
            else
            {
                OnMessage?.Invoke("Начинаем новую игру."); // Выводим сообщение о начале новой игры, если сохранения нет
                ProcessCurrentFork(); // Начинаем новую игру
            }
        }

      
        
        // Вспомогательный класс для хранения состояния игры.
        public partial class GameState
        {
            public int CurrentForkIndex { get; set; } // Индекс текущей развилки
            public List<Fork> Forks { get; set; } // Список развилок

        }


    }
}

