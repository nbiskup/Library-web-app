using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RwaLib.MODELS
{
    public class User
    {
        public int IDUser { get; set; }
        public string UserCode { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [NotMapped]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
        public string OIB { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string CityName { get; set; }
        public string Address { get; set; }
        public int ZipCode { get; set; }
        public bool IsAdmin { get; set; }
        public string WorkPosition { get; set; }
        public IList<Purchase> Purchases { get; set; }
        public IList<Loan> Loans { get; set; }
    }
}
