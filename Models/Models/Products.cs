using System.ComponentModel.DataAnnotations;

namespace Models.Models
{
    public class Products
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Required")]
        [StringLength(50, ErrorMessage = "Must be at max {1} characters")]
        public string Name { get; set; }
        public string MainImage { get; set; }
        public string Images { get; set; }
        public decimal Price { get; set; }
        public int OFF { get; set; }

        [Required(ErrorMessage = "Required")]
        [StringLength(500, ErrorMessage = "Must be at max {1} characters")]
        public string Desc { get; set; }
        public int Sold { get; set; }
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }
    }
}