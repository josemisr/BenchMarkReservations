using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using BenchmarkProjec.Models.Dto;
using BenchmarkProjec.Models.Reservation;
using DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BenchmarkProject
{
    [RankColumn]
    [Orderer(SummaryOrderPolicy.FastestToSlowest)]
    [MemoryDiagnoser]
    public class BenchmarkReservation
    {
        public HttpServicesReponse _clientService = new HttpServicesReponse();
        string localhostApi = "https://localhost:44371/";
        string validJwt = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJqdGkiOiJlZDJjNGMyMy1jMWM5LTQ1YzMtYjM0OS0wZTc5Y2E4NTcwNDIiLCJuYW1laWQiOiIxIiwiTmFtZSI6ImFkbWluIiwiU3VybmFtZSI6ImFkbWluIiwiVXNlckp3dCI6IntcIklkXCI6MSxcIk5hbWVcIjpcImFkbWluXCIsXCJTdXJuYW1lXCI6XCJhZG1pblwiLFwiU3VybmFtZTJcIjpcImFkbWluXCIsXCJJZENhcmRcIjpudWxsLFwiRW1haWxcIjpcImFkbWluQGdtYWlsLmNvbVwiLFwiQmlydGhkYXlcIjpcIjAwMDEtMDEtMDFUMDA6MDA6MDBcIixcIklkUm9sZVwiOjEsXCJSb2xcIjpcIkFkbWluXCIsXCJQYXNzd29yZFwiOm51bGx9IiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9yb2xlIjoiQWRtaW4iLCJuYmYiOjE1OTM3MzkzMjgsImV4cCI6MTU5Mzc1NzMyOCwiaXNzIjoiSm9zZSBNaWd1ZWwiLCJhdWQiOiJBbGwifQ.LSYp_W_Ae0GTQ3gDs1KYT05WMCf4yBkeBRQ1ZVqsEmM";
        readonly ReservationManagementDbContext _context = new ReservationManagementDbContext();

        public ReservationModelDto GetIndexReservation()
        {
            string responseBodyServiceList = this._clientService.GetResponse(localhostApi + "api/ServiceApi", "", validJwt).GetAwaiter().GetResult();
            List<ServiceDto> servicesList = JsonConvert.DeserializeObject<List<ServiceDto>>(responseBodyServiceList);

            ReservationModelDto reservationModel = new ReservationModelDto();
            string responseBodyReservationsList = this._clientService.GetResponse(localhostApi + "api/ReservationApi", "", validJwt).GetAwaiter().GetResult();
            List<ReservationDto> reservationsList = JsonConvert.DeserializeObject<List<ReservationDto>>(responseBodyReservationsList);
            reservationModel.Reservations = reservationsList;
            return reservationModel;
        }


        public ReservationModelDto GetAvailabilityReservationsNewReservation()
        {
            ReservationDto reservation = new ReservationDto();
            string responseBodyService = this._clientService.GetResponse(localhostApi + "api/ServiceApi/" + reservation.IdService, "", validJwt).GetAwaiter().GetResult();
            ServiceDto service = JsonConvert.DeserializeObject<ServiceDto>(responseBodyService);
            reservation.IdServiceNavigation = service;

            string responseBodyEmployee = this._clientService.GetResponse(localhostApi + "api/EmployeeApi/" + reservation.IdEmployee, "", validJwt).GetAwaiter().GetResult();
            EmployeeDto employee = JsonConvert.DeserializeObject<EmployeeDto>(responseBodyEmployee);

            reservation.IdEmployeeNavigation = employee;

            string responseBodyEmployeesList = this._clientService.GetResponse(localhostApi + "api/EmployeeApi", "", validJwt).GetAwaiter().GetResult();
            List<EmployeeDto> employeesList = JsonConvert.DeserializeObject<List<EmployeeDto>>(responseBodyEmployeesList);
            reservation.IdUser = 29;
            reservation.IdService = 85;
            reservation.IdEmployee = 23;
            reservation.Date = DateTime.Today;
            ReservationModelDto reseservationModel = new ReservationModelDto();
            reseservationModel.Reservation = reservation;
            string responseBodyReservationAvailabilityList = this._clientService.GetResponse(localhostApi + "api/ReservationApi/Availability?idEmployee=" + reservation.IdEmployee + "&idService=" + reservation.IdService + "&idUser=" + reservation.IdUser + "&datetime=" + reservation.Date.ToString(), "", validJwt).GetAwaiter().GetResult();
            List<ReservationDto> reservationAvailabilityList = JsonConvert.DeserializeObject<List<ReservationDto>>(responseBodyReservationAvailabilityList);
            reseservationModel.Reservations = reservationAvailabilityList;
            return reseservationModel;
        }

        public async Task<ReservationDto> CreateReservation()
        {
            ReservationDto reservation = new ReservationDto()
            {
                Date = DateTime.Today,
                IdService = 85,
                IdEmployee = 23,
                IdUser = 29
            };
            await this._clientService.PostResponse(localhostApi + "api/ReservationApi", JsonConvert.SerializeObject(reservation), validJwt);

            string responseBodyEmployeesList = this._clientService.GetResponse(localhostApi + "api/EmployeeApi", "", validJwt).GetAwaiter().GetResult();
            List<EmployeeDto> employeesList = JsonConvert.DeserializeObject<List<EmployeeDto>>(responseBodyEmployeesList);

            string responseBodyServiceList = this._clientService.GetResponse(localhostApi + "api/ServiceApi", "", validJwt).GetAwaiter().GetResult();
            List<ServiceDto> servicesList = JsonConvert.DeserializeObject<List<ServiceDto>>(responseBodyServiceList);

            string responseBodyUserList = this._clientService.GetResponse(localhostApi + "api/AccountApi", "", validJwt).GetAwaiter().GetResult();
            List<UserDto> usersList = JsonConvert.DeserializeObject<List<UserDto>>(responseBodyServiceList);

            string responseBodyReservationAvailabilityList = this._clientService.GetResponse(localhostApi + "api/ReservationApi/Availability?idEmployee=" + reservation.IdEmployee + "&idService=" + reservation.IdService + "&idUser=" + reservation.IdUser + "&datetime=" + reservation.Date.ToString(), "", validJwt).GetAwaiter().GetResult();
            List<ReservationDto> reservationAvailabilityList = JsonConvert.DeserializeObject<List<ReservationDto>>(responseBodyReservationAvailabilityList);
            return reservation;
        }

        public async Task<ReservationDto> DeleteReservation()
        {
            int id = 1135;
            string responseMessage = await this._clientService.DeleteResponse(localhostApi + "api/ReservationApi/" + id, "", validJwt);
            ReservationDto reservation = JsonConvert.DeserializeObject<ReservationDto>(responseMessage);
            return reservation;
        }

        [Benchmark]
        public ReservationModel GetIndexReservationMonolithic()
        {
            List<Services> serviceList = _context.Services.ToList();
            ReservationModel reservationModel = new ReservationModel();
            reservationModel.Reservations = _context.Reservations.Where(elem => elem.IdUser == 1 && elem.Date >= DateTime.Today).Include(r => r.IdEmployeeNavigation)
              .Include(r => r.IdServiceNavigation);
            return reservationModel;
        }

        [Benchmark]
        public ReservationModel GetAvailabilityReservationsNewReservationMonolithic()
        {
            Reservations reservation = new Reservations();
            reservation.IdServiceNavigation = _context.Services.FirstOrDefault(elem => elem.Id == reservation.IdService);
            reservation.IdEmployeeNavigation = _context.Employees.FirstOrDefault(elem => elem.Id == reservation.IdEmployee);
            _context.Employees.ToList();
            reservation.IdUser = 29;
            reservation.IdService = 85;
            reservation.IdEmployee = 23;
            reservation.Date = DateTime.Today;
            ReservationModel reseservationModel = new ReservationModel();
            reseservationModel.Reservation = reservation;
            reseservationModel.Reservations = Getavailability(reservation);
            return reseservationModel;
        }

        [Benchmark]
        public async Task<Reservations> CreateReservationMonolithic()
        {
            Reservations reservation = new Reservations()
            {
                Date = DateTime.Today,
                IdService = 84,
                IdEmployee = 23,
                IdUser = 29
            };
            _context.Add(reservation);
            await _context.SaveChangesAsync();
            _context.Employees.ToList();
            _context.Services.ToList();
            _context.Users.ToList();
            _context.Reservations.ToList();

            return reservation;
        }

        [Benchmark]
        public async Task<Reservations> DeleteReservationMonolithic()
        {
            int id = 1211;
            Reservations reservations = _context.Reservations.Find(id);
            if (reservations != null)
            {
                _context.Reservations.Remove(reservations);
                await _context.SaveChangesAsync();

            }
            return reservations;
        }


        private List<Reservations> Getavailability(Reservations reservation)
        {
            List<Reservations> currentReservations = GetCurrentsReservations(reservation);
            List<Reservations> possibleReservations = GetPossibleReservations(reservation);
            possibleReservations = possibleReservations.Where(elem => currentReservations.FirstOrDefault(elem2 => elem.Date == elem2.Date) == null).ToList();
            return possibleReservations;
        }

        private List<Reservations> GetCurrentsReservations(Reservations reservation)
        {
            List<Reservations> currentReservations = _context.Reservations
               .Where(elem => elem.IdEmployee == reservation.IdEmployee &&
               elem.Date.Date == reservation.Date.Date).ToList();
            return currentReservations;
        }

        private List<Reservations> GetPossibleReservations(Reservations reservation)
        {
            List<Reservations> possibleReservations = new List<Reservations>();

            List<EmployeesShifts> employeesShiftsList = _context.EmployeesShifts.Where(elem => elem.IdEmployee == reservation.IdEmployee && elem.WorkDay == reservation.Date).OrderBy(elem => elem.InitHour).ToList();
            foreach (EmployeesShifts employeeShifts in employeesShiftsList)
                if (employeeShifts != null)
                {
                    for (int i = employeeShifts.InitHour; i < employeeShifts.EndHour; i++)
                    {
                        Reservations possibleReservation = new Reservations();
                        possibleReservation.IdEmployee = reservation.IdEmployee;
                        possibleReservation.IdService = reservation.IdService;
                        possibleReservation.IdUser = reservation.IdUser;
                        possibleReservation.IdServiceNavigation = reservation.IdServiceNavigation;
                        possibleReservation.IdEmployeeNavigation = reservation.IdEmployeeNavigation;
                        DateTime possibleDatetime = reservation.Date.AddHours(i);
                        possibleReservation.Date = possibleDatetime;
                        possibleReservations.Add(possibleReservation);
                    }
                }
            return possibleReservations;
        }
    }
}
