using Microsoft.ApplicationBlocks.Data;
using RwaLib.DAL;
using RwaLib.MODELS;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RwaLib.DAL.Managers
{
    public class BookManager
    {
        private string CS;
        public AuthorManager am;

        public BookManager(string connectionString)
        {
            CS = connectionString;
            am = new AuthorManager(connectionString);
        }
        public void Create(Book book, params object[] args)
        {
            SqlHelper.ExecuteNonQuery(CS, nameof(Create) + nameof(book),
                book.Title, book.Author.IDAuthor, book.PicturePath, book.Price,
                book.LoanPrice, book.ISBN, book.Status.IdStatus,
                book.Quantity, book.Description);
        }

        public void Delete(Book book)
        {
            SqlHelper.ExecuteNonQuery(CS, nameof(Delete) + nameof(Book), book.IDBook);
        }

        public Book Get(int id)
        {
            var tblBook = SqlHelper.ExecuteDataset(CS, nameof(Get) + nameof(Book), id).Tables[0];
            if (tblBook == null) return null;
            DataRow row = tblBook.Rows[0];

            Book book = new Book();

            //return new Book
            //{
            book.IDBook = id;
            book.Title = row[nameof(Book.Title)].ToString();
            book.Author = am.Get((int)row[nameof(Author.IDAuthor)]);
            book.PicturePath = row[nameof(Book.PicturePath)].ToString();
            book.Price = (decimal)row[nameof(Book.Price)];
            book.LoanPrice = (decimal)row[nameof(Book.LoanPrice)];
            book.ISBN = (string)row[nameof(Book.ISBN)];
            book.Status = GetStatus(1);
            book.Quantity = (int)row[nameof(Book.Quantity)];
            book.Description = row[nameof(Book.Description)].ToString();
            //};
            return book;
        }

        public Status GetStatus(int id)
        {
            var tblStatus = SqlHelper.ExecuteDataset(CS, nameof(GetStatus), id).Tables[0];
            if (tblStatus == null) return null;
            DataRow row = tblStatus.Rows[0];
            return new Status
            {
                IdStatus = (int)row[nameof(Status.IdStatus)],
                StatusName = row[nameof(Status.StatusName)].ToString()
            };

        }

        public IEnumerable<Status> GetAllStatus()
        {
            var tblStatus = SqlHelper.ExecuteDataset(CS, nameof(GetAllStatus)).Tables[0];
            if (tblStatus == null) return null;
            IList<Status> result = new List<Status>();

            foreach (DataRow row in tblStatus.Rows)
            {
                result.Add(new Status
                {
                    IdStatus = (int)row[nameof(Status.IdStatus)],
                    StatusName = row[nameof(Status.StatusName)].ToString()
                });
            }
            return result;
        }

        public IEnumerable<Book> GetAll()
        {
            var tblBook = SqlHelper.ExecuteDataset(CS, nameof(GetAll) + nameof(Book)).Tables[0];
            if (tblBook == null) return null;
            IList<Book> result = new List<Book>();

            foreach (DataRow row in tblBook.Rows)
            {
                result.Add(new Book
                {
                    Title = row[nameof(Book.Title)].ToString(),
                    Author = am.Get((int)row[nameof(Book.Author.IDAuthor)]),
                    Price = (decimal)row[nameof(Book.Price)],
                    PicturePath = row[nameof(Book.PicturePath)].ToString()
                });
            }
            return result;
        }
        public IEnumerable<Book> GetAllBooksByReading(FILTER filter)
        {
            var tblBook = SqlHelper.ExecuteDataset(CS, nameof(GetAllBooksByReading), filter.ToString()).Tables[0];
            if (tblBook == null) return null;
            IList<Book> result = new List<Book>();

            foreach (DataRow row in tblBook.Rows)
            {
                result.Add(new Book
                {
                    Title = row[nameof(Book.Title)].ToString(),
                    Author = am.Get((int)row[nameof(Book.Author.IDAuthor)]),
                    PicturePath = row[nameof(Book.PicturePath)].ToString(),
                    Price = (decimal)row[nameof(Book.Price)],
                });
            }
            return result;
        }

        public void RemoveBook(int bookid)
        {
            SqlHelper.ExecuteNonQuery(CS, nameof(RemoveBook), bookid);
        }


        public void Update(Book book, params object[] args)
        {
            SqlHelper.ExecuteNonQuery(CS, nameof(Update) + nameof(book),
                book.IDBook, book.Title, book.Author.IDAuthor, book.PicturePath,
                book.Price, book.LoanPrice, book.ISBN, book.Status.IdStatus,
                book.Quantity, book.Description);
        }


        public Book GetByTitle(string title)
        {
            var tblBook = SqlHelper.ExecuteDataset(CS, nameof(GetByTitle), title).Tables[0];
            if (tblBook == null) return null;
            DataRow row = tblBook.Rows[0];

            int qnt = (int)row[nameof(Book.Quantity)];
            return new Book
            {
                IDBook = (int)row[nameof(Book.IDBook)],
                Title = row[nameof(Book.Title)].ToString(),
                Author = am.Get((int)row[nameof(Author.IDAuthor)]),
                PicturePath = row[nameof(Book.PicturePath)].ToString(),
                Price = (decimal)row[nameof(Book.Price)],
                LoanPrice = (decimal)row[nameof(Book.LoanPrice)],
                ISBN = (string)row[nameof(Book.ISBN)],
                Status = GetStatus((int)row[nameof(Book.Status.IdStatus)]),
                Quantity = qnt,
                Description = row[nameof(Book.Description)].ToString()
            };
        }

        public void UpdateToUsed(Book book, params object[] args)
        {
            SqlHelper.ExecuteNonQuery(CS, nameof(UpdateToUsed) + nameof(book),
            book.IDBook, book.Title, book.Author.IDAuthor, book.PicturePath,
            book.Price, book.LoanPrice, book.ISBN,
            book.Quantity, book.Description);
        }

        public int GetQntOfBook(string title, int statusid)
        {
            var tbl = SqlHelper.ExecuteDataset(CS, nameof(GetQntOfBook), title, statusid).Tables[0];
            if (tbl == null || tbl.Rows.Count == 0) return 0;
            DataRow row = tbl.Rows[0];
            return (int)row[nameof(Book.Quantity)];
        }
    }
}
