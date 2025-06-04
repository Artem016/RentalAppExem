using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
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
        private DbContext db;
        private DataTable _clientsTable;

        public MainWindow()
        {
            InitializeComponent();

            db = new DbContext();
            _clientsTable = db.GetClints();
            ClientsGrid.ItemsSource = _clientsTable.DefaultView;
            DeviceTypesGrid.ItemsSource = db.GetDeviceTypes().DefaultView;
            DevicePassportsGrid.ItemsSource = db.GetDevicePassports().DefaultView;
            RentalAgreementGrid.ItemsSource = db.GetAgreements().DefaultView;
            OverDueClientsGrid.ItemsSource = db.GetOverdueClients().DefaultView;
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

        private void SaveClientChanges_Click(object sender, RoutedEventArgs e)
        {
            foreach (DataRow row in _clientsTable.Rows)
            {
                if (row.RowState == DataRowState.Modified)
                {
                    db.UpdateClient(row);
                }
            }

            MessageBox.Show("Изменения сохранены!");
            _clientsTable.AcceptChanges();
        }

        private void DeleteClient_Click(object sender, RoutedEventArgs e)
        {
            if (ClientsGrid.SelectedItem == null)
            {
                MessageBox.Show("Выберите клиента для удаления.");
                return;
            }

            DataRowView selectedRow = (DataRowView)ClientsGrid.SelectedItem;
            int clientId = Convert.ToInt32(selectedRow["Id"]);

            MessageBoxResult result = MessageBox.Show(
                $"Удалить клиента с ID {clientId}?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                db.DeleteClient(clientId);

                // Удаление из DataTable (визуально)
                selectedRow.Row.Delete();

                MessageBox.Show("Клиент удалён.");
            }
        }

        private void AddAgreement_Click(object sender, RoutedEventArgs e)
        {
            string number = AgreementNumberBox.Text;
            DateTime? agreementDate = AgreementDatePicker.SelectedDate;
            DateTime? startDate = RentStartDatePicker.SelectedDate;
            DateTime? endDate = RentEndDatePicker.SelectedDate;

            if (!int.TryParse(ClientIdBox.Text, out int clientId) || !int.TryParse(DeviceIdBox.Text, out int deviceId))
            {
                AgreementResultText.Text = "Ошибка: ID клиента или устройства неверны";
                AgreementResultText.Foreground = Brushes.Red;
                return;
            }

            if (agreementDate == null || startDate == null || endDate == null)
            {
                AgreementResultText.Text = "Пожалуйста заполните все поля";
                AgreementResultText.Foreground = Brushes.Red;
                return;
            }

            bool succes = db.AddAgreement(number, agreementDate.Value, startDate.Value, endDate.Value, clientId, deviceId);

            if (succes)
            {
                AgreementResultText.Text = "Договор успешно добавлен!";
                AgreementResultText.Foreground = Brushes.Green;
                RentalAgreementGrid.ItemsSource = db.GetAgreements().DefaultView;
            }
            else
            {
                AgreementResultText.Text = "Ошибка при добавлении договора.";
                AgreementResultText.Foreground = Brushes.Red;
            }
        }
    }
}
