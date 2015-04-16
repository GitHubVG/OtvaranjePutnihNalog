using OtvaranjePutnihNaloga_Gikić.Models.Baza_zaposlenika;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OtvaranjePutnihNaloga_Gikić.Models
{
    public class ProvjeraPodataka
    {
        public void ProvjeraIspunjenogNaloga()
        {
            PutniNaloziEntities db = new PutniNaloziEntities();

            
          
            if (db.PutniNalog.Any(x => x.Smještaj) || db.PutniNalog.Any(x => x.IDPrijevoznogSredstva== 2))
            {
                List<int> IDputnihNalogaSaSmjestajem = new List<int>();
                List<int> IDPrijevoznihSredstava = new List<int>();
                var smjestaji = db.Smejstaj;
                var privatnaVozila = db.PrivatnoVozilo;



                foreach (var id in db.PutniNalog.Where(x => x.Smještaj))
                {
                    IDputnihNalogaSaSmjestajem.Add(id.ID);
                }

                
                List<int> listaIdovaPutnihNalogaSaSmjestajem = new List<int>();

                foreach (var v in smjestaji)
                {
                    listaIdovaPutnihNalogaSaSmjestajem.Add(v.IDPutnogNaloga);
                }
                var idZaObrisati = IDputnihNalogaSaSmjestajem.Except(listaIdovaPutnihNalogaSaSmjestajem).ToList();

                foreach (var x in idZaObrisati)
                {
                    if (db.PrivatnoVozilo.Any(y => y.IDPutnogNaloga == x)) //mora se obirsati i privatno vozilo ako postoji zbog dependencija u bazi (foreign key)
                    {
                        db.PrivatnoVozilo.Remove(db.PrivatnoVozilo.Single(y => y.IDPutnogNaloga == x));

                    }
                    db.PutniNalog.Remove(db.PutniNalog.Single(var => var.ID == x));
                    db.SaveChanges();
                }
                                                
                foreach (var id in db.PutniNalog.Where(x => x.IDPrijevoznogSredstva == 2)) //2 je foreign key koji pokazuje na privatno vozilo
                {
                    IDPrijevoznihSredstava.Add(id.ID);
                }
                List<int> listaIdovaPutnihNalogaPrivatnihVozila = new List<int>();

                foreach (var v in privatnaVozila)
                {
                    listaIdovaPutnihNalogaPrivatnihVozila.Add(v.IDPutnogNaloga);
                }

                //ID-ovi putnih naloga koji se pojavljuju samo u tablici putni nalog a sadrže ID prijevoznog
                //sredstva==2 (nije dovršen unos informacija o privatnim vozilima pa se ovaj nalog mora obrisati)
                var idZaObrisatiVozila = IDPrijevoznihSredstava.Except(listaIdovaPutnihNalogaPrivatnihVozila).ToList();

                if (idZaObrisatiVozila != null)
                {
                    foreach (var x in idZaObrisatiVozila)
                    {
                        db.PutniNalog.Remove(db.PutniNalog.Single(var => var.ID == x));
                        db.SaveChanges();
                    }
                }
               
            }

        }

    }
}