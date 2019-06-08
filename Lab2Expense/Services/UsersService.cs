using Lab2Expense.Models;
using Lab2Expense.Validators;
using Lab2Expense.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Lab2Expense.Services
{
    public interface IUsersService
    {
        UserGetModel Authenticate(string username, string password);
        ErrorsCollection Register(RegisterPostModel registerInfo);
        User GetCurrentUser(HttpContext httpContext);
        IEnumerable<UserGetModelWithRole> GetAll();
        User Delete(int id);
        User Upsert(int id, User user, User userCurrent);
        UserGetModelWithRole ChangeRole(int id, string Role, User currentUser);
        IEnumerable<UserUserRoleGetModel> GetHistoryRoles(int id);


    }

    public class UsersService : IUsersService
    {
        private ExpensesDbContext context;
        private readonly AppSettings appSettings;
        private IRegisterValidator registerValidator;
        public UsersService(ExpensesDbContext context, IRegisterValidator registerValidator, IOptions<AppSettings> appSettings)
        {
            this.context = context;
            this.appSettings = appSettings.Value;
            this.registerValidator = registerValidator;


        }

        public UserGetModel Authenticate(string username, string password)
        {
            var user = context.Users
                .SingleOrDefault(x => x.Username == username &&
                                 x.Password == ComputeSha256Hash(password));

            // return null if user not found
            if (user == null)
                return null;

            // authentication successful so generate jwt token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = System.Text.Encoding.ASCII.GetBytes(appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Username.ToString()),
            //        new Claim(ClaimTypes.Role, user.UserRole.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var result = new UserGetModel
            {
                Id = user.Id,
                Email = user.Email,
                Username = user.Username,
                Token = tokenHandler.WriteToken(token)
            };
            // remove password before returning

            return result;
        }

        private string ComputeSha256Hash(string rawData)
        {
            // Create a SHA256   
            // TODO: also use salt
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array  
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                // Convert byte array to a string   
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }



        public IEnumerable<UserUserRoleGetModel> GetHistoryRoles(int id)
        {

            //var user = context.Users.FirstOrDefault(user1 => user1.Id == id);

            var result = context.UserUserRole.Select(userUserRole => new UserUserRoleGetModel
            {
                User = userUserRole.User,
                UserRole = userUserRole.UserRole,
                StartTime = userUserRole.StartTime,
                EndTime = userUserRole.EndTime,

            }).Where(u => u.User.Id == id).OrderBy(us => us.StartTime).ToList();

            return result;





        }



        public ErrorsCollection Register(RegisterPostModel registerInfo)
        {
            var errors = registerValidator.Validate(registerInfo, context);
            if (errors != null)
            {
                return errors;
            }

            User toAdd = new User
            {
                Email = registerInfo.Email,
                LastName = registerInfo.LastName,
                FirstName = registerInfo.FirstName,
                Password = ComputeSha256Hash(registerInfo.Password),
                Username = registerInfo.Username,
                UserUserRoles = new List<UserUserRole>()
            };

            var regularRole = context
                .UserRole
                .FirstOrDefault(ur => ur.Name == "Regular");

            context.Users.Add(toAdd);
            context.UserUserRole.Add(new UserUserRole
            {
                User = toAdd,
                UserRole = regularRole,
                StartTime = DateTime.Now,
                EndTime = null,
            });

            context.SaveChanges();
            return null;
        }

        public UserRole GetCurrentUserRole(User user)
        {
            return user
                .UserUserRoles
                .FirstOrDefault(userUserRole => userUserRole.EndTime == null)
                .UserRole;
        }

        public User GetCurrentUser(HttpContext httpContext)
        {
            string username = httpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name).Value;
            //string accountType = httpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.AuthenticationMethod).Value;
            //return _context.Users.FirstOrDefault(u => u.Username == username && u.AccountType.ToString() == accountType);
            return context
                .Users
                .Include(u => u.UserUserRoles)
                .FirstOrDefault(u => u.Username == username);
        }

        public IEnumerable<UserGetModelWithRole> GetAll()
        {
            // return users without passwords
            return context.Users.Select(user => new UserGetModelWithRole
            {
                Id = user.Id,
                Email = user.Email,
                Username = user.Username,

            });
        }
        public UserGetModelWithRole ChangeRole(int id, string Role, User userCurrent)
        {
            DateTime dateCurrent = DateTime.Now;
            TimeSpan diferenta = dateCurrent.Subtract(userCurrent.DateAdded);
            var userCurentRole = GetCurrentUserRole(userCurrent);
            var user = context.Users.FirstOrDefault(U => U.Id == id);
            var userRoleFromTheUserToChange = GetCurrentUserRole(user);
            if ((userCurentRole.Name == "Admin" || diferenta.Days > 190) && userRoleFromTheUserToChange.Name != "Admin")
            {

                var userActiveRole = user.UserUserRoles.FirstOrDefault(role => role.EndTime == null);
                userActiveRole.EndTime = DateTime.Now;
                var regularRole = context
                   .UserRole
                   .FirstOrDefault(ur => ur.Name == Role);
                if (regularRole != null)
                {
                    context.UserUserRole.Add(new UserUserRole
                    {
                        User = user,
                        UserRole = regularRole,
                        StartTime = DateTime.Now,
                        EndTime = null,
                    });

                }
            }

            return UserGetModelWithRole.FromUser(user);

        }
        public User Delete(int id)
        {
            var existing = context.Users
            .FirstOrDefault(user => user.Id == id);
            if (existing == null)
            {
                return null;
            }
            existing.isRemoved = true;
            context.Update(existing);
            context.SaveChanges();
            return existing;
        }

        public User Upsert(int id, User user, User userCurrent)
        {
            var existing = context.Users.AsNoTracking().FirstOrDefault(f => f.Id == id);
            if (existing == null)
            {
                context.
                    Users.Add(user);
                context.SaveChanges();
                return user;
            }
            DateTime dateCurrent = DateTime.Now;
            TimeSpan diferenta = dateCurrent.Subtract(userCurrent.DateAdded);

            user.Id = id;
            var userCurentRole = GetCurrentUserRole(userCurrent);
            var curentRoleForExisting = GetCurrentUserRole(existing);
            if ((userCurentRole.Name == "Admin" || diferenta.Days > 190) && curentRoleForExisting.Name != "Admin")
            {
                var getUserRole = GetCurrentUserRole(user);
                ChangeRole(user.Id, getUserRole.Name, existing);
                context.Users.Update(user);

                context.SaveChanges();
                return user;
            }
            var getUserRoleNotChangeRole = GetCurrentUserRole(user);
            var getExistingRole = GetCurrentUserRole(existing);
            ChangeRole(user.Id, getExistingRole.Name, existing);

            context.Users.Update(user);
            context.SaveChanges();
            return user;

        }




    }

}

