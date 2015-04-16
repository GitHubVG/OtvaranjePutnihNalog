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

           // var test = db.PutniNalog.OrderByDescending(x => x.ID).First().PrijevoznaSredstva.IDTipPrijevoznogSredstva;
           // var test2 = db.PutniNalog.OrderByDescending(x => x.ID).First().Smještaj;

            if (db.PutniNalog.Any(x => x.Smještaj) || db.PutniNalog.Any(x => x.PrijevoznaSredstva.IDTipPrijevoznogSredstva == 2))
            {
                var IDzadnjiPutniNalog = db.PutniNalog.OrderByDescending(x => x.ID).First().ID;
                List<int> IDputnihNalogaSaSmjestajem = new List<int>();
                List<int> IDPrijevoznihSredstava = new List<int>();

                foreach (var id in db.PutniNalog.Where(x => x.Smještaj))
                {
                    IDputnihNalogaSaSmjestajem.Add(id.ID);
                }
                var smjestaji = db.Smejstaj;
                var privatnaVozila = db.PrivatnoVozilo;

                foreach (var id in db.PutniNalog.Where(x => x.IDPrijevoznogSredstva == 2)) //2 je foreign key koji pokazuje na privatno vozilo
                {
                    IDPrijevoznihSredstava.Add(id.ID);
                }


                for (int i = 0; i < IDputnihNalogaSaSmjestajem.Count; i++)
                {
                    int placeholder = IDputnihNalogaSaSmjestajem[i];
                    if (smjestaji.Any(x => x.IDPutnogNaloga != placeholder))
                    {
                        db.PutniNalog.Remove(db.PutniNalog.Single(x => x.ID == placeholder));

                        //mora se obirsati i privatno vozilo ako postoji zbog dependencija u bazi (foreign key)
                        if (db.PrivatnoVozilo.Any(x => x.IDPutnogNaloga == placeholder))
                        {
                            db.PrivatnoVozilo.Remove(db.PrivatnoVozilo.Single(x => x.IDPutnogNaloga == placeholder));

                        }


                        db.SaveChanges();
                    }

                }

                for(int i=0;i<IDPrijevoznihSredstava.Count;i++)
                {
                    int placeholder = IDPrijevoznihSredstava[i];
                    List<int> listaIdovaPutnihNalogaPrivatnihVozila = new List<int>();

                    foreach(var v in privatnaVozila)
                    {
                        listaIdovaPutnihNalogaPrivatnihVozila.Add(v.IDPutnogNaloga);
                    }
                  
                    //ID-ovi putnih naloga koji se pojavljuju samo u tablici putni nalog a sadrže ID prijevoznog
                    //sredstva==2 (nije dovršen unos informacija o privatnim vozilima pa se ovaj nalog mora obrisati)
                    var test = IDPrijevoznihSredstava.Except(listaIdovaPutnihNalogaPrivatnihVozila).ToList();

                    foreach(var x in test)
                    {
                       db.PutniNalog.Remove(db.PutniNalog.Single(var=>var.ID==x));
                       db.SaveChanges();
                    }
                                                        
                }

            }

        }

    }
}