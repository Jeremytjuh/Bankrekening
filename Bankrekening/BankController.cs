using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bankrekening
{
    class BankController
    {
        BankrekeningDataContext db;

        public BankController(BankrekeningDataContext db)
        {
            this.db = db;
        }

        public void NewRekening(string IBAN, string BSN, double Saldo, Typen Type)
        {
            Rekeningen R = new Rekeningen();
            R.IBAN = IBAN;
            R.Rekeninghouder = BSN;
            R.Saldo = Saldo;
            R.Type = Type.Naam;
            R.OpenDatum = DateTime.Now;

            db.Rekeningens.InsertOnSubmit(R);
        }

        public void NewRekening(string IBAN, string BSN, double Saldo, Typen Type, DateTime Sluitdatum)
        {
            Rekeningen R = new Rekeningen();
            R.IBAN = IBAN;
            R.Rekeninghouder = BSN;
            R.Saldo = Saldo;
            R.Type = Type.Naam;
            R.OpenDatum = DateTime.Now;
            R.SluitDatum = Sluitdatum;

            db.Rekeningens.InsertOnSubmit(R);
        }

        public void EditRekening(string IBAN, string BSN, double Saldo, Typen Type)
        {
            Rekeningen R = (from Rekening in db.Rekeningens where Rekening.IBAN == IBAN select Rekening).Single();
            R.Rekeninghouder = BSN;
            R.Saldo = Saldo;
            R.Type = Type.Naam;
        }

        public void EditRekening(string IBAN, string BSN, double Saldo, Typen Type, DateTime SluitDatum)
        {
            Rekeningen R = (from Rekening in db.Rekeningens where Rekening.IBAN == IBAN select Rekening).Single();
            Klanten K = (from Klant in db.Klantens where Klant.BSN == BSN select Klant).Single();
            R.IBAN = IBAN;
            R.Klanten = K;
            R.Saldo = Saldo;
            R.Typen = Type;
            R.SluitDatum = SluitDatum;
        }

        public void DeleteRekening(string IBAN)
        {
            Rekeningen R = (from Rekening in db.Rekeningens where Rekening.IBAN == IBAN select Rekening).Single();
            db.Rekeningens.DeleteOnSubmit(R);
        }

        public void Save()
        {
            db.SubmitChanges();
        }

        public List<Rekeningen> AllRekeningen()
        {
            return db.Rekeningens.ToList();
        }
    }
}
