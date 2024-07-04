using Microsoft.EntityFrameworkCore;

namespace QualityControl.Models
{
    public class ApplicationContext : DbContext
    {
        public DbSet<ReviewListModel> ReviewListModels { get; set; } = null!;
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            :base(options)
            {
                Database.EnsureCreated();
            }
    }

}
