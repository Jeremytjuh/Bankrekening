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

namespace Bankrekening
{
    /// <summary>
    /// Interaction logic for UCTypes.xaml
    /// </summary>
    public partial class UCTypes : UserControl
    {
        BankrekeningDataContext db;
        TypeController TC;
        bool editing;
        Typen SelectedItem;

        public UCTypes(BankrekeningDataContext db)
        {
            InitializeComponent();
            this.db = db;
            TC = new TypeController(db);
            SetData();
        }

        private void SetData()
        {
            ClearForm();
            dgTypen.ItemsSource = TC.AllTypen();
        }

        private void ClearForm()
        {
            txtNaam.IsReadOnly = false;
            txtNaam.Text = "";
            txtRente.Text = "";
            txtMaxOpname.Text = "";
            chBoete.IsChecked = false;
            btnSave.Visibility = Visibility.Visible;
            btnEdit.Visibility = Visibility.Hidden;
            btnDelete.Visibility = Visibility.Hidden;
            SelectedItem = null;
        }

        private bool notEmpty()
        {
            if (txtNaam.Text != "" && txtRente.Text != "" && txtMaxOpname.Text != "")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (notEmpty())
            {
                if (double.TryParse(txtRente.Text, out double Rente) && double.TryParse(txtMaxOpname.Text, out double MaxOpname))
                {
                    TC.NewType(txtNaam.Text, Rente, MaxOpname, (bool)chBoete.IsChecked);
                    TC.Save();
                    SetData();
                }
                else
                {
                    MessageBox.Show("Ongeldig rente of maximale opname", "Error");
                }
            }
            else
            {
                MessageBox.Show("Vul de verplichte velden in!", "Error");
            }
        }

        private void txtFilter_TextChanged(object sender, TextChangedEventArgs e)
        {
            List<Typen> FilteredTypen = (from Typen in db.Typens where Typen.Naam.Contains(txtFilter.Text) select Typen).ToList();
            dgTypen.ItemsSource = FilteredTypen;
        }

        private void dgTypen_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (editing && (Typen)dgTypen.SelectedItem == SelectedItem)
            {
                editing = false;
                ClearForm();
            }
            else
            {
                editing = true;
                SelectedItem = (Typen)dgTypen.SelectedItem;
                if (SelectedItem != null)
                {
                    txtNaam.IsReadOnly = true;
                    txtNaam.Text = SelectedItem.Naam;
                    txtRente.Text = SelectedItem.Rente.ToString();
                    txtMaxOpname.Text = SelectedItem.MaxOpname.ToString();
                    chBoete.IsChecked = SelectedItem.Boete;

                    btnSave.Visibility = Visibility.Hidden;
                    btnEdit.Visibility = Visibility.Visible;
                    btnDelete.Visibility = Visibility.Visible;
                }
            }
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (notEmpty())
            {
                if (double.TryParse(txtRente.Text, out double Rente) && double.TryParse(txtMaxOpname.Text, out double MaxOpname))
                {
                    TC.EditType(SelectedItem.Naam, Rente, MaxOpname, (bool)chBoete.IsChecked);
                    TC.Save();
                    SetData();
                }
                else
                {
                    MessageBox.Show("Ongeldig rente of maximale opname", "Error");
                }
            }
            else
            {
                MessageBox.Show("Vul de verplichte velden in!", "Error");
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult Result = MessageBox.Show("Weet je zeker dat je dit type wilt verwijderen?", "Pop-up", MessageBoxButton.OKCancel);
            if (Result == MessageBoxResult.OK)
            {
                TC.DeleteType(SelectedItem.Naam);
                TC.Save();
                SetData();
            }
            else
            {
                ClearForm();
            }
        }
    }
}
