using System;
using System.Globalization;
using System.IO;
using System.Diagnostics;
using System.Xml;
using System.Xml.Serialization;

namespace phone_book
{
    /// <summary>
    /// Абстрактный класс телефонной книги
    /// </summary>
    [XmlInclude(typeof(person))]
    [XmlInclude(typeof(friend))]
    [XmlInclude(typeof(organization))]
    [Serializable]
    public abstract class phonebook
    {
        /// <summary>
        /// Имя
        /// </summary>
        public string name;

        /// <summary>
        /// Адрес
        /// </summary>
        public string address;

        /// <summary>
        /// Номер телефона
        /// </summary>
        public string phonenumber;

        /// <summary>
        /// Конструктор класса
        /// </summary>
        /// <param name="name">Имя</param>
        /// <param name="address">Адрес</param>
        /// <param name="phonenumber">Номер телефона</param>
        public phonebook(string name, string address, string phonenumber)
        {
            this.name = name;
            this.address = address;
            this.phonenumber = phonenumber;
        }

        public phonebook() { }

        /// <summary>
        /// Проверка на совпадение по именги
        /// </summary>
        /// <param name="name"></param>
        /// <returns>Булево значение: совпадает ли имя?</returns>
        public bool CheckName(string name)
        {
            Trace.WriteLine("phonebook.CheckName");
            return name == this.name;
        }

        /// <summary>
        /// Метод вывода информации о записи на экран
        /// </summary>
        public abstract void Display();
    }

    /// <summary>
    /// Класс для персоны
    /// </summary>
    [Serializable]
    public sealed class person : phonebook
    {

        /// <summary>
        /// Конструктор класса
        /// </summary>
        /// <param name="name">Имя</param>
        /// <param name="address">Адрес</param>
        /// <param name="phonenumber">Номер телефона</param>
        public person(string name, string address, string phonenumber) : base(name, address, phonenumber)
        {
            Trace.WriteLine("person.person");
        }

        public person() { }

        /// <summary>
        /// Метод выводит информацию о персоне на экран
        /// </summary>
        public sealed override void Display()
        {
            Trace.WriteLine("person.Display");
            Console.WriteLine("Персона " + this.name + ":");
            Console.WriteLine("    Адрес: " + this.address);
            Console.WriteLine("    Номер телефона: " + this.phonenumber);
        }
    }

    /// <summary>
    /// Класс для друга
    /// </summary>
    [Serializable]
    public sealed class friend : phonebook
    {

        /// <summary>
        /// День рождения
        /// </summary>
        public string birthday;


        /// <summary>
        /// Конструктор класса
        /// </summary>
        /// <param name="name">Имя</param>
        /// <param name="address">Адрес</param>
        /// <param name="phonenumber">Номер телефона</param>
        /// <param name="birthday">День рождения</param>
        public friend(string name, string address, string phonenumber, string birthday) : base(name, address, phonenumber)
        {
            Trace.WriteLine("friend.friend");
            this.birthday = birthday;
        }

        public friend() { }

        /// <summary>
        /// Метод выводит информацию о друге на экран
        /// </summary>
        public sealed override void Display()
        {
            Trace.WriteLine("friend.Display");
            Console.WriteLine("Друг " + this.name + ":");
            Console.WriteLine("    Адрес: " + this.address);
            Console.WriteLine("    Номер телефона: " + this.phonenumber);
            Console.WriteLine("    День рождения: " + this.birthday);
        }
    }

    /// <summary>
    /// Класс организациии
    /// </summary>
    [Serializable]
    public sealed class organization : phonebook
    {
        /// <summary>
        /// Факс
        /// </summary>
        public string fax;

        /// <summary>
        /// Контактное лицо
        /// </summary>
        public string contact;


        /// <summary>
        /// Конструктор класса
        /// </summary>
        /// <param name="name">Имя</param>
        /// <param name="address">Адрес</param>
        /// <param name="phonenumber">Номер телефона</param>
        /// <param name="fax">Факс</param>
        /// /// <param name="contact">Контактное лицо</param>
        public organization(string name, string address, string phonenumber, string fax, string contact) : base(name, address, phonenumber)
        {
            Trace.WriteLine("organization.organization");
            this.fax = fax;
            this.contact = contact;
        }

        public organization() { }

        /// <summary>
        /// Метод выводит информацию об организации на экран
        /// </summary>
        public sealed override void Display()
        {
            Trace.WriteLine("organization.Display");
            Console.WriteLine("Организация \"" + this.name + "\":");
            Console.WriteLine("    Адрес: " + this.address);
            Console.WriteLine("    Номер телефона: " + this.phonenumber);
            Console.WriteLine("    Факс: " + this.fax);
            Console.WriteLine("    Контактное лицо: " + this.contact);
        }
    }

