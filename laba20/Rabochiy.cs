using laba20;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace laba20
{
}

public class Rabochiy
{
    // Приватные поля
    private string _fio;
    private decimal _oklad;
    private DateTime _dataPostupleniya;
    private decimal _premiya;
    private int _kolichestvoDney;
    private decimal _nachisleno;
    private decimal _uderzhano;

    // Конструктор с параметрами
    public Rabochiy(string fio, decimal oklad, DateTime dataPostupleniya, decimal premiya, int kolichestvoDney, decimal nachisleno, decimal uderzhano)
    {
        Fio = fio;
        Oklad = oklad;
        DataPostupleniya = dataPostupleniya;
        Premiya = premiya;
        KolichestvoDney = kolichestvoDney;
        Nachisleno = nachisleno;
        Uderzhano = uderzhano;
    }

    // Конструктор без параметров
    public Rabochiy() { }

    // Свойства с проверками
    public string Fio
    {
        get => _fio;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new WorkerException("ФИО не может быть пустым.");
            _fio = value;
        }
    }

    public decimal Oklad
    {
        get => _oklad;
        set
        {
            if (value < 0)
                throw new WorkerException("Оклад не может быть отрицательным.");
            _oklad = Math.Round(value, 2);
        }
    }

    public DateTime DataPostupleniya
    {
        get => _dataPostupleniya;
        set
        {
            if (value > DateTime.Now)
                throw new WorkerException("Дата поступления не может быть в будущем.");
            _dataPostupleniya = value;
        }
    }

    public decimal Premiya
    {
        get => _premiya;
        set
        {
            if (value < 0)
                throw new WorkerException("Премия не может быть отрицательной.");
            _premiya = Math.Round(value, 2);
        }
    }

    public int KolichestvoDney
    {
        get => _kolichestvoDney;
        set
        {
            if (value < 0)
                throw new WorkerException("Количество дней не может быть отрицательным.");
            _kolichestvoDney = value;
        }
    }

    public decimal Nachisleno
    {
        get => _nachisleno;
        set
        {
            if (value < 0)
                throw new WorkerException("Начислено не может быть отрицательным.");
            _nachisleno = Math.Round(value, 2);
        }
    }

    public decimal Uderzhano
    {
        get => _uderzhano;
        set
        {
            if (value < 0)
                throw new WorkerException("Удержано не может быть отрицательным.");
            _uderzhano = Math.Round(value, 2);
        }
    }

    // Метод для расчета зарплаты
    public decimal CalculateSalary()
    {
        return _nachisleno - _uderzhano;
    }

    // Метод для проверки стажа более 3 лет
    public bool HasMoreThan3YearsExperience()
    {
        return (DateTime.Now - _dataPostupleniya).TotalDays > 3 * 365;
    }

    // Переопределение ToString для вывода информации о работнике
    public override string ToString()
    {
        return $"ФИО: {_fio}, Оклад: {_oklad}, Дата поступления: {_dataPostupleniya.ToShortDateString()}, Премия: {_premiya}, Дней: {_kolichestvoDney}, Начислено: {_nachisleno}, Удержано: {_uderzhano}";
    }
}
