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
                using var connection = Database.GetConnection();
                connection.Open();

                MessageBox.Show(
                    "Подключение к базе данных успешно!",
                    "Чудо Обувь",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Ошибка подключения к базе данных:\n" + ex.Message,
                    "Ошибка",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }
    }
}
