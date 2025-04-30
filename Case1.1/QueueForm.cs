using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;

namespace Case1._1
{
    public partial class QueueForm : Form
    {
        private readonly DatabaseConnection dbConnection = DatabaseConnection.Instance;
        private readonly string reportsDirectory = Path.Combine(Application.StartupPath, "ProductionPlans");

        public QueueForm()
        {
            InitializeComponent();
            Load += QueueForm_Load;
        }

        private void QueueForm_Load(object sender, EventArgs e)
        {
            dbConnection.OpenConnection();
            ConfigureDataGridView();
            LoadOrders();
            dbConnection.CloseConnection();
        }

        private void ConfigureDataGridView()
        {
            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.Columns.Clear();

            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "OrderId",
                HeaderText = "ID заказа",
                Width = 80
            });

            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "SteelGrade",
                HeaderText = "Марка стали",
                Width = 100
            });

            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Quantity",
                HeaderText = "Количество (тонн)",
                Width = 120
            });

            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Price",
                HeaderText = "Цена (руб)",
                Width = 100
            });

            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "OrderDate",
                HeaderText = "Дата заказа",
                Width = 120
            });

            dataGridView1.Columns.Add(new DataGridViewCheckBoxColumn
            {
                DataPropertyName = "IsCompleted",
                HeaderText = "Выполнен",
                Width = 80,
                ReadOnly = true
            });
        }

        private void LoadOrders()
        {
            try
            {
                Directory.CreateDirectory(reportsDirectory);
                var orders = new List<OrderViewModel>();

                LoadOrdersFromTable("MakeMarkA", "Марка A", "OreMarkA", "NikelMarkA", "ChromeMarkA", "MarganecMarkA", "PriceA", orders);
                LoadOrdersFromTable("MakeMarkB", "Марка B", "OreMarkB", "NikelMarkB", "ChromeMarkB", "MarganecMarkB", "PriceB", orders);
                LoadOrdersFromTable("MakeMarkC", "Марка C", "OreMarkC", "NikelMarkC", "ChromeMarkC", "MarganecMarkC", "PriceC", orders);

                foreach (var order in orders)
                {
                    string reportFileName = Path.Combine(reportsDirectory, $"Производственный_план_{order.OrderId}.txt");
                    order.IsCompleted = File.Exists(reportFileName) || order.OrderId == MakeOrderForm.LastCompletedOrderId;
                }

                dataGridView1.DataSource = orders.OrderByDescending(o => o.OrderDate).ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке заказов: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadOrdersFromTable(string tableName, string steelGrade,
            string oreColumn, string nikelColumn, string chromeColumn,
            string marganecColumn, string priceColumn, List<OrderViewModel> orders)
        {
            string query = $"SELECT Id, {oreColumn}, {nikelColumn}, {chromeColumn}, {marganecColumn}, {priceColumn} FROM {tableName}";

            using (var command = new SqlCommand(query, dbConnection.Connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        orders.Add(new OrderViewModel
                        {
                            OrderId = reader.GetInt32(0),
                            SteelGrade = steelGrade,
                            Quantity = Convert.ToSingle(reader[1]),
                            Price = Convert.ToSingle(reader[5]),
                            OrderDate = DateTime.Now
                        });
                    }
                }
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            dbConnection.OpenConnection();
            LoadOrders();
            dbConnection.CloseConnection();
        }

        private void QueueForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            dbConnection.CloseConnection();
        }
    }

    public class OrderViewModel
    {
        public int OrderId { get; set; }
        public string SteelGrade { get; set; }
        public float Quantity { get; set; }
        public float Price { get; set; }
        public DateTime OrderDate { get; set; }
        public bool IsCompleted { get; set; }
    }
}