using LanguageClasses.Queries;
using LanguageClasses;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            string menuItem = "";

            using (LanguageClassesContext context = new LanguageClassesContext())
            {
                while (menuItem != "11")
                {
                    menuItem = PrintMenu();
                    Console.Clear();
                    switch (menuItem)
                    {
                        case "1":
                            var positionsTypes = QueryExplorer.GetPositionsTypes(context, 10);
                            Print(positionsTypes, "Типы должностей");
                            break;
                        case "3":
                            var amountSum = QueryExplorer.GetAmountSum(context, 10);
                            Print(amountSum, "Сотрудники и их должности");
                            break;
                        case "4":
                            var employeesPositions = QueryExplorer.GetEmployeesPositions(context, 10);
                            Print(employeesPositions, "Должности сотрудников: ");
                            break;
                        case "5":
                            var employeesByPositions = QueryExplorer.GetEmployeesByPositions(context, 10);
                            Print(employeesByPositions, "Сотрудники и их должности с определенным названием");
                            break;
                        case "6":
                            List<string> positions = new List<string> { "Доцент", "Ст. преподаватель", "Профессор" };
                            Random random = new Random();
                            string position = positions[random.Next(3)];
                            QueryExplorer.AddPosition(context, position);
                            Print(context.Positions.OrderByDescending(p => p.Id).Take(10), "Последние добавленные должности: \n");
                            Console.WriteLine("Была добавлена должность: ");
                            Console.WriteLine(context.Positions.OrderByDescending(p => p.Id).First());
                            Console.WriteLine("Нажмите любую клавишу, чтобы продолжить...");
                            Console.ReadKey();
                            break;
                        case "7":
                            Console.WriteLine("Введите фамилию: ");
                            string surname = Console.ReadLine();
                            Console.WriteLine("Введите имя: ");
                            string name = Console.ReadLine();
                            Console.WriteLine("Введите отчество: ");
                            string patronymic = Console.ReadLine();
                            Console.WriteLine("Введите образование: ");
                            string education = Console.ReadLine();
                            int positionId = context.Positions.OrderByDescending(p => p.Id).First().Id;
                            QueryExplorer.AddEmployee(context, name, surname, patronymic, education, positionId);
                            Print(QueryExplorer.GetEmployeesPositions(context, 10), "Последние добавленные должности: \n");
                            Print(QueryExplorer.GetEmployeesPositions(context, 1), "Был добавлен сотрудник: \n");
                            break;
                        case "8":
                            Print(context.Positions.OrderByDescending(p => p.Id).Take(10), "Должности: \n");
                            Console.WriteLine("Введите код должности: ");
                            int PositionId = Convert.ToInt32(Console.ReadLine());
                            Console.WriteLine("Будет удалена должность: ");
                            Console.WriteLine(context.Positions.Where(p => p.Id == PositionId).First());
                            QueryExplorer.DelPosition(context, PositionId);
                            Print(context.Positions.OrderByDescending(p => p.Id).Take(10), "Должности: \n");
                            break;
                        case "9":
                            Print(QueryExplorer.GetEmployeesPositions(context, 10), "Последние добавленные сотрудники: \n");
                            Console.WriteLine("Введите код сотрудника: \n");
                            int EmployeeId = Convert.ToInt32(Console.ReadLine());
                            Console.WriteLine("Будет удален сотрудник с кодом: ");
                            Console.WriteLine(context.Employees.Where(p => p.Id == EmployeeId).First().Id);
                            QueryExplorer.DelEmployee(context, EmployeeId);
                            Print(QueryExplorer.GetEmployeesPositions(context, 10), "Последние добавленные сотрудники: \n");
                            break;
                        case "10":
                            Print(QueryExplorer.GetCourseIntensity(context), "Размер групп до обновления: \n");
                            QueryExplorer.UpdateListenersByDate(context);
                            Print(QueryExplorer.GetCourseIntensity(context), "Размер групп после обновления: \n");
                            break;
                        case "11": break;
                        default: break;
                    }
                }
            }
        }

        public static string PrintMenu()
        {
            Console.Clear();
            Console.WriteLine("Выберите пункт меню...");
            Console.WriteLine("1. Запрос на все типы должностей");
            Console.WriteLine("3. Запрос на сумму платежей разных назначений");
            Console.WriteLine("4. Запрос на выборку сотрудников и их должностей");
            Console.WriteLine("5. Запрос на выборку сотрудников и их должностей с определенным названием");
            Console.WriteLine("6. Добавить должность;");
            Console.WriteLine("7. Добавить сотрудника;");
            Console.WriteLine("8. Удалить должность;");
            Console.WriteLine("9. Удалить сотрудника;");
            Console.WriteLine("10. Увеличить размер группы на 2 для курсов с интенсивностью занятий больше, чем 3 часа");
            Console.WriteLine("11. Выход.");
            Console.WriteLine();
            string menuItem = Console.ReadLine();
            return menuItem;
        }

        public static void Print(IEnumerable items, string message)
        {
            Console.WriteLine(message);
            foreach (var item in items)
            {
                Console.WriteLine(item);
            }
            Console.WriteLine("Нажмите любую клавишу, чтобы продолжить...");
            Console.ReadKey();
        }
    }
}