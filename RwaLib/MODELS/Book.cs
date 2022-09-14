using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RwaLib.MODELS
{
    public class Book : IComparable<Book>
    {
        public int IDBook { get; set; }
        public string ISBN { get; set; }
        public string Title { get; set; }
        public decimal Price { get; set; }
        public decimal LoanPrice { get; set; }
        public Author Author { get; set; }
        public int Quantity { get; set; }
        public string PicturePath { get; set; }
        public Status Status { get; set; }
        public string Description { get; set; }
        public int CompareTo(Book other) => Title.CompareTo(other.Title);
        public override bool Equals(object obj)
        {
            return obj is Book book &&
                   Title == book.Title;
        }
        public override int GetHashCode()
        {
            return Title.GetHashCode();
        }
    }
}
