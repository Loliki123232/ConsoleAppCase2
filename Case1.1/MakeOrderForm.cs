using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Data;
using Microsoft.Data.SqlClient;

namespace Case1._1
{
    public partial class MakeOrderForm : Form
    {
        private readonly DBQueryClass dBQueryClass = new DBQueryClass();
        private readonly DatabaseConnection dbConnection = DatabaseConnection.Instance;
        public static int LastCompletedOrderId { get; private set; } = -1;
        bool flagcompleted = false;

        public MakeOrderForm()
        {
            InitializeComponent();
            dbConnection.OpenConnection();
            checkedListBox1.Items.AddRange(new[] { "Марка A", "Марка B", "Марка C" });
            checkedListBox1.CheckOnClick = true;
            checkedListBox1.ItemCheck += (s, e) => BeginInvoke((MethodInvoker)UpdateTextBoxes);

            flowLayoutPanel1.FlowDirection = FlowDirection.TopDown;
            flowLayoutPanel1.WrapContents = true;
            flowLayoutPanel1.AutoScroll = true;
        }

        private void UpdateTextBoxes()
        {
            flowLayoutPanel1.Controls.Clear();
            string[,] parameters = new string[7, 3]
            {
                { "1", "1", "1" }, // Ore
                { "1", "1", "1" }, // Nikel
                { "1", "1", "1" }, // Chrome
                { "1", "1", "1" }, // Marganec
                { "24", "24", "24" }, // TimeFurnace
                { "24", "24", "24" }, // TimeConverter
                { "24", "24", "24" }  // TimeRollingMachine
            };

            if (checkedListBox1.CheckedItems.Contains("Марка A"))
                AddMarkGroup(0, "Марка A", parameters);
            if (checkedListBox1.CheckedItems.Contains("Марка B"))
                AddMarkGroup(1, "Марка B", parameters);
            if (checkedListBox1.CheckedItems.Contains("Марка C"))
                AddMarkGroup(2, "Марка C", parameters);
        }

        private void AddMarkGroup(int markIndex, string markName, string[,] defaultValues)
        {
            var panel = new Panel { Width = 220, BorderStyle = BorderStyle.FixedSingle, Margin = new Padding(5) };
            var header = new Label { Text = markName, Dock = DockStyle.Top, Font = new Font(Font, FontStyle.Bold) };
            panel.Controls.Add(header);

            var paramsPanel = new Panel { Top = header.Height, Width = panel.Width, Padding = new Padding(5) };
            int topPos = 0;

            string[] paramNames = { "Руда (тонн)", "Никель (кг)", "Хром (кг)", "Марганец (кг)",
                                  "Время печи (ч)", "Время конвертера (ч)", "Время стана (ч)" };

            for (int i = 0; i < 7; i++)
            {
                var lbl = new Label { Text = paramNames[i], Top = topPos, Width = 190 };
                var txt = new TextBox { Text = defaultValues[i, markIndex], Top = topPos + 20, Width = 190, Tag = i };
                paramsPanel.Controls.AddRange(new Control[] { lbl, txt });
                topPos += 50;
            }

            var priceLbl = new Label { Text = "Цена (руб)", Top = topPos, Width = 190 };
            var priceTxt = new TextBox
            {
                Text = markName switch { "Марка A" => "10000", "Марка B" => "12000", _ => "8000" },
                Top = topPos + 20,
                Width = 190,
                Tag = "price"
            };
            topPos += 50;

            var qtyLbl = new Label { Text = "Количество (тонн)", Top = topPos, Width = 190 };
            var qtyTxt = new TextBox { Text = "1", Top = topPos + 20, Width = 190, Tag = "quantity" };

            paramsPanel.Controls.AddRange(new Control[] { priceLbl, priceTxt, qtyLbl, qtyTxt });
            paramsPanel.Height = topPos + 70;
            panel.Height = header.Height + paramsPanel.Height;
            panel.Controls.Add(paramsPanel);
            flowLayoutPanel1.Controls.Add(panel);
        }

