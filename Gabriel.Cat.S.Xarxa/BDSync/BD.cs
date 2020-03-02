using System;
using System.Collections.Generic;
using System.Text;

namespace Gabriel.Cat.S.Xarxa.BDSync
{
    public abstract class BD
    {
        public class Change {
        
        public string SQL { get; set; }
        }

        public abstract List<Change> GetChanges(DateTime lastSync);
        public abstract void SetChanges(IList<Change> changes);
        
    }
}
