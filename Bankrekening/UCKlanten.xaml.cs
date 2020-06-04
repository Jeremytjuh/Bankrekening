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
    /// Interaction logic for UCKlanten.xaml
    /// </summary>
    public partial class UCKlanten : UserControl
    {
        BankrekeningDataContext db;
        KlantController KC;
        bool editing;
        Klanten SelectedItem;

        public UCKlanten(BankrekeningDataContext db)
        {
            InitializeComponent();
            this.db = db;
            KC = new KlantController(db);
            SetData();
        }

        private void SetData()
        {
            ClearForm();
            dgKlanten.ItemsSource = KC.AllKlanten();
        }

        private void ClearForm()
        {
            txtBSN.IsReadOnly = false;
            txtBSN.Text = "";
            txtVoorletters.Text = "";
            txtVoornaam.Text = "";
            txtAchternaam.Text = "";
            txtAdres.Text = "";
            txtPostcode.Text = "";
            txtWoonplaats.Text = "";
            txtTelefoonnummer.Text = "";
            txtEmail.Text = "";
            btnSave.Visibility = Visibility.Visible;
            btnEdit.Visibility = Visibility.Hidden;
            btnDelete.Visibility = Visibility.Hidden;
            SelectedItem = null;
        }

        private bool notEmpty()
        {
            if (txtBSN.Text != "" && txtVoorletters.Text != "" && txtVoornaam.Text != "" && txtAchternaam.Text != "" && txtAdres.Text != "" && txtPostcode.Text != "" && txtWoonplaats.Text != "" && txtTelefoonnummer.Text != "" && txtEmail.Text != "")
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
                KC.NewKlant(txtBSN.Text, txtVoorletters.Text, txtVoornaam.Text, txtAchternaam.Text, txtAdres.Text, txtPostcode.Text, txtWoonplaats.Text, txtTelefoonnummer.Text, txtEmail.Text);
                KC.Save();
                SetData();
            }
            else
            {
                MessageBox.Show("Vul de verplichte velden in!", "Error");
            }
        }

        private void txtFilter_TextChanged(object sender, TextChangedEventArgs e)
        {
            List<Klanten> FilteredKlanten = (from Klanten in db.Klantens where Klanten.Voornaam.Contains(txtFilter.Text) select Klanten).ToList();
            dgKlanten.ItemsSource = FilteredKlanten;
        }

        private void dgKlanten_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (editing && (Klanten)dgKlanten.SelectedItem == SelectedItem)
            {
                editing = false;
                ClearForm();
            }
            else
            {
                editing = true;
                SelectedItem = (Klanten)dgKlanten.SelectedItem;
                if (SelectedItem != null)
                {
                    txtBSN.IsReadOnly = true;
                    txtBSN.Text = SelectedItem.BSN;
                    txtVoorletters.Text = SelectedItem.Voorletters;
                    txtVoornaam.Text = SelectedItem.Voornaam;
                    txtAchternaam.Text = SelectedItem.Achternaam;
                    txtAdres.Text = SelectedItem.Adres;
                    txtPostcode.Text = SelectedItem.Postcode;
                    txtWoonplaats.Text = SelectedItem.Woonplaats;
                    txtTelefoonnummer.Text = SelectedItem.Telefoonnummer;
                    txtEmail.Text = SelectedItem.Email;

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
                KC.EditKlant(SelectedItem.BSN, txtVoorletters.Text, txtVoornaam.Text, txtAchternaam.Text, txtAdres.Text, txtPostcode.Text, txtWoonplaats.Text, txtTelefoonnummer.Text, txtEmail.Text);
                KC.Save();
                SetData();
            }
            else
            {
                MessageBox.Show("Vul de verplichte velden in!", "Error");
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult Result = MessageBox.Show("Weet je zeker dat je deze klant wilt verwijderen?", "Pop-up", MessageBoxButton.OKCancel);
            if (Result == MessageBoxResult.OK)
            {
                KC.DeleteKlant(SelectedItem.BSN);
                KC.Save();
                SetData();
            }
            else
            {
                ClearForm();
            }
        }
    }
}
