using Gabriel.Cat.S.Utilitats;
using System;
using System.Collections.Generic;
using System.Text;
//using Gabriel.Cat.S.Seguretat;
//using Gabriel.Cat.S.Binaris;

namespace Gabriel.Cat.S.Xarxa.KeyServer
{
    internal delegate IdUnico GetIdUnico();
    //internal class KeyServer
    //{
       
    //    public static int NumeroItemsCrazyKey = 5;

    //    static readonly ElementoBinario SerializadorKey=new Gabriel.Cat.S.Binaris.KeyBinario();
    //    static readonly ElementoBinario SerializadorCrazyKey = new Gabriel.Cat.S.Binaris.CrazyKeyBinario();
    //    LlistaOrdenada<IdUnico, CrazyKey> dicCliente;
    //   //genero una se la mando al usuario, luego cuando soliciten una conexion genero una crazykey se la mando al usuario y luego cifro su key con esa crazykey y se la mando a la otra parte asi el usuario de forma segura se hace con su key y luego solo recive crazykeys para establecer su parte por otro lado al otro se le envia cifrada con su key la key generada y listo :)
    //    LlistaOrdenada<IdUnico, DateTime> dicIdsFecha;
    //    GetIdUnico GetId { get; set; } 
    //    public IServidorKeyUser ServidorKey { get; set; }
    //    public KeyServer(IServidorKeyUser servidorKey=null):this(servidorKey,()=>new IdUnico())
    //    {

    //    }
    //    public KeyServer(IServidorKeyUser servidorKey,GetIdUnico getIdUnico)
    //    {
    //        if (servidorKey == null)
    //            servidorKey = new ServidorKey();

    //        GetId = getIdUnico;
    //        dicCliente = new LlistaOrdenada<IdUnico, CrazyKey>();
    //        ServidorKey = servidorKey;
    //        dicIdsFecha = new LlistaOrdenada<IdUnico, DateTime>();
    //    }
    //    public Key GetKey(IdUnico idUsuario)
    //    {
    //        return ServidorKey.GetKey(idUsuario);
    //    }
    //    public CrazyKey GetCrazyKey()
    //    {
    //        return GetCrazyKey(GetId());
    //    }

        
    //    public CrazyKey GetCrazyKey(IdUnico idCrazyKey)
    //    {
    //        if (!dicCliente.ContainsKey(idCrazyKey))
    //        {
    //            dicCliente.Add(idCrazyKey, new CrazyKey(NumeroItemsCrazyKey));
    //            dicIdsFecha.Add(idCrazyKey, DateTime.UtcNow);
    //        }
    //        return dicCliente[idCrazyKey];
    //    }
    //    public byte[] GetCrazyKeyEncrypted(IdUnico idUsuario,IdUnico idCrazyKey)
    //    {
    //        return GetKey(idUsuario).Encrypt(SerializadorCrazyKey.GetBytes(GetCrazyKey(idCrazyKey)));
    //    }
    //    public Key GetKey(IdUnico idUsuario, IdUnico idCrazyKey)
    //    {
    //        return dicCliente[idCrazyKey].Encrypt(GetKey(idUsuario));
    //    }
    //    public byte[] GetEncryptedKey(IdUnico idCliente,IdUnico idUsuario,IdUnico idCrazyKey)
    //    {
    //        return GetKey(idCliente).Encrypt(SerializadorKey.GetBytes((GetKey(idUsuario, idCrazyKey))));
    //    }
    //    public Key GetDecryptedKeyClient(IdUnico idCliente,byte[] keyEncrypted)
    //    {
    //        return (Key)SerializadorKey.GetObject(GetKey(idCliente).Decrypt(keyEncrypted));
    //    }
    //    public Key GetDecryptedKeyUser(IdUnico idUsuario,byte[] crazyKeyEncrypted)
    //    {
    //        return ((CrazyKey)SerializadorCrazyKey.GetObject(GetKey(idUsuario).Decrypt(crazyKeyEncrypted))).Encrypt(GetKey(idUsuario));
    //    }
    //    public void LimpiarCrazyKeys(DateTime fechaMaxima=default)
    //    {
    //        List<IdUnico> ids;
    //        IdUnico aux;
    //        if (fechaMaxima == default)
    //            fechaMaxima = DateTime.UtcNow;
    //        ids = new List<IdUnico>();
    //        for(int i=0;i<dicIdsFecha.Count;i++)
    //        {
    //            aux = dicIdsFecha.GetKey(i);
    //            if (dicIdsFecha[aux] < fechaMaxima)
    //            {
    //                ids.Add(aux);
    //            }
    //        }
    //        dicIdsFecha.RemoveRange(ids);
    //        dicCliente.RemoveRange(ids);
            
    //    }
    //}
}
