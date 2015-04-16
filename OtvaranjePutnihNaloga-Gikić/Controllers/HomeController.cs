using OtvaranjePutnihNaloga_Gikić.Models.Baza_zaposlenika;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace OtvaranjePutnihNaloga_Gikić.Controllers
{

    public class HomeController : Controller
    {
        // GET: Home

        //provjera smještaja i prijevoza
        private static bool provjeraSmjestaj = false;
        private static bool provjeraPrijevozPrivatnimVozilom = false;

        PutniNaloziEntities db = new PutniNaloziEntities();

        public ActionResult Index()
        {
            //brisanje nedovršenih putnih naloga iz baze 
            //(ako korisnik popuni putni nalog a ne ispuni podatke o smjestaju ili privatnom vozilu (ako postoji))

            var test = db.PutniNalog.OrderByDescending(x => x.ID).First().PrijevoznaSredstva.IDTipPrijevoznogSredstva;
            var test2 = db.PutniNalog.OrderByDescending(x => x.ID).First().Smještaj;

            if (db.PutniNalog.Any(x=>x.Smještaj) || db.PutniNalog.Any(x => x.PrijevoznaSredstva.IDTipPrijevoznogSredstva==2))
            {
                var IDzadnjiPutniNalog = db.PutniNalog.OrderByDescending(x => x.ID).First().ID;
                List<int> IDputnihNalogaSaSmjestajem = new List<int>();
                List<int> listaPrijevoznihSredstava = new List<int>();

                foreach (var id in db.PutniNalog.Where(x=>x.Smještaj))
                {
                    IDputnihNalogaSaSmjestajem.Add(id.ID);
                }
                var smjestaji = db.Smejstaj;

                foreach(var id in db.PutniNalog.Where(x=>x.PrijevoznaSredstva.IDTipPrijevoznogSredstva==2))
                {
                    listaPrijevoznihSredstava.Add(id.ID);
                }
                                                
                
                for (int i = 0; i < IDputnihNalogaSaSmjestajem.Count;i++ )
                {
                    int placeholder= IDputnihNalogaSaSmjestajem[i];
                    if (smjestaji.Any(x => x.IDPutnogNaloga != placeholder))
                    {
                        db.PutniNalog.Remove(db.PutniNalog.Single(x => x.ID == placeholder));

                        //mora se obirsati i privatno vozilo ako postoji zbog dependencija u bazi (foreign key)
                        if(db.PrivatnoVozilo.Any(x=>x.IDPutnogNaloga==placeholder))
                        {
                            db.PrivatnoVozilo.Remove(db.PrivatnoVozilo.Single(x => x.IDPutnogNaloga == placeholder));

                        }


                        db.SaveChanges();
                    }

                }
                                               
            }


            provjeraSmjestaj = false;
            provjeraPrijevozPrivatnimVozilom = false;

            return View();
        }

        public ActionResult IspisZaposlenika()
        {
            return View(db.Zaposlenici.ToList());
        }

        public ActionResult Edit(int id)
        {
            var zaposlenik = db.Zaposlenici.Single(identitet => identitet.ID == id);
            ViewBag.OdjelZaposlenika = new SelectList(db.Odjel, "ID", "Ime_odjela");
            return View(zaposlenik);
        }

        [HttpPost]
        public ActionResult Edit(Zaposlenici zaposlenik, string OdjelZaposlenika)
        {
            ViewBag.OdjelZaposlenika = new SelectList(db.Odjel, "ID", "Ime_odjela");
            zaposlenik.IDOdjela = int.Parse(OdjelZaposlenika);

            if (ModelState.IsValid)
            {
                db.Entry(zaposlenik).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("IspisZaposlenika", db.Zaposlenici.ToList());
            }
            else
            {
                return View();
            }
        }


        public ActionResult Create()
        {
            ViewBag.OdjelZaposlenika = new SelectList(db.Odjel, "ID", "Ime_odjela");


            return View();
        }

        [HttpPost]
        public ActionResult Create(Zaposlenici zaposlenik, string odjelZaposlenika)
        {
            ViewBag.OdjelZaposlenika = new SelectList(db.Odjel, "ID", "Ime_odjela");

            if (ModelState.IsValid)
            {
                zaposlenik.IDOdjela = int.Parse(odjelZaposlenika);
                db.Zaposlenici.Add(zaposlenik);
                db.SaveChanges();
                return RedirectToAction("IspisZaposlenika", db.Zaposlenici.ToList());

            }
            else
            {
                return View();
            }
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var zaposlenik = db.Zaposlenici.Single(x => x.ID == id);
            db.Zaposlenici.Remove(zaposlenik);
            db.SaveChanges();

            return RedirectToAction("IspisZaposlenika", db.Zaposlenici.ToList());

        }

        public ActionResult Details(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Zaposlenici zaposlenik = db.Zaposlenici.Find(id);
            if (zaposlenik == null)
            {
                return HttpNotFound();
            }
            return View(zaposlenik);

        }

        public ActionResult OtvaranjePutnogNaloga()
        {
            ViewBag.PrijevoznoSredstvo = new SelectList(db.PrijevoznaSredstva, "IDTipPrijevoznogSredstva", "PrijevoznoSredstvo");
            return View();
        }

        [HttpPost]
        public ActionResult OtvaranjePutnogNaloga(PutniNalog nalog, string PrijevoznoSredstvo)
        {
            ViewBag.PrijevoznoSredstvo = new SelectList(db.PrijevoznaSredstva, "IDTipPrijevoznogSredstva", "PrijevoznoSredstvo");

            nalog.IDPrijevoznogSredstva = int.Parse(PrijevoznoSredstvo);

            if (ModelState.IsValid)
            {
                db.PutniNalog.Add(nalog);
                db.SaveChanges();

                if (nalog.Smještaj == true)
                {
                    provjeraSmjestaj = true;
                }

                if (int.Parse(PrijevoznoSredstvo) == 2)
                {
                    provjeraPrijevozPrivatnimVozilom = true;

                    return RedirectToAction("PrivatnoPrijevoznoSredstvo");
                }

                if (provjeraSmjestaj)
                {
                    return RedirectToAction("Smjestaj");
                }
                else
                {
                    return RedirectToAction("Index");
                }
            }

            else
            {
                return View();

            }

        }


        public ActionResult PrivatnoPrijevoznoSredstvo()
        {

            return View();
        }

        [HttpPost]
        public ActionResult PrivatnoPrijevoznoSredstvo(PrivatnoVozilo privatnoVozilo)
        {
            privatnoVozilo.IDPutnogNaloga = db.PutniNalog.Select(x => x.ID).OrderByDescending(x => x).First();


            if (ModelState.IsValid)
            {
                db.PrivatnoVozilo.Add(privatnoVozilo);
                db.SaveChanges();
                if (provjeraSmjestaj)
                {
                    return RedirectToAction("Smjestaj");
                }
                else
                {
                    return RedirectToAction("Index");
                }
            }
            else
            {
                return View();
            }
        }


        public ActionResult Smjestaj()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Smjestaj(Smejstaj smjestaj)
        {
            smjestaj.IDPutnogNaloga = db.PutniNalog.Select(x => x.ID).OrderByDescending(x => x).First();

            if (ModelState.IsValid)
            {
                db.Smejstaj.Add(smjestaj);
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }
        }
    }
}