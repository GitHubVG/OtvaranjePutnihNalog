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
    
    public partial class TipPrijevoznogSredstva
    {
        public TipPrijevoznogSredstva()
        {
            this.PrijevoznaSredstva = new HashSet<PrijevoznaSredstva>();
        }
    
        public int ID { get; set; }
        public string VrstaPrijevoznogSredstva { get; set; }
    
        public virtual ICollection<PrijevoznaSredstva> PrijevoznaSredstva { get; set; }
    }
}
