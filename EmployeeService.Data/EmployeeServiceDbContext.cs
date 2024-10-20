﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeService.Data
{
    public class EmployeeServiceDbContext : DbContext
    {
        public DbSet<EmployeeType> EmployeeTypes { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<AccountSession> AccountSessions { get; set; }

        public EmployeeServiceDbContext(DbContextOptions options) : base(options)
        {

        }
    }
}
