using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Лаба3;
using static Лаба3.GameManager;
namespace Laboratory_Work_4
{
  
    public partial class MainWindow : Window, IUIUpdater
    {
        // Переменные состояния для отслеживания различных действий и событий в игре
        private bool monsterGreeted = false;
        private bool monsterHit = false;
        private bool monsterRun = false;
        private bool takeMap = false;
        private bool takePotion = false;
        private bool ignoreGirl = false;
        private bool talkWithGirl = false;
        private bool hitSerpent = false;
        private bool runBySerpent = false;
        private bool chooseRightDoor = false;
        bool happyEnd = false;  
        bool puzzleSolved = false;
        int attempts = 2;
        private GameManager gameManager; // Ссылка на менеджер игры


        public MainWindow()
        {
            InitializeComponent(); // Инициализация компонентов интерфейса

            // Скрытие кнопок выбора и текстовых полей при запуске
            ButtonChoice1.Visibility = Visibility.Collapsed;
            ButtonChoice2.Visibility = Visibility.Collapsed;
            TextBox.Visibility = Visibility.Collapsed;
            TextBox2.Visibility = Visibility.Collapsed;
            TextBox3.Visibility = Visibility.Collapsed;
            gameManager = new GameManager(this);
            // Создание нового экземпляра GameManager и передача ссылки на текущий экземпляр MainWindow
            this.DataContext = gameManager; // Установка контекста данных для привязки данных к интерфейсу

            gameManager.OnMessage += UpdateMessage; // Подписка на событие OnMessage для обновления сообщений в интерфейсе

            UpdateUI(0); // Обновление интерфейса с начальным состоянием 
        }

       

        public  void UpdateMessage(string message)
        {

            // Обновление TextBlockMessage для отображения сообщения
            textBlockMessage.Text = message; 
        }
        public void UpdateUI(int currentForkIndex)
        {
             
            TextBlockKeys.Text = PlayerStats.Inventory.Keys.ToString(); //Отображение количества ключей в инвентаре
            TextBlockHealth.Text = PlayerStats.Health.Health.ToString(); //Отображение текущего состояния здоровья
            ButtonChoice1.Visibility = Visibility.Collapsed; //Скрытие кнопок и текста при обновлении развилки
            ButtonChoice2.Visibility = Visibility.Collapsed;
            TextBox.Visibility = Visibility.Collapsed;
            TextBox2.Visibility = Visibility.Collapsed;
            TextBox3.Visibility = Visibility.Collapsed;
            Button1.Visibility = Visibility.Visible;
            Button2.Visibility = Visibility.Visible;
            // Обновление TextBlock для отображения описания развилки
             textBlockDescription.Text = gameManager.Forks[currentForkIndex].Description;
            // В зависимости от значения currentForkIndex отображаем соответствующие кнопки и изображения
            switch (currentForkIndex)
            {
                case 0:
                    ShowForkButtons(0);
                    ShowOneImage(0);
                    break;
                case 1:
                   
                    ShowForkButtons(1);
                    ShowImages(1);
                    break;
                case 2:
                   
                    ShowForkButtons(2);
                    ShowOneImage(2);
                    break;
                case 3:
                    
                    ShowForkButtons(3);
                    ShowOneImage(3);
                    break;
                case 4:
                   
                    ShowForkButtons(4);
                    ShowOneImage(4);
                    // Показываем кнопки выбора только если монстр был поприветствован
                    if (monsterGreeted)
                    {

                        ButtonChoice1.Visibility = Visibility.Visible;
                        ButtonChoice2.Visibility = Visibility.Visible;
                        ButtonChoice1.Content = "За солнце";
                        ButtonChoice2.Content = "За луну.Ведь солнце светит\nднем когда итак светло";
                        Button3.Visibility = Visibility.Collapsed;
                        Button2.Visibility = Visibility.Collapsed;
                    }
                    else
                    {
                        ButtonChoice1.Visibility = Visibility.Collapsed; // Скрываем кнопки
                        ButtonChoice2.Visibility = Visibility.Collapsed; // Скрываем кнопки
                    }
                    break;
                case 5:
                    ShowForkButtons(5);
                    ShowOneImage(5);
                    break;
                case 6:
                    ShowForkButtons(6);
                    ShowOneImage(6);
                    break;
                case 7:
                    ShowForkButtons(7);
                    ShowOneImage(7);
                    break;
                case 8:
                    ShowForkButtons(8);
                    ShowOneImage(8);
                    if (takeMap) //Отображаем элементы интерфейса только если игрок решил взять карту
                    {
                        TextBox.Visibility = Visibility.Visible;
                        TextBox.Text = "Место для вашего ответа";
                        ButtonChoice2.Visibility = Visibility.Visible;
                        ButtonChoice2.Content = "Ответить";
                        Button1.Visibility = Visibility.Collapsed;
                        Button2.Visibility = Visibility.Collapsed;
                        Button3.Visibility = Visibility.Collapsed;
                    }
                    else
                    {
                        TextBox.Visibility = Visibility.Collapsed;
                        ButtonChoice2.Visibility = Visibility.Collapsed; 
                    }
                    break;
                case 9:
                    ShowForkButtons(9);
                    ShowOneImage(9);
                    break;
                case 10:
                    ShowForkButtons(10);
                    ShowOneImage(10);
                    if (talkWithGirl) //Отображаем элементы интерфейса только если игрок решил поговорить с девочкой
                    {
                        TextBox2.Visibility = Visibility.Visible;
                        TextBox2.Text = "Место для вашего ответа";
                        ButtonChoice2.Visibility = Visibility.Visible;
                        ButtonChoice2.Content = "Ответить";
                        Button1.Visibility = Visibility.Collapsed;
                        Button2.Visibility = Visibility.Collapsed;
                        Button3.Visibility = Visibility.Collapsed;
                    }
                    else
                    {
                        TextBox.Visibility = Visibility.Collapsed;
                        ButtonChoice2.Visibility = Visibility.Collapsed;
                    }
                    break;
                   
                case 11:
                    ShowForkButtons(11);
                    ShowOneImage(11);
                    break;
                case 12:
                    ShowForkButtons(12);
                    ShowOneImage(12);
                    break;
                case 13:
                    ShowForkButtons(13);
                    ShowOneImage(13);
                    break;
                case 14:
                    ShowForkButtons(14);
                    if (happyEnd) //Если вышли на хорошую концовку
                    {
                        imagesPanel.Children.Clear(); UpdateImage($"pack://application:,,,/Images/13.jpg", 350, 300); //Отображаем соответствующее изображание
                    }
                    else ShowOneImage(14);
                    break;

            }
        }