    class Program
    {
        /// <summary>
        /// Загружает базу из файла
        /// </summary>
        /// <param name="fileName">Путь или имя файла</param>
        /// <returns>Массив записей</returns>
        private static phonebook[] ImportFromFile(string fileName)
        {
            Trace.WriteLine("Program.ImportFromFile");
            try
            {
                string[] lines = File.ReadAllLines(fileName);
                int phonebookCount = int.Parse(lines[0]);
                phonebook[] phonebooks = new phonebook[phonebookCount];
                for (int i = 0; i < phonebookCount; i++)
                {
                    string phonebookType = lines[i + 1].Split(':')[0].Trim();
                    string[] args = lines[i + 1].Split(':')[1].Split(',');
                    switch (phonebookType)
                    {
                        case "person":
                            phonebooks[i] = new person(args[0].Trim(), args[1].Trim(), args[2].Trim());
                            break;
                        case "friend":
                            phonebooks[i] = new friend(args[0].Trim(), args[1].Trim(), args[2].Trim(), args[3].Trim());
                            break;
                        case "organization":
                            phonebooks[i] = new organization(args[0].Trim(), args[1].Trim(), args[2].Trim(), args[3].Trim(), args[4].Trim());
                            break;

                        default:
                            break;
                    }
                }
                return phonebooks;
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("Файл не найден");
                return new phonebook[] { };
            }
        }

        /// <summary>
        /// Выводит все записи в консоль
        /// </summary>
        /// <param name="phonebooks">Массив записей</param>
        private static void DisplayAll(phonebook[] phonebooks)
        {
            Trace.WriteLine("Program.DisplayAll");
            if (phonebooks.Length == 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Нет ни одной записи");
                Console.ResetColor();
                return;
            }
            foreach (var phonebook in phonebooks)
            {
                phonebook.Display();
            }
        }

        /// <summary>
        /// Поиск по имени
        /// </summary>
        /// <param name="phonebooks">Массив записей</param>
        /// <param name="searchName">Имя для поиска</param>
        private static void SearchByName(phonebook[] phonebooks, string searchName)
        {
            Trace.WriteLine("Program.SearchByName");
            bool z = true;
            foreach (var phonebook in phonebooks)
            {
                if (phonebook.CheckName(searchName))
                {
                    phonebook.Display();
                    z = false;
                }
            }
            if (z)
            {
                Console.WriteLine("Поиск не дал результатов");
            }
        }

        /// <summary>
        /// Сценарий импорта из фйла
        /// </summary>
        /// <returns>Массив записей</returns>
        static phonebook[] ShowImportFromFile()
        {
            Trace.WriteLine("Program.ShowImportFromFile");
            Console.Write("Введите имя файла для импорта: ");
            string fileName = Console.ReadLine();
            return ImportFromFile(fileName);
        }

        /// <summary>
        /// Показать список доступных команд
        /// </summary>
        static void ShowHelp()
        {
            Trace.WriteLine("Program.ShowHelp");
            Console.WriteLine("Телефонная книга:");
            Console.WriteLine("    Помощь - help");
            Console.WriteLine("    Вывести все записи - list");
            Console.WriteLine("    Поиск по имени - find");
            Console.WriteLine("    Вывести базу данных в xml - xml");
            Console.WriteLine("    Сохранить базу данных в xml файл - drop");
            Console.WriteLine("    Очистить консоль - clear");
            Console.WriteLine("    Выход - exit");
        }

        /// <summary>
        /// Показать все записи
        /// </summary>
        /// <param name="phonebooks">Массив записей</param>
        static void Showphonebooks(phonebook[] phonebooks)
        {
            Trace.WriteLine("Program.Showphonebooks");
            DisplayAll(phonebooks);
        }

        /// <summary>
        /// Показать сценарий поиска по имени
        /// </summary>
        /// <param name="phonebooks">Массив записей</param>
        static void ShowSearch(phonebook[] phonebooks)
        {
            Trace.WriteLine("Program.ShowSearch");
            Console.Write("Поиск по имени: ");
            string searchString = Console.ReadLine();
            SearchByName(phonebooks, searchString);
        }

        /// <summary>
        /// Основная логика программы + CLI
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            Trace.WriteLine("Program.Main");
            phonebook[] phonebooks = ShowImportFromFile();
            XmlSerializer serializer = new XmlSerializer(typeof(phonebook[]));
            ShowHelp();
            while (true)
            {
                Console.Write(">");
                string cmd = Console.ReadLine().Trim();
                switch (cmd)
                {
                    case "help":
                        ShowHelp();
                        break;
                    case "list":
                        Showphonebooks(phonebooks);
                        break;
                    case "find":
                        ShowSearch(phonebooks);
                        break;
                    case "xml":
                        using (StringWriter textWriter = new StringWriter())
                        {
                            serializer.Serialize(textWriter, phonebooks);
                            Console.WriteLine(textWriter.ToString());
                        }
                        break;
                    case "drop":
                        using (StreamWriter streamWriter = new StreamWriter("db.xml"))
                        {
                            serializer.Serialize(streamWriter, phonebooks);
                        }
                        break;
                    case "clear":
                        Console.Clear();
                        break;
                    case "exit":
                        return;
                    default:
                        Console.WriteLine("Неизвестная команда");
                        break;
                }
            }
        }
    }
}