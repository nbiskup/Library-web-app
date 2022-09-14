using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RwaLib.MODELS
{
    public class Loan
    {
        public int IDLoan { get; set; }
        public User User { get; set; }
        public Book Book { get; set; }

        [DataType(DataType.Date)]
        public DateTime LoanBeginDate { get; set; }
        public DateTime LoanEndDate { get; set; }
        public decimal DelayPrice { get; set; }
        public decimal TotalDelayAmount { get; set; }
        public bool IsSettled { get; set; }
    }
}
