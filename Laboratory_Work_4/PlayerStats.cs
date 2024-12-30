using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Лаба3
{

    // Интерфейс, определяющий статистику здоровья игрока.
    public interface IHealthStats
    {
        int Health { get; set; } //Текущее значение здоровье
        void AddHealth(int amount); //Прибавление здоровья
        void RemoveHealth(int amount); //Уменьшение здоровья
    }

    // Интерфейс, определяющий состояние инвентаря игрока.
    public interface IInventory
    {
        int Keys { get; set; } //Количество собранных ключей
        List<string> Objects { get; set; } //Список собранных объектов
        void AddObject(string obj); //Добавление объекта
        void RemoveObject(string obj); //Удаление объекта
        void AddKeys(int keys); //Добавление ключей
    }

    // Интерфейс, определяющий статистику прогресса персонажа.
    public interface IProgressionStats
    {
        string ChoosedPath { get; set; } //Выбранный путь

    }

    // Интерфейс, определяющий игровые флаги.
    public interface IGameFlags
    {
        // Флаг, указывающий, встретил ли игрок хорошего монстра.
        bool MeetGoodMonster { get; set; }
        // Флаг, указывающий, встретил ли игрок незнакомца.
        bool MeetStranger { get; set; }
    }
    public class HealthStats : IHealthStats // Класс, реализующий статистику здоровья персонажа.
    {
        public int Health { get; set; }
        public HealthStats(int health) // Конструктор класса HealthStats.
        {
            Health = health;
        }

        public void AddHealth(int amount) // Увеличение здоровья персонажа на заданную величину.
        {
            Health += amount;
            
        }
        public void RemoveHealth(int amount) // Уменьшение здоровья персонажа на заданную величину.
        {
            Health -= amount;
           
        }

    }
    public class Inventory : IInventory
    {
        // Количество ключей в инвентаре.
        public int Keys { get; set; }

        // Список объектов в инвентаре.
        public List<string> Objects { get; set; } = new List<string>();

        // Конструктор класса Inventory.
        public Inventory(int keys)
        {
            Keys = keys;
        }

        // Добавляет объект в инвентарь.
        public void AddObject(string obj)
        {
            Objects.Add(obj);
            
        }

        // Удаляет объект из инвентаря.
        public void RemoveObject(string obj)
        {
            Objects.Remove(obj);
            
        }

        // Добавляет ключи в инвентарь.
        public void AddKeys(int keys)
        {
           
            Keys += keys;
           
        }
    }
    public class ProgressionStats : IProgressionStats //Класс,реализующий прогресс игрока
    {
        public string ChoosedPath { get; set; } //Выбранный путь
     
    }
    public class GameFlags : IGameFlags //Класс,реализующий игровые флаги
    {
        public bool MeetGoodMonster { get; set; } //Встреча с хорошим монстром
        public bool MeetStranger { get; set; } //Встреча с незнакомцем
    }
    public  partial class PlayerStats //Содержит параметры игрока
    {
        public static IHealthStats Health { get; set; } //Здоровье
        public static IInventory Inventory { get; set; } //Инвентарь
        public static IProgressionStats Progression { get; set; } //Прогресс
        public static IGameFlags Flags { get; set; } //Флаги состояния игрока
        
        public PlayerStats(IHealthStats health, IInventory inventory, IProgressionStats progression, IGameFlags flags) //Конструктор с параметрами
        {
            Health = health;
            Inventory = inventory;
            Progression = progression;
            Flags = flags;
           
        }
    }
    // Действия игрока.
    public enum PlayerAction
    {
        // Нанести удар.
        Hit,

        // Поприветствовать.
        Greet,

        // Убежать.
        Run,

        // Взять зелье
        TakePotion,

        // Взять карту сокровищ.
        TakeTreasureMap,

        // Проигнорировать.
        Ignore
    }
}

