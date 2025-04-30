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
        public void SaveSelectedValueWarehouseToDatabase(float ore, float nikel, float chrome, float marganec,
            float timeFurnace, float timeConverter, float timeRollingMachine);

        public void SaveSelectedValueOrderMarkAToDatabase(float ore, float nikel, float chrome, float marganec,
            float timeFurnace, float timeConverter, float timeRollingMachine, float price);

        public void SaveSelectedValueOrderMarkBToDatabase(float ore, float nikel, float chrome, float marganec,
            float timeFurnace, float timeConverter, float timeRollingMachine, float price);

        public void SaveSelectedValueOrderMarkCToDatabase(float ore, float nikel, float chrome, float marganec,
            float timeFurnace, float timeConverter, float timeRollingMachine, float price);

        public (double Ore, double Nickel, double Chrome, double Manganese) GetLastUserResources(SqlTransaction transaction = null);
        
        public void DeductResourcesFromWarehouse(float ore, float nikel, float chrome, float marganec, SqlTransaction transaction = null);
        
    }

    public class DatabaseConnection
    {
        private static DatabaseConnection _instance;
        private static readonly object _lock = new object();
        private SqlConnection _connection;

        // Убедитесь, что конструктор недоступен для внешних классов
        private DatabaseConnection()
        {
            string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\Кирилл\\Source\\Repos\\ConsoleAppCase2\\Case1.1\\Database1.mdf;Integrated Security=True"; // Замените на вашу строку подключения
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

        public (double Ore, double Nickel, double Chrome, double Manganese) GetLastUserResources(SqlTransaction transaction = null)
        {
            using var command = new SqlCommand(
                "SELECT TOP 1 Ore, Nikel, Chrome, Marganec FROM Warehouse ORDER BY Id DESC",
                dbConnection.Connection,
                transaction);

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

        public (float TimeFurnace, float TimeConverter, float TimeRollingMachine) GetAvailableTime(SqlTransaction transaction = null)
        {
            using var command = new SqlCommand(
                "SELECT TOP 1 TimeFurnace, TimeConverter, TimeRollingMachine FROM Warehouse ORDER BY Id DESC",
                dbConnection.Connection,
                transaction);

            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                return (
                    (float)Convert.ToDouble(reader["TimeFurnace"]),
                    (float)Convert.ToDouble(reader["TimeConverter"]),
                    (float)Convert.ToDouble(reader["TimeRollingMachine"])
                );
            }
            return (0, 0, 0);
        }

        public (bool IsEnough, string Message) CheckResourcesAndTime(
            float ore, float nikel, float chrome, float marganec,
            float timeFurnace, float timeConverter, float timeRollingMachine,
            SqlTransaction transaction = null)
        {
            var errorMessage = new StringBuilder();
            bool resourcesEnough = true;
            bool timeEnough = true;

            // Проверка ресурсов
            var resources = GetLastUserResources(transaction);
            if (resources.Ore < ore)
            {
                errorMessage.AppendLine($"- Руда: не хватает {ore - resources.Ore:0.##} т (есть {resources.Ore:0.##} т)");
                resourcesEnough = false;
            }
            if (resources.Nickel < nikel)
            {
                errorMessage.AppendLine($"- Никель: не хватает {nikel - resources.Nickel:0.##} кг (есть {resources.Nickel:0.##} кг)");
                resourcesEnough = false;
            }
            if (resources.Chrome < chrome)
            {
                errorMessage.AppendLine($"- Хром: не хватает {chrome - resources.Chrome:0.##} кг (есть {resources.Chrome:0.##} кг)");
                resourcesEnough = false;
            }
            if (resources.Manganese < marganec)
            {
                errorMessage.AppendLine($"- Марганец: не хватает {marganec - resources.Manganese:0.##} кг (есть {resources.Manganese:0.##} кг)");
                resourcesEnough = false;
            }

            // Проверка времени
            var availableTime = GetAvailableTime(transaction);
            if (timeFurnace > availableTime.TimeFurnace)
            {
                errorMessage.AppendLine($"- Печь: не хватает {timeFurnace - availableTime.TimeFurnace:0.##} ч (есть {availableTime.TimeFurnace:0.##} ч)");
                timeEnough = false;
            }
            if (timeConverter > availableTime.TimeConverter)
            {
                errorMessage.AppendLine($"- Конвертер: не хватает {timeConverter - availableTime.TimeConverter:0.##} ч (есть {availableTime.TimeConverter:0.##} ч)");
                timeEnough = false;
            }
            if (timeRollingMachine > availableTime.TimeRollingMachine)
            {
                errorMessage.AppendLine($"- Прокатный стан: не хватает {timeRollingMachine - availableTime.TimeRollingMachine:0.##} ч (есть {availableTime.TimeRollingMachine:0.##} ч)");
                timeEnough = false;
            }

            string message = "";
            if (!resourcesEnough || !timeEnough)
            {
                message = "Недостаточно ресурсов или времени:\n" + errorMessage.ToString();
            }

            return (resourcesEnough && timeEnough, message);
        }

        public void SaveSelectedValueWarehouseToDatabase(float ore, float nikel, float chrome, float marganec,
            float timeFurnace, float timeConverter, float timeRollingMachine)
        {
            using var transaction = dbConnection.Connection.BeginTransaction();
            try
            {
                using var command = new SqlCommand(
                    "INSERT INTO Warehouse (Ore, Nikel, Chrome, Marganec, TimeFurnace, TimeConverter, TimeRollingMachine) " +
                    "VALUES (@Ore, @Nikel, @Chrome, @Marganec, @TimeFurnace, @TimeConverter, @TimeRollingMachine)",
                    dbConnection.Connection,
                    transaction);

                command.Parameters.AddWithValue("@Ore", ore);
                command.Parameters.AddWithValue("@Nikel", nikel);
                command.Parameters.AddWithValue("@Chrome", chrome);
                command.Parameters.AddWithValue("@Marganec", marganec);
                command.Parameters.AddWithValue("@TimeFurnace", timeFurnace);
                command.Parameters.AddWithValue("@TimeConverter", timeConverter);
                command.Parameters.AddWithValue("@TimeRollingMachine", timeRollingMachine);

                command.ExecuteNonQuery();
                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        public void SaveSelectedValueOrderMarkAToDatabase(float ore, float nikel, float chrome, float marganec,
            float timeFurnace, float timeConverter, float timeRollingMachine, float price)
        {
            ProcessOrder("MakeMarkA", ore, nikel, chrome, marganec,
                timeFurnace, timeConverter, timeRollingMachine, price,
                "OreMarkA", "NikelMarkA", "ChromeMarkA", "MarganecMarkA",
                "TimeFurnaceMarkA", "TimeConverterMarkA", "TimeRollingMachineA", "PriceA");
        }

        public void SaveSelectedValueOrderMarkBToDatabase(float ore, float nikel, float chrome, float marganec,
            float timeFurnace, float timeConverter, float timeRollingMachine, float price)
        {
            ProcessOrder("MakeMarkB", ore, nikel, chrome, marganec,
                timeFurnace, timeConverter, timeRollingMachine, price,
                "OreMarkB", "NikelMarkB", "ChromeMarkB", "MarganecMarkB",
                "TimeFurnaceMarkB", "TimeConverterMarkB", "TimeRollingMachineB", "PriceB");
        }

        public void SaveSelectedValueOrderMarkCToDatabase(float ore, float nikel, float chrome, float marganec,
            float timeFurnace, float timeConverter, float timeRollingMachine, float price)
        {
            ProcessOrder("MakeMarkC", ore, nikel, chrome, marganec,
                timeFurnace, timeConverter, timeRollingMachine, price,
                "OreMarkC", "NikelMarkC", "ChromeMarkC", "MarganecMarkC",
                "TimeFurnaceMarkC", "TimeConverterMarkC", "TimeRollingMachineC", "PriceC");
        }

        private void ProcessOrder(string tableName, float ore, float nikel, float chrome, float marganec,
            float timeFurnace, float timeConverter, float timeRollingMachine, float price,
            string oreColumn, string nikelColumn, string chromeColumn, string marganecColumn,
            string timeFurnaceColumn, string timeConverterColumn, string timeRollingColumn, string priceColumn)
        {
            using (var transaction = dbConnection.Connection.BeginTransaction())
            {
                try
                {
                    // Проверка ресурсов и времени с детализацией
                    var check = CheckResourcesAndTime(ore, nikel, chrome, marganec,
                        timeFurnace, timeConverter, timeRollingMachine, transaction);

                    if (!check.IsEnough)
                    {
                        throw new Exception(check.Message);
                    }

                    // Добавление заказа
                    using (var insertCommand = new SqlCommand(
                        $"INSERT INTO {tableName} ({oreColumn}, {nikelColumn}, {chromeColumn}, {marganecColumn}, " +
                        $"{timeFurnaceColumn}, {timeConverterColumn}, {timeRollingColumn}, {priceColumn}) " +
                        $"VALUES (@Ore, @Nikel, @Chrome, @Marganec, @TimeFurnace, @TimeConverter, @TimeRolling, @Price)",
                        dbConnection.Connection,
                        transaction))
                    {
                        insertCommand.Parameters.AddWithValue("@Ore", ore);
                        insertCommand.Parameters.AddWithValue("@Nikel", nikel);
                        insertCommand.Parameters.AddWithValue("@Chrome", chrome);
                        insertCommand.Parameters.AddWithValue("@Marganec", marganec);
                        insertCommand.Parameters.AddWithValue("@TimeFurnace", timeFurnace);
                        insertCommand.Parameters.AddWithValue("@TimeConverter", timeConverter);
                        insertCommand.Parameters.AddWithValue("@TimeRolling", timeRollingMachine);
                        insertCommand.Parameters.AddWithValue("@Price", price);

                        insertCommand.ExecuteNonQuery();
                    }

                    // Списание ресурсов
                    DeductResourcesFromWarehouse(-ore, -nikel, -chrome, -marganec, transaction);

                    // Списание времени
                    using (var timeCommand = new SqlCommand(
                        "UPDATE Warehouse SET " +
                        "TimeFurnace = TimeFurnace - @FurnaceTime, " +
                        "TimeConverter = TimeConverter - @ConverterTime, " +
                        "TimeRollingMachine = TimeRollingMachine - @RollingTime " +
                        "WHERE Id = (SELECT TOP 1 Id FROM Warehouse ORDER BY Id DESC)",
                        dbConnection.Connection,
                        transaction))
                    {
                        timeCommand.Parameters.AddWithValue("@FurnaceTime", timeFurnace);
                        timeCommand.Parameters.AddWithValue("@ConverterTime", timeConverter);
                        timeCommand.Parameters.AddWithValue("@RollingTime", timeRollingMachine);
                        timeCommand.ExecuteNonQuery();
                    }

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception($"Ошибка при создании заказа {tableName}:\n{ex.Message}");
                }
            }
        }

        public void DeductResourcesFromWarehouse(float ore, float nikel, float chrome, float marganec,
            SqlTransaction transaction = null)
        {
            using var command = new SqlCommand(
                "UPDATE Warehouse SET " +
                "Ore = Ore + @Ore, " +
                "Nikel = Nikel + @Nikel, " +
                "Chrome = Chrome + @Chrome, " +
                "Marganec = Marganec + @Marganec " +
                "WHERE Id = (SELECT TOP 1 Id FROM Warehouse ORDER BY Id DESC)",
                dbConnection.Connection,
                transaction);

            command.Parameters.AddWithValue("@Ore", ore);
            command.Parameters.AddWithValue("@Nikel", nikel);
            command.Parameters.AddWithValue("@Chrome", chrome);
            command.Parameters.AddWithValue("@Marganec", marganec);

            command.ExecuteNonQuery();
        }
    }
}


