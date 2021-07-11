using System;
using System.Collections.Generic;
using System.Text;
using Gabriel.Cat.S;
using Gabriel.Cat.S.Utilitats;
namespace Gabriel.Cat.S.Xarxa.Cartes
{
    internal abstract class Carta
    {
        static GenIdInt genId = new GenIdInt();
        int idIntern;
        public Carta(int idIntern=-1)
        {
            if (idIntern < 0)
                idIntern = genId.Siguiente();
            this.IdIntern = idIntern; 
        }

        public int IdIntern { get => idIntern; private set => idIntern = value; }
        protected abstract string Tipo { get; }
        public override string ToString()
        {
            return "Carta:"+Tipo;
        }
    }
}
