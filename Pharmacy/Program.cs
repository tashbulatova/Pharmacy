using System.Data.SqlClient;
using System;

namespace my1
{
    class Program
    {
        static void Main(string[] args)
        {
            Menu();
        }

        public static void Menu()
        {
            Console.Clear();

            Console.WriteLine($"{"Автоматизированная система компании ООО Фармацевт"}\n{"Что бы вы хотели посмотреть? Напишите цифру для перехода"}\n\n" +
                $"{"1- Посмотреть весь список товаров"}\n{"2- Список аптек"}\n{"3- Список складов"}\n{"4-Список партий товара"}\n{"5-Выход из системы"}");

            int numberMenu = Convert.ToInt32(Console.ReadLine());

            switch (numberMenu)
            {
                case 1:
                    Products();
                    break;

                case 2:
                    Pharmacy();
                    break;

                case 3:
                    Warehouse();
                    break;

                case 4:
                    Party();
                    break;

                case 5:
                    Environment.Exit(0);
                    break;

                default:
                    Console.WriteLine("Неверное значение!");
                    break;
            }
        }
        public static void Products()
        {
            Console.Clear();

            SqlDataReader reader = Select("SELECT * FROM Product"); //выгружаем все товары из БД

            if (reader.HasRows) // если есть данные
            {
                // выводим названия столбцов
                Console.WriteLine($"{"№"}\t{"Наименование товара"}");

                while (reader.Read()) // построчно считываем данные
                {
                    object id = reader.GetValue(0);
                    object name = reader.GetValue(1);

                    Console.WriteLine($"{id} \t{name}");
                }
            }

            Console.WriteLine($"\n{"Выберете действие"}\n{"1-Посмотреть товары в выбранной аптеке"}\n{"2-Добавить товар"}\n{"3-Удалить товар"}\n{"4-Выйти в главное меню"}");
            int numberMenuProduct = Convert.ToInt32(Console.ReadLine());

            switch (numberMenuProduct)
            {

                case 1:
                    reader = Select("SELECT * FROM Pharmacy");
                    if (reader.HasRows)
                    {
                        Console.WriteLine($"{"№"}\t{"Наименование "}\t{"Адрес"}\t{"Телефон"}");

                        while (reader.Read())
                        {
                            object id = reader.GetValue(0);
                            object name = reader.GetValue(1);
                            object adress = reader.GetValue(2);
                            object phone = reader.GetValue(3);

                            Console.WriteLine($"{id} \t{name}\t{adress}\t{phone}");
                        }

                        Console.WriteLine("Выберете аптеку из списка и укажите код: ");
                        int idPharmacy = Convert.ToInt32(Console.ReadLine());

                        reader = Select($"SELECT id_Warehouse, Name INTO #Pharma FROM Warehouse WHERE id_Pharmacy = {idPharmacy}" +
                                        $"SELECT Product.Name, SUM(Party.Quantity) AS Quantity FROM Party " +
                                        $"INNER JOIN #Pharma on Party.id_Warehouse = #Pharma.id_Warehouse " +
                                        $"INNER JOIN Product on Party.id_Product = Product.id_Product " +
                                        $"GROUP BY Product.Name");

                        if (reader.HasRows)
                        {
                            Console.WriteLine($"{"№"}\t{"Наименование товара"}\t{"Количество"}");

                            while (reader.Read())
                            {
                                object name = reader.GetValue(0);
                                object quantity = reader.GetValue(1);

                                Console.WriteLine($"{name} \t{quantity}");
                            }
                        }

                        Console.WriteLine("Нажмите Enter");
                        Console.ReadKey();
                    }
                    else
                    {
                        Console.WriteLine("Данных нет");
                        Console.ReadKey();
                    }
                    break;

                case 2:
                    Console.Write("Введите наименование товара: ");
                    string nameProduct = Console.ReadLine();
                    Console.WriteLine(Insert($"INSERT INTO Product (Name) VALUES (N'{nameProduct}')"));
                    break;

                case 3:
                    Console.Write("Введите код товара для удаления: ");
                    int idProduct = Convert.ToInt32(Console.ReadLine());
                    Console.WriteLine(Delete($"DELETE FROM Product WHERE id_Product = {idProduct}"));
                    break;

                case 4:
                    Menu();
                    break;

                default:
                    Console.WriteLine("Такого пункта нет в меню!");
                    break;
            }

            Products();
        }

