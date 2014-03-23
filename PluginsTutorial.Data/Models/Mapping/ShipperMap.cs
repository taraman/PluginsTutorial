using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace PluginsTutorial.Data.Models.Mapping
{
    public class ShipperMap : EntityTypeConfiguration<Shipper>
    {
        public ShipperMap()
        {
            // Primary Key
            this.HasKey(t => t.ShipperId);

            // Properties
            this.Property(t => t.CompanyName)
                .IsRequired()
                .HasMaxLength(40);

            this.Property(t => t.Phone)
                .HasMaxLength(24);

        }
    }
}
