using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RwaLib.MODELS
{
    public class Purchase
    {
        public int IDPurchase { get; set; }
        public Book Book { get; set; }
        public User User { get; set; }
        public DateTime PurchaseDate { get; set; }
    }
}
