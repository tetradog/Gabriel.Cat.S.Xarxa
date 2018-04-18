using Gabriel.Cat.S.Extension;
using Gabriel.Cat.S.Utilitats;
using Gabriel.Cat.S.Xarxa.Cartes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gabriel.Cat.S.Xarxa
{
    class Paquet : IComparable, IComparable<Paquet>, IClauUnicaPerObjecte
    {
        public const int LENGHTPARTPAQUET = 120 * 1024;//120KB

        IdUnico idPaquete;
        byte[] dadesPaquet;
        bool[] parts;

        public Paquet(CartaPartPaquet cartaPart)
        {
            this.IdPaquet = cartaPart.IdPaquet;
            this.parts = new bool[GetNumParts(cartaPart.LenghtPaquet)];
            this.DadesPaquet = new byte[cartaPart.LenghtPaquet];
        }

        public Paquet(byte[] dades)
        {
            DadesPaquet = dades;
            IdPaquet = new IdUnico();
            parts = new bool[GetNumParts(DadesPaquet.Length)];
        }
        public bool Acabat
        {
            get
            {
                bool acabat = true;
                for (int i = 0; i < parts.Length && acabat; i++)
                    acabat = parts[i];
                return acabat;
            }
        }

        public IdUnico IdPaquet { get => idPaquete; private set => idPaquete = value; }
        public byte[] DadesPaquet { get => dadesPaquet; private set => dadesPaquet = value; }
        public int Total => parts.Length;

        IComparable IClauUnicaPerObjecte.Clau => IdPaquet;

        private int GetNumParts(int lenghtPaquet)
        {
            return lenghtPaquet / LENGHTPARTPAQUET < 0 ? 1 : lenghtPaquet / LENGHTPARTPAQUET;
        }
        public void PartRebuda(int posicioPaquet, byte[] paquetRebut)
        {
            DadesPaquet.SetArray(LENGHTPARTPAQUET * posicioPaquet, paquetRebut);
            parts[posicioPaquet] = true;
        }
        public byte[] EnviarPart()
        {
            int pos;
            byte[] part;
            pos = PartFaltant();
            if (pos == -1)
                part = null;
            else part = EnviarPart(pos);
            return part;
        }

        private int PartFaltant()
        {
            int pos = -1;
            for (int i = 0; i < parts.Length && pos == -1; i++)
                if (!parts[i])
                    pos = i;
            return pos;
        }

        public byte[] EnviarPart(int posicioPaquet)
        {
            return DadesPaquet.SubArray(LENGHTPARTPAQUET * posicioPaquet, LENGHTPARTPAQUET);
        }
        public void PartEnviadaRebuda(int posicioPaquet)
        {
            parts[posicioPaquet] = true;
        }
        public Cartes.CartaPartPaquet EnviarCartaPartPaquet()
        {
            int pos;
            Cartes.CartaPartPaquet cartaPartPaquet;
            pos = PartFaltant();
            if (pos == -1)
                cartaPartPaquet = null;
            else cartaPartPaquet = EnviarCartaPartPaquet(pos);
            return cartaPartPaquet;
        }
        public Cartes.CartaPartPaquet EnviarCartaPartPaquet(int posicioPaquet)
        {
            return new Cartes.CartaPartPaquet(IdPaquet, posicioPaquet, dadesPaquet.Length);
        }

        int IComparable<Paquet>.CompareTo(Paquet other)
        {
            return CompareTo(other);
        }
        int CompareTo(Paquet other)
        {
            int compareTo;
            if (other != null)
                compareTo = IdPaquet.CompareTo(other.IdPaquet);
            else compareTo = (int)Gabriel.Cat.S.Utilitats.CompareTo.Inferior;
            return compareTo;
        }

        int IComparable.CompareTo(object obj)
        {
            return CompareTo(obj as Paquet);
        }
    }
}