        public static void Pharmacy()
        {
            Console.Clear();

            SqlDataReader reader = Select("SELECT * FROM Pharmacy");

            if (reader.HasRows)
            {
                Console.WriteLine($"{"№"}\t{"Наименование "}\t{"Адрес"}\t{"Телефон"}");

                while (reader.Read())
                {
                    object id = reader.GetValue(0);
                    object name = reader.GetValue(1);
                    object adress = reader.GetValue(2);
                    object phone = reader.GetValue(3);

                    Console.WriteLine($"{id} \t{name}\t{adress}\t{phone}");
                }
            }

            Console.WriteLine($"{ "Аптеки"}\n{"Выберете действие"}\n{"1-Добавить аптеку"}\n{"2-Удалить аптеку"}\n{"3-Выйти в главное меню"}");
            int numberMenuProduct = Convert.ToInt32(Console.ReadLine());

            switch (numberMenuProduct)
            {
                case 1:
                    Console.Write("Введите наименование аптеки: ");
                    string namePharmacy = Console.ReadLine();

                    Console.Write("Введите адрес: ");
                    string adress = Console.ReadLine();

                    Console.Write("Введите телефон: ");
                    string phone = Console.ReadLine();

                    Console.WriteLine(Insert($"INSERT INTO Pharmacy (Name, Adress, Phone) VALUES (N'{namePharmacy}', N'{adress}', N'{phone}')"));
                    break;

                case 2:
                    Console.Write("Введите код аптеки для удаления: ");
                    int idPharmacy = Convert.ToInt32(Console.ReadLine());
                    Console.WriteLine(Delete($"DELETE FROM Pharmacy WHERE id_Pharmacy = {idPharmacy}"));
                    break;

                case 3:
                    Menu();
                    break;

                default:
                    Console.WriteLine("Такого пункта нет в меню!");
                    break;
            }

            Pharmacy();
        }

        public static void Warehouse()
        {
            Console.Clear();

            SqlDataReader reader = Select("SELECT id_Warehouse, Warehouse.Name, Pharmacy.Name FROM Warehouse, Pharmacy WHERE Warehouse.id_Pharmacy = Pharmacy.id_Pharmacy");

            if (reader.HasRows)
            {
                Console.WriteLine($"{"№"}\t{"Наименование аптеки"}\t{"Склад"}");

                while (reader.Read())
                {
                    object id = reader.GetValue(0);
                    object nameWarehouse = reader.GetValue(1);
                    object namePharmacy = reader.GetValue(2);

                    Console.WriteLine($"{id} \t{namePharmacy}\t{nameWarehouse}");
                }
            }

            Console.WriteLine($"{ "Склады"}\n{"Выберете действие"}\n{"1-Добавить склад"}\n{"2-Удалить склад"}\n{"3-Выйти в главное меню"}");
            int numberMenuProduct = Convert.ToInt32(Console.ReadLine());

            switch (numberMenuProduct)
            {
                case 1:
                    Console.Write("Введите наименование склада: ");
                    string nameWarehouse = Console.ReadLine();

                    Console.WriteLine("Выберете аптеку из списка: ");
                    reader = Select("SELECT * FROM Pharmacy");

                    if (reader.HasRows)
                    {
                        Console.WriteLine($"{"№"}\t{"Наименование "}\t{"Адрес"}\t{"Телефон"}");

                        while (reader.Read())
                        {
                            object id = reader.GetValue(0);
                            object name = reader.GetValue(1);
                            object adress = reader.GetValue(2);
                            object phone = reader.GetValue(3);

                            Console.WriteLine($"{id} \t{name}\t{adress}\t{phone}");
                        }
                    }

                    int idPharmacy = Convert.ToInt32(Console.ReadLine());

                    Console.WriteLine(Insert($"INSERT INTO Warehouse (Name, id_Pharmacy) VALUES (N'{nameWarehouse}', {idPharmacy})"));
                    break;

                case 2:
                    Console.Write("Введите код склада для удаления: ");
                    int idWarehouse = Convert.ToInt32(Console.ReadLine());
                    Console.WriteLine(Delete($"DELETE FROM Warehouse WHERE id_Warehouse = {idWarehouse}"));
                    break;

                case 3:
                    Menu();
                    break;

                default:
                    Console.WriteLine("Такого пункта нет в меню!");
                    break;
            }
            Warehouse();
        }

