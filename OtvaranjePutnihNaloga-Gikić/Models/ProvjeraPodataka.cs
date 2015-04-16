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
                List<int> listaPrijevoznihSredstava = new List<int>();

                foreach (var id in db.PutniNalog.Where(x => x.Smještaj))
                {
                    IDputnihNalogaSaSmjestajem.Add(id.ID);
                }
                var smjestaji = db.Smejstaj;

                foreach (var id in db.PutniNalog.Where(x => x.PrijevoznaSredstva.IDTipPrijevoznogSredstva == 2))
                {
                    listaPrijevoznihSredstava.Add(id.ID);
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

            }

        }

    }
}