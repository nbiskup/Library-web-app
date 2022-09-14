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
    public class AuthorManager
    {
        private string CS;

        public AuthorManager(string connectionString)
        {
            CS = connectionString;
        }
        public void Create(Author author, params object[] args)
        {
            SqlHelper.ExecuteNonQuery(CS, nameof(Create) + nameof(Author), author.FullName, author.Description, author.PicturePath);
        }

        public void Delete(Author author)
        {
            SqlHelper.ExecuteNonQuery(CS, nameof(Delete) + nameof(Author), author.IDAuthor);
        }

        public IEnumerable<Author> GetAll()
        {
            var tblAuthor = SqlHelper.ExecuteDataset(CS, nameof(GetAll) + nameof(Author)).Tables[0];
            if (tblAuthor == null) return null;
            IList<Author> authors = new List<Author>();
            foreach (DataRow row in tblAuthor.Rows)
            {
                authors.Add(new Author
                {
                    IDAuthor = (int)row[nameof(Author.IDAuthor)],
                    FullName = row[nameof(Author.FullName)].ToString(),
                    Description = row[nameof(Author.Description)].ToString(),
                    PicturePath = row[nameof(Author.PicturePath)].ToString()
                });
            }
            return authors;
        }

        public Author Get(int id)
        {
            var tblAuthor = SqlHelper.ExecuteDataset(CS, nameof(Get) + nameof(Author), id).Tables[0];
            if (tblAuthor == null) return null;
            DataRow row = tblAuthor.Rows[0];

            return new Author
            {
                IDAuthor = id,
                FullName = row[nameof(Author.FullName)].ToString(),
                Description = row[nameof(Author.Description)].ToString(),
                PicturePath = row[nameof(Author.PicturePath)].ToString()
            };
        }

        public void Update(Author author, params object[] args)
        {
            SqlHelper.ExecuteNonQuery(CS, nameof(Update) + nameof(Author), author.IDAuthor, author.FullName, author.Description, author.PicturePath);
        }


    }
}
