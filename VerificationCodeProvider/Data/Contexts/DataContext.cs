

using Microsoft.EntityFrameworkCore;
using VerificationCodeProvider.Data.Entities;

namespace VerificationCodeProvider.Data.Contexts;

public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
{

    public DbSet<VerificationRequestEntity> VerificationRequests { get; set; }  


}
