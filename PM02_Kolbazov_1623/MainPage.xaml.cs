using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PM02_Kolbazov_1623
{
    public partial class MainPage : Page
    {
        private int suppliersCount;
        private int consumersCount;
        private double[,] costs;
        private double[] supplies;
        private double[] demands;

        public MainPage()
        {
            InitializeComponent();
            SuppliersComboBox.SelectedIndex = 2;
            ConsumersComboBox.SelectedIndex = 2;
            MethodComboBox.SelectedIndex = 0;
        }
        private void GenerateTable()
        {
            MainTableGrid.Children.Clear();
            MainTableGrid.RowDefinitions.Clear();
            MainTableGrid.ColumnDefinitions.Clear();

            suppliersCount = Math.Max(1, SuppliersComboBox.SelectedIndex + 2);
            consumersCount = Math.Max(1, ConsumersComboBox.SelectedIndex + 1);

            for (int i = 0; i < suppliersCount + 2; i++)
            {
                MainTableGrid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            }

            for (int i = 0; i < consumersCount + 2; i++)
            {
                MainTableGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
            }

            for (int row = 0; row < suppliersCount + 2; row++)
            {
                for (int col = 0; col < consumersCount + 2; col++)
                {
                    if (row == suppliersCount + 1 && col == consumersCount + 1)
                    {
                        continue;
                    }

                    Border border = null;

                    if (row == 0 && col == 0)
                    {
                        border = new Border()
                        {
                            BorderBrush = Brushes.Green,
                            BorderThickness = new Thickness(1),
                            Margin = new Thickness(1),
                            Padding = new Thickness(5)
                        };
                        border.Child = new TextBlock() { Text = "Поставщик" };
                        Grid.SetRow(border, row);
                        Grid.SetColumn(border, col);
                        Grid.SetRowSpan(border, 2);
                        MainTableGrid.Children.Add(border);
                        continue;
                    }
                    else if (row == 0 && col == consumersCount + 1)
                    {
                        border = new Border()
                        {
                            BorderBrush = Brushes.Green,
                            BorderThickness = new Thickness(1),
                            Margin = new Thickness(1),
                            Padding = new Thickness(5)
                        };
                        border.Child = new TextBlock() { Text = "Запас" };
                        Grid.SetRow(border, row);
                        Grid.SetColumn(border, col);
                        Grid.SetRowSpan(border, 2);
                        MainTableGrid.Children.Add(border);
                        continue;
                    }
                    else if (row == 0 && col == 1)
                    {
                        border = new Border()
                        {
                            BorderBrush = Brushes.Green,
                            BorderThickness = new Thickness(1),
                            Margin = new Thickness(1),
                            Height = 40
                        };
                        border.Child = new TextBlock() { Text = "Потребитель" };
                        Grid.SetRow(border, row);
                        Grid.SetColumn(border, col);
                        Grid.SetColumnSpan(border, consumersCount);
                        MainTableGrid.Children.Add(border);
                        continue;
                    }

                    else if (row == 0 && col <= consumersCount)
                    {
                        continue;
                    }

                    if (row == 1 && (col == 0 || col == consumersCount + 1))
                    {
                        continue;
                    }

                    border = new Border()
                    {
                        BorderBrush = Brushes.Green,
                        BorderThickness = new Thickness(1),
                        Margin = new Thickness(1),
                        Padding = new Thickness(5)
                    };

                    if (row == 1 && col > 0 && col <= consumersCount)
                    {
                        border.Child = new TextBlock() { Text = $"В{col}" };
                    }
                    else if (col == 0 && row > 1 && row <= suppliersCount)
                    {
                        border.Child = new TextBlock() { Text = $"А{row - 1}" };
                    }
                    else if (col == 0 && row == suppliersCount + 1)
                    {
                        border.Child = new TextBlock() { Text = "Потребность" };
                    }

                    else if (row > 1 && col > 0 && !(row == suppliersCount + 1 && col == consumersCount + 1))
                    {
                        TextBox textBox = new TextBox()
                        {
                            Width = 50,
                            Height = 30,
                        };
                        border.Child = textBox;
                    }

                    Grid.SetRow(border, row);
                    Grid.SetColumn(border, col);
                    MainTableGrid.Children.Add(border);
                }
            }

            costs = new double[suppliersCount, consumersCount];
            supplies = new double[suppliersCount];
            demands = new double[consumersCount];
        }

        private void GenerateResultTable(double[,] allocation, double totalCost)
        {
            if (ResultGrid == null) return;

            ResultGrid.Children.Clear();
            ResultGrid.RowDefinitions.Clear();
            ResultGrid.ColumnDefinitions.Clear();

            for (int i = 0; i < suppliersCount + 2; i++)
            {
                ResultGrid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            }

            for (int i = 0; i < consumersCount + 2; i++)
            {
                ResultGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
            }

            for (int row = 0; row < suppliersCount + 2; row++)
            {
                for (int col = 0; col < consumersCount + 2; col++)
                {
                    if (row == suppliersCount + 1 && col == consumersCount + 1)
                    {
                        continue;
                    }

                    Border border = new Border()
                    {
                        BorderBrush = Brushes.Green,
                        BorderThickness = new Thickness(1),
                        Margin = new Thickness(1),
                        Padding = new Thickness(5),
                        Height = 40
                    };

                    TextBlock textBlock;

                    if (row == 0 && col == 0)
                    {
                        border = new Border()
                        {
                            BorderBrush = Brushes.Green,
                            BorderThickness = new Thickness(1),
                            Margin = new Thickness(1),
                            Padding = new Thickness(5),
                            Height = 70
                        };

                        textBlock = new TextBlock() { Text = "Поставщик" };
                        Grid.SetRowSpan(border, 2);
                    }

                    else if (row == 0 && col == consumersCount + 1)
                    {
                        border = new Border()
                        {
                            BorderBrush = Brushes.Green,
                            BorderThickness = new Thickness(1),
                            Margin = new Thickness(1),
                            Padding = new Thickness(5),
                            Height = 70,
                            Width = 60
                        };

                        textBlock = new TextBlock() { Text = "Запас" };
                        Grid.SetRow(border, 2);
                        Grid.SetRowSpan(border, 2);
                    }

                    else if (row == 0 && col == 1)
                    {
                        border = new Border()
                        {
                            BorderBrush = Brushes.Green,
                            BorderThickness = new Thickness(1),
                            Margin = new Thickness(1),
                            Padding = new Thickness(5),
                            Height = 40
                        };

                        textBlock = new TextBlock() { Text = "Потребитель" };
                        Grid.SetColumnSpan(border, consumersCount);
                    }

                    else if (row == 0 && col <= consumersCount)
                    {
                        continue;
                    }

                    else if (row == 1 && (col == 0 || col == consumersCount + 1))
                    {
                        continue;
                    }

                    else if (row == 1 && col > 0 && col <= consumersCount)
                    {
                        textBlock = new TextBlock() { Text = $"В{col}" };

                        border = new Border()
                        {
                            BorderBrush = Brushes.Green,
                            BorderThickness = new Thickness(1),
                            Margin = new Thickness(1),
                            Padding = new Thickness(5),
                            Width = 62
                        };
                    }

                    else if (col == 0 && row > 1 && row <= suppliersCount)
                    {
                        textBlock = new TextBlock() { Text = $"А{row - 1}" };
                    }

                    else if (col == 0 && row == suppliersCount + 1)
                    {
                        textBlock = new TextBlock() { Text = "Потребность" };
                    }

                    else if (row == suppliersCount + 1 && col > 0 && col <= consumersCount)
                    {
                        textBlock = new TextBlock() { Text = $"{demands[col - 1]}" };
                    }

                    else if (row > 1 && row <= suppliersCount && col == consumersCount + 1)
                    {
                        textBlock = new TextBlock() { Text = $"{supplies[row - 2]}" };
                    }

                    else if (row > 1 && row <= suppliersCount && col > 0 && col <= consumersCount)
                    {
                        double value = allocation[row - 2, col - 1];
                        textBlock = new TextBlock() { Text = $"{value}" };
                        if (value > 0)
                        {
                            border.Background = Brushes.LightGreen;
                            textBlock.FontWeight = FontWeights.Bold;
                        }
                    }

                    else
                    {
                        textBlock = new TextBlock() { Text = "" };
                    }

                    border.Child = textBlock;

                    Grid.SetRow(border, row);
                    Grid.SetColumn(border, col);
                    ResultGrid.Children.Add(border);
                }
            }

            if (TotalCostTextBlock != null)
            {
                TotalCostTextBlock.Text = $"Стоимость перевозок: {totalCost}";
            }
        }

        private void ReadTableData()
        {
            foreach (var child in MainTableGrid.Children)
            {
                if (child is Border border && border.Child is TextBox textBox)
                {
                    int row = Grid.GetRow(border);
                    int col = Grid.GetColumn(border);

                    double value = double.TryParse(textBox.Text, out double v) ? v : 0;

                    if (row > 1 && row <= suppliersCount && col > 0 && col <= consumersCount)
                    {
                        costs[row - 2, col - 1] = value;
                    }
                    else if (row > 1 && row <= suppliersCount && col == consumersCount + 1)
                    {
                        supplies[row - 2] = value;
                    }
                    else if (row == suppliersCount + 1 && col > 0 && col <= consumersCount)
                    {
                        demands[col - 1] = value;
                    }
                }
            }
        }

        private void Calculate()
        {
            ReadTableData();

            double totalSupply = 0, totalDemand = 0;
            for (int i = 0; i < suppliersCount; i++) totalSupply += supplies[i];
            for (int i = 0; i < consumersCount; i++) totalDemand += demands[i];

            if (Math.Abs(totalSupply - totalDemand) > 1e-6)
            {
                MessageBox.Show("Задача не сбалансирована! Сумма запасов должна равняться сумме потребностей", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            double[,] allocation = new double[suppliersCount, consumersCount];
            double totalCost = 0;

            if (MethodComboBox.SelectedIndex == 0)
            {
                (allocation, totalCost) = NorthwestCornerMethod();
            }
            else if (MethodComboBox.SelectedIndex == 1)
            {
                (allocation, totalCost) = MinimumCostMethod();
            }

            GenerateResultTable(allocation, totalCost);
        }

        private (double[,], double) NorthwestCornerMethod()
        {
            double[,] allocation = new double[suppliersCount, consumersCount];
            double[] supply = (double[])supplies.Clone();
            double[] demand = (double[])demands.Clone();
            double totalCost = 0;

            int i = 0, j = 0;
            while (i < suppliersCount && j < consumersCount)
            {
                double amount = Math.Min(supply[i], demand[j]);
                allocation[i, j] = amount;
                totalCost += amount * costs[i, j];
                supply[i] -= amount;
                demand[j] -= amount;

                if (supply[i] == 0) i++;
                if (demand[j] == 0) j++;
            }

            return (allocation, totalCost);
        }

        private (double[,], double) MinimumCostMethod()
        {
            double[,] allocation = new double[suppliersCount, consumersCount];
            double[] supply = (double[])supplies.Clone();
            double[] demand = (double[])demands.Clone();
            double totalCost = 0;

            while (true)
            {
                double minCost = double.MaxValue;
                int minI = -1, minJ = -1;
                for (int i = 0; i < suppliersCount; i++)
                {
                    for (int j = 0; j < consumersCount; j++)
                    {
                        if (supply[i] > 0 && demand[j] > 0 && costs[i, j] < minCost)
                        {
                            minCost = costs[i, j];
                            minI = i;
                            minJ = j;
                        }
                    }
                }

                if (minI == -1) break;

                double amount = Math.Min(supply[minI], demand[minJ]);
                allocation[minI, minJ] = amount;
                totalCost += amount * costs[minI, minJ];
                supply[minI] -= amount;
                demand[minJ] -= amount;
            }

            return (allocation, totalCost);
        }

        private void SuppliersComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            GenerateTable();
        }

        private void ConsumersComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            GenerateTable();
        }

        private void MethodComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Calculate();
        }

        private void CalculateButton_Click(object sender, RoutedEventArgs e)
        {
            Calculate();
        }

    }
}
