using Gabriel.Cat.S.Utilitats;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gabriel.Cat.S.Xarxa.Cartes
{
    class CartaPartPaquet:Carta
    {
        IdUnico idPaquet;
        int posicioPart;
        int lenghtPaquet;

        public CartaPartPaquet(IdUnico idPaquet, int posicioPart,int lenghtPaquet,int idIntern=-1):base(idIntern)
        {
            IdPaquet = idPaquet;
            PosicioPart = posicioPart;
            LenghtPaquet = lenghtPaquet;
        }

        public IdUnico IdPaquet { get => idPaquet; private set => idPaquet = value; }
        public int PosicioPart { get => posicioPart; private set => posicioPart = value; }
        public int LenghtPaquet { get => lenghtPaquet; private set => lenghtPaquet = value; }

        protected override string Tipo => "PartPaquet";
    }
}
