using System;
using System.Collections.Generic;
using System.Text;

namespace Gabriel.Cat.S.Xarxa.Cartes
{
    class CartaConfirmacio:Carta
    {
        public enum Tipus
        {
            CartaPartPaquet,
            PartPaquet,
        }
        Tipus tipus;
       
        public CartaConfirmacio(Tipus tipus,int idIntern=-1):base(idIntern)
        { this.tipus = tipus; }
        protected override string Tipo => "Confirmació";
    }
}
