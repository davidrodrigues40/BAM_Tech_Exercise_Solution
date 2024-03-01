using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations.Schema;

namespace StargateAPI.Business.Data
{
    [Table("Astronaut")]
    public class Astronaut
    {
        public int Id { get; set; }
        public Person Person { get; set; } = new();

        public virtual AstronautDetail? AstronautDetail { get; set; }

        public virtual ICollection<AstronautDuty> AstronautDuties { get; set; } = new HashSet<AstronautDuty>();
    }

    public class AstronautConfiguration : IEntityTypeConfiguration<Astronaut>
    {
        public void Configure(EntityTypeBuilder<Astronaut> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.HasOne(z => z.AstronautDetail).WithOne(z => z.Astronaut).HasForeignKey<AstronautDetail>(z => z.Id);
            builder.HasMany(z => z.AstronautDuties).WithOne(z => z.Astronaut).HasForeignKey(z => z.Id);
        }
    }
}
