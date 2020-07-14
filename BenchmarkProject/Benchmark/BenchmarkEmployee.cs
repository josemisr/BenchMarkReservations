using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using BenchmarkProjec.Models.Dto;
using BenchmarkProjec.Models.EmployeeShiftModel;
using BenchmarkProjec.Models.ServiceEmployeeModel;
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
    public class BenchmarkEmployee
    {
        public HttpServicesReponse _clientService = new HttpServicesReponse();
        string localhostApi = "https://localhost:44310/";
        string validJwt = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJqdGkiOiJkZmYyNGZhNy1iYmFjLTRhOWMtYmQ3OC1iNjFhODA4N2YyNzciLCJuYW1laWQiOiIxIiwiTmFtZSI6ImFkbWluIiwiU3VybmFtZSI6ImFkbWluIiwiVXNlckp3dCI6IntcIklkXCI6MSxcIk5hbWVcIjpcImFkbWluXCIsXCJTdXJuYW1lXCI6XCJhZG1pblwiLFwiU3VybmFtZTJcIjpcImFkbWluXCIsXCJJZENhcmRcIjpudWxsLFwiRW1haWxcIjpcImFkbWluQGdtYWlsLmNvbVwiLFwiQmlydGhkYXlcIjpcIjAwMDEtMDEtMDFUMDA6MDA6MDBcIixcIklkUm9sZVwiOjEsXCJSb2xcIjpcIkFkbWluXCIsXCJQYXNzd29yZFwiOm51bGx9IiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9yb2xlIjoiQWRtaW4iLCJuYmYiOjE1OTQyNDM0ODMsImV4cCI6MTU5NDI2MTQ4MywiaXNzIjoiSm9zZSBNaWd1ZWwiLCJhdWQiOiJBbGwifQ.sE_rMLHhGkMQBg_vwfShubVcDPjrxY8hIDKIyL_lIP4";
        readonly ReservationManagementDbContext _context = new ReservationManagementDbContext();

        #region Employee
        [Benchmark]
        public List<EmployeeDto> GetIndexEmployee()
        {
            string responseBody = this._clientService.GetResponse(localhostApi + "api/EmployeeApi", "", validJwt).GetAwaiter().GetResult();
            List<EmployeeDto> list = JsonConvert.DeserializeObject<List<EmployeeDto>>(responseBody);

            return list;
        }

        [Benchmark]
        public async Task<EmployeeDto> GetDetailsEmployee()
        {
            int id = 84;
            string responseBody = await this._clientService.GetResponse(localhostApi + "api/EmployeeApi/" + id, "", validJwt);
            EmployeeDto service = JsonConvert.DeserializeObject<EmployeeDto>(responseBody);
            return service;
        }

        [Benchmark]
        public EmployeeDto CreateEmployee()
        {
            EmployeeDto employee = new EmployeeDto()
            {
                Surname = "name",
                Name = "Name",
                Surname2 = "",
                IdCard = ""
            };
            string responseBody = this._clientService.PostResponse(localhostApi + "api/EmployeeApi", JsonConvert.SerializeObject(employee), validJwt).GetAwaiter().GetResult();
            employee = JsonConvert.DeserializeObject<EmployeeDto>(responseBody);
            return employee;
        }

        [Benchmark]
        public async Task<EmployeeDto> UpdateEmployee()
        {
            int id = 28;
            string responseBody = await this._clientService.GetResponse(localhostApi + "api/EmployeeApi/" + id, "", validJwt);
            EmployeeDto employeeSearch = JsonConvert.DeserializeObject<EmployeeDto>(responseBody);
            Employees employee = new Employees()
            {
                Surname = "Salfishn",
                Name = "José",
                Surname2 = "",
                IdCard = "3333333333Z"

            };
            string responseBody2 = this._clientService.PutResponse(localhostApi + "api/EmployeeApi/" + id, JsonConvert.SerializeObject(employee), validJwt).GetAwaiter().GetResult();
            EmployeeDto EmployeeDto = JsonConvert.DeserializeObject<EmployeeDto>(responseBody);
            return EmployeeDto;
        }

        [Benchmark]
        public async Task<EmployeeDto> DeleteEmployee()
        {
            int id = 2821;//idEmployee
            string responseMessage = await this._clientService.DeleteResponse(localhostApi + "api/EmployeeApi/" + id, "", validJwt);
            EmployeeDto employee = JsonConvert.DeserializeObject<EmployeeDto>(responseMessage);
            return employee;
        }


        public async Task<List<Employees>> GetIndexEmployeeMonolithic()
        {
            return await _context.Employees.ToListAsync();
        }

        public async Task<Employees> GetDetailsEmployeeMonolithic()
        {
            int id = 22;
            var Employees = await _context.Employees
               .FirstOrDefaultAsync(m => m.Id == id);
            return Employees;
        }

        public string CreateEmployeeMonolithic()
        {
            Employees employee = new Employees()
            {
                Surname = "name",
                Name = "Name",
                Surname2 = "",
                IdCard = ""
            };
            _context.Add(employee);
            _context.SaveChanges();
            return "";
        }

        public string UpdateEmployeeMonolithic()
        {
            Employees employee = new Employees()
            {
                Surname = "Salfishn",
                Name = "José",
                Surname2 = "",
                IdCard = "3333333333Z"

            };
            _context.Update(employee);
            _context.SaveChanges();
            return "";
        }

        public Employees DeleteEmployeeMonolithic()
        {
            int id = 28;
            var employee = _context.Employees.Find(id);
            var servicesEmployeesList = _context.ServicesEmployees.Where(elem => elem.IdEmployee == id);
            var employeesShiftsList = _context.EmployeesShifts.Where(elem => elem.IdEmployee == id);
            var reservationsList = _context.Reservations.Where(elem => elem.IdEmployee == id);

            if (employee != null)
            {
                foreach (var employeeShift in employeesShiftsList)
                {
                    _context.Remove(employeeShift);
                }
                foreach (var servicesEmployee in servicesEmployeesList)
                {
                    _context.Remove(servicesEmployee);
                }
                foreach (var reservation in reservationsList)
                {
                    _context.Remove(reservation);
                }

                _context.Remove(employee);
                _context.SaveChanges();
            }
            return employee;

        }
        #endregion
        #region EmployeeShift
        [Benchmark]
        public EmployeeShiftModelDto GetIndexEmployeeShift()
        {
            int? idEmployee = 1;
            DateTime? workDay = null;
            string responseBodyEmployeesShiftsList = this._clientService.GetResponse(localhostApi + "api/EmployeeShiftApi", "", validJwt).GetAwaiter().GetResult();
            List<EmployeeShiftDto> employeeShift = JsonConvert.DeserializeObject<List<EmployeeShiftDto>>(responseBodyEmployeesShiftsList);
            var employeesShiftsList = employeeShift.Where(elem => elem.IdEmployee == idEmployee
            && ((workDay != null && elem.WorkDay == workDay) || (workDay == null && elem.WorkDay >= DateTime.Today))).OrderBy(elem => elem.WorkDay);/*.Include(e => e.IdEmployeeNavigation);*/
            EmployeeShiftModelDto employeeShiftModel = new EmployeeShiftModelDto();
            employeeShiftModel.EmployeeShiftsList = employeesShiftsList;
            employeeShiftModel.EmployeeShift = new EmployeeShiftDto();

            string responseBodyEmployee = this._clientService.GetResponse(localhostApi + "api/EmployeeApi/" + idEmployee, "", validJwt).GetAwaiter().GetResult();
            EmployeeDto employeeDto = JsonConvert.DeserializeObject<EmployeeDto>(responseBodyEmployee);

            employeeShiftModel.EmployeeShift.IdEmployeeNavigation = employeeDto;
            employeeShiftModel.EmployeeShift.IdEmployee = idEmployee.Value;
            employeeShiftModel.EmployeeShift.WorkDay = workDay == null ? DateTime.Now : workDay.Value;
            employeeShiftModel.EndDate = workDay == null ? DateTime.Now : workDay.Value;
            return employeeShiftModel;
        }
        [Benchmark]
        public async Task<EmployeeShiftDto> CreateEmployeeShift()
        {
            EmployeeShiftDto employeeShift = new EmployeeShiftDto();
            employeeShift.IdEmployee = 23;
            employeeShift.InitHour = 1;
            employeeShift.EndHour = 2;
            employeeShift.WorkDay = DateTime.Now;
            string responseBodyEmployeeShift = await this._clientService.PostResponse(localhostApi + "api/EmployeeShiftApi", JsonConvert.SerializeObject(employeeShift), validJwt);
            EmployeeShiftDto employeeDto = JsonConvert.DeserializeObject<EmployeeShiftDto>(responseBodyEmployeeShift);
            return employeeDto;
        }
        [Benchmark]
        public async Task<EmployeeShiftDto> DeleteEmployeeShift()
        {
            int id = 1165;//idEmployee
            string responseBodyEmployeeShift = await this._clientService.DeleteResponse(localhostApi + "api/EmployeeShiftApi/" + id, "", validJwt);
            EmployeeShiftDto employeeDto = JsonConvert.DeserializeObject<EmployeeShiftDto>(responseBodyEmployeeShift);
            return employeeDto;
        }

        public EmployeeShiftModel GetIndexEmployeeShiftMonolithic()
        {
            int? idEmployee = 1;
            DateTime? workDay = null;
            var employeesShiftsList = _context.EmployeesShifts.Where(elem => elem.IdEmployee == idEmployee
            && ((workDay != null && elem.WorkDay == workDay) || (workDay == null && elem.WorkDay >= DateTime.Today))).OrderBy(elem => elem.WorkDay).Include(e => e.IdEmployeeNavigation);
            EmployeeShiftModel employeeShiftModel = new EmployeeShiftModel();
            employeeShiftModel.EmployeeShiftsList = employeesShiftsList;
            employeeShiftModel.EmployeeShift = new EmployeesShifts();
            employeeShiftModel.EmployeeShift.IdEmployeeNavigation = _context.Employees.FirstOrDefault(elem => elem.Id == idEmployee.Value);
            employeeShiftModel.EmployeeShift.IdEmployee = idEmployee.Value;
            employeeShiftModel.EmployeeShift.WorkDay = workDay == null ? DateTime.Now : workDay.Value;
            employeeShiftModel.EndDate = workDay == null ? DateTime.Now : workDay.Value;

            return employeeShiftModel;
        }

        public async Task<EmployeesShifts> CreateEmployeeShiftMonolithic()
        {
            EmployeesShifts employeeShift = new EmployeesShifts();
            employeeShift.IdEmployee = 23;
            employeeShift.InitHour = 1;
            employeeShift.EndHour = 2;
            employeeShift.WorkDay = DateTime.Now;
            _context.Add(employeeShift);
            await _context.SaveChangesAsync();
            return employeeShift;
        }

        public async Task<EmployeesShifts> DeleteEmployeeShiftMonolithic()
        {
            int id = 566;
            var employeesShifts = await _context.EmployeesShifts.FindAsync(id);
            if (employeesShifts != null)
            {
                _context.EmployeesShifts.Remove(employeesShifts);
                await _context.SaveChangesAsync();
            }
            return employeesShifts;
        }
        #endregion
        #region ServiceEmployee
        [Benchmark]
        public ServiceEmployeeModelDto GetIndexServiceEmployee()
        {
            int idEmployee = 47;
            string responseBodyServicesEmployeeList = this._clientService.GetResponse(localhostApi + "api/ServiceEmployeeApi", "", validJwt).GetAwaiter().GetResult();
            List<ServiceEmployeeDto> serviceEmployeeListDto = JsonConvert.DeserializeObject<List<ServiceEmployeeDto>>(responseBodyServicesEmployeeList);
            var servicesEmployeesList = serviceEmployeeListDto.Where(elem => elem.IdEmployee == idEmployee);

            string responseBodyServiceList = this._clientService.GetResponse(localhostApi + "api/ServiceApi", "", validJwt).GetAwaiter().GetResult();
            List<ServiceDto> serviceDto = JsonConvert.DeserializeObject<List<ServiceDto>>(responseBodyServiceList);

            string responseBodyEmployee = this._clientService.GetResponse(localhostApi + "api/EmployeeApi/" + idEmployee, "", validJwt).GetAwaiter().GetResult();
            EmployeeDto employeeDto = JsonConvert.DeserializeObject<EmployeeDto>(responseBodyEmployee);
            ServiceEmployeeModelDto serviceEmployeeModel = new ServiceEmployeeModelDto();
            serviceEmployeeModel.ServicesEmployeesList = servicesEmployeesList;
            serviceEmployeeModel.ServiceEmployee = new ServiceEmployeeDto();
            serviceEmployeeModel.ServiceEmployee.IdEmployeeNavigation = employeeDto;
            return serviceEmployeeModel;
        }
        [Benchmark]
        public async Task<ServiceEmployeeDto> CreateServiceEmployee()
        {
            ServiceEmployeeDto serviceEmployee = new ServiceEmployeeDto();
            serviceEmployee.IdEmployee = 23;
            serviceEmployee.IdService = 84;

            string responseBodyServicesEmployeeList = this._clientService.GetResponse(localhostApi + "api/ServiceEmployeeApi", "", validJwt).GetAwaiter().GetResult();
            List<ServiceEmployeeDto> serviceEmployeeListDto = JsonConvert.DeserializeObject<List<ServiceEmployeeDto>>(responseBodyServicesEmployeeList);

            string responseBodyServicesEmployee = await this._clientService.PostResponse(localhostApi + "api/ServiceEmployeeApi", JsonConvert.SerializeObject(serviceEmployee), validJwt);
            ServiceEmployeeDto employeeDto = JsonConvert.DeserializeObject<ServiceEmployeeDto>(responseBodyServicesEmployee);
            return employeeDto;
        }

        [Benchmark]
        public async Task<ServiceEmployeeDto> DeleteServiceEmployee()
        {
            int id = 1320; //idEmployee
            string responseBodyServicesEmployeeList = await this._clientService.DeleteResponse(localhostApi + "api/ServiceEmployeeApi/" + id, "", validJwt);
            ServiceEmployeeDto serviceEmployee = JsonConvert.DeserializeObject<ServiceEmployeeDto>(responseBodyServicesEmployeeList);
            return serviceEmployee;
        }

        public ServiceEmployeeModel GetIndexServiceEmployeeMonolithic()
        {
            int idEmployee = 22;
            var servicesEmployeesList = _context.ServicesEmployees.Where(elem => elem.IdEmployee == idEmployee).Include(s => s.IdEmployeeNavigation).Include(s => s.IdServiceNavigation);

            ServiceEmployeeModel serviceEmployeeModel = new ServiceEmployeeModel();
            serviceEmployeeModel.ServicesEmployeesList = servicesEmployeesList;
            serviceEmployeeModel.ServiceEmployee = new ServicesEmployees();
            serviceEmployeeModel.ServiceEmployee.IdEmployeeNavigation = _context.Employees.FirstOrDefault(elem => elem.Id == idEmployee);
            serviceEmployeeModel.ServiceEmployee.IdEmployee = idEmployee;
            return serviceEmployeeModel;
        }

        public async Task<ServicesEmployees> CreateServiceEmployeeMonolithic()
        {
            ServicesEmployees serviceEmployee = new ServicesEmployees();
            serviceEmployee.IdEmployee = 23;
            serviceEmployee.IdService = 84;

            _context.ServicesEmployees.FirstOrDefault(elem => elem.IdEmployee == serviceEmployee.IdEmployee && elem.IdService == serviceEmployee.IdService);

            _context.Add(serviceEmployee);
            await _context.SaveChangesAsync();
            return serviceEmployee;
        }


        public async Task<ServicesEmployees> DeleteServiceEmployeeMonolithic()
        {
            int id = 894;
            var serviceEmployee = await _context.ServicesEmployees.FindAsync(id);
            if (serviceEmployee != null)
            {
                _context.ServicesEmployees.Remove(serviceEmployee);
                await _context.SaveChangesAsync();
            }
            return serviceEmployee;
        }
        #endregion
    }
}