        private void ShowForkButtons(int index) //Функция показа кнопок
        {
            Button1.Visibility = Visibility.Collapsed;
            Button2.Visibility = Visibility.Collapsed;
            Button3.Visibility = Visibility.Collapsed;
            switch (gameManager.Forks[index].Options.Count) //Отображаем кнопки взависимости от их количества в развилке
            {
                case 1:
                    Button2.Visibility = Visibility.Visible;
                    Button2.Content = gameManager.Forks[index].Options[0];
                    break;
                case 2:
                    Button3.Visibility = Visibility.Visible;
                    Button2.Visibility = Visibility.Visible;
                    Button2.Content = gameManager.Forks[index].Options[0];
                    Button3.Content = gameManager.Forks[index].Options[1];
                    break;
                case 3:
                    Button3.Visibility = Visibility.Visible;
                    Button2.Visibility = Visibility.Visible;
                    Button1.Visibility = Visibility.Visible;
                    Button2.Content = gameManager.Forks[index].Options[1];
                    Button3.Content = gameManager.Forks[index].Options[2];
                    Button1.Content = gameManager.Forks[index].Options[0];
                    break;

            }
          

        }
        private void ShowOneImage(int index) //Отображение одного изображения
        {
            imagesPanel.Children.Clear(); // Очистка предыдущих изображений
            var imageName = gameManager.Forks[index].ImageNames[0];
            UpdateImage($"pack://application:,,,/Images/{imageName}", 500, 320);

        }


        private void ShowImages(int index) //Отображение нескольких изображений
        {
            imagesPanel.Children.Clear(); // Очистка предыдущих изображений

            // Пример создания кнопок для первой развилки
            foreach (var imageName in gameManager.Forks[index].ImageNames) //Показываем все изображения в развилке
            {
                UpdateImage($"pack://application:,,,/Images/{imageName}", 350, 300);
            }
        }



