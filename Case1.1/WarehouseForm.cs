using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Case1._1
{
    
    public partial class WarehouseForm : Form
    {
        DBQueryClass dBQueryClass = new DBQueryClass();
        DatabaseConnection dbConnection = DatabaseConnection.Instance;
        private float ore;
        private float nikel;
        private float chrome;
        private float marganec;
        private float id;
        public WarehouseForm()
        {
            InitializeComponent();
            dbConnection.OpenConnection();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
            dbConnection.CloseConnection();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtTimeFurnace.Text) || string.IsNullOrEmpty(txtTimeConverter.Text) || string.IsNullOrEmpty(txtTimeRollingMachine.Text))
            {
                MessageBox.Show("Пожалуйста, введите время работы: печи,конвертора,прокатного стана.");
                return;
            }
            if (!float.TryParse(txtOre.Text, out ore) || ore < 0)
            {
                MessageBox.Show("Пожалуйста, введите кол-во  руды (положительное целое число).");
                return;
            }
            if (!float.TryParse(txtNikel.Text, out nikel) || nikel < 0)
            {
                MessageBox.Show("Пожалуйста, введите кол-во  никеля (положительное целое число).");
                return;
            }
            if (!float.TryParse(txtChrome.Text, out chrome) || chrome < 0)
            {
                MessageBox.Show("Пожалуйста, введите кол-во  хрома (положительное целое число).");
                return;
            }
            if (!float.TryParse(txtMarganec.Text, out marganec) || marganec < 0)
            {
                MessageBox.Show("Пожалуйста, введите кол-во  марганец (положительное целое число).");
                return;
            }
            dBQueryClass.SaveSelectedValueWarehouseToDatabase(  
         ore,      
         nikel,       
         chrome,       
        marganec,                
         txtTimeFurnace.Text,       
         txtTimeConverter.Text,       
         txtTimeRollingMachine.Text                
     );
            MessageBox.Show("Данные успешно записаны");

            dbConnection.CloseConnection();
        }
    }
}
