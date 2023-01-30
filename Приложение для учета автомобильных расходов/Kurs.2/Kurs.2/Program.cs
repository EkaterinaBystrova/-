/// <summary>
/// Приложение предназначено для учета автомобильных расходов. 
/// Оно читает из файла записанные ранее расходы и пользовательские названия,
/// а также производит запись расходов и названий в конце работы. 
/// </summary>
using System;
using System.Text;
using System.IO;

namespace Kurs._2
{
    abstract class Expense
    {
        public int Day;
        public int Month;
        public int Year;
        public string Name;
        public bool PlanOrNot;
        public Expense(int day, int month, int year, string name, bool planornot)
        {
            Day = day;
            Month = month;
            Year = year;
            Name = name;
            PlanOrNot = planornot;
        }
        public abstract void ShowExpense();
        
        public abstract int SumExpense();

    }
    class Payment : Expense
    {
        public int Fees;
        public Payment(int day, int month, int year, string name, 
            bool planornot, int cost1) : base(day, month, year, name, planornot)
        {
            Fees = cost1;
        }
        public override void ShowExpense()
        {
            Console.Write("{0,-2}.{1,-2}.{2,-4} {3,-15} {4,-15}", Day, Month, Year, 
                Name, Fees);
            if (PlanOrNot == true)
                Console.WriteLine("                          (планируемый)");
            else Console.WriteLine("");
        }
        public override int SumExpense()
        {
            return Fees;
        }
    }
    class Repair : Expense
    {
        public int WorkPrice;
        public int ItemPrice;
        public Repair(int day, int month, int year, string name, bool planornot, 
            int cost1, int cost2) : base(day, month, year, name, planornot)
        {
            WorkPrice = cost1;
            ItemPrice = cost2;
        }
        public override void ShowExpense()
        {
            Console.Write("{0,-2}.{1,-2}.{2,-4} {3,-25} {4,-12} {5,-15}", Day, Month, 
                Year, Name, WorkPrice, ItemPrice);
            if (PlanOrNot == true)
                Console.WriteLine("(планируемый)");
            else Console.WriteLine("");
        }
        public override int SumExpense()
        {
            return WorkPrice + ItemPrice;
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            Expense[] expense = new Expense[0]; // объявление массива расходов
            string[] Names = new string[] { "Замена масла", "Замена резины","Топливо", 
                "Оплата Штрафа", "Замена колодок" }; // объявление массива названий.
            // чтение из файла расходов и их названий.
            StreamReader f = new StreamReader("text.txt");
            try
            { // чтение количества расходов и количества названий
                int expensenumber = int.Parse(f.ReadLine());
                int namesnumber = int.Parse(f.ReadLine());
                
                string[] arr;
                Array.Resize(ref expense, expensenumber);
                Array.Resize(ref Names, Names.Length+namesnumber);
                int day;
                int month;
                int year;
                string name;
                bool planornot;
                int CostInt1;
                int CostInt2;
                for (int i = 0; i < expensenumber; i++)
                {
                    if (expensenumber == 0)
                        break;
                    // чтение информации о расходах и создание объектов на ее основе.
                    arr = f.ReadLine().Split(";");
                    day = int.Parse(arr[0]);
                    month = int.Parse(arr[1]);
                    year = int.Parse(arr[2]);
                    name = arr[3];
                    if (arr[4] == "True") planornot = true; else planornot = false;
                    if (arr.Length == 6)
                    {
                        CostInt1 = int.Parse(arr[5]);
                        expense[i] = new Payment(day, month, year, name, planornot, 
                            CostInt1);
                    }
                    else if (arr.Length == 7)
                    {
                        CostInt1 = int.Parse(arr[5]);
                        CostInt2 = int.Parse(arr[6]);
                        expense[i] = new Repair(day, month, year, name, planornot, CostInt1, 
                            CostInt2);
                    }
                }
                // чтение названий пользовательских расходов и добавление их 
                // в основной массив.
                for (int i = 0; i < namesnumber; i++)
                {
                    if (expensenumber == 0)
                        break;
                    Names[5 + i] = f.ReadLine();
                }
                f.Close();
            }
            catch (Exception) { Console.WriteLine("Ошибка чтения файла. Для выхода нажмите " +
                "любую кнопку"); Console.ReadKey(); return; }
            // основная часть программы
            while (true)
            {
                Console.WriteLine("\nГлавное меню:\n1)Ввести новый расход\n2)Просмотреть " +
                    "расходы за период\n3)Просмотреть расходы определенного вида\n4)Удалить " +
                    "расход");
                string numb = Console.ReadLine();
                switch (numb)
                {
                    case ("1"): // ввод нового расхода.
                        bool key = true;
                        int day = 0;
                        int month = 0;
                        int year = 0;
                        bool keyq = true;
                        while (key)
                        {
                            Console.WriteLine("Введите дату, в которую произошел расход в формате " +
                                "ДД.ММ.ГГГГ");
                            string date = Console.ReadLine();
                            string[] arr = date.Split('.');

                            try
                            {
                                if (arr.Length == 3)
                                {
                                    day = int.Parse(arr[0]);
                                    month = int.Parse(arr[1]);
                                    year = int.Parse(arr[2]);
                                    if ((day < 1) || (day > 31) || (month < 1) || (month > 12) 
                                        || (year < 0) || (year > 9999))
                                        throw new Exception();
                                    key = false;
                                }
                                else if (arr[0] == "q")
                                {
                                    Console.WriteLine("Переход в главное меню");
                                    key = false;
                                    keyq = false;
                                }

                                else throw new Exception();
                            }
                            catch (Exception) { Console.WriteLine("Введенная дата не соответствует " +
                                "шаблону.Введите снова"); }
                        }
                        if (keyq == false)
                            break;
                        Console.WriteLine("Выберите из списка или введите новое название расхода:");
                        for (int n = 0; n < Names.Length; n++)
                            Console.WriteLine((n + 1) + ". " + Names[n]);
                        string name = Console.ReadLine();
                        if (name == "q")
                        {
                            Console.WriteLine("Введенные данные не будут записаны. Переход в главное " +
                                "меню");
                            break;
                        }

                        try
                        {
                            for (int k = 0; k < Names.Length; k++)
                                if ((int.Parse(name) - 1) == k)
                                    name = Names[k];

                        }
                        catch (Exception) { };
                        Console.WriteLine("Введите «да», если расход-планируемый");
                        string answer = Console.ReadLine();
                        bool planornot = false;
                        if (answer == "да")
                            planornot = true;
                        Console.WriteLine("При взносе, штрафе или платеже введите сумму платежа,  при " +
                            "замене запчасти введите цену работы и цену запчасти через пробел");
                        while (true)
                        {
                            string cost = Console.ReadLine();
                            string[] arr = cost.Split();
                            int CostInt1 = 0;
                            int CostInt2 = 0;
                            if (arr.Length == 1)
                            {
                                if (cost == "q")
                                    break;
                                bool key2 = true;
                                while (key2)
                                {
                                    try
                                    {
                                        CostInt1 = int.Parse(cost);
                                        key2 = false;
                                    }
                                    catch (Exception) { Console.WriteLine("Сумма введена с ошибкой. " +
                                        "Введите снова"); 
                                        cost = Console.ReadLine();
                                    };
                                }
                                int i = 0;
                                while (i < expense.Length)
                                {
                                    if (year < expense[i].Year)
                                        break;
                                    else if ((year == expense[i].Year) && (month < expense[i].Month))
                                        break;
                                    else if ((year == expense[i].Year) && (month == expense[i].Month) 
                                        && (day < expense[i].Year))
                                        break;
                                    i++;
                                }
                                Array.Resize(ref expense, expense.Length + 1);
                                if (i != expense.Length - 1)
                                    for (int j = expense.Length - 2; j >= i; j--)
                                        expense[j + 1] = expense[j];
                                expense[i] = new Payment(day, month, year, name, planornot, CostInt1);
                                Console.WriteLine("Новый расход создан");
                                key2 = false;
                                for (int n = 0; n < Names.Length; n++)
                                    if (Names[n] == name)
                                        key2 = true;
                                if (key2 == false)
                                {
                                    Array.Resize(ref Names, Names.Length + 1);
                                    Names[Names.Length - 1] = name;
                                }
                                break;
                            }
                            else if (arr.Length == 2)
                            {
                                bool key2 = true;
                                while (key2)
                                {
                                    try
                                    {
                                        CostInt1 = int.Parse(arr[0]);
                                        CostInt2 = int.Parse(arr[1]);
                                        key2 = false;
                                    }
                                    catch (Exception) 
                                    { Console.WriteLine("Сумма введена с ошибкой. Введите снова"); 
                                        cost = Console.ReadLine();
                                        arr = cost.Split();
                                    };
                                }
                                int i = 0;
                                while (i < expense.Length)
                                {
                                    if (year < expense[i].Year)
                                        break;
                                    else if ((year == expense[i].Year) && (month < expense[i].Month))
                                        break;
                                    else if ((year == expense[i].Year) && (month == expense[i].Month) 
                                        && (day < expense[i].Year))
                                        break;
                                    i++;
                                }
                                Array.Resize(ref expense, expense.Length + 1);
                                if (i != expense.Length - 1)
                                    for (int j = expense.Length - 2; j >= i; j--)
                                        expense[j + 1] = expense[j];
                                expense[i] = new Repair(day, month, year, name, planornot, CostInt1, 
                                    CostInt2);
                                Console.WriteLine("Новый расход создан");
                                key2 = false;
                                for (int n = 0; n < Names.Length; n++)
                                    if (Names[n] == name)
                                        key2 = true;
                                if (key2 == false)
                                {
                                    Array.Resize(ref Names, Names.Length + 1);
                                    Names[Names.Length - 1] = name;
                                }
                                break;
                            }
                            else Console.WriteLine("Сумма введена с ошибкой. Введите снова");
                        }
                        break;
                    case ("2"): // просмотр расходов за период.
                        key = true;
                        int day1 = 0;
                        int day2 = 0;
                        int month1 = 0;
                        int month2 = 0;
                        int year1 = 0;
                        int year2 = 0;
                        try
                        {
                            while (key)
                            {
                                string[] arr;
                                Console.WriteLine("Введите дату начала периода");
                                string date1 = Console.ReadLine();
                                if (date1 == "q")
                                    throw new Exception();
                                arr = date1.Split('.');
                                try
                                {
                                    if (arr.Length == 3)
                                    {
                                        day1 = int.Parse(arr[0]);
                                        month1 = int.Parse(arr[1]);
                                        year1 = int.Parse(arr[2]);
                                        if ((day1 < 1) || (day1 > 31) || (month1 < 1) || (month1 > 12) 
                                            || (year1 < 0) || (year1 > 9999))
                                            throw new Exception();
                                        key = false;
                                    }
                                    else throw new Exception();
                                }
                                catch (Exception) { Console.WriteLine("Введенная дата не соответствует " +
                                    "шаблону.Введите снова"); }
                            }
                            while (!key)
                            {

                                Console.WriteLine("Введите дату конца периода");
                                string date2 = Console.ReadLine();
                                if (date2 == "q")
                                    throw new Exception();
                                string[] arr = date2.Split('.');
                                try
                                {
                                    if (arr.Length == 3)
                                    {
                                        day2 = int.Parse(arr[0]);
                                        month2 = int.Parse(arr[1]);
                                        year2 = int.Parse(arr[2]);
                                        if ((day2 < 1) || (day2 > 31) || (month2 < 1) || (month2 > 12) 
                                            || (year2 < 0) || (year2 > 9999))
                                            throw new Exception();
                                        key = true;
                                    }
                                    else throw new Exception();
                                }
                                catch (Exception) { Console.WriteLine("Введенная дата не соответствует " +
                                    "шаблону.Введите снова"); }
                            }
                            int sum = 0;
                            Console.WriteLine("-----------------------------------------------------" +
                                "--------------\n" +
                                             "|№_|Дата______|Название_______|Платеж___|Цена_работы_|" +
                                             "Цена_запчасти_|");
                            for (int j = 0; j < expense.Length; j++)
                                if ((expense[j].Year > year1) && (expense[j].Year < year2))
                                {
                                    Console.Write(" {0}. ", j + 1);
                                    expense[j].ShowExpense();
                                    sum += expense[j].SumExpense();
                                }
                                else if ((expense[j].Year == year1) && (expense[j].Year == year2))
                                {
                                    if ((expense[j].Month > month1) && (expense[j].Month < month2))
                                    {
                                        Console.Write(" {0}. ", j + 1);
                                        expense[j].ShowExpense();
                                        sum += expense[j].SumExpense();
                                    }
                                    else if ((expense[j].Month == month1) 
                                        && (expense[j].Month == month2))
                                    {
                                        if ((expense[j].Day >= day1) && (expense[j].Day <= day2))
                                        {
                                            Console.Write(" {0}. ", j + 1);
                                            expense[j].ShowExpense();
                                            sum += expense[j].SumExpense();
                                        }
                                    }
                                    else if ((expense[j].Month == month1))
                                    {
                                        if (expense[j].Day >= day1)
                                        {
                                            Console.Write(" {0}. ", j + 1);
                                            expense[j].ShowExpense();
                                            sum += expense[j].SumExpense();
                                        }
                                    }
                                    else if (expense[j].Month == month2)
                                    {
                                        if (expense[j].Day <= day2)
                                        {
                                            Console.Write(" {0}. ", j + 1);
                                            expense[j].ShowExpense();
                                            sum += expense[j].SumExpense();
                                        }
                                    }
                                }
                                else if (expense[j].Year == year1)
                                {
                                    if (expense[j].Month > month1)
                                    {
                                        Console.Write(" {0}. ", j + 1);
                                        expense[j].ShowExpense();
                                        sum += expense[j].SumExpense();
                                    }
                                    else if (expense[j].Month == month1)
                                        if (expense[j].Day >= day1)
                                        {
                                            Console.Write(" {0}. ", j + 1);
                                            expense[j].ShowExpense();
                                            sum += expense[j].SumExpense();
                                        }
                                }
                                else if (expense[j].Year == year2)
                                {
                                    if (expense[j].Month < month1)
                                    {
                                        Console.Write(" {0}. ", j);
                                        expense[j].ShowExpense();
                                        sum += expense[j].SumExpense();
                                    }
                                    else if (expense[j].Month == month1)
                                        if (expense[j].Day <= day1)
                                        {
                                            Console.Write(" {0}. ", j);
                                            expense[j].ShowExpense();
                                            sum += expense[j].SumExpense();
                                        }
                                }

                            Console.WriteLine("\nИтого за период: " + sum);
                            throw new Exception();
                        }
                        catch (Exception) { Console.WriteLine("Переход в главное меню."); break; }
                    case ("3"): // просмотр расходов определенного вида.
                        Console.WriteLine("Выберите из списка название расхода");
                        for (int n = 0; n < Names.Length; n++)
                            Console.WriteLine((n + 1) + ". " + Names[n]);
                        name = Console.ReadLine();
                        if (name == "q")
                            break;
                        int p = 0;
                        try
                        {
                            Console.WriteLine("-----------------------------------------------------" +
                                "--------------\n" +
                                             "|№_|Дата______|Название_______|Платеж___|Цена_работы_|" +
                                             "Цена_запчасти_|");
                            int sum = 0;
                            for (int n = 0; n < expense.Length; n++)
                                if (expense[n].Name == Names[int.Parse(name) - 1])
                                {
                                    Console.Write(" {0}. ", n);
                                    expense[n].ShowExpense();
                                    p++;
                                    sum = sum + expense[n].SumExpense();
                                }
                            if (p == 0)
                                throw new Exception();
                            Console.WriteLine("Итого по виду расхода: " + sum);
                        }
                        catch (IndexOutOfRangeException) { }
                        catch (Exception) { Console.WriteLine("Нет расходов, соответствующих этому " +
                            "названию\n"); };
                        Console.WriteLine("Переход в главное меню");
                        break;
                    case ("4"): // удаление расхода.
                        key = true;
                        day = 0;
                        month = 0;
                        year = 0;
                        try
                        {
                            while (key)
                            {
                                string[] arr;
                                Console.WriteLine("Введите дату удаляемого расхода");
                                string date = Console.ReadLine();
                                if (date == "q")
                                    throw new Exception();
                                arr = date.Split('.');
                                try
                                {
                                    if (arr.Length == 3)
                                    {
                                        day = int.Parse(arr[0]);
                                        month = int.Parse(arr[1]);
                                        year = int.Parse(arr[2]);
                                        if ((day < 1) || (day > 31) || (month < 1) || (month > 12) 
                                            || (year < 0) || (year > 9999))
                                            throw new Exception();
                                        key = false;
                                    }
                                    else throw new Exception();
                                }
                                catch (Exception) { Console.WriteLine("Введенная дата не соответствует " +
                                    "шаблону.Введите снова"); }
                            }
                            int t = 0;
                            for (int n = 0; n < expense.Length; n++)
                                if ((expense[n].Year == year) && (expense[n].Month == month) 
                                    && (expense[n].Day == day))
                                {
                                    Console.Write((n + 1) + ". ");
                                    expense[n].ShowExpense();
                                    t++;
                                }
                            if (t == 0)
                            {
                                Console.WriteLine("Нет расходов, соответствующих этой дате");
                                throw new Exception();
                            }
                            Console.WriteLine("Выберите из списка номер расхода");
                            numb = Console.ReadLine();
                            try
                            {
                                if (((int.Parse(numb) - 1) < 0) 
                                    || ((int.Parse(numb) - 1) >= expense.Length))
                                    throw new Exception();
                            }
                            catch (Exception) { Console.WriteLine("Расхода с таким номером не " +
                                "существует"); throw; }
                            for (int n = (int.Parse(numb) - 1); n < expense.Length - 1; n++)
                                expense[n] = expense[n + 1];
                            Array.Resize(ref expense, expense.Length - 1);
                            Console.WriteLine("Расход удален.Возврат в меню расходов");
                            throw new Exception();
                        }
                        catch (Exception) { Console.WriteLine("Переход в главное меню."); break; }
                    case ("q"):
                        // запись всех расходов и пользовательских названий в файл 
                        StreamWriter g=new StreamWriter("text.txt", false);
                        g.WriteLine(expense.Length);
                        g.WriteLine(Names.Length-5);
                        if (expense.Length != 0)
                            for (int i = 0; i < expense.Length; i++)
                            {
                                if (expense[i] is Payment)
                                    g.WriteLine("{0};{1};{2};{3};{4};{5}", expense[i].Day, 
                                        expense[i].Month, expense[i].Year, expense[i].Name, 
                                        expense[i].PlanOrNot, (expense[i] as Payment).Fees);
                                else
                                    g.WriteLine("{0};{1};{2};{3};{4};{5};{6}", expense[i].Day, 
                                        expense[i].Month, expense[i].Year, expense[i].Name, 
                                        expense[i].PlanOrNot, (expense[i] as Repair).WorkPrice, 
                                        (expense[i] as Repair).ItemPrice);
                            }
                        if (Names.Length != 5)
                            for (int i = 5; i < Names.Length; i++)
                                g.WriteLine(Names[i]);
                        g.Close();
                        return;
                    default:
                        Console.WriteLine("Действий с таким номером нет");
                        break;





                }
            }
        }
    }
}
