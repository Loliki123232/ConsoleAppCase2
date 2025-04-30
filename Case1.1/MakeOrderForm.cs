namespace Case1._1
{
    public partial class MakeOrderForm : Form
    {
        DBQueryClass dBQueryClass = new DBQueryClass();
        DatabaseConnection dbConnection = DatabaseConnection.Instance;

        public MakeOrderForm()
        {
            InitializeComponent();
            dbConnection.OpenConnection();
            // Заполняем CheckedListBox марками
            checkedListBox1.Items.AddRange(new object[] { "Марка A", "Марка B", "Марка C" });
            checkedListBox1.CheckOnClick = true;
            checkedListBox1.ItemCheck += CheckedListBox1_ItemCheck; // Подписка на событие изменения выбора

            // Настройка FlowLayoutPanel
            flowLayoutPanel1.FlowDirection = FlowDirection.TopDown;
            flowLayoutPanel1.WrapContents = true;
            flowLayoutPanel1.AutoScroll = true;
            flowLayoutPanel1.AutoSize = false;
            flowLayoutPanel1.Height = this.ClientSize.Height - 50;
        }

        private void CheckedListBox1_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            // Ждем, пока изменение применится (используем BeginInvoke)
            BeginInvoke((MethodInvoker)(() => UpdateTextBoxes()));
        }

        private void UpdateTextBoxes()
        {
            flowLayoutPanel1.Controls.Clear();
            flowLayoutPanel1.FlowDirection = FlowDirection.LeftToRight; // Горизонтальное расположение групп

            // Массив с названиями параметров для каждой марки
            string[,] nameValue = new string[7, 3]
            {
        { "Ore mark a", "Ore mark b", "Ore mark c" },
        { "Nikel mark a", "Nikel mark b", "Nikel mark c" },
        { "Chrome mark a", "Chrome mark b", "Chrome mark c" },
        { "Marganec mark a", "Marganec mark b", "Marganec mark c" },
        { "TimeFurnace mark a", "TimeFurnace mark b", "TimeFurnace mark c" },
        { "TimeConverter mark a", "TimeConverter mark b", "TimeConverter mark c" },
        { "TimeRollingMachine mark a", "TimeRollingMachine mark b", "TimeRollingMachine mark c" }
            };

            // Создаем группу для каждой выбранной марки
            if (checkedListBox1.CheckedItems.Contains("Марка A"))
                AddMarkGroup(0, "Марка A", nameValue);
            if (checkedListBox1.CheckedItems.Contains("Марка B"))
                AddMarkGroup(1, "Марка B", nameValue);
            if (checkedListBox1.CheckedItems.Contains("Марка C"))
                AddMarkGroup(2, "Марка C", nameValue);
        }

        private void AddMarkGroup(int columnIndex, string markName, string[,] values)
        {
            // Создаем панель для группы параметров марки
            Panel markPanel = new Panel
            {
                Width = 220,
                AutoSize = true,
                BorderStyle = BorderStyle.FixedSingle,
                Margin = new Padding(5)
            };

            // Добавляем заголовок марки
            Label header = new Label
            {
                Text = markName,
                Dock = DockStyle.Top,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font(Font, FontStyle.Bold),
                Height = 25
            };
            markPanel.Controls.Add(header);

            // Создаем контейнер для параметров
            Panel paramsPanel = new Panel
            {
                Location = new Point(0, header.Height),
                Width = markPanel.Width,
                AutoSize = true,
                Padding = new Padding(5)
            };

            int topPosition = 0;

            // Добавляем параметры марки
            for (int row = 0; row < 7; row++)
            {
                // Добавляем подпись к параметру
                Label paramLabel = new Label
                {
                    Text = $"Параметр {row + 1}:",
                    Width = 190,
                    Height = 15,
                    Location = new Point(5, topPosition)
                };
                topPosition += 20;

                var textBox = new TextBox
                {
                    Text = values[row, columnIndex],
                    Width = 190,
                    Height = 25,
                    Location = new Point(5, topPosition),
                    Tag = row
                };
                topPosition += 30;

                paramsPanel.Controls.Add(paramLabel);
                paramsPanel.Controls.Add(textBox);
            }

            // Добавляем поле для цены
            Label priceLabel = new Label
            {
                Text = $"Price {markName}:",
                Width = 190,
                Height = 15,
                Location = new Point(5, topPosition)
            };
            topPosition += 20;

            TextBox priceTextBox = new TextBox
            {
                Text = markName switch
                {
                    "Марка A" => "10000",
                    "Марка B" => "12000",
                    "Марка C" => "8000",
                    _ => "0"
                },
                Width = 190,
                Height = 25,
                Location = new Point(5, topPosition),
                Tag = "price"
            };
            topPosition += 30;
            paramsPanel.Controls.Add(priceLabel);
            paramsPanel.Controls.Add(priceTextBox);

            // Добавляем поле для количества
            Label quantityLabel = new Label
            {
                Text = "Количество (тонн):",
                Width = 190,
                Height = 15,
                Location = new Point(5, topPosition)
            };
            topPosition += 20;

            TextBox quantityTextBox = new TextBox
            {
                Text = "1",
                Width = 190,
                Height = 25,
                Location = new Point(5, topPosition),
                Tag = "quantity"
            };
            topPosition += 30;
            paramsPanel.Controls.Add(quantityLabel);
            paramsPanel.Controls.Add(quantityTextBox);

            paramsPanel.Height = topPosition + 5;
            markPanel.Controls.Add(paramsPanel);
            markPanel.Height = header.Height + paramsPanel.Height;

            flowLayoutPanel1.Controls.Add(markPanel);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
            dbConnection.CloseConnection();

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                var orderQuantities = new Dictionary<string, (float Quantity, float Price, Dictionary<string, float> Parameters)>();

                foreach (Panel markPanel in flowLayoutPanel1.Controls)
                {
                    string markName = markPanel.Controls[0].Text;
                    var paramsPanel = markPanel.Controls[1] as Panel;

                    // Получаем количество и цену
                    float quantity = float.Parse(paramsPanel.Controls.OfType<TextBox>()
                        .First(tb => tb.Tag?.ToString() == "quantity").Text);

                    float price = float.Parse(paramsPanel.Controls.OfType<TextBox>()
                        .First(tb => tb.Tag?.ToString() == "price").Text);

                    var parameters = new Dictionary<string, float>();
                    foreach (Control control in paramsPanel.Controls)
                    {
                        if (control is TextBox textBox &&
                            textBox.Tag?.ToString() != "quantity" &&
                            textBox.Tag?.ToString() != "price")
                        {
                            string paramName = ((Label)paramsPanel.Controls[paramsPanel.Controls.IndexOf(textBox) - 1]).Text;
                            parameters[paramName] = float.Parse(textBox.Text);
                        }
                    }

                    orderQuantities[markName] = (quantity, price, parameters);
                }

                foreach (var order in orderQuantities)
                {
                    var (quantity, price, parameters) = order.Value;

                    float totalOre = parameters["Параметр 1:"] * quantity;
                    float totalNikel = parameters["Параметр 2:"] * quantity;
                    float totalChrome = parameters["Параметр 3:"] * quantity;
                    float totalMarganec = parameters["Параметр 4:"] * quantity;

                    // Добавьте price в параметры сохранения (вам нужно обновить методы SaveSelectedValueOrderMark...)
                    switch (order.Key)
                    {
                        case "Марка A":
                            dBQueryClass.SaveSelectedValueOrderMarkAToDatabase(
                                totalOre, totalNikel, totalChrome, totalMarganec,
                                parameters["Параметр 5:"].ToString(),
                                parameters["Параметр 6:"].ToString(),
                                parameters["Параметр 7:"].ToString(),
                                price);
                            break;
                        case "Марка B":
                            dBQueryClass.SaveSelectedValueOrderMarkBToDatabase(
                                totalOre, totalNikel, totalChrome, totalMarganec,
                                parameters["Параметр 5:"].ToString(),
                                parameters["Параметр 6:"].ToString(),
                                parameters["Параметр 7:"].ToString(),
                                price);
                            break;
                        case "Марка C":
                            dBQueryClass.SaveSelectedValueOrderMarkCToDatabase(
                                totalOre, totalNikel, totalChrome, totalMarganec,
                                parameters["Параметр 5:"].ToString(),
                                parameters["Параметр 6:"].ToString(),
                                parameters["Параметр 7:"].ToString(),
                                price);
                            break;
                            // Аналогично для других марок
                    }
                }

                MessageBox.Show("Заказы успешно сохранены!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }
    }
    }