        private void UpdateImage(string imagePath, int width, int height) //Добавление изображения
        {

            Image image = new Image
            {
                Source = new BitmapImage(new Uri(imagePath)),
                Width = width,
                Height = height,
                Stretch = Stretch.Fill,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(9)
            };

            // Добавление изображения в панель
            imagesPanel.Children.Add(image);
        }




        private void Save_Click(object sender, RoutedEventArgs e) //Реакция на нажатие кнопки сохранения
        {
            gameManager.SaveGame("save.json");
        }

        private void Load_Click(object sender, RoutedEventArgs e) //Реакция на нажатие кнопки загрузки
        {

            gameManager.LoadGame("save.json");
            UpdateUI(gameManager.CurrentForkIndex);
        }

        private void Button1_Click(object sender, RoutedEventArgs e) 
        {
           
            gameManager.HandlePlayerChoice(1);

            switch (gameManager.CurrentForkIndex) //В зависимости от развилки
            {
                case 3: 
                    HandleMonsterRun(); // вызываем метод HandleMonsterRun, который обрабатывает ситуацию с монстром.
                    break;
                case 8:
                    HandleTakePotion(); // вызываем метод, который обрабатывает ситуацию со взятием зелья у странника
                    break;

            }
        }

    
        private void Button2_Click(object sender, RoutedEventArgs e)
        {
          


            switch (gameManager.Forks[gameManager.CurrentForkIndex].Options.Count)
            {
                case 1:
                    gameManager.HandlePlayerChoice(1);
                    break;
                case 2:
                    gameManager.HandlePlayerChoice(1);
                    break;
                case 3:
                    gameManager.HandlePlayerChoice(2);
                    break;


            }
           
            switch (gameManager.CurrentForkIndex)
            {
                
                case 3:
                    HandleMonsterEncounter();
                    break;
                case 4:
                    HandleMonsterEncounter();
                    
                    break;
                case 8:
                    HandleWandererEncounter();
                    break;
                case 10:
                    HandleGirlEncounter();
                    break;
                case 11:
                    HandleSerpentEncounter();
                    break;
                case 13:
                    HandleDoorPuzzle();
                    break;
            }
}

        private void Button3_Click(object sender, RoutedEventArgs e)
        {
           
            switch (gameManager.Forks[gameManager.CurrentForkIndex].Options.Count)
            {
                case 2:
                    gameManager.HandlePlayerChoice(2);
                    break;
                case 3:
                    gameManager.HandlePlayerChoice(3);
                    break;
            }
            switch (gameManager.CurrentForkIndex)
            {
                case 3:
                    HandleBadMonsterEncounter();
                    break;
                case 4:
                    
                    HandleGoodMonsterGreet();
                   
                    break;
                case 10:

                    HandleGirlEncounter();
                    break;
                case 11:
                    HandleSerpentEncounter();
                    break;

            }
        }




        private void ButtonChoice2_Click(object sender, RoutedEventArgs e)
        {
           

            switch (gameManager.CurrentForkIndex)
            {
                case 4:
                    
                    HandleGoodMonsterQuestion();
                    break;

                case 8:
                    HandleWandererMap();
                    break;

                case 10:
                    HandleGirlAnswer();
                    break;

                case 13:
                    HandlePuzzleEncounter();
                    break;
            }
           
        }
  
        

        private void ButtonChoice1_Click(object sender, RoutedEventArgs e)
        {
            GoodMonster goodMonster = new GoodMonster(this);
            goodMonster.Question(2);
            gameManager.CurrentForkIndex = 5;
            UpdateUI(5);

        }
        private void HandleMonsterRun()
        {
            if (monsterRun)
            {
                Monster badMonster = new BadMonster(this);
                badMonster.ReactToAction(PlayerAction.Run);
                gameManager.CurrentForkIndex = 5;
                UpdateUI(5);
            }
        }

