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
    /// Interaction logic for UCRekening.xaml
    /// </summary>
    public partial class UCRekening : UserControl
    {
        BankrekeningDataContext db;
        BankController BC;
        TypeController TC;
        KlantController KC;
        List<String> BSNs;
        bool editing;
        Rekeningen SelectedItem;

        public UCRekening(BankrekeningDataContext db)
        {
            InitializeComponent();
            this.db = db;
            BC = new BankController(db);
            TC = new TypeController(db);
            KC = new KlantController(db);
            SetData();
        }

        private void SetData()
        {
            ClearForm();
            dgRekeningen.ItemsSource = BC.AllRekeningen();
            List<String> Fullnames = new List<String>();
            BSNs = new List<String>();
            foreach (Klanten K in db.Klantens)
            {
                Fullnames.Add(K.Voornaam + " " + K.Achternaam);
                BSNs.Add(K.BSN);
            }
            cbKlanten.ItemsSource = Fullnames;
            cbTypes.ItemsSource = TC.AllTypen();
            cbTypes.DisplayMemberPath = "Naam";
        }

        private void ClearForm()
        {
            txtIBAN.IsReadOnly = false;
            txtIBAN.Text = "";
            cbKlanten.SelectedItem = null;
            txtSaldo.Text = "";
            cbTypes.SelectedItem = null;
            dpEinddatum.Text = "";
            btnSave.Visibility = Visibility.Visible;
            btnEdit.Visibility = Visibility.Hidden;
            btnDelete.Visibility = Visibility.Hidden;
        }

        private bool notEmpty()
        {
            if (txtIBAN.Text != "" && cbKlanten.SelectedItem != null && txtSaldo.Text != "" && cbTypes.SelectedItem != null)
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
                if (dpEinddatum.Text != "")
                {
                    if (DateTime.TryParse(dpEinddatum.Text, out DateTime Einddatum))
                    {
                        if (double.TryParse(txtSaldo.Text, out double dSaldo))
                        {
                            BC.NewRekening(txtIBAN.Text, BSNs[cbKlanten.SelectedIndex], dSaldo, (Typen)cbTypes.SelectedItem, Einddatum);
                            BC.Save();
                            SetData();
                        }
                        else
                        {
                            MessageBox.Show("Ongeldig saldo", "Error");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Ongeldige datum", "Error");
                    }
                }
                else
                {
                    if (double.TryParse(txtSaldo.Text, out double dSaldo))
                    {
                        BC.NewRekening(txtIBAN.Text, BSNs[cbKlanten.SelectedIndex], dSaldo, (Typen)cbTypes.SelectedItem);
                        BC.Save();
                        SetData();
                    }
                    else
                    {
                        MessageBox.Show("Ongeldig saldo", "Error");
                    }
                }
            }
            else
            {
                MessageBox.Show("Vul de verplichte velden in!", "Error");
            }
        }

        private void txtFilter_TextChanged(object sender, TextChangedEventArgs e)
        {
            List<Rekeningen> FilteredRekeningen = (from Rekeningen in db.Rekeningens where Rekeningen.Klanten.Voornaam.Contains(txtFilter.Text) select Rekeningen).ToList();
            dgRekeningen.ItemsSource = FilteredRekeningen;
        }

        private void dgRekeningen_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (editing && (Rekeningen)dgRekeningen.SelectedItem == SelectedItem)
            {
                editing = false;
                ClearForm();
            }
            else
            {
                editing = true;
                SelectedItem = (Rekeningen)dgRekeningen.SelectedItem;
                if (SelectedItem != null)
                {
                    txtIBAN.IsReadOnly = true;
                    txtIBAN.Text = SelectedItem.IBAN;
                    //cbKlanten.ItemsSource = KC.AllKlanten();
                    //cbKlanten.DisplayMemberPath = "Voornaam";
                    //cbKlanten.SelectedItem = SelectedItem.Klanten;
                    int i = 0;
                    foreach (String BSN in BSNs)
                    {
                        if (BSN == SelectedItem.Klanten.BSN)
                        {
                            cbKlanten.SelectedIndex = i;
                        }
                        else
                        {
                            i++;
                        }
                        
                    }
                    txtSaldo.Text = SelectedItem.Saldo.ToString();
                    cbTypes.SelectedItem = SelectedItem.Typen;
                    dpEinddatum.Text = SelectedItem.SluitDatum.ToString();

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
                if (dpEinddatum.Text != "")
                {
                    if (DateTime.TryParse(dpEinddatum.Text, out DateTime Einddatum))
                    {
                        if (double.TryParse(txtSaldo.Text, out double dSaldo))
                        {
                            BC.EditRekening(SelectedItem.IBAN, BSNs[cbKlanten.SelectedIndex], dSaldo, (Typen)cbTypes.SelectedItem, Einddatum);
                            BC.Save();
                            SetData();
                        }
                        else
                        {
                            MessageBox.Show("Ongeldig saldo", "Error");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Ongeldige datum", "Error");
                    }
                }
                else
                {
                    if (double.TryParse(txtSaldo.Text, out double dSaldo))
                    {
                        BC.EditRekening(SelectedItem.IBAN, BSNs[cbKlanten.SelectedIndex], dSaldo, (Typen)cbTypes.SelectedItem);
                        BC.Save();
                        SetData();
                    }
                    else
                    {
                        MessageBox.Show("Ongeldig saldo", "Error");
                    }
                }
            }
            else
            {
                MessageBox.Show("Vul de verplichte velden in!", "Error");
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult Result = MessageBox.Show("Weet je zeker dat je deze rekening wilt verwijderen?", "Pop-up", MessageBoxButton.OKCancel);
            if (Result == MessageBoxResult.OK)
            {
                BC.DeleteRekening(SelectedItem.IBAN);
                BC.Save();
                SetData();
            }
            else
            {
                ClearForm();
            }
        }
    }
}
