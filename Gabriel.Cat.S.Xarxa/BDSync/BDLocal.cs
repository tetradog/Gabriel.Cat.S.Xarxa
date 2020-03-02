using System;
using System.Collections.Generic;
using System.Text;

namespace Gabriel.Cat.S.Xarxa.BDSync
{
    public class BDLocal : BD
    {
        public override List<Change> GetChanges(DateTime lastSync)
        {
            throw new NotImplementedException();
        }

        public override void SetChanges(IList<Change> changes)
        {
            throw new NotImplementedException();
        }
    }
}