        private void HandleTakePotion()
        {
            if (takePotion)
            {
                Wanderer wanderer = new Wanderer(this);
                wanderer.ReactToAction(PlayerAction.TakePotion);
                gameManager.CurrentForkIndex = 9;
                UpdateUI(9);
            }
        }
        private void HandleMonsterEncounter()
        {
            Monster monster = null;
            switch (gameManager.CurrentForkIndex)
            {
                case 3:
                    monster = new BadMonster(this);
                    break;
                case 4: 
                    monster = new GoodMonster(this);
                    break;
            }
            if (monster != null && monsterHit)
            {
                monster.ReactToAction(PlayerAction.Hit);
                gameManager.CurrentForkIndex = 5;
                UpdateUI(5);
            }
        }
        private void HandleGoodMonsterGreet()
        {
            if (gameManager.CurrentForkIndex == 4 && monsterGreeted)
            {
                GoodMonster goodMonster = new GoodMonster(this);
                goodMonster.ReactToAction(PlayerAction.Greet);
                textBlockDescription.Text = "";
            }
        }
        private void HandleGoodMonsterQuestion()
        {
            textBlockDescription.Text = "";
            GoodMonster goodMonster = new GoodMonster(this);
            goodMonster.Question(1);
            gameManager.CurrentForkIndex = 5;
            UpdateUI(5);
        }


        private void HandleBadMonsterEncounter()
        {
            if (gameManager.CurrentForkIndex == 3 && monsterGreeted)
            {
                Monster badMonster = new BadMonster(this);
                badMonster.ReactToAction(PlayerAction.Greet);
                gameManager.CurrentForkIndex = 5;
                UpdateUI(5);
            }
        }
        private void HandleWandererEncounter()
        {
           
            if (takeMap)
            {
                Wanderer wanderer = new Wanderer(this);
                wanderer.ReactToAction(PlayerAction.TakeTreasureMap);
                textBlockDescription.Text = "";
            }
            
        }
        private void HandleWandererMap()
        {
            textBlockDescription.Text = "";
            Wanderer wanderer = new Wanderer(this);
            int maxAttempts = 1;
            bool mapGiven = false;

            if (!mapGiven)
            {
                string answer = TextBox.Text; // Получаем ответ от пользователя
                mapGiven = wanderer.HandleRiddle(maxAttempts, answer, attempts); // Обрабатываем загадку
                gameManager.CurrentForkIndex = 9; // Переход к следующей развилке
                UpdateUI(9);
            }
        }


        private void HandleGirlEncounter()
        {
            Girl girl = new Girl(this);
            if (talkWithGirl)
            {
              
                girl.ReactToAction(PlayerAction.Greet);
                textBlockDescription.Text = "";
            }
            if (ignoreGirl)
            {
                
                girl.ReactToAction(PlayerAction.Ignore);
                gameManager.CurrentForkIndex = 12;
                UpdateUI(12);
            }
        }
        private void HandleGirlAnswer()
        {
            textBlockDescription.Text = "";
            Girl girl = new Girl(this);
            string answer = TextBox2.Text;
            girl.HandleAnswer(answer);
            gameManager.CurrentForkIndex = 12;
            UpdateUI(12);
        }

        private void HandleSerpentEncounter()
        {
            Serpent serpent = new Serpent(this);
            if (hitSerpent)
            {
                textBlockDescription.Text = "";
               
                serpent.ReactToAction(PlayerAction.Hit);

                if (PlayerStats.Health.Health <= 0)
                {
                    gameManager.CurrentForkIndex = 14;
                    UpdateUI(14);
                }
                else
                {
                    gameManager.CurrentForkIndex = 12;
                    UpdateUI(12);
                }
            }
            if (runBySerpent)
            {
                serpent.ReactToAction(PlayerAction.Run);

                if (PlayerStats.Health.Health <= 0)
                {
                    gameManager.CurrentForkIndex = 14;
                    UpdateUI(14);
                }
                else
                {
                    gameManager.CurrentForkIndex = 12;
                    UpdateUI(12);
                }
            }
        }

