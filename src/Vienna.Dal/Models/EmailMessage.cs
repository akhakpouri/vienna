using Daytona.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Vienna.Dal.Models
{
    public class EmailMessage : AuditableEntity
    {
        public string Name { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
    }

    public class EmailMessageEntityConfiguration : AuditableEntityConfiguration<EmailMessage>
    {
        public override void Configure(EntityTypeBuilder<EmailMessage> builder)
        {
            builder.Property(p => p.Name).IsRequired().HasMaxLength(150);
            builder.Property(p => p.Subject).IsRequired().HasMaxLength(250);
            builder.Property(p => p.Message).IsRequired().HasColumnType("ntext");
            base.Configure(builder);
        }
    }
}
