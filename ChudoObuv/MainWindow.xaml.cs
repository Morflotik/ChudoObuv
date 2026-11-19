using System;
using System.Windows;

namespace ChudoObuv
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            try
            {
                var products = Database.GetProducts();

                MessageBox.Show(
                    $"Подключение успешно!\nНайдено товаров: {products.Rows.Count}");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
