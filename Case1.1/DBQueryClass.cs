using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace Case1._1
{
    public interface IMySqlConnection 
    {
        public void SaveSelectedValueWarehouseToDatabase(float ore, float nikel, float chrome, float marganec, string timeFurnace, string timeConverter, string timeRollingMachine);
        public void SaveSelectedValueOrderMarkAToDatabase(float ore, float nikel, float chrome, float marganec, string timeFurnace, string timeConverter, string timeRollingMachine, float price);
        public void SaveSelectedValueOrderMarkBToDatabase(float ore, float nikel, float chrome, float marganec, string timeFurnace, string timeConverter, string timeRollingMachine, float price);
        public void SaveSelectedValueOrderMarkCToDatabase(float ore, float nikel, float chrome, float marganec, string timeFurnace, string timeConverter, string timeRollingMachine,float price);
    }

    public class DatabaseConnection
    {
        private static DatabaseConnection _instance;
        private static readonly object _lock = new object();
        private SqlConnection _connection;

        // Убедитесь, что конструктор недоступен для внешних классов
        private DatabaseConnection()
        {
            string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\User\\source\\repos\\Case1FactoryK\\Case1.1\\Database1.mdf;Integrated Security=True"; // Замените на вашу строку подключения
            _connection = new SqlConnection(connectionString);
        }

        public static DatabaseConnection Instance
        {
            get
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new DatabaseConnection();
                    }
                    return _instance;
                }
            }
        }

        public SqlConnection Connection
        {
            get
            {
                return _connection;
            }
        }

        public void OpenConnection()
        {
            if (_connection.State == System.Data.ConnectionState.Closed)
            {
                _connection.Open();
            }
        }

        public void CloseConnection()
        {
            if (_connection.State == System.Data.ConnectionState.Open)
            {
                _connection.Close();
            }
        }
    }
    public class DBQueryClass : IMySqlConnection
    {
        private readonly DatabaseConnection dbConnection = DatabaseConnection.Instance;

        public void SaveSelectedValueWarehouseToDatabase(float ore, float nikel, float chrome, float marganec,
            string timeFurnace, string timeConverter, string timeRollingMachine)
        {
            using var command = new SqlCommand(
                "INSERT INTO Warehouse(Ore, Nikel, Chrome, Marganec, TimeFurnace, TimeConverter, TimeRollingMachine) " +
                "VALUES (@Ore, @Nikel, @Chrome, @Marganec, @TimeFurnace, @TimeConverter, @TimeRollingMachine)",
                dbConnection.Connection);

            command.Parameters.AddWithValue("@Ore", ore);
            command.Parameters.AddWithValue("@Nikel", nikel);
            command.Parameters.AddWithValue("@Chrome", chrome);
            command.Parameters.AddWithValue("@Marganec", marganec);
            command.Parameters.AddWithValue("@TimeFurnace", timeFurnace);
            command.Parameters.AddWithValue("@TimeConverter", timeConverter);
            command.Parameters.AddWithValue("@TimeRollingMachine", timeRollingMachine);

            command.ExecuteNonQuery();
        }

        public (double Ore, double Nickel, double Chrome, double Manganese) GetTotalResources()
        {
            using var command = new SqlCommand(
                "SELECT SUM(Ore) AS TotalOre, SUM(Nikel) AS TotalNickel, " +
                "SUM(Chrome) AS TotalChrome, SUM(Marganec) AS TotalManganese " +
                "FROM Warehouse",
                dbConnection.Connection);

            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                return (
                    reader.IsDBNull(0) ? 0 : reader.GetDouble(0),
                    reader.IsDBNull(1) ? 0 : reader.GetDouble(1),
                    reader.IsDBNull(2) ? 0 : reader.GetDouble(2),
                    reader.IsDBNull(3) ? 0 : reader.GetDouble(3)
                );
            }
            return (0, 0, 0, 0);
        }

        public void SaveOrderWithResourcesCheck(Dictionary<string, int> orderQuantities)
        {
            if (!CheckResourcesForOrder(orderQuantities))
            {
                throw new Exception("Недостаточно ресурсов на складе для выполнения заказа");
            }

            var (totalOreNeeded, totalNickelNeeded, totalChromeNeeded, totalManganeseNeeded) = CalculateResourcesNeeded(orderQuantities);

            foreach (var order in orderQuantities)
            {
                SaveOrder(order.Key, order.Value);
            }

            DeductResources(totalOreNeeded, totalNickelNeeded, totalChromeNeeded, totalManganeseNeeded);
        }

        private bool CheckResourcesForOrder(Dictionary<string, int> orderQuantities)
        {
            var resources = GetTotalResources();
            var needed = CalculateResourcesNeeded(orderQuantities);

            return resources.Ore >= needed.OreNeeded &&
                   resources.Nickel >= needed.NickelNeeded &&
                   resources.Chrome >= needed.ChromeNeeded &&
                   resources.Manganese >= needed.ManganeseNeeded;
        }

        private (double OreNeeded, double NickelNeeded, double ChromeNeeded, double ManganeseNeeded)
            CalculateResourcesNeeded(Dictionary<string, int> orderQuantities)
        {
            double ore = 0, nickel = 0, chrome = 0, manganese = 0;

            foreach (var order in orderQuantities)
            {
                var grade = GetGradeParameters(order.Key);
                int quantity = order.Value;

                ore += grade.OrePerTon * quantity;
                nickel += grade.NickelPerTon * quantity;
                chrome += grade.ChromePerTon * quantity;
                manganese += grade.ManganesePerTon * quantity;
            }

            return (ore, nickel, chrome, manganese);
        }

        private void SaveOrder(string gradeName, int quantity)
        {
            var grade = GetGradeParameters(gradeName);
            string tableName = gradeName switch
            {
                "Марка A" => "MakeMarkA",
                "Марка B" => "MakeMarkB",
                "Марка C" => "MakeMarkC",
                _ => throw new ArgumentException("Неизвестная марка стали")
            };

            using var command = new SqlCommand(
                $"INSERT INTO {tableName}(Ore, Nikel, Chrome, Marganec, TimeFurnace, TimeConverter, TimeRollingMachine) " +
                "VALUES (@Ore, @Nikel, @Chrome, @Marganec, @TimeFurnace, @TimeConverter, @TimeRollingMachine)",
                dbConnection.Connection);

            command.Parameters.AddWithValue("@Ore", grade.OrePerTon * quantity);
            command.Parameters.AddWithValue("@Nikel", grade.NickelPerTon * quantity);
            command.Parameters.AddWithValue("@Chrome", grade.ChromePerTon * quantity);
            command.Parameters.AddWithValue("@Marganec", grade.ManganesePerTon * quantity);
            command.Parameters.AddWithValue("@TimeFurnace", (grade.FurnaceTimePerTon * quantity).ToString());
            command.Parameters.AddWithValue("@TimeConverter", (grade.ConverterTimePerTon * quantity).ToString());
            command.Parameters.AddWithValue("@TimeRollingMachine", (grade.RollingMachineTimePerTon * quantity).ToString());

            command.ExecuteNonQuery();
        }

        private void DeductResources(double ore, double nickel, double chrome, double manganese)
        {
            SaveSelectedValueWarehouseToDatabase(
                -(int)Math.Ceiling(ore),
                -(int)Math.Ceiling(nickel),
                -(int)Math.Ceiling(chrome),
                -(int)Math.Ceiling(manganese),
                "00:00:00", "00:00:00", "00:00:00");
        }

        private SteelGradeParameters GetGradeParameters(string gradeName)
        {
            return gradeName switch
            {
                "Марка A" => SteelGrades.GradeA,
                "Марка B" => SteelGrades.GradeB,
                "Марка C" => SteelGrades.GradeC,
                _ => throw new ArgumentException("Неизвестная марка стали")
            };
        }

        private bool CheckResourcesAvailability(float ore, float nikel, float chrome, float marganec)
        {
            var resources = GetTotalResources();
            return resources.Ore >= ore &&
                   resources.Nickel >= nikel &&
                   resources.Chrome >= chrome &&
                   resources.Manganese >= marganec;
        }

        public void SaveSelectedValueOrderMarkAToDatabase(float ore, float nikel, float chrome, float marganec,
            string timeFurnace, string timeConverter, string timeRollingMachine, float price)
        {
            // Получаем текущие ресурсы
            var resources = GetTotalResources();

            // Проверяем, хватает ли ресурсов
            if (resources.Ore < ore || resources.Nickel < nikel ||
                resources.Chrome < chrome || resources.Manganese < marganec)
            {
                throw new Exception($"Недостаточно ресурсов на складе для создания Марки A. Требуется: {ore} руды, {nikel} никеля, {chrome} хрома, {marganec} марганца. На складе: {resources.Ore} руды, {resources.Nickel} никеля, {resources.Chrome} хрома, {resources.Manganese} марганца.");
            }

            // Сохраняем заказ
            using var command = new SqlCommand(
                "INSERT INTO MakeMarkA(OreMarkA, NikelMarkA, ChromeMarkA, MarganecMarkA, " +
                "TimeFurnaceMarkA, TimeConverterMarkA, TimeRollingMachineA, PriceA) " +
                "VALUES (@OreMarkA, @NikelMarkA, @ChromeMarkA, @MarganecMarkA, @TimeFurnaceMarkA, @TimeConverterMarkA, @TimeRollingMachineA,@PriceA)",
                dbConnection.Connection);

            // Исправленные имена параметров, соответствующие именам столбцов в таблице
            command.Parameters.AddWithValue("@OreMarkA", ore);
            command.Parameters.AddWithValue("@NikelMarkA", nikel);
            command.Parameters.AddWithValue("@ChromeMarkA", chrome);
            command.Parameters.AddWithValue("@MarganecMarkA", marganec);
            command.Parameters.AddWithValue("@TimeFurnaceMarkA", timeFurnace);
            command.Parameters.AddWithValue("@TimeConverterMarkA", timeConverter);
            command.Parameters.AddWithValue("@TimeRollingMachineA", timeRollingMachine);
            command.Parameters.AddWithValue("@PriceA", price);

            command.ExecuteNonQuery();

            // Списание ресурсов
            SaveSelectedValueWarehouseToDatabase(
                -ore, -nikel, -chrome, -marganec,
                "00:00:00", "00:00:00", "00:00:00");
        }

        public void SaveSelectedValueOrderMarkBToDatabase(float ore, float nikel, float chrome, float marganec,
            string timeFurnace, string timeConverter, string timeRollingMachine, float price)
        {
            var resources = GetTotalResources();

            if (resources.Ore < ore || resources.Nickel < nikel ||
                resources.Chrome < chrome || resources.Manganese < marganec)
            {
                throw new Exception($"Недостаточно ресурсов на складе для создания Марки B. Требуется: {ore} руды, {nikel} никеля, {chrome} хрома, {marganec} марганца. На складе: {resources.Ore} руды, {resources.Nickel} никеля, {resources.Chrome} хрома, {resources.Manganese} марганца.");
            }

            using var command = new SqlCommand(
                "INSERT INTO MakeMarkB(OreMarkB, NikelMarkB, ChromeMarkB, MarganecMarkB, " +
                "TimeFurnaceMarkB, TimeConverterMarkB, TimeRollingMachineB, PriceB) " +
                "VALUES (@OreMarkB, @NikelMarkB, @ChromeMarkB, @MarganecMarkB, @TimeFurnaceMarkB, @TimeConverterMarkB, @TimeRollingMachineB,@PriceB)",
                dbConnection.Connection);

            command.Parameters.AddWithValue("@OreMarkB", ore);
            command.Parameters.AddWithValue("@NikelMarkB", nikel);
            command.Parameters.AddWithValue("@ChromeMarkB", chrome);
            command.Parameters.AddWithValue("@MarganecMarkB", marganec);
            command.Parameters.AddWithValue("@TimeFurnaceMarkB", timeFurnace);
            command.Parameters.AddWithValue("@TimeConverterMarkB", timeConverter);
            command.Parameters.AddWithValue("@TimeRollingMachineB", timeRollingMachine);
            command.Parameters.AddWithValue("@PriceB", price);

            command.ExecuteNonQuery();

            SaveSelectedValueWarehouseToDatabase(
                -ore, -nikel, -chrome, -marganec,
                "00:00:00", "00:00:00", "00:00:00");
        }

        public void SaveSelectedValueOrderMarkCToDatabase(float ore, float nikel, float chrome, float marganec,
            string timeFurnace, string timeConverter, string timeRollingMachine, float price)
        {
            var resources = GetTotalResources();

            if (resources.Ore < ore || resources.Nickel < nikel ||
                resources.Chrome < chrome || resources.Manganese < marganec)
            {
                throw new Exception($"Недостаточно ресурсов на складе для создания Марки C. Требуется: {ore} руды, {nikel} никеля, {chrome} хрома, {marganec} марганца. На складе: {resources.Ore} руды, {resources.Nickel} никеля, {resources.Chrome} хрома, {resources.Manganese} марганца.");
            }

            using var command = new SqlCommand(
                "INSERT INTO MakeMarkC(OreMarkC, NikelMarkC, ChromeMarkC, MarganecMarkC, " +
                "TimeFurnaceMarkC, TimeConverterMarkC, TimeRollingMachineC, PriceC) " +
                "VALUES (@OreMarkC, @NikelMarkC, @ChromeMarkC, @MarganecMarkC, @TimeFurnaceMarkC, @TimeConverterMarkC, @TimeRollingMachineC,@PriceC)",
                dbConnection.Connection);

            command.Parameters.AddWithValue("@OreMarkC", ore);
            command.Parameters.AddWithValue("@NikelMarkC", nikel);
            command.Parameters.AddWithValue("@ChromeMarkC", chrome);
            command.Parameters.AddWithValue("@MarganecMarkC", marganec);
            command.Parameters.AddWithValue("@TimeFurnaceMarkC", timeFurnace);
            command.Parameters.AddWithValue("@TimeConverterMarkC", timeConverter);
            command.Parameters.AddWithValue("@TimeRollingMachineC", timeRollingMachine);
            command.Parameters.AddWithValue("@PriceC", price);

            command.ExecuteNonQuery();

            SaveSelectedValueWarehouseToDatabase(
                -ore, -nikel, -chrome, -marganec,
                "00:00:00", "00:00:00", "00:00:00");
        }
        
    }
}

