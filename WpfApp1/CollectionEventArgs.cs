using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Proxy;
namespace DetControlTower
{
    public class FactureEventArgs <TFacturable> : EventArgs
    {
        public ObservableCollection<TFacturable> Collection { set; get; }
        public Facture Facture { set; get; }

        public FactureEventArgs (Facture facture , ObservableCollection<TFacturable> collection)
        {
            this.Collection = collection;
            this.Facture = facture;
        }
    }
}
