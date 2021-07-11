using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Gabriel.Cat.S.Utilitats;


namespace Gabriel.Cat.S.Xarxa
{
    internal delegate void PaquetRebutEventHandler(MissatgerObject missatget, Paquet paquet);
    internal class MissatgerObject
    {
        public  int PortEnviar = 9087;
        public  int PortRebre = 9085;

        LlistaOrdenada<Paquet> paquetsPerEnviar;
        LlistaOrdenada<Paquet> paquetsPerRebre;

        TcpClient clientEnviar;
        TcpListener clientRebre;

        Thread threadEnviar;
        Thread threadRebre;

        public event PaquetRebutEventHandler PaquetRebut;
        public event PaquetRebutEventHandler PaquetEnviat;

        public MissatgerObject(TcpClient client)
        {
            this.clientEnviar = new TcpClient(((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString(), PortEnviar);
            this.clientRebre = new TcpListener(IPAddress.Any,PortRebre);
            paquetsPerEnviar = new LlistaOrdenada<Paquet>();
            paquetsPerRebre = new LlistaOrdenada<Paquet>();
  
        }
        public void Start()
        {
            if(threadEnviar==null||!threadEnviar.IsAlive)
            {
                threadEnviar = new Thread(() => {


                });
                threadEnviar.Start();
            }
            if (threadRebre == null || !threadRebre.IsAlive)
            {
                threadRebre = new Thread(() => {
                    clientRebre.Start(); 
                    byte[] buffer;
                    while(true)
                    {
                        try
                        {
                            buffer = new byte[Paquet.LENGHTPARTPAQUET];

                            clientRebre.AcceptSocket().Receive(buffer);
                            DatosRecividos(buffer);
                        }
                        catch { }
                    }

                });
                threadRebre.Start();
            }
        }

        internal void Enviar(byte[] v)
        {
            throw new NotImplementedException();
        }

        private void DatosRecividos(byte[] buffer)
        {
            throw new NotImplementedException();
        }
    }
}
