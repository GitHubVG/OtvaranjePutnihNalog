using OtvaranjePutnihNaloga_Gikić.Models;
using OtvaranjePutnihNaloga_Gikić.Models.Baza_zaposlenika;
using Postal;
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
        private static bool postojeLiSuputnici = false;
        private static int brojDodatnihPutnika;
        private static int brojSuputnika;
        List<string> ImenaSuputnika = new List<string>();


        PutniNaloziEntities db = new PutniNaloziEntities();

        public ActionResult Index()
        {
            //brisanje nedovršenih putnih naloga iz baze 
            //(ako korisnik popuni putni nalog a ne ispuni podatke o smjestaju ili privatnom vozilu (ako postoji))
            ProvjeraPodataka provjera = new ProvjeraPodataka();
            provjera.ProvjeraIspunjenogNaloga();


            provjeraSmjestaj = false;
            provjeraPrijevozPrivatnimVozilom = false;
            postojeLiSuputnici = false;
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

            ViewBag.PodnositeljZahtjeva = new SelectList(db.Zaposlenici.Where(x => x.Student == false), "Prezime", "Prezime");

            List<int> ukupnoZaposlenika = new List<int>();
            int brojZaposlenika = db.Zaposlenici.Count();

            ViewBag.Suputnici = new SelectList(ukupnoZaposlenika);

            for (int i = 0; i < brojZaposlenika; i++)
            {
                ukupnoZaposlenika.Add(i);
            }


            return View();
        }

        [HttpPost]
        public ActionResult OtvaranjePutnogNaloga(PutniNalog nalog, string PrijevoznoSredstvo, DateTime? Datum_pocetka_putovanja, DateTime? Datum_zavrsetka_putovanja, string PodnositeljZahtjeva, int Suputnici)
        {
            ViewBag.PrijevoznoSredstvo = new SelectList(db.PrijevoznaSredstva, "IDTipPrijevoznogSredstva", "PrijevoznoSredstvo");
            ViewBag.PodnositeljZahtjeva = new SelectList(db.Zaposlenici.Where(x => x.Student == false), "Prezime", "Prezime");

            nalog.IDPrijevoznogSredstva = int.Parse(PrijevoznoSredstvo);
            nalog.Broj_osoba = Suputnici + 1;
            nalog.Podnositelj_zahtjeva = PodnositeljZahtjeva;
            brojSuputnika = Suputnici;
            int brojZaposlenika = db.Zaposlenici.Count();

            List<int> ukupnoZaposlenika = new List<int>();
            brojDodatnihPutnika = Suputnici;
            ViewBag.Suputnici = new SelectList(ukupnoZaposlenika);

            for (int i = 0; i < brojZaposlenika; i++)
            {
                ukupnoZaposlenika.Add(i);
            }



            //  provjera razlike u datumima

            if (Datum_pocetka_putovanja != null && Datum_zavrsetka_putovanja != null)
            {
                DateTime d1 = (DateTime)Datum_pocetka_putovanja;
                DateTime d2 = (DateTime)Datum_zavrsetka_putovanja;

                // string polazakMDY = String.Format("{0:MM/d/yyyy HH:mm:ss}", d1);
                // string zavrsetakMDY = String.Format("{0:MM/d/yyyy HH:mm:ss}", d2);
                //d1 = Convert.ToDateTime(polazakMDY);
                //d2 = Convert.ToDateTime(zavrsetakMDY);

                TimeSpan razlikaUDatumima = d2.Subtract(d1);

                if (razlikaUDatumima.Days < 0)
                {
                    ModelState.AddModelError("Datum_pocetka_putovanja", "Datum pocetka putovanja mora biti prije datuma zavrsetka putovanja (DMY format).");


                }
            }



            if (ModelState.IsValid)
            {
                db.PutniNalog.Add(nalog);
                db.SaveChanges();


                if (nalog.Smještaj == true)
                {
                    provjeraSmjestaj = true;
                }
                if (Suputnici > 0)
                {
                    postojeLiSuputnici = true;


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

                if (Suputnici > 0)
                {
                    postojeLiSuputnici = true;
                    return RedirectToAction("UnosSuputnika", Suputnici);

                }
                else
                {
                    dynamic email = new Email("PutniNalogEmail"); // POSTAL - ZA SLANJE EMAILOVA
                    email.To = "testiranjepostalemaila@gmail.com ";
                    email.BrojPutnihNaloga = brojDodatnihPutnika + 1;
                    email.Nalog = nalog;
                    email.Send();
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
                if (postojeLiSuputnici)
                {

                    return RedirectToAction("UnosSuputnika", brojSuputnika);

                }

                else
                {
                    dynamic email = new Email("PutniNalogEmail"); // POSTAL - ZA SLANJE EMAILOVA
                    email.To = "testiranjepostalemaila@gmail.com ";
                    email.PrivatnoVozilo = privatnoVozilo;
                    email.BrojPutnihNaloga = brojDodatnihPutnika + 1;
                    email.Nalog = db.PutniNalog.OrderByDescending(x => x.ID).First();
                    email.ImenaPutnika = ImenaSuputnika;
                    email.Send();
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

            TimeSpan razlikaUDatumimaDolaskaIOdlaska = smjestaj.Odlazak_iz_smještaja.Subtract(smjestaj.Dolazak_u_smještaj);
            TimeSpan razlikaUDatumimaNocenja = smjestaj.Zadnje_noćenje.Subtract(smjestaj.Prvo_noćenje);

            var pocetakPutovanja = db.PutniNalog.OrderByDescending(x => x.ID).First().Datum_pocetka_putovanja; //zadnji datum pocetka putovanja iz klase PutniNalog
            var zavrsetakPutovanja = db.PutniNalog.OrderByDescending(x => x.ID).First().Datum_zavrsetka_putovanja; //zadnji datum završetka putovanja iz klase PutniNalog




            if (razlikaUDatumimaNocenja.Days < 0)
            {
                ModelState.AddModelError("Prvo_noćenje", "Datum prvog noćenja ne može biti nakon datuma poslijednjeg noćenja.");

            }
            if (razlikaUDatumimaDolaskaIOdlaska.Days < 0)
            {
                ModelState.AddModelError("Dolazak_u_smještaj", "Datum dolaska u smjestaj ne može biti nakon datuma odlaska.");


            }

            if (smjestaj.Prvo_noćenje < pocetakPutovanja || smjestaj.Prvo_noćenje > zavrsetakPutovanja)
            {
                ModelState.AddModelError("Prvo_noćenje", "Datum prvog noćenja ne može biti prije datuma početka putovanja te nakon datuma završetka putovanja.");

            }
            if (smjestaj.Zadnje_noćenje > zavrsetakPutovanja)
            {
                ModelState.AddModelError("Zadnje_noćenje", "Datum zadnjeg noćenja ne može biti nakon datuma završetka putnovanja.");

            }

            if (smjestaj.Dolazak_u_smještaj > smjestaj.Zadnje_noćenje || smjestaj.Dolazak_u_smještaj < pocetakPutovanja || smjestaj.Dolazak_u_smještaj > zavrsetakPutovanja)
            {
                ModelState.AddModelError("Dolazak_u_smještaj", "Datum dolaska u smještaj ne može biti nakon datuma zadnjeg noćenja, ne smije biti manji od datuma početka putovanja te veći od datuma završetka putovanja.");

            }
            if (smjestaj.Dolazak_u_smještaj > smjestaj.Prvo_noćenje)
            {
                ModelState.AddModelError("Dolazak_u_smještaj", "Datum dolaska u smještaj ne može biti nakon datuma prvog noćenja.");

            }
            if (smjestaj.Odlazak_iz_smještaja < smjestaj.Prvo_noćenje || smjestaj.Odlazak_iz_smještaja < smjestaj.Zadnje_noćenje || smjestaj.Odlazak_iz_smještaja < smjestaj.Dolazak_u_smještaj || smjestaj.Odlazak_iz_smještaja > zavrsetakPutovanja)
            {
                ModelState.AddModelError("Odlazak_iz_smještaja", "Provjerite datum odlaska iz smještaja. (mora biti nakon datuma: prvog noćenja, zadnjeg noćenja , dolaska u smještaj te datuma završetka putovanja.)");

            }


            if (ModelState.IsValid)
            {
                db.Smejstaj.Add(smjestaj);
                db.SaveChanges();

                if (postojeLiSuputnici)
                {

                    return RedirectToAction("UnosSuputnika", brojSuputnika);

                }

                dynamic email = new Email("PutniNalogEmail"); // POSTAL - ZA SLANJE EMAILOVA
                email.To = "testiranjepostalemaila@gmail.com ";
                email.PrivatnoVozilo = db.PrivatnoVozilo.OrderByDescending(x => x.ID).FirstOrDefault();
                email.Nalog = db.PutniNalog.OrderByDescending(x => x.ID).FirstOrDefault();
                email.BrojPutnihNaloga = brojDodatnihPutnika + 1;
                email.Smjestaj = smjestaj;
                email.ImenaPutnika = ImenaSuputnika;
                email.Send();


                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }
        }

        public ActionResult IspisPutnogNaloga()
        {

            return View(db.PutniNalog);
        }


        public ActionResult UnosSuputnika()
        {
            @ViewBag.DodatniPutnici = brojDodatnihPutnika;
            return View();
        }

        [HttpPost]
        public ActionResult UnosSuputnika(string[] Suputnici)
        {
            @ViewBag.DodatniPutnici = brojDodatnihPutnika;
            List<int> studentsID = new List<int>();
            string suputnik;
            for (int i = 0; i < Suputnici.Length; i++)
            {
                suputnik = Suputnici[i];
                string[] imeIPrezime = Suputnici[i].Split();

                for (int j = 0; j < imeIPrezime.Length; j++)
                {
                    if (j + 1 < imeIPrezime.Length || imeIPrezime.Length==1)
                    {
                        string ime = imeIPrezime[j];
                        string prezime="";
                     
                        if (imeIPrezime.Length > 1)
                        {
                            prezime = imeIPrezime[j + 1];
                        }
                     
                        if (db.Zaposlenici.Any(x => x.Ime.ToLower().StartsWith(ime.ToLower())) && db.Zaposlenici.Any(x => x.Prezime.ToLower().StartsWith(prezime.ToLower())))
                        {
                           
                           var uneseniSuputnik = db.Zaposlenici.Where(x => x.Prezime.ToLower().StartsWith(prezime)).Where(x => x.Ime.ToLower().StartsWith(ime.ToLower()));
                           if (uneseniSuputnik.First().Student)
                           {
                               studentsID.Add(uneseniSuputnik.First().ID);
                           }
                        }
                        else
                        {
                            ModelState.AddModelError("Suputnici", "Molim unesite točno ime i prezime zaposlenika (bez korištenja slova č,ć,ž i š).");

                        }
                    }
                }

          


            }
            if(Suputnici.Any(x=>x==""))
            {
                ModelState.AddModelError("Suputnici", "Molim unesite točno ime i prezime zaposlenika (bez korištenja slova č,ć,ž i š.");

            }

            if (ModelState.IsValid)
            {
                ImenaSuputnika = Suputnici.ToList();
                dynamic email = new Email("PutniNalogEmail"); // POSTAL - ZA SLANJE EMAILOVA
                email.To = "testiranjepostalemaila@gmail.com ";
                email.PrivatnoVozilo = db.PrivatnoVozilo.OrderByDescending(x => x.ID).FirstOrDefault();
                email.Nalog = db.PutniNalog.OrderByDescending(x => x.ID).First();
                email.Smjestaj = db.Smejstaj.OrderByDescending(x => x.ID).FirstOrDefault();
                email.BrojPutnihNaloga = brojDodatnihPutnika + 1 - studentsID.Count();
                email.BrojStudenata = studentsID.Count();
                email.UkupanBrojPutnika = brojDodatnihPutnika;
                email.ImenaPutnika = ImenaSuputnika;
                email.Send();

                return View("Index");
            }
            else
            {
                return View();
            }
        }

    }
}