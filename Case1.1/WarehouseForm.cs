using System.Windows.Forms;

namespace Case1._1
{
    public partial class WarehouseForm : Form
    {
        private readonly DBQueryClass dBQueryClass = new DBQueryClass();
        private readonly DatabaseConnection dbConnection = DatabaseConnection.Instance;

        public WarehouseForm()
        {
            InitializeComponent();
            dbConnection.OpenConnection();

            // Установка подсказок для текстовых полей
            SetPlaceholderText();

            // Обработчики событий для работы с placeholder-текстом
            txtOre.Enter += RemovePlaceholderText;
            txtOre.Leave += SetPlaceholderTextIfEmpty;
            txtNikel.Enter += RemovePlaceholderText;
            txtNikel.Leave += SetPlaceholderTextIfEmpty;
            txtChrome.Enter += RemovePlaceholderText;
            txtChrome.Leave += SetPlaceholderTextIfEmpty;
            txtMarganec.Enter += RemovePlaceholderText;
            txtMarganec.Leave += SetPlaceholderTextIfEmpty;
            txtTimeFurnace.Enter += RemovePlaceholderText;
            txtTimeFurnace.Leave += SetPlaceholderTextIfEmpty;
            txtTimeConverter.Enter += RemovePlaceholderText;
            txtTimeConverter.Leave += SetPlaceholderTextIfEmpty;
            txtTimeRollingMachine.Enter += RemovePlaceholderText;
            txtTimeRollingMachine.Leave += SetPlaceholderTextIfEmpty;
        }

        private void SetPlaceholderText()
        {
            // Установка подсказок для каждого поля
            SetPlaceholder(txtOre, "Введите количество руды (тонны)");
            SetPlaceholder(txtNikel, "Введите количество никеля (кг)");
            SetPlaceholder(txtChrome, "Введите количество хрома (кг)");
            SetPlaceholder(txtMarganec, "Введите количество марганца (кг)");
            SetPlaceholder(txtTimeFurnace, "Введите время работы печи (часы)");
            SetPlaceholder(txtTimeConverter, "Введите время работы конвертера (часы)");
            SetPlaceholder(txtTimeRollingMachine, "Введите время работы стана (часы)");
        }

        private void SetPlaceholder(TextBox textBox, string placeholder)
        {
            if (string.IsNullOrEmpty(textBox.Text))
            {
                textBox.Text = placeholder;
                textBox.ForeColor = System.Drawing.Color.Gray;
            }
        }

        private void RemovePlaceholderText(object sender, System.EventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox.ForeColor == System.Drawing.Color.Gray)
            {
                textBox.Text = "";
                textBox.ForeColor = System.Drawing.Color.Black;
            }
        }

        private void SetPlaceholderTextIfEmpty(object sender, System.EventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (string.IsNullOrEmpty(textBox.Text))
            {
                SetPlaceholderText();
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!ValidateInputs(out float ore, out float nikel, out float chrome, out float marganec,
                              out float timeFurnace, out float timeConverter, out float timeRolling))
            {
                return;
            }

            dBQueryClass.SaveSelectedValueWarehouseToDatabase(ore, nikel, chrome, marganec,
                timeFurnace, timeConverter, timeRolling);

            MessageBox.Show("Данные успешно сохранены");
            dbConnection.CloseConnection();
            Close();
        }

        private bool ValidateInputs(out float ore, out float nikel, out float chrome, out float marganec,
                                   out float timeFurnace, out float timeConverter, out float timeRolling)
        {
            ore = nikel = chrome = marganec = timeFurnace = timeConverter = timeRolling = 0;

            if (!ValidateTextBox(txtOre, "количество руды", out ore)) return false;
            if (!ValidateTextBox(txtNikel, "количество никеля", out nikel)) return false;
            if (!ValidateTextBox(txtChrome, "количество хрома", out chrome)) return false;
            if (!ValidateTextBox(txtMarganec, "количество марганца", out marganec)) return false;
            if (!ValidateTextBox(txtTimeFurnace, "время работы печи", out timeFurnace)) return false;
            if (!ValidateTextBox(txtTimeConverter, "время работы конвертера", out timeConverter)) return false;
            if (!ValidateTextBox(txtTimeRollingMachine, "время работы прокатного стана", out timeRolling)) return false;

            return true;
        }

        private bool ValidateTextBox(TextBox textBox, string fieldName, out float value)
        {
            value = 0;
            if (textBox.ForeColor == System.Drawing.Color.Gray ||
                !float.TryParse(textBox.Text, out value) || value < 0)
            {
                MessageBox.Show($"Введите корректное {fieldName} (число ≥0)", "Ошибка ввода");
                textBox.Focus();
                return false;
            }
            return true;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            dbConnection.CloseConnection();
            Close();
        }
    }
}