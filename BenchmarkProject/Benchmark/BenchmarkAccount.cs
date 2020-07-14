using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using BenchmarkProjec.Models.Dto;
using DataAccess.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace BenchmarkProject
{
    [RankColumn]
    [Orderer(SummaryOrderPolicy.FastestToSlowest)]
    [MemoryDiagnoser]
    public class BenchmarkAccount
    {
        public HttpServicesReponse _clientService = new HttpServicesReponse();
        string localhostApi = "https://localhost:44310/";// layers
                                                         // string localhostApi = "https://localhost:44371/"; //microservices and functions
        readonly ReservationManagementDbContext _context = new ReservationManagementDbContext();

        [Benchmark]
        public string Login()
        {
            string email = "admin@gmail.com";
            string password = "admin";

            string content = "{\"email\":\"" + email + "\",\"password\":\"" + password + "\"}";
            string responseBody = this._clientService.PostResponse(localhostApi + "api/AccountApi", content).GetAwaiter().GetResult();
            JwtSecurityTokenHandler hand = new JwtSecurityTokenHandler();
            if (!string.IsNullOrEmpty(responseBody))
            {
                var tokenS = hand.ReadJwtToken(responseBody);
                if (responseBody != null)
                {
                    return responseBody;
                }
            }
            return string.Empty;
        }

        [Benchmark]
        public async Task<string> Register()
        {
            UserDto user = new UserDto()
            {
                Birthday = DateTime.Today,
                Email = "emailLayers",
                IdCard = "1111111",
                Name = "name",
                Surname = "surname",
                Surname2 = "surname2",
                Password = "password",

            };
            user.IdRole = 2;
            string responseBody = await this._clientService.GetResponse(localhostApi + "api/AccountApi");
            List<UserDto> list = JsonConvert.DeserializeObject<List<UserDto>>(responseBody);
            if (list.FirstOrDefault(elem => elem.Email == user.Email) != null)
            {
                return "Not valid";
            }
            string responseBody2 = await this._clientService.PostResponse(localhostApi + "api/AccountApi/Register", JsonConvert.SerializeObject(user));
            JwtSecurityTokenHandler hand = new JwtSecurityTokenHandler();
            if (!string.IsNullOrEmpty(responseBody2))
            {
                var tokenS = hand.ReadJwtToken(responseBody2);
                if (responseBody2 != null)
                {
                    return responseBody;
                }
            }
            return string.Empty;

        }

        public Users LoginMonolithic()
        {
            string email = "admin@gmail.com";
            string password = "admin";

            string content = "{\"email\":\"" + email + "\",\"password\":\"" + password + "\"}";
            Users userDb = _context.Users.FirstOrDefault(elem => elem.Email == email && elem.Password == password);
            if (userDb != null)
            {
                return userDb;
            }
            return null;
        }

        public async Task<Users> RegisterMonolithic()
        {
            Users user = new Users()
            {
                Birthday = DateTime.Today,
                Email = "emailLayers",
                IdCard = "1111111",
                Name = "name",
                Surname = "surname",
                Surname2 = "surname2",
                Password = "password",
                IdRole = 2

            };
            if (_context.Users.FirstOrDefault(elem => elem.Email == user.Email) != null)
            {
                return null;
            }
            _context.Add(user);
            var result = await _context.SaveChangesAsync();
            if (result > 0)
            {
                return user;
            }
            return null;
        }
    }
}
