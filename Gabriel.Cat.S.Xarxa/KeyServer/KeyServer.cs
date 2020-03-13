using Gabriel.Cat.S.Utilitats;
using System;
using System.Collections.Generic;
using System.Text;
using Gabriel.Cat.S.Seguretat;
namespace Gabriel.Cat.S.Xarxa.KeyServer
{
    public class KeyServer
    {
        public static long NumeroPasswords = 100;
        public static int NumeroItemsCrazyKey = 5;

        LlistaOrdenada<long, CrazyKey> dicCliente;
        LlistaOrdenada<long, Key> dicKeysUsuarios;//genero una se la mando al usuario, luego cuando soliciten una conexion genero una crazykey se la mando al usuario y luego cifro su key con esa crazykey y se la mando a la otra parte asi el usuario de forma segura se hace con su key y luego solo recive crazykeys para establecer su parte por otro lado al otro se le envia cifrada con su key la key generada y listo :)
        LlistaOrdenada<long, DateTime> dicIdsFecha;
        private long GetIdUnico()
        {
            return DateTime.UtcNow.Ticks;
        }

        public Key GetKey(long idUsuario)
        {
            if (!dicKeysUsuarios.ContainsKey(idUsuario))
            {
                dicKeysUsuarios.Add(idUsuario, Key.GetKey(NumeroPasswords));

            }
            return dicKeysUsuarios[idUsuario];
        }
        public CrazyKey GetCrazyKey()
        {
            return GetCrazyKey(GetIdUnico());
        }

        
        public CrazyKey GetCrazyKey(long idCrazyKey)
        {
            if (!dicCliente.ContainsKey(idCrazyKey))
            {
                dicCliente.Add(idCrazyKey, new CrazyKey(NumeroItemsCrazyKey));
                dicIdsFecha.Add(idCrazyKey, DateTime.UtcNow);
            }
            return dicCliente[idCrazyKey];
        }
        public Key GetKey(long idUsuario,long idCrazyKey)
        {
            return dicCliente[idCrazyKey].Encrypt(dicKeysUsuarios[idUsuario]);
        }
        public void LimpiarCrazyKeys(DateTime fechaMaxima=default)
        {
            List<long> ids;
            long aux;
            if (fechaMaxima == default)
                fechaMaxima = DateTime.UtcNow;
            ids = new List<long>();
            for(int i=0;i<dicIdsFecha.Count;i++)
            {
                aux = dicIdsFecha.GetKey(i);
                if (dicIdsFecha[aux] < fechaMaxima)
                {
                    ids.Add(aux);
                }
            }
            dicIdsFecha.RemoveRange(ids);
            dicKeysUsuarios.RemoveRange(ids);
        }
    }
}
