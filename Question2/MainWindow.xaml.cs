using Question2.Models;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Question2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private Prn212TrialContext context = new Prn212TrialContext();
        private int selectedEm = -1;

        public MainWindow()
        {
            InitializeComponent();
            LoadEmployees();
        }

        private void LoadEmployees()
        {
           List<Employee> employees= context.Employees.Select( e => e).ToList();
            dg.ItemsSource = null;
            dg.ItemsSource = employees;
        }

        private void dg_onSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // check for null itemsource
            if (dg.ItemsSource == null)
            {
                selectedEm = -1;
                return;
            }

            if (dg.SelectedItems.Count == 0)
            {
                selectedEm = -1;
                return;
            }

            // get dataGrid
            DataGrid dataGrid = sender as DataGrid;
            // get selected row
            DataGridRow row = (DataGridRow)dataGrid.ItemContainerGenerator.ContainerFromIndex(dg.SelectedIndex);
            // get the cell contain the index
            DataGridCell cell = dataGrid.Columns[0].GetCellContent(row).Parent as DataGridCell;
            string id = ((TextBlock)cell.Content).Text.ToString();
            selectedEm = int.Parse(id);

            // get employee
            Employee em = context.Employees.Where(e => e.Id == selectedEm).FirstOrDefault();
            // display employee info
            txtId.Text = em.Id.ToString();
            txtName.Text = em.Name;
            if (em.Gender == "Male")
            {
                radioMale.IsChecked = true;
                radioFemale.IsChecked = false;
            } else
            {
                radioMale.IsChecked = false;
                radioFemale.IsChecked = true;
            }
            dob.Text = em.Dob.ToString();
            txtPhone.Text = em.Phone;
            txtIdNumber.Text = em.Idnumber;

        }

        private void btnRefreshClicked(object sender, RoutedEventArgs e)
        {
            Reset();
        }

        private void Reset()
        {
            txtId.Text = string.Empty;
            txtName.Text = string.Empty;
            radioFemale.IsChecked = false;
            radioMale.IsChecked = true;
            dob.Text = string.Empty;
            txtPhone.Text = string.Empty;
            txtIdNumber.Text = string.Empty;
            selectedEm = -1;
            LoadEmployees();
        }

        private void btnAddClicked(object sender, RoutedEventArgs e)
        {
            try
            {
                string name = txtName.Text;
                string gender = checkGender() ? "Male" : "Female";
                DateOnly db = DateOnly.Parse(dob.Text);
                string phone = txtPhone.Text;
                string idnumber = txtIdNumber.Text;
                
                // check for valid field
                if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(gender)
                    || string.IsNullOrEmpty(phone) || string.IsNullOrEmpty(idnumber)) {
                    throw new Exception("Invalid input");
                }
                if (phone.Length > 15)
                {
                    MessageBox.Show("Phone number too long");
                    return;
                }

                if (idnumber.Length > 12)
                {
                    MessageBox.Show("Id number too long");
                    return;
                }
                Employee employee = new Employee() {
                    Name = name,
                    Gender = gender,
                    Phone = phone,
                    Idnumber = idnumber,
                    Dob = db
                };

                context.Employees.Add(employee);
                context.SaveChanges();
                LoadEmployees();

            } catch(Exception ex)
            {
                MessageBox.Show("Please provide all valid information!");
            }
        }

        private void onBtnEditClicked(object sender, RoutedEventArgs e)
        {
            if (selectedEm == -1)
            {
                MessageBox.Show("Please choose an employee");
                return;
            }
            string name = txtName.Text;
            string gender = checkGender() ? "Male" : "Female";
            DateOnly db = DateOnly.Parse(dob.Text);
            string phone = txtPhone.Text;
            string idnumber = txtIdNumber.Text;

            if (phone.Length > 15)
            {
                MessageBox.Show("Phone number too long");
                return;
            }

            if (idnumber.Length > 12)
            {
                MessageBox.Show("Id number too long");
                return;
            }
            // get employee
            Employee em = context.Employees.Where(e => e.Id == selectedEm).FirstOrDefault();
            em.Name = name;
            em.Gender = gender;
            em.Idnumber = idnumber;
            em.Dob = db;
            em.Phone = phone;
            // save employee
            context.Employees.Update(em);
            context.SaveChanges();
            MessageBox.Show("Update Successful");
            Reset();

        }

        private void onBtnDeleteClicked(object sender, RoutedEventArgs e)
        {
            if (selectedEm == -1)
            {
                MessageBox.Show("Please select an employee");
                return;
            }

            MessageBoxResult res = MessageBox.Show("Do you want to delete?", "Confirmation", MessageBoxButton.YesNo);
            if (res == MessageBoxResult.No)
            {
                return;
            }

            Employee em = context.Employees.Where(e => e.Id == selectedEm).FirstOrDefault();
            context.Employees.Remove(em);
            context.SaveChanges();
            MessageBox.Show("Delete successful!");
            Reset();
        }

        private bool checkGender()
        {
            return radioMale.IsChecked == true;
        }
    }
}