        public static void Party()
        {
            Console.Clear();

            SqlDataReader reader = Select("SELECT id_Party, Product.Name, Warehouse.Name, Quantity " +
                                           "FROM Party " +
                                           "INNER JOIN Product on Party.id_Product = Product.id_Product " +
                                           "INNER JOIN Warehouse on Party.id_Warehouse = Warehouse.id_Warehouse");

            if (reader.HasRows)
            {
                Console.WriteLine($"{"№"}\t{"Наименование товара"}\t{"Склад"}\t{"Количество"}");

                while (reader.Read())
                {
                    object id = reader.GetValue(0);
                    object nameProduct = reader.GetValue(1);
                    object nameWarehouse = reader.GetValue(2);
                    object quantity = reader.GetValue(3);

                    Console.WriteLine($"{id} \t{nameProduct}\t{nameWarehouse}\t{quantity}");
                }
            }

            Console.WriteLine($"{ "Склады"}\n{"Выберете действие"}\n{"1-Добавить партию товара"}\n{"2-Удалить партию товара"}\n{"3-Выйти в главное меню"}");
            int numberMenuProduct = Convert.ToInt32(Console.ReadLine());

            switch (numberMenuProduct)
            {
                case 1:
                    Console.WriteLine("Выберете товар из списка и напишите код: ");
                    reader = Select("SELECT * FROM Product");

                    if (reader.HasRows)
                    {
                        Console.WriteLine($"{"№"}\t{"Наименование "}");

                        while (reader.Read())
                        {
                            object id = reader.GetValue(0);
                            object name = reader.GetValue(1);

                            Console.WriteLine($"{id} \t{name}");
                        }
                    }

                    int idProduct = Convert.ToInt32(Console.ReadLine());
                    Console.WriteLine("Выберете склад и напишите код: ");
                    reader = Select($"SELECT id_Warehouse, Warehouse.Name, Pharmacy.Name FROM Warehouse, Pharmacy WHERE Warehouse.id_Pharmacy = Pharmacy.id_Pharmacy");

                    if (reader.HasRows)
                    {
                        Console.WriteLine($"{"№"}\t{"Наименование аптеки"}\t{"Склад"}");

                        while (reader.Read())
                        {
                            object id = reader.GetValue(0);
                            object nameWarehouse = reader.GetValue(1);
                            object namePharmacy = reader.GetValue(2);

                            Console.WriteLine($"{id} \t{namePharmacy}\t{nameWarehouse}");
                        }
                    }

                    int idWarehouse = Convert.ToInt32(Console.ReadLine());

                    Console.WriteLine("Введите количество: ");
                    int quantity = Convert.ToInt32(Console.ReadLine());
                    Console.WriteLine(Insert($"INSERT INTO Party (id_Product, id_Warehouse, Quantity) VALUES ({idProduct}, {idWarehouse}, {quantity})"));
                    break;

                case 2:
                    Console.Write("Введите код партии для удаления: ");
                    int idParty = Convert.ToInt32(Console.ReadLine());
                    Console.WriteLine(Delete($"DELETE FROM Party WHERE id_Party = {idParty}"));
                    break;

                case 3:
                    Menu();
                    break;

                default:
                    Console.WriteLine("Такого пункта нет в меню!");
                    break;
            }
            Party();
        }

        public static SqlDataReader Select(string sql)
        {
            try
            {
                SqlConnection conn = DBUtils.GetDBConnection();
                conn.Open();
                SqlCommand command = new SqlCommand(sql, conn);
                SqlDataReader reader = command.ExecuteReader();
                return reader;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static string Insert(string sql)
        {
            try
            {
                SqlConnection conn = DBUtils.GetDBConnection();
                conn.Open();
                SqlCommand command = new SqlCommand(sql, conn);
                int number = command.ExecuteNonQuery();
                string result = $"Добавлено объектов: {number}";
                return result;
            }
            catch (Exception)
            {
                throw;
            }

        }

        public static string Delete(string sql)
        {
            try
            {
                SqlConnection conn = DBUtils.GetDBConnection();
                conn.Open();
                SqlCommand command = new SqlCommand(sql, conn);
                int number = command.ExecuteNonQuery();
                string result = $"Удалено объектов: {number}";
                return result;
            }
            catch (Exception)
            {
                throw;
            }

        }


    }
}
