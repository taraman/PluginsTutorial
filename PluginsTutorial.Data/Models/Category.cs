using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;


namespace PluginsTutorial.Data.Models
{

    public partial class Category
    {
        public Category()
        {
            this.Products = new List<Product>();
        }

        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string Description { get; set; }
        public byte[] Picture { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }
}
