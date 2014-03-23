using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace PluginsTutorial.Data.Models.Mapping
{
    public class sysdiagramMap : EntityTypeConfiguration<sysdiagram>
    {
        public sysdiagramMap()
        {
            // Primary Key
            this.HasKey(t => t.diagram_id);

            // Properties
            this.Property(t => t.name)
                .IsRequired()
                .HasMaxLength(128);

        }
    }
}
