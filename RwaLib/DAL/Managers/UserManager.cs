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
    public class UserManager
    {
        private string CS;
        public UserActionsManager uam;

        public UserManager(string connectionString)
        {
            CS = connectionString;
            uam = new UserActionsManager(connectionString);
        }


        public void Create(User user, params object[] args)
        {
            SqlHelper.ExecuteNonQuery(CS, nameof(Create) + nameof(User),
                user.FirstName, user.LastName, user.DateOfBirth, user.Email, Cryptography.HashPassword(args[0].ToString()),
                user.CityName, user.ZipCode, user.Address);
        }

        public User AuthUser(string email, string password)
        {
            var tblUser = SqlHelper.ExecuteDataset(CS, nameof(AuthUser), email, Cryptography.HashPassword(password)).Tables[0];
            if (tblUser == null || tblUser.Rows.Count == 0) return null;
            DataRow row = tblUser.Rows[0];

            User user = new User
            {
                IDUser = (int)row[nameof(User.IDUser)],
                FirstName = row[nameof(User.FirstName)].ToString(),
                LastName = row[nameof(User.LastName)].ToString(),
                DateOfBirth = (DateTime)row[nameof(User.DateOfBirth)],
                Email = row[nameof(User.Email)].ToString(),
                UserCode = row[nameof(User.UserCode)].ToString(),
                IsAdmin = (bool)row[nameof(User.IsAdmin)],
            };

            if (user.IsAdmin)
            {
                user.OIB = row[nameof(User.OIB)].ToString();
                user.WorkPosition = row[nameof(User.WorkPosition)].ToString();
                return user;
            }


            user.CityName = row[nameof(User.CityName)].ToString();
            user.ZipCode = (int)row[nameof(User.ZipCode)];
            user.Address = row[nameof(User.Address)].ToString();

            IList<Loan> allLoans = (IList<Loan>)uam.GetAllLoan(user);
            IEnumerable<Loan> onGoing = allLoans.Where(l => !l.IsSettled);
            user.Loans = onGoing.ToList();
            user.Purchases = (IList<Purchase>)uam.GetAllPurchase(user);

            return user;
        }


        public void Delete(User user)
        {
            SqlHelper.ExecuteNonQuery(CS, nameof(Delete) + nameof(User), user.IDUser);
        }

        public IEnumerable<User> GetAll()
        {
            var tblUser = SqlHelper.ExecuteDataset(CS, nameof(GetAll) + nameof(User)).Tables[0];
            if (tblUser == null) return null;
            IList<User> users = new List<User>();

            foreach (DataRow row in tblUser.Rows)
            {
                User user = new User
                {
                    IDUser = (int)row[nameof(User.IDUser)],
                    FirstName = row[nameof(User.FirstName)].ToString(),
                    LastName = row[nameof(User.LastName)].ToString(),
                    UserCode = row[nameof(User.UserCode)].ToString(),
                    Email = row[nameof(User.Email)].ToString(),
                    DateOfBirth = (DateTime)row[nameof(User.DateOfBirth)],
                    IsAdmin = (bool)row[nameof(User.IsAdmin)],
                };
                user.Loans = (IList<Loan>)uam.GetAllLoan(user);
                user.Purchases = (IList<Purchase>)uam.GetAllPurchase(user);
                users.Add(user);
            }
            return users;
        }

        public User Get(int id)
        {
            var tblUser = SqlHelper.ExecuteDataset(CS, nameof(Get) + nameof(User), id).Tables[0];
            if (tblUser == null) return null;
            DataRow row = tblUser.Rows[0];

            if ((bool)row[nameof(User.IsAdmin)])
            {
                return new User
                {
                    IDUser = id,
                    UserCode = row[nameof(User.UserCode)].ToString(),
                    FirstName = row[nameof(User.FirstName)].ToString(),
                    LastName = row[nameof(User.LastName)].ToString(),
                    DateOfBirth = (DateTime)row[nameof(User.DateOfBirth)],
                    Email = row[nameof(User.Email)].ToString(),
                    IsAdmin = (bool)row[nameof(User.IsAdmin)],
                    OIB = row[nameof(User.OIB)].ToString()
                };
            }
            return new User
            {
                IDUser = id,
                UserCode = row[nameof(User.UserCode)].ToString(),
                FirstName = row[nameof(User.FirstName)].ToString(),
                LastName = row[nameof(User.LastName)].ToString(),
                DateOfBirth = (DateTime)row[nameof(User.DateOfBirth)],
                Email = row[nameof(User.Email)].ToString(),
                CityName = row[nameof(User.CityName)].ToString(),
                ZipCode = (int)row[nameof(User.ZipCode)],
                Address = row[nameof(User.Address)].ToString(),
                IsAdmin = (bool)row[nameof(User.IsAdmin)],
            };
        }

        public bool CheckEmail(User user)
        {
            var tblUser = SqlHelper.ExecuteDataset(CS, nameof(CheckEmail), user.IDUser).Tables[0];
            if (tblUser == null || tblUser.Rows.Count == 0) return false;
            DataRow row = tblUser.Rows[0];

            try
            {
                bool status = bool.Parse(row["EmailConfirmed"].ToString());
                return status;
            }
            catch (Exception)
            {
                return false;
            }

        }

        public User GetUserByEmail(string email)
        {
            var tblUser = SqlHelper.ExecuteDataset(CS, nameof(GetUserByEmail), email).Tables[0];
            if (tblUser == null || tblUser.Rows.Count == 0) return null;
            DataRow row = tblUser.Rows[0];



            bool admin = (bool)row[nameof(User.IsAdmin)];
            if (admin)
            {
                return new User
                {
                    IDUser = (int)row[nameof(User.IDUser)],
                    FirstName = row[nameof(User.FirstName)].ToString(),
                    LastName = row[nameof(User.LastName)].ToString(),
                    UserCode = row[nameof(User.UserCode)].ToString(),
                    DateOfBirth = (DateTime)row[nameof(User.DateOfBirth)],
                    Email = row[nameof(User.Email)].ToString(),
                    IsAdmin = admin,
                    OIB = row[nameof(User.OIB)].ToString()
                };
            }
            return new User
            {
                IDUser = (int)row[nameof(User.IDUser)],
                UserCode = row[nameof(User.UserCode)].ToString(),
                FirstName = row[nameof(User.FirstName)].ToString(),
                LastName = row[nameof(User.LastName)].ToString(),
                DateOfBirth = (DateTime)row[nameof(User.DateOfBirth)],
                Email = row[nameof(User.Email)].ToString(),
                CityName = row[nameof(User.CityName)].ToString(),
                ZipCode = (int)row[nameof(User.ZipCode)],
                Address = row[nameof(User.Address)].ToString(),
                IsAdmin = admin,
            };
        }

        public string GetUserPassword(int id)
        {
            var tblUser = SqlHelper.ExecuteDataset(CS, nameof(GetUserPassword), id).Tables[0];
            if (tblUser == null) return null;
            DataRow row = tblUser.Rows[0];

            return row["PasswordHash"].ToString();
        }

        public void Update(User user, params object[] args)
        {
            SqlHelper.ExecuteNonQuery(CS, nameof(Update) + nameof(User),
                user.IDUser, user.FirstName, user.LastName, user.DateOfBirth, user.Email,
                Cryptography.HashPassword(args[0].ToString()), user.CityName, user.ZipCode, user.Address);
        }

        public void CreateEmployee(User user, string password)
        {
            SqlHelper.ExecuteNonQuery(CS, nameof(CreateEmployee),
                user.FirstName, user.LastName, user.DateOfBirth, user.Email, Cryptography.HashPassword(password),
                user.OIB, user.WorkPosition);
        }

        public void ConfirmEmail(int userid)
        {
            SqlHelper.ExecuteNonQuery(CS, nameof(ConfirmEmail), userid);
        }

        public void ConfirmPassword(int userid)
        {
            SqlHelper.ExecuteNonQuery(CS, nameof(ConfirmPassword), userid);
        }

        public void ChangePassword(User user, string password)
        {
            SqlHelper.ExecuteNonQuery(CS, nameof(ChangePassword), user.IDUser, Cryptography.HashPassword(password));
        }
    }
}
