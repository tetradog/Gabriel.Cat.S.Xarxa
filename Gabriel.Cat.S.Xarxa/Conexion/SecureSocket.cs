using System;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Gabriel.Cat.S.Seguretat;
using Gabriel.Cat.S.Utilitats;

namespace Gabriel.Cat.S.Xarxa
{
    public class SecureSocket
    {
     

        public static int NumPasswordsToEncrypt = 25;
        public static int TimeWaitNextAtentToListen = 5 * 1000;//5 segundos 
        
        Key keyConnexion;
        MissatgerObject missatger;
        Thread threadListen;

        public event EventHandler<SecureSocketReciveDataEventArgs> Recived;
        public SecureSocket(TcpClient tcpClient, Key key = null)
        {
            if (tcpClient == null)
                throw new ArgumentNullException("tcpClient");
            if (key == null)
                key = Key.GetKey(NumPasswordsToEncrypt);
            this.missatger = new MissatgerObject(tcpClient);
            this.missatger.PaquetRebut += (missatger,paquet) =>
            {
                Recived(this, new SecureSocketReciveDataEventArgs(this, keyConnexion.Decrypt(paquet.DadesPaquet)));
            };
            
        }
        public void Start(bool sendKey=true)
        {
            if (threadListen == null || !threadListen.IsAlive)
            {
                //send Key
                if (sendKey)
                {
                   
                }
                threadListen = new Thread(() => Listen());
            }

        }
        public void Stop()
        {
            if (threadListen != null && threadListen.IsAlive)
                threadListen.Abort();
        }
        private void Listen()
        {
            missatger.Start();
        }

        public void Send(byte[] data)
        {
            missatger.Enviar(keyConnexion.Encrypt(data));
        }

    }
   
}
