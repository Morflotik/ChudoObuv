
using System;
using System.Data;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ChudoObuv
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            string logoPath = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "Resources",
                "Чудо Обувь.png");

            if (File.Exists(logoPath))
            {
                LogoImage.Source = new BitmapImage(
                    new Uri(logoPath, UriKind.Absolute));
            }

            LoadProducts();
        }

        private void LoadProducts()
        {
            try
            {
                DataTable products = Database.GetProducts();

                ProductsPanel.Children.Clear();

                foreach (DataRow product in products.Rows)
                {
                    int quantity = Convert.ToInt32(
                        product["stock_quantity"]);

                    decimal price = Convert.ToDecimal(
                        product["price"]);

                    decimal discountMultiplier = Convert.ToDecimal(
                        product["discount_multiplier"]);

                    decimal finalPrice = price * discountMultiplier;

                    string stockText = quantity > 5
                        ? "много"
                        : "мало";

                    Border card = new Border
                    {
                        Margin = new Thickness(0, 0, 0, 15),
                        Padding = new Thickness(15),

                        Background = quantity <= 3
                            ? new SolidColorBrush(Color.FromRgb(255, 128, 128))
                            : new SolidColorBrush(Color.FromRgb(210, 246, 231)),

                        BorderBrush = new SolidColorBrush(
                            Color.FromRgb(112, 178, 175)),

                        BorderThickness = new Thickness(1),
                        CornerRadius = new CornerRadius(8),
                        HorizontalAlignment = HorizontalAlignment.Stretch
                    };

                    Grid content = new Grid();

                    content.ColumnDefinitions.Add(
                        new ColumnDefinition
                        {
                            Width = new GridLength(220)
                        });

                    content.ColumnDefinitions.Add(
                        new ColumnDefinition
                        {
                            Width = new GridLength(1, GridUnitType.Star)
                        });

                    Image photo = new Image
                    {
                        Height = 170,
                        Width = 200,
                        Stretch = Stretch.Uniform,
                        VerticalAlignment = VerticalAlignment.Center
                    };

                    string imageName = product["image_url"]?.ToString() ?? "";

                    string imagePath = Path.Combine(
                        AppDomain.CurrentDomain.BaseDirectory,
                        "Resources",
                        "Images",
                        imageName);

                    string placeholderPath = Path.Combine(
                        AppDomain.CurrentDomain.BaseDirectory,
                        "Resources",
                        "Images",
                        "picture.png");

                    string selectedPath = File.Exists(imagePath)
                        ? imagePath
                        : placeholderPath;

                    if (File.Exists(selectedPath))
                    {
                        photo.Source = new BitmapImage(
                            new Uri(selectedPath, UriKind.Absolute));
                    }

                    Grid.SetColumn(photo, 0);
                    content.Children.Add(photo);

                    StackPanel info = new StackPanel
                    {
                        Margin = new Thickness(20, 5, 5, 5),
                        VerticalAlignment = VerticalAlignment.Center
                    };

                    Grid.SetColumn(info, 1);
                    content.Children.Add(info);

                    info.Children.Add(new TextBlock
                    {
                        Text = product["product_name"]?.ToString(),
                        FontSize = 22,
                        FontWeight = FontWeights.Bold,
                        Foreground = new SolidColorBrush(
                            Color.FromRgb(51, 51, 51)),
                        TextWrapping = TextWrapping.Wrap,
                        Margin = new Thickness(0, 0, 0, 10)
                    });

                    info.Children.Add(new TextBlock
                    {
                        Text = "Категория: " + product["category_name"],
                        FontSize = 16,
                        Margin = new Thickness(0, 3, 0, 3)
                    });

                    info.Children.Add(new TextBlock
                    {
                        Text = "Производитель: " + product["manufacturer_name"],
                        FontSize = 16,
                        Margin = new Thickness(0, 3, 0, 3)
                    });

                    info.Children.Add(new TextBlock
                    {
                        Text = "Описание: " + product["description"],
                        FontSize = 15,
                        TextWrapping = TextWrapping.Wrap,
                        Margin = new Thickness(0, 3, 0, 3)
                    });

                    info.Children.Add(new TextBlock
                    {
                        Text = "Состав: " + product["composition"],
                        FontSize = 15,
                        TextWrapping = TextWrapping.Wrap,
                        Margin = new Thickness(0, 3, 0, 3)
                    });

                    info.Children.Add(new TextBlock
                    {
                        Text = "Количество на складе: " + stockText +
                               " (" + quantity + " пар)",
                        FontSize = 16,
                        Margin = new Thickness(0, 3, 0, 3)
                    });

                    info.Children.Add(new TextBlock
                    {
                        Text = discountMultiplier < 1
                            ? "Цена: " + finalPrice.ToString("N2") +
                              " руб. (скидка 25%, старая цена: " +
                              price.ToString("N2") + " руб.)"
                            : "Цена: " + finalPrice.ToString("N2") + " руб.",

                        FontSize = 18,
                        FontWeight = FontWeights.Bold,
                        Foreground = new SolidColorBrush(
                            Color.FromRgb(70, 140, 135)),
                        TextWrapping = TextWrapping.Wrap,
                        Margin = new Thickness(0, 8, 0, 0)
                    });

                    card.Child = content;
                    ProductsPanel.Children.Add(card);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Ошибка загрузки товаров:\n" + ex.Message,
                    "Чудо Обувь",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }
    }
}
