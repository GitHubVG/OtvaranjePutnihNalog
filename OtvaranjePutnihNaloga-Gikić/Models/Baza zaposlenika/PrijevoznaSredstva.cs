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
    
    public partial class PrijevoznaSredstva
    {
        public PrijevoznaSredstva()
        {
            this.PutniNalog = new HashSet<PutniNalog>();
        }
    
        public int ID { get; set; }
        public string PrijevoznoSredstvo { get; set; }
        public int IDTipPrijevoznogSredstva { get; set; }
    
        public virtual TipPrijevoznogSredstva TipPrijevoznogSredstva { get; set; }
        public virtual ICollection<PutniNalog> PutniNalog { get; set; }
    }
}
