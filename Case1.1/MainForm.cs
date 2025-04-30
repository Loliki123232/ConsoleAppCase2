namespace Case1._1
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void btnWarehouse_Click(object sender, EventArgs e)
        {
            WarehouseForm warehouseForm = new WarehouseForm();
            warehouseForm.Show();
        }

        private void btnMakeOrder_Click(object sender, EventArgs e)
        {
            MakeOrderForm makeOrderForm = new MakeOrderForm();
            makeOrderForm.Show();
        }

        private void btnQueue_Click(object sender, EventArgs e)
        {

        }
    }
}
