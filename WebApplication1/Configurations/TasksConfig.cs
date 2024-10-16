using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApplication1.Models;

namespace WebApplication1.Configurations
{
    public class TasksConfig : IEntityTypeConfiguration<Tasks>
    {
        public void Configure(EntityTypeBuilder<Tasks> builder)
        {
            builder.HasKey(X => X.ID);
            builder.Property (x => x.Answer).IsRequired();
            builder.Property (x => x.Text).IsRequired();
            builder.HasMany(x => x.UsersAnswers);
        }
    }
}
