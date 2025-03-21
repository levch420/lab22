using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace laba20
{
    internal class XmlHelper
    {
        // Сохранение группировки в XML
        public static void SaveGroupedDataToXml(Dictionary<string, List<Rabochiy>> groupedData, string filePath, string groupByFieldName)
        {
            try
            {
                // Создаем корневой элемент XML
                XElement root = new XElement("GroupedWorkers",
                    new XAttribute("GroupedBy", groupByFieldName) // Атрибут для указания поля группировки
                );

                // Проходим по каждой группе
                foreach (var group in groupedData)
                {
                    // Создаем элемент группы с атрибутом, указывающим значение поля группировки
                    XElement groupElement = new XElement("Group",
                        new XAttribute(groupByFieldName, group.Key) // Атрибут группы
                    );

                    // Добавляем данные каждого работника в группу
                    foreach (var worker in group.Value)
                    {
                        XElement workerElement = new XElement("Worker",
                            new XElement("FIO", worker.Fio),
                            new XElement("Oklad", worker.Oklad),
                            new XElement("DataPostupleniya", worker.DataPostupleniya.ToString("yyyy-MM-dd")), // Форматируем дату
                            new XElement("Premiya", worker.Premiya),
                            new XElement("KolichestvoDney", worker.KolichestvoDney),
                            new XElement("Nachisleno", worker.Nachisleno),
                            new XElement("Uderzhano", worker.Uderzhano)
                        );

                        // Добавляем работника в группу
                        groupElement.Add(workerElement);
                    }

                    // Добавляем группу в корневой элемент
                    root.Add(groupElement);
                }

                // Сохраняем XML в файл
                root.Save(filePath);
                Console.WriteLine($"Группировка по полю '{groupByFieldName}' сохранена в файл: {filePath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при сохранении группировки в XML: {ex.Message}");
            }
        }
    }
}