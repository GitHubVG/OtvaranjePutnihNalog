using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OtvaranjePutnihNaloga_Gikić.Models
{
    public class MetadataKlasa
    {


        //ZAPOSLENICI

        [MetadataType(typeof(MetadataZaposlenika))]
        public partial class Zaposlenici
        {

        }

        internal class MetadataZaposlenika
        {
            [Required(ErrorMessage = "Molim unesite ime.")]
            public string Ime { get; set; }

            [Required(ErrorMessage = "Molim unesite prezime.")]
            public string Prezime { get; set; }

            [Required(ErrorMessage = "Molim unesite email adresu.")]
            [EmailAddress(ErrorMessage = "Pogrešna email adresa")]
            public string Email { get; set; }

            [Required(ErrorMessage = "Jeste li student?.")]
            public bool Student { get; set; }

            [Required(ErrorMessage = "Molim unesite odjel.")]
            [Display(Name = "Odjel")]
            public int IDOdjela { get; set; }

        }


        //PUTNI NALOG

        [MetadataType(typeof(MetadataPutniNalog))]
        public partial class PutniNalog
        {

        }

        internal class MetadataPutniNalog
        {
            [Required(ErrorMessage = "Molim unesite podnositelja zahtjeva.")]
            [Display(Name = "Podnositelj zahtjeva")]

            public string Podnositelj_zahtjeva { get; set; }

            [Required(ErrorMessage = "Molim unesite datum početka putovanja.")]
            [Display(Name = "Datum početka putovanja")]
            public System.DateTime Datum_pocetka_putovanja { get; set; }

            [Required(ErrorMessage = "Molim unesite datum završetka putovanja.")]
            [Display(Name = "Datum završetka putovanja")]
            public System.DateTime Datum_zavrsetka_putovanja { get; set; }

           // [Required(ErrorMessage = "Molim unesite prijevozno sredstvo kojim ćete putovati.")]
            [Display(Name = "Prijevozno sredstvo")]
            public Nullable<int> IDPrijevoznogSredstva { get; set; }

            [Required(ErrorMessage = "Molim unesite razlog odlaska na put.")]
            [Display(Name = "Razlog putovanja")]
            public string Razlog_dolaska { get; set; }

            [Required(ErrorMessage = "Molim unesite relaciju.")]
            [Display(Name = "Relacija")]
            public string Relacija_putovanja { get; set; }

            [Display(Name = "Broj projekta (opcionalno)")]
            public Nullable<int> Broj_projekta { get; set; }

            
            [Display(Name = "Je li smještaj potreban?")]
            public bool Smještaj { get; set; }
        }




        //PRIVATNO VOZILO

        [MetadataType(typeof(MetadataPrivatnoVozilo))]
        public partial class PrivatnoVozilo
        {

        }

        public class MetadataPrivatnoVozilo
        {
            [Required(ErrorMessage = "Molim unesite registraciju vozila.")]
            [Display(Name = "Registracija")]
            public int RegistracijaPrivatnogVozila { get; set; }

            [Required(ErrorMessage = "Jeste li suvozač?.")]
            [Display(Name = "Jeste li suvozač?")]
            public bool Suvozac { get; set; }

        }

        //Smjestaj

        [MetadataType(typeof(MetadataSmjestaj))]
        public partial class Smejstaj
        {

        }
        public class MetadataSmjestaj
        {
            [Required(ErrorMessage = "Molim unesite datum.")]
            [Display(Name = "Datum prvog noćenja")]
            public System.DateTime Prvo_noćenje { get; set; }

            [Required(ErrorMessage = "Molim unesite datum.")]
            [Display(Name = "Datum zadnjeg noćenja")]
            public System.DateTime Zadnje_noćenje { get; set; }

            [Required(ErrorMessage = "Molim unesite datum.")]
            [Display(Name = "Datum dolaska u smještaj")]
            public System.DateTime Dolazak_u_smještaj { get; set; }

            [Required(ErrorMessage = "Molim unesite datum.")]
            [Display(Name = "Datum odlaska iz smještaja")]
            public System.DateTime Odlazak_iz_smještaja { get; set; }

        }


    }
}