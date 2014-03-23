using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace PluginsTutorial.Data.Models.Mapping
{
    public class OrderMap : EntityTypeConfiguration<Order>
    {
        public OrderMap()
        {
            // Primary Key
            this.HasKey(t => t.OrderId);

            // Properties
            this.Property(t => t.CustomerId)
                .IsFixedLength()
                .HasMaxLength(5);

            this.Property(t => t.ShipName)
                .HasMaxLength(40);

            this.Property(t => t.ShipAddress)
                .HasMaxLength(60);

            this.Property(t => t.ShipCity)
                .HasMaxLength(15);

            this.Property(t => t.ShipRegion)
                .HasMaxLength(15);

            this.Property(t => t.ShipPostalCode)
                .HasMaxLength(10);

            this.Property(t => t.ShipCountry)
                .HasMaxLength(15);


            // Relationships
            this.HasOptional(t => t.Customer)
                .WithMany(t => t.Orders)
                .HasForeignKey(d => d.CustomerId);
            this.HasOptional(t => t.Employee)
                .WithMany(t => t.Orders)
                .HasForeignKey(d => d.EmployeeId);
            this.HasOptional(t => t.Shipper)
                .WithMany(t => t.Orders)
                .HasForeignKey(d => d.ShipperId);

        }
    }
}
