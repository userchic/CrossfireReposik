using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using WebApplication1.Models;

namespace WebApplication1.Configurations
{
    public class ShotsConfig : IEntityTypeConfiguration<Shots>
    {
        public void Configure(EntityTypeBuilder<Shots> builder)
        {
        }
    }
}
