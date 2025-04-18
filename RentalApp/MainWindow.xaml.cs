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

namespace RentalApp
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            DbContext db = new DbContext();
            ClientsGrid.ItemsSource = db.GetClints().DefaultView;
        }

        private void AddClient_Click(object sender, RoutedEventArgs e)
        {
            string lastName = LastNameBox.Text;
            string firstName = FirstNameBox.Text;
            string middleName = MiddleNameBox.Text;
            string passportSeries = PassportSeriesBox.Text;
            string passportNumber = PassportNumberBox.Text;
            string address = AddressBox.Text;

            if (string.IsNullOrWhiteSpace(lastName) || string.IsNullOrWhiteSpace(firstName))
            {
                MessageBox.Show("Введите как минимум имя и фамилию!");
                return;
            }

            DbContext db = new DbContext();
            db.AddClint(lastName, firstName, middleName, passportSeries, passportNumber, address);

            MessageBox.Show("Клиент добавлен");
            ClientsGrid.ItemsSource = db.GetClints().DefaultView;

            LastNameBox.Clear();
            FirstNameBox.Clear();
            MiddleNameBox.Clear();
            PassportSeriesBox.Clear();
            PassportNumberBox.Clear();
            AddressBox.Clear();
        }
    }
}
