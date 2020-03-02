using System;
using System.Collections.Generic;
using System.Text;

namespace Gabriel.Cat.S.Xarxa.BDSync
{
   public class SyncBDs
    {
        public List<BD> BDs { get; private set; }
        public DateTime LastSync { get; private set; }
        public bool IsOnline { get; }
        public SyncBDs()
        {
            BDs = new List<BD>();
        }
        public bool Sync()
        {
            bool syncronitzed = IsOnline;
            List<BD.Change> changesBD;
            if (syncronitzed)
            {
                //obtengo los cambios de cada BD
                for(int i=0;i<BDs.Count;i++)
                {
                    changesBD = BDs[i].GetChanges(LastSync);
                    for(int j = 0; j < BDs.Count; j++)
                    {
                        if (i != j)
                        {
                            BDs[j].SetChanges(changesBD);
                        }
                    }
                }
                LastSync = DateTime.UtcNow;
            }

            return syncronitzed;
        }
    }
}
