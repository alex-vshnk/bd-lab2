using LanguageClasses;
using LanguageClasses.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;


namespace LanguageClasses.Queries
{
    public static class QueryExplorer
    {
        //1.	Выборку всех данных из таблицы,
        //стоящей в схеме базы данных нас стороне отношения «один» – 1 шт.

        //Запрос на все типы должностей

        public static IEnumerable GetPositionsTypes(LanguageClassesContext context, int recordsNumber)
        {
            var query = context.Positions.Select(p => new
            {
                Код_типа_должности = p.Id,
                Наименование_должности = p.Name
            });

            return query.Take(recordsNumber).ToList();
        }

        //2.	Выборку данных из таблицы, стоящей в схеме базы данных нас
        //стороне отношения «один», отфильтрованные по определенному условию,
        //налагающему ограничения на одно или несколько полей – 1 шт.

        //Нет подходящих таблиц, диаграмму бд приложу в отчете


        //3.	Выборку данных, сгруппированных по любому из полей данных
        //с выводом какого-либо итогового результата(min, max, avg, сount или др.)
        //по выбранному полю из таблицы, стоящей в схеме базы данных
        //на стороне отношения «многие» – 1 шт.

        // Запрос на сумму платежей разных назначений
        public static IEnumerable GetAmountSum(LanguageClassesContext context, int recordsNumber)
        {
            var query = context.Payments.GroupBy(p => p.PurposeId, p => p.Amount)
                .Select(grp => new
                {
                    Код_назначения_платежа = grp.Key,
                    Общая_сумма = grp.Sum()
                });

            return query.Take(recordsNumber).ToList();
        }


        //4.	Выборку данных из двух полей двух таблиц,
        //связанных между собой отношением «один-ко-многим» – 1 шт.

        //Запрос на выборку сотрудников и их должностей
        public static IEnumerable GetEmployeesPositions(LanguageClassesContext context, int recordsNumber)
        {
            var query = context.Employees
                .OrderByDescending(e => e.Id)
                .Join(context.Positions,
                e => e.PositionId,
                p => p.Id,
                (e, p) => new
                {
                    Код_сотрудника = e.Id,
                    Фамилия = e.Surname,
                    Имя = e.FirstName,
                    Отчество = e.Patronymic,
                    Образование = e.Education,
                    Должность = p.Name
                });
            return query.Take(recordsNumber).ToList();
        }

        //5.	Выборку данных из двух таблиц, связанных между собой отношением
        //«один-ко-многим» и отфильтрованным по некоторому условию,
        //налагающему ограничения на значения одного или нескольких полей – 1 шт.

        //Запрос на выборку сотрудников и их должностей с определенным названием"
        public static IEnumerable GetEmployeesByPositions(LanguageClassesContext context, int recordsNumber)
        {
            var query = context.Employees
                .OrderByDescending(e => e.Id)
                //.Where(e => e.PositionId == 1)
                .Join(context.Positions,
                e => e.PositionId,
                p => p.Id,
                (e, p) => new
                {
                    Код_сотрудника = e.Id,
                    Фамилия = e.Surname,
                    Имя = e.FirstName,
                    Отчество = e.Patronymic,
                    Образование = e.Education,
                    Должность = p.Name
                }).Where(grp => grp.Должность == "PositionName1");

            return query.Take(recordsNumber).ToList();
        }

        //6.	Вставку данных в таблицы, стоящей на стороне отношения «Один» – 1 шт.

        //Добавить должность
        public static void AddPosition(LanguageClassesContext context, string name)
        {
            Position position = new Position
            {
                Name = name
            };
            context.Positions.Add(position);
            context.SaveChanges();
        }
        
        //7.	Вставку данных в таблицы, стоящей на стороне отношения «Многие» – 1 шт
        
        //Добавление сотрудника
        public static void AddEmployee(LanguageClassesContext context, string 
            firstName, string surname, string patronymic, string education, 
            int positionId)
        {
            Employee employee = new Employee
            {
                Surname = surname,
                FirstName = firstName,
                Patronymic = patronymic,
                Education = education,
                PositionId = positionId,
            };
            context.Employees.Add(employee);
            context.SaveChanges();
        }

        //8.	Удаление данных из таблицы, стоящей на стороне отношения «Один» – 1 шт.

        //Удаление должности
        public static void DelPosition(LanguageClassesContext context, int id)
        {
            context.Employees.RemoveRange(context.Employees.Where(e => e.PositionId == id));
            context.Positions.Remove(context.Positions.Where(p => p.Id == id).First());
            context.SaveChanges();
        }

        //9.	Удаление данных из таблицы, стоящей на стороне отношения «Многие» – 1 шт.

        //Удаление сотрудника
        public static void DelEmployee(LanguageClassesContext context, int id)
        {
            context.Employees.Remove(context.Employees.Where(p => p.Id == id).First());
            context.SaveChanges();
        }

        //10.	Обновление удовлетворяющих определенному условию записей
        //в любой из таблиц базы данных – 1 шт.

        //Увеличить размер группы на 2 для курсов с интенсивностью занятий больше, чем 3 часа 

        public static void UpdateListenersByDate(LanguageClassesContext context)
        {
            IQueryable<Course> courses =
                context.Courses.Where(c => c.Intensity > 3);
            if (courses != null)
            {
                foreach (Course course in courses)
                {
                    course.GroupSize += 2;
                }
            }
            context.SaveChanges();
        }

        public static IEnumerable GetCourseIntensity(LanguageClassesContext context)
        {
            var query = context.Courses
                .Where(c => c.Intensity > 2)
                .Select(c =>
            new
            {
                Код_курса = c.Id,
                Интенсивность_занятий = c.Intensity,
                Размер_группы = c.GroupSize
            });

            return query.Take(10).ToList();
        }
    }
}
