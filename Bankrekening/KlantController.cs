using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bankrekening
{
    class KlantController
    {
        BankrekeningDataContext db;

        public KlantController(BankrekeningDataContext db)
        {
            this.db = db;
        }

        public void NewKlant(string BSN, string Voorletters, string Voornaam, string Achternaam, string Adres, string Postcode, string Woonplaats, string Telefoonnummer, string Email)
        {
            Klanten K = new Klanten();
            K.BSN = BSN;
            K.Voorletters = Voorletters;
            K.Voornaam = Voornaam;
            K.Achternaam = Achternaam;
            K.Adres = Adres;
            K.Postcode = Postcode;
            K.Woonplaats = Woonplaats;
            K.Telefoonnummer = Telefoonnummer;
            K.Email = Email;

            db.Klantens.InsertOnSubmit(K);
        }

        public void EditKlant(string BSN, string Voorletters, string Voornaam, string Achternaam, string Adres, string Postcode, string Woonplaats, string Telefoonnummer, string Email)
        {
            Klanten K = (from Klant in db.Klantens where Klant.BSN == BSN select Klant).Single();
            K.Voorletters = Voorletters;
            K.Voornaam = Voornaam;
            K.Achternaam = Achternaam;
            K.Adres = Adres;
            K.Postcode = Postcode;
            K.Woonplaats = Woonplaats;
            K.Telefoonnummer = Telefoonnummer;
            K.Email = Email;
        }

        public void DeleteKlant(string BSN)
        {
            Klanten K = (from Klant in db.Klantens where Klant.BSN == BSN select Klant).Single();
            db.Klantens.DeleteOnSubmit(K);
        }

        public void Save()
        {
            db.SubmitChanges();
        }

        public List<Klanten> AllKlanten()
        {
            return db.Klantens.ToList();
        }
    }
}
