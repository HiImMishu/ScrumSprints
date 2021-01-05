using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Project.Models
{
    public class Item
    {
        public int ItemId { get; set; }

        [Required(ErrorMessage = "Please setup item description.")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Description too long.")]
        public string Description { get; set; }

        [DataType(DataType.Date)]
        public DateTime AddedAt { get; set; } = DateTime.Now;

        [StringLength(maximumLength: 1000, ErrorMessage = "Status edscription too long.")]
        public string status { get; set; } = "ToDo";

        [Required]
        [ForeignKey("Product")]
        public int ProductId { get; set; }

        [ForeignKey("SprintBacklog")]
        public int? SprintId { get; set; }

        [ForeignKey("User")]
        public int? ModifiedBy { get; set; }

        [JsonIgnore]
        public virtual Product Product { get; set; }
    }
}
