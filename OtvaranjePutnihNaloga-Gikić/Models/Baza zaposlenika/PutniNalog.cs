//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace OtvaranjePutnihNaloga_Gikić.Models.Baza_zaposlenika
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;


    [MetadataType(typeof(OtvaranjePutnihNaloga_Gikić.Models.MetadataKlasa.MetadataPutniNalog))]
    public partial class PutniNalog
    {
        public PutniNalog()
        {
            this.PrivatnoVozilo = new HashSet<PrivatnoVozilo>();
            this.Smejstaj = new HashSet<Smejstaj>();
        }
    
        public int ID { get; set; }
        public string Podnositelj_zahtjeva { get; set; }
        public System.DateTime Datum_pocetka_putovanja { get; set; }
        public System.DateTime Datum_zavrsetka_putovanja { get; set; }
        public Nullable<int> IDPrijevoznogSredstva { get; set; }
        public string Razlog_dolaska { get; set; }
        public string Relacija_putovanja { get; set; }
        public Nullable<int> Broj_projekta { get; set; }
        public bool Smještaj { get; set; }
        public Nullable<int> Broj_osoba { get; set; }
    
        public virtual PrijevoznaSredstva PrijevoznaSredstva { get; set; }
        public virtual ICollection<PrivatnoVozilo> PrivatnoVozilo { get; set; }
        public virtual ICollection<Smejstaj> Smejstaj { get; set; }
    }
}
