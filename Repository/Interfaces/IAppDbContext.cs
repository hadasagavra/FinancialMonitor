using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Repository.Entities;
namespace Repository.Interfaces
{
    public interface IAppDbContext
    {
        DbSet<Transaction> Transactions { get; set; }
        Task Save();
    }
}