        private void HandleDoorPuzzle()
        {
            if (chooseRightDoor)
            {
                textBlockDescription.Text = "";
                TextBox3.Visibility = Visibility.Visible;
                TextBox3.Text = "Место для вашего ответа";
                ButtonChoice2.Visibility = Visibility.Visible;
                ButtonChoice2.Content = "Ответить";
                Button1.Visibility = Visibility.Collapsed;
                Button2.Visibility = Visibility.Collapsed;
                Button3.Visibility = Visibility.Collapsed;

                attempts = 2; // Количество попыток для решения головоломки

                string message = "Вы решаете выбрать правую дверь.\nВы оказываетесь в комнате, в которой видите еще одну дверь, ведущую на свободу." +
                                 " Но она закрыта,\n а чтобы открыть ее, придется решить еще одну головоломку." +
                                 " Найдите корни квадратного уравнения: х^2 - 2х - 35 = 0\nВведите первый корень(через пробел второй)\n";

                UpdateMessage(message);

                if (PlayerStats.Inventory.Objects.Contains("коробка"))
                {
                    message += "Вы вспоминаете, что у вас еще осталась загадочная коробка.Открыв ее, вы видите на ее дне подсказку:Дискриминант равен 144. Один из корней равен 7.";
                    UpdateMessage(message);
                }
            }
        }
        private void HandlePuzzleEncounter()
        {
            textBlockDescription.Text = "";
            if (attempts > 0 && !puzzleSolved) // Цикл для попыток решения головоломки
            {
                ProcessPuzzleInput();
            }

            // Если головоломка не решена
            if (!puzzleSolved && attempts == 0)
            {
                UpdateMessage("К сожалению, все попытки исчерпаны. Дверь так и остается закрытой, а вы запертым в лабиринте...\nПлохой конец.\n");
                gameManager.CurrentForkIndex = 14; // Переход к следующей развилке
                UpdateUI(14); // Обновление интерфейса
            }
        }

        private void ProcessPuzzleInput()
        {
            string playerInput = TextBox3.Text; // Получение ввода игрока
            string message;

            if (string.IsNullOrEmpty(playerInput)) // Проверка, что ввод не пустой
            {
                message = "Пожалуйста, введите два корня через пробел.";
                UpdateMessage(message);
                return;
            }

            string[] parts = playerInput.Split(' '); // Разбиение ввода на части по пробелу
            if (parts.Length != 2) // Проверка, что введено два числа
            {
                message = "Пожалуйста, введите два корня через пробел."; // Сообщение об ошибке ввода
                UpdateMessage(message);
                return;
            }

            // Попытка преобразовать ввод в числа
            if (int.TryParse(parts[0], out int root1) && int.TryParse(parts[1], out int root2))
            {
                // Проверка правильности ответа
                if ((root1 == 7 && root2 == -5) || (root1 == -5 && root2 == 7))
                {
                    message = "Правильно! Дверь открывается, и вы видите долгожданный выход!\nХороший конец.\n";
                    UpdateMessage(message); // Сообщение о правильном ответе
                    puzzleSolved = true; // Установка флага решения головоломки
                    happyEnd = true;
                    gameManager.CurrentForkIndex = 14; // Переход к следующей развилке
                    UpdateUI(14); // Обновление интерфейса
                }
                else
                {
                    message = $"Ответ неверный. Попробуйте снова\nПопыток осталось: {attempts}"; // Сообщение о неверном ответе
                    UpdateMessage(message);
                }
                attempts--; // Уменьшение количества попыток
            }
            else
            {
                message = "Неверный формат ввода. Пожалуйста, введите целые числа."; // Сообщение об ошибке формата ввода
                UpdateMessage(message);
            }
        }
        public void SetMonsterGreeted(bool flag)
        {
            monsterGreeted = flag; // Устанавливаем состояние
        }
        public void SetMonsterHit(bool flag)
        {
            monsterHit = flag; // Устанавливаем состояние
        }
        public void SetMonsterRun(bool flag)
        {
            monsterRun = flag; // Устанавливаем состояние
        }
       
        public void SetTakeMap(bool flag)
        {
            takeMap = flag;  
        }
        public void SetTakePotion(bool flag)
        {
            takePotion = flag;    
        }
        public void SetIgnoreGirl(bool flag)
        {
            ignoreGirl = flag;
        }
        public void SetTalkWithGirl(bool flag)
        {
            talkWithGirl = flag;
        }
        public void SetHitSerpent(bool flag)
        {
            hitSerpent = flag;
        }
        public void SetRunBySerpent(bool flag)
        {
            runBySerpent = flag;
        }
        public void SetChooseRightDoor(bool flag)
        {
           chooseRightDoor = flag;
        }
        public void SetHappyEnd(bool flag)
        {
            happyEnd = flag;
        }

        public void CloseGame() //Закрытие игры
        {
            this.Close();
        }
      
        
    }

}
