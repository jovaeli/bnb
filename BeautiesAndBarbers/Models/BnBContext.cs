using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace BeautiesAndBarbers.Models
{
    public class BnBContext : DbContext
    {
        public BnBContext() : base("DefaultConnection")
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
        }

        public DbSet<PriorityLevel> PriorityLevels { get; set; }

        public DbSet<ListStatus> ListStatus { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<ClientList> ClientLists { get; set; }

        public DbSet<Service> Services { get; set; }

        public DbSet<EmployeeService> EmployeeServices { get; set; }

        public DbSet<Country> Countries { get; set; }

        public DbSet<Address> Addresses { get; set; }

        public DbSet<Business> Businesses { get; set; }

        public DbSet<UserFavoriteBusiness> UserFavoriteBusinesses { get; set; }

        public DbSet<UserEmployeeRating> UserEmployeeRatings { get; set; }

        public DbSet<Employee> Employees { get; set; }

        public DbSet<Customer> Customers { get; set; }

    }

    
}
/*

ALLOW AUTOMATIC MIGRATION

1. In 'Package Manager Console' run:
    • PM> Enable-Migrations -ContextTypeName yourDbContext -EnableAutomaticMigrations -Force

2. Under Migrations folder, find Configuration.cs and Add following line below 'AutomaticMigrationsEnabled = true;'
    • AutomaticMigrationDataLossAllowed = true;

3. In the Global.asax (Application_Start() method), add the following line:
    • Database.SetInitializer(new MigrateDatabaseToLatestVersion<yourDbContext, Configuration>());
    • Inport:
        - using System.Data.Entity;
        - using yourSolutionNamespace.Models;
        - using yourSolutionNamespace.Migrations;
 */
