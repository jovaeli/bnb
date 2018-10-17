using BeautiesAndBarbers.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace BeautiesAndBarbers.Helpers
{
    public class ListsHelper : IDisposable
    {
        public static BnBContext db = new BnBContext();

        public static List<Country> GetCountries()
        {
            return db.Countries.OrderBy(c => c.Name).ToList();
        }

        public static List<ClientListByCompanyViewModel> GetBusinessCustomers(string employeeEmail)
        {
            User user = db.Users.Where(u => u.UserName == employeeEmail).FirstOrDefault();

            //Employee's Ids Array
            IQueryable<int> businessEmployeeIds = db.Employees
                .Where(e => e.UserId == user.UserId && e.IsDisabled == false)
                .Select(e => e.EmployeeId);

            //Left List 
            var employeeBusiness = (from em in db.Employees
                                    join bu in db.Businesses on em.BusinessId equals bu.BusinessId
                                    where (businessEmployeeIds.Contains(em.EmployeeId) && em.IsDisabled == false)
                                    select new { BusinessName = bu.Name, EmployeeId = em.EmployeeId }).ToList();

            //Right List (Group for Aggregate)
            var employeeClientList = (from es in db.EmployeeServices
                                      join cl in db.ClientLists on new { es.EmployeeId, es.ServiceId } equals new { cl.EmployeeId, cl.ServiceId }
                                      join ls in db.ListStatus on cl.ListStatusId equals ls.ListStatusId
                                      select new { es, cl, ls } into ecl
                                      group ecl by ecl.es.EmployeeId into eclGroup
                                      select new { eclGroup }).ToList();

            // Left Outer Join in LinQ
            List<ClientListByCompanyViewModel> employeeClientLists = (
                from ebu in employeeBusiness
                join ecl in employeeClientList on ebu.EmployeeId equals ecl.eclGroup.FirstOrDefault().es.EmployeeId into groupjoin
                from joined in groupjoin.DefaultIfEmpty()
                select new ClientListByCompanyViewModel
                {
                    BusinessName = ebu.BusinessName,
                    EmployeeId = ebu.EmployeeId,
                    ActiveClients = joined == null ? 0 : joined.eclGroup.Where(g => g.ls.Active == true).Count(),
                    DoneToday = joined == null ? 0 : joined.eclGroup.Where(g => g.ls.Completed == true && (g.cl.ModifiedDate).ToString("M/d/yyyy") == DateTime.Today.ToString("M/d/yyyy")).Count(),
                    DoneTotal = joined == null ? 0 : joined.eclGroup.Where(g => g.ls.Completed == true).Count()
                }).ToList();

            return employeeClientLists;
        }

        public static List<Customer> GetCustomers(bool isEmployee, int id)
        {
            IQueryable<Customer> customers = isEmployee ? 
                customers = db.Customers.Where(c => c.UserId != id  || (c.UserId == id && c.IsAccountOwner == false)) :
                customers = db.Customers.Where(c => c.UserId == id);
            
            return customers.OrderBy(c => c.CustomerName).ToList();
        }

        public static List<EmployeeServiceViewModel> GetEmployeeServices(int employeId)
        {
            var employeeServices = (from es in db.EmployeeServices
                                    join se in db.Services on es.ServiceId equals se.ServiceId
                                    where es.IsDisabled == false && es.EmployeeId == employeId
                                    orderby se.Description
                                    select new EmployeeServiceViewModel
                                    {
                                        EmployeeId = es.EmployeeId,
                                        ServiceId = es.ServiceId,
                                        Description = se.Description,
                                        IsDisabled = es.IsDisabled
                                    }).ToList();

            return employeeServices;
        }

        public static List<ListStatus> GetListStatus(bool isEmployee)
        {

            //var list = (from ls in db.ListStatus
            //            join pl in db.PriorityLevels on ls.PriorityLevelId equals pl.PriorityLevelId
            //            where ls.SystemStatus == false
            //            select new { ls }).ToList();

            var list = (from ls in db.ListStatus
                        join pl in db.PriorityLevels on ls.PriorityLevelId equals pl.PriorityLevelId
                        select new { ls }).ToList();

            list = isEmployee ? list.Where(l => l.ls.SystemStatus == false).ToList() :
                                list.Where(l => l.ls.Canceled == true).ToList();

            List<ListStatus> listStatus = new List<ListStatus>();
            foreach (var item in list)
            {
                listStatus.Add(item.ls);
            }
            return listStatus.OrderBy(ls => ls.PriorityLevel.Value).ToList();
        }

        public static List<PriorityLevel> GetPriorityLevels()
        {
            return db.PriorityLevels.OrderBy(pl => pl.Value).ToList();
        }

        public static List<Service> GetServices()
        {
            return db.Services.OrderBy(c => c.Description).ToList();
        }

        public static List<UserAddressViewModel> GetUserAddresses(int userId)
        {
            //var products = db.Products
            //    .Include(p => p.Inventories.Select(i => i.Warehouse))
            //    .Where(p => p.Inventories.Count > 0)
            //    .ToList();

            List<UserAddressViewModel> userAddresses = (
                from us in db.Users
                join ad in db.Addresses on us.UserId equals ad.UserId
                join co in db.Countries on ad.CountryId equals co.CountryId
                where us.UserId == userId
                select new UserAddressViewModel
                {
                    UserId = us.UserId,
                    AddressId = ad.AddressId,
                    Description = ad.Description,
                    FullName = ad.FullName,
                    Address1 = ad.Address1,
                    Address2 = ad.Address2,
                    City = ad.City,
                    State = ad.State,
                    ZIP = ad.ZIP,
                    IsDefault = ad.IsDefault,
                    IsDisabled = ad.IsDisabled,
                    CountryId = co.CountryId,
                    Code = co.Code,
                    Name = co.Name,
                }).OrderBy(ua => ua.Description)
                .ToList();

            return userAddresses;
        }

        public static List<Business> GetUserBusinesses(int userId)
        {
            return db.Businesses.Where(b => b.UserId == userId).OrderBy(b => b.Name).ToList();
        }
        
        public void Dispose()
        {
            db.Dispose();
        }
    }
}