        private int GetLastOrderId(string markName)
        {
            string tableName = markName switch
            {
                "Марка A" => "MakeMarkA",
                "Марка B" => "MakeMarkB",
                "Марка C" => "MakeMarkC",
                _ => throw new ArgumentException("Неизвестная марка стали")
            };

            string query = $"SELECT TOP 1 Id FROM {tableName} ORDER BY Id DESC";
            using (var command = new SqlCommand(query, dbConnection.Connection))
            {
                return Convert.ToInt32(command.ExecuteScalar());
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                var orders = new Dictionary<string, (float Qty, float Price, float[] Params)>();
                var totalResources = new Dictionary<string, float>
                {
                    { "Ore", 0 }, { "Nikel", 0 }, { "Chrome", 0 }, { "Marganec", 0 }
                };
                var totalTime = new Dictionary<string, float>
                {
                    { "Furnace", 0 }, { "Converter", 0 }, { "RollingMachine", 0 }
                };
                float totalProfit = 0;

                foreach (Panel panel in flowLayoutPanel1.Controls)
                {
                    var markName = panel.Controls[0].Text;
                    var controls = panel.Controls[1].Controls.OfType<TextBox>().ToList();

                    if (!float.TryParse(controls.First(c => c.Tag?.ToString() == "quantity").Text, out float qty) || qty <= 0)
                    {
                        MessageBox.Show($"Некорректное количество для {markName}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    if (!float.TryParse(controls.First(c => c.Tag?.ToString() == "price").Text, out float price) || price <= 0)
                    {
                        MessageBox.Show($"Некорректная цена для {markName}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    var @params = new float[7];
                    for (int i = 0; i < 7; i++)
                    {
                        if (!float.TryParse(controls[i].Text, out @params[i]) || @params[i] < 0)
                        {
                            MessageBox.Show($"Некорректное значение параметра {i + 1} для {markName}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }

                    orders[markName] = (qty, price, @params);
                    totalResources["Ore"] += @params[0] * qty;
                    totalResources["Nikel"] += @params[1] * qty;
                    totalResources["Chrome"] += @params[2] * qty;
                    totalResources["Marganec"] += @params[3] * qty;

                    totalTime["Furnace"] += @params[4] * qty;
                    totalTime["Converter"] += @params[5] * qty;
                    totalTime["RollingMachine"] += @params[6] * qty;

                    totalProfit += price * qty;
                }

                var errorMessage = new StringBuilder();
                var availableResources = dBQueryClass.GetLastUserResources();
                var availableTime = dBQueryClass.GetAvailableTime();

                if (totalResources["Ore"] > availableResources.Ore)
                    errorMessage.AppendLine($"- Руда: не хватает {totalResources["Ore"] - availableResources.Ore:0.##} т");
                if (totalResources["Nikel"] > availableResources.Nickel)
                    errorMessage.AppendLine($"- Никель: не хватает {totalResources["Nikel"] - availableResources.Nickel:0.##} кг");
                if (totalResources["Chrome"] > availableResources.Chrome)
                    errorMessage.AppendLine($"- Хром: не хватает {totalResources["Chrome"] - availableResources.Chrome:0.##} кг");
                if (totalResources["Marganec"] > availableResources.Manganese)
                    errorMessage.AppendLine($"- Марганец: не хватает {totalResources["Marganec"] - availableResources.Manganese:0.##} кг");
                if (totalTime["Furnace"] > availableTime.TimeFurnace)
                    errorMessage.AppendLine($"- Печь: не хватает {totalTime["Furnace"] - availableTime.TimeFurnace:0.##} ч");
                if (totalTime["Converter"] > availableTime.TimeConverter)
                    errorMessage.AppendLine($"- Конвертер: не хватает {totalTime["Converter"] - availableTime.TimeConverter:0.##} ч");
                if (totalTime["RollingMachine"] > availableTime.TimeRollingMachine)
                    errorMessage.AppendLine($"- Прокатный стан: не хватает {totalTime["RollingMachine"] - availableTime.TimeRollingMachine:0.##} ч");

                if (errorMessage.Length > 0)
                {
                    MessageBox.Show($"Недостаточно ресурсов или времени:\n{errorMessage}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var report = new StringBuilder()
                    .AppendLine("Оптимальный производственный план:")
                    .AppendLine(string.Join("\n", orders.Select(o => $"Марка {o.Key}: {o.Value.Qty} тонн")))
                    .AppendLine("\nИспользованные ресурсы:")
                    .AppendLine($"Руда: {totalResources["Ore"]:0.##} тонн")
                    .AppendLine($"Никель: {totalResources["Nikel"]:0.##} кг")
                    .AppendLine($"Хром: {totalResources["Chrome"]:0.##} кг")
                    .AppendLine($"Марганец: {totalResources["Marganec"]:0.##} кг")
                    .AppendLine("\nВремя работы оборудования:")
                    .AppendLine($"Печь: {totalTime["Furnace"]:0.##} ч")
                    .AppendLine($"Конвертер: {totalTime["Converter"]:0.##} ч")
                    .AppendLine($"Прокатный стан: {totalTime["RollingMachine"]:0.##} ч")
                    .AppendLine($"\nОбщая прибыль: {totalProfit:0.##} руб.");

                using (var sfd = new SaveFileDialog
                {
                    Filter = "Текстовые файлы|*.txt",
                    FileName = $"Производственный_план_{DateTime.Now:yyyyMMdd_HHmmss}.txt"
                })
                {
                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        File.WriteAllText(sfd.FileName, report.ToString());

                        foreach (var order in orders)
                        {
                            var (qty, price, @params) = order.Value;
                            try
                            {
                                switch (order.Key)
                                {
                                    case "Марка A":
                                        dBQueryClass.SaveSelectedValueOrderMarkAToDatabase(
                                            @params[0] * qty, @params[1] * qty, @params[2] * qty, @params[3] * qty,
                                            @params[4] * qty, @params[5] * qty, @params[6] * qty, price * qty);
                                        break;
                                    case "Марка B":
                                        dBQueryClass.SaveSelectedValueOrderMarkBToDatabase(
                                            @params[0] * qty, @params[1] * qty, @params[2] * qty, @params[3] * qty,
                                            @params[4] * qty, @params[5] * qty, @params[6] * qty, price * qty);
                                        break;
                                    case "Марка C":
                                        dBQueryClass.SaveSelectedValueOrderMarkCToDatabase(
                                            @params[0] * qty, @params[1] * qty, @params[2] * qty, @params[3] * qty,
                                            @params[4] * qty, @params[5] * qty, @params[6] * qty, price * qty);
                                        break;
                                }
                                LastCompletedOrderId = GetLastOrderId(order.Key);
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show($"Ошибка при сохранении {order.Key}:\n{ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                        }

                        MessageBox.Show("Все заказы успешно сохранены!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        flagcompleted = true;
                        btnSave.Enabled = false;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Критическая ошибка: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            dbConnection.CloseConnection();
            Close();
        }
    }
}