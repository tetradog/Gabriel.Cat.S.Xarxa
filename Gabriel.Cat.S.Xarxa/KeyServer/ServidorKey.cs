using Gabriel.Cat.S.Binaris;
using Gabriel.Cat.S.Extension;
using Gabriel.Cat.S.Seguretat;
using Gabriel.Cat.S.Utilitats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gabriel.Cat.S.Xarxa
{
    public class ServidorKey : IServidorKeyUser
    {
        static readonly ElementoBinario SerializadorBin = new ElementoIListBinario<byte[]>(new ByteArrayBinario());
        static readonly ElementoBinario SerializadorKey = new ElementoIListBinario<Key>(new KeyBinario());
        public static long NumeroPasswords = 100;
        LlistaOrdenada<IdUnico, Key> dicKeysUsuarios;

        public ServidorKey(byte[] data=null)
        {
            dicKeysUsuarios = new LlistaOrdenada<IdUnico, Key>();
            if(data!=null)
              SetBytes(data);
        }
        public byte[] GetBytes()
        {
            return SerializadorBin.GetBytes(new byte[][] { SerializadorBin.GetBytes(dicKeysUsuarios.GetKeys().Select((id)=>id.GetId())).ToArray(), SerializadorKey.GetBytes(dicKeysUsuarios.GetValues()) });
        }
        public void SetBytes(byte[] data)
        {
            byte[][] dataPartes =(byte[][]) SerializadorBin.GetObject(data);
            int pos= 0;
            Key[] keys =(Key[]) SerializadorKey.GetObject(dataPartes[1]);
            dicKeysUsuarios.Clear();
            foreach (IdUnico id in ((byte[][])SerializadorBin.GetObject(dataPartes[0])).Select((idBin)=>new IdUnico(idBin)))
            {
                dicKeysUsuarios.Add(id, keys[pos++]);
            }
           
        }
        public bool ContainsUser(IdUnico idUser)
        {
            return dicKeysUsuarios.ContainsKey(idUser);
        }

        public Key GetKey(IdUnico idUser)
        {
            if (!ContainsUser(idUser))
                dicKeysUsuarios.Add(idUser, Key.GetKey(NumeroPasswords));
            return dicKeysUsuarios[idUser];
        }
    }
}
