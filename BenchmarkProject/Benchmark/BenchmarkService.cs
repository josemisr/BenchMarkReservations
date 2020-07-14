using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using BenchmarkProjec.Models.Dto;
using DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BenchmarkProject
{
    [RankColumn]
    [Orderer(SummaryOrderPolicy.FastestToSlowest)]
    [MemoryDiagnoser]
    public class BenchmarkService
    {
        public HttpServicesReponse _clientService = new HttpServicesReponse();
        string localhostApi = "https://localhost:44371/";
        string validJwt = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJqdGkiOiIyNGFlOGE2OS04Nzg5LTQ0MjMtYmVhMS1iYWVjNWRiYzFkNTgiLCJuYW1laWQiOiIxIiwiTmFtZSI6ImFkbWluIiwiU3VybmFtZSI6ImFkbWluIiwiVXNlckp3dCI6IntcIklkXCI6MSxcIk5hbWVcIjpcImFkbWluXCIsXCJTdXJuYW1lXCI6XCJhZG1pblwiLFwiU3VybmFtZTJcIjpcImFkbWluXCIsXCJJZENhcmRcIjpudWxsLFwiRW1haWxcIjpcImFkbWluQGdtYWlsLmNvbVwiLFwiQmlydGhkYXlcIjpcIjAwMDEtMDEtMDFUMDA6MDA6MDBcIixcIklkUm9sZVwiOjEsXCJSb2xcIjpcIkFkbWluXCIsXCJQYXNzd29yZFwiOm51bGx9IiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9yb2xlIjoiQWRtaW4iLCJuYmYiOjE1OTM5OTcwNDcsImV4cCI6MTU5NDAxNTA0NywiaXNzIjoiSm9zZSBNaWd1ZWwiLCJhdWQiOiJBbGwifQ.2zAxht1RdUv209xWaYXKUWcOUUIUz3nx3hMeWWiCH5g";
        readonly ReservationManagementDbContext _context = new ReservationManagementDbContext();


        public List<ServiceDto> GetIndexService()
        {
            string responseBody = this._clientService.GetResponse(localhostApi + "api/ServiceApi", "", validJwt).GetAwaiter().GetResult();
            List<ServiceDto> list = JsonConvert.DeserializeObject<List<ServiceDto>>(responseBody);

            return list;
        }


        public async Task<ServiceDto> GetDetailsService()
        {
            int id = 84;
            string responseBody = await this._clientService.GetResponse(localhostApi + "api/ServiceApi/" + id, "", validJwt);
            ServiceDto service = JsonConvert.DeserializeObject<ServiceDto>(responseBody);
            return service;
        }


        public ServiceDto CreateService()
        {
            ServiceDto service = new ServiceDto()
            {
                Image = "",
                Name = "Name",
                Price = 84,
                Description = "Description"
            };
            string responseBody = this._clientService.PostResponse(localhostApi + "api/ServiceApi", JsonConvert.SerializeObject(service), validJwt).GetAwaiter().GetResult();
            service = JsonConvert.DeserializeObject<ServiceDto>(responseBody);
            return service;
        }


        public async Task<ServiceDto> UpdateService()
        {
            int id = 84;
            string responseBody = await this._clientService.GetResponse(localhostApi + "api/ServiceApi/" + id, "", validJwt);
            ServiceDto service = JsonConvert.DeserializeObject<ServiceDto>(responseBody);
            service.Name = "name";
            string responseBody2 = this._clientService.PutResponse(localhostApi + "api/ServiceApi/" + id, JsonConvert.SerializeObject(service), validJwt).GetAwaiter().GetResult();
            ServiceDto serviceDto = JsonConvert.DeserializeObject<ServiceDto>(responseBody);
            return serviceDto;
        }


        public async Task<ServiceDto> DeleteService()
        {
            int id = 1463;
            string responseMessage = await this._clientService.DeleteResponse(localhostApi + "api/ServiceApi/" + id, "", validJwt);
            ServiceDto service = JsonConvert.DeserializeObject<ServiceDto>(responseMessage);
            return service;
        }

        [Benchmark]
        public async Task<List<Services>> GetIndexServiceMonolithic()
        {
            return await _context.Services.ToListAsync();
        }
        [Benchmark]
        public async Task<Services> GetDetailsServiceMonolithic()
        {
            int id = 84;
            var services = await _context.Services
               .FirstOrDefaultAsync(m => m.Id == id);
            return services;
        }

        [Benchmark]
        public string CreateServiceMonolithic()
        {
            Services service = new Services()
            {
                Image = "",
                Name = "Name",
                Price = 84,
                Description = "Description"
            };
            _context.Add(service);
            _context.SaveChanges();
            return "";
        }

        [Benchmark]
        public string UpdateServiceMonolithic()
        {
            Services service = new Services()
            {
                Image = "",
                Name = "Name",
                Price = 84,
                Description = "Description"
            };
            _context.Update(service);
            _context.SaveChanges();
            return "";
        }

        [Benchmark]
        public Services DeleteServiceMonolithic()
        {
            int id = 1880;
            var service = _context.Services
              .FirstOrDefault(m => m.Id == id);
            if (service != null)
            {
                var servicesEmployeesList = _context.ServicesEmployees.Where(elem => elem.IdService == id);
                var reservationsList = _context.Reservations.Where(elem => elem.IdService == id);
                foreach (var servicesEmployee in servicesEmployeesList)
                {
                    _context.Remove(servicesEmployee);
                }
                foreach (var reservation in reservationsList)
                {
                    _context.Remove(reservation);
                }

                _context.Remove(service);
                _context.SaveChanges();
            }
            return service;
        }
    }
}
