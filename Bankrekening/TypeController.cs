using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bankrekening
{
    class TypeController
    {
        BankrekeningDataContext db;

        public TypeController(BankrekeningDataContext db)
        {
            this.db = db;
        }

        public void NewType(string Naam, double Rente, double MaxOpname, bool Boete)
        {
            Typen T = new Typen();
            T.Naam = Naam;
            T.Rente = Rente;
            T.MaxOpname = MaxOpname;
            T.Boete = Boete;

            db.Typens.InsertOnSubmit(T);
        }

        public void EditType(string Naam, double Rente, double MaxOpname, bool Boete)
        {
            Typen T = (from Typen in db.Typens where Typen.Naam == Naam select Typen).Single();
            T.Rente = Rente;
            T.MaxOpname = MaxOpname;
            T.Boete = Boete;
        }

        public void DeleteType(string Naam)
        {
            Typen T = (from Type in db.Typens where Type.Naam == Naam select Type).Single();
            db.Typens.DeleteOnSubmit(T);
        }

        public void Save()
        {
            db.SubmitChanges();
        }

        public List<Typen> AllTypen()
        {
            return db.Typens.ToList();
        }
    }
}
