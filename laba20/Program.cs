using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace laba20
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<Rabochiy> workers = new List<Rabochiy>
            {
                new Rabochiy("Аотский Кирилл Сергеевич", 1000, new DateTime(2020, 1, 1), 10000000, 3, 10000000, 100),
                new Rabochiy("Бавчук Никита Андреевич", 3500, new DateTime(2020, 1, 1), 200, 12, 1200, 100),
                  new Rabochiy("Берчук Владимер Сергеевич", 800, new DateTime(2020, 1, 1), 300, 220, 1200, 100),
                new Rabochiy("Былван Балван Балванов", 2, new DateTime(2020, 1, 1), 20, 220, 1200, 100),
                new Rabochiy("Бятискав Самир Акинвеевич", 11000, new DateTime(2020, 1, 1), 1200, 420, 1200, 100),
                new Rabochiy("Валодко Григорий Семенович", 121000, new DateTime(2020, 1, 1), 2200, 420, 1200, 100),
                new Rabochiy("Самали Василий Топ", 1, new DateTime(2020, 1, 1), 200, 260, 13200, 100),
                new Rabochiy("Семенов Семен Семенович", 0, new DateTime(2020, 1, 1), 200, 820, 1200, 100),
                new Rabochiy("Титанов Иван Иванович", 102300, new DateTime(2020, 1, 1), 200, 290, 1200, 100),
            };

            bool exit = false;
            while (!exit)
            {
                Console.WriteLine("Меню:");
                Console.WriteLine("1. Вывести текущие данные");
                Console.WriteLine("2. Добавить объект");
                Console.WriteLine("3. Выполнить запросы");
                Console.WriteLine("4. Сохранить в XML");
                Console.WriteLine("5. Выйти");

                int choice = int.Parse(Console.ReadLine());

                switch (choice)
                {
                    case 1:
                        PrintWorkers(workers);
                        break;
                    case 2:
                        AddWorker(workers);
                        break;
                    case 3:
                        ExecuteQueries(workers);
                        break;
                    case 4:
                        SaveToXml(workers, "workers.xml");
                        break;
                    case 5:
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("Неверный выбор.");
                        break;
                }
            }
        }

        // Вывод списка работников
        static void PrintWorkers(List<Rabochiy> workers)
        {
            foreach (var worker in workers)
            {
                Console.WriteLine(worker);
            }
        }

        // Добавление нового работника
        static void AddWorker(List<Rabochiy> workers)
        {
            try
            {
                Console.WriteLine("Введите ФИО:");
                string fio = Console.ReadLine();
                Console.WriteLine("Введите оклад:");
                decimal oklad = decimal.Parse(Console.ReadLine());
                Console.WriteLine("Введите дату поступления (гггг-мм-дд):");
                DateTime dataPostupleniya = DateTime.Parse(Console.ReadLine());
                Console.WriteLine("Введите премию:");
                decimal premiya = decimal.Parse(Console.ReadLine());
                Console.WriteLine("Введите количество отработанных дней:");
                int kolichestvoDney = int.Parse(Console.ReadLine());
                Console.WriteLine("Введите начислено:");
                decimal nachisleno = decimal.Parse(Console.ReadLine());
                Console.WriteLine("Введите удержано:");
                decimal uderzhano = decimal.Parse(Console.ReadLine());

                workers.Add(new Rabochiy(fio, oklad, dataPostupleniya, premiya, kolichestvoDney, nachisleno, uderzhano));
            }
            catch (WorkerException ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
        }

        // Выполнение запросов
        static void ExecuteQueries(List<Rabochiy> workers)
        {
            // Сортировка по ФИО и дате поступления
            var sortedWorkers = workers.OrderBy(w => w.Fio).ThenBy(w => w.DataPostupleniya).ToList();
            Console.WriteLine("Сортировка по ФИО и дате поступления:");
            PrintWorkers(sortedWorkers);

            // Работники с опытом более 3 лет
            var experiencedWorkers = workers.Where(w => w.HasMoreThan3YearsExperience()).ToList();
            Console.WriteLine("Работники с опытом более 3 лет:");
            PrintWorkers(experiencedWorkers);

            // Сортировка по премии
            var sortedByPremiya = workers.OrderBy(w => w.Premiya).ToList();
            Console.WriteLine("Сортировка по премии:");
            PrintWorkers(sortedByPremiya);

            // Сумма всех начислений с учетом удержаний
            decimal totalSalary = workers.Sum(w => w.CalculateSalary());
            Console.WriteLine($"Сумма всех начислений: {totalSalary}");

            // Группировка по каждому полю
            GroupAndSaveByField(workers, w => w.Uderzhano.ToString(), "Uderzhano", "grouped_by_uderzhano.xml");
        }

        // Группировка и сохранение в XML
        static void GroupAndSaveByField(List<Rabochiy> workers, Func<Rabochiy, string> fieldSelector, string fieldName, string filePath)
        {
            var groupedData = GroupByField(workers, fieldSelector);
            SaveGroupedDataToXml(groupedData, filePath, fieldName);
        }

        // Группировка по полю
        public static Dictionary<string, List<Rabochiy>> GroupByField(List<Rabochiy> workers, Func<Rabochiy, string> fieldSelector)
        {
            return workers
                .GroupBy(fieldSelector)
                .ToDictionary(g => g.Key, g => g.ToList());
        }

        // Сохранение группировки в XML
        public static void SaveGroupedDataToXml(Dictionary<string, List<Rabochiy>> groupedData, string filePath, string groupByFieldName)
        {
            try
            {
                XElement root = new XElement("GroupedWorkers",
                    new XAttribute("GroupedBy", groupByFieldName)
                );

                foreach (var group in groupedData)
                {
                    XElement groupElement = new XElement("Group",
                        new XAttribute(groupByFieldName, group.Key)
                    );

                    foreach (var worker in group.Value)
                    {
                        XElement workerElement = new XElement("Worker",
                            new XElement("FIO", worker.Fio),
                            new XElement("Oklad", worker.Oklad),
                            new XElement("DataPostupleniya", worker.DataPostupleniya.ToString("yyyy-MM-dd")),
                            new XElement("Premiya", worker.Premiya),
                            new XElement("KolichestvoDney", worker.KolichestvoDney),
                            new XElement("Nachisleno", worker.Nachisleno),
                            new XElement("Uderzhano", worker.Uderzhano)
                        );

                        groupElement.Add(workerElement);
                    }

                    root.Add(groupElement);
                }

                root.Save(filePath);
                Console.WriteLine($"Группировка по полю '{groupByFieldName}' сохранена в файл: {filePath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при сохранении группировки в XML: {ex.Message}");
            }
        }

        // Сохранение всех данных в XML
        static void SaveToXml(List<Rabochiy> workers, string filePath)
        {
            try
            {
                XElement root = new XElement("Workers",
                    from worker in workers
                    select new XElement("Worker",
                        new XElement("FIO", worker.Fio),
                        new XElement("Oklad", worker.Oklad),
                        new XElement("DataPostupleniya", worker.DataPostupleniya.ToString("yyyy-MM-dd")),
                        new XElement("Premiya", worker.Premiya),
                        new XElement("KolichestvoDney", worker.KolichestvoDney),
                        new XElement("Nachisleno", worker.Nachisleno),
                        new XElement("Uderzhano", worker.Uderzhano)
                    )
                );

                root.Save(filePath);
                Console.WriteLine($"Данные сохранены в {filePath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при сохранении данных в XML: {ex.Message}");
            }
        }
    }
}



