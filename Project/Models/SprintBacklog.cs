using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Project.Models
{
    public class SprintBacklog
    {
        public int Id { get; set; }

        [StringLength(maximumLength: 1000, ErrorMessage = "Sprint description too long.")]
        public string Description { get; set; }

        [Required]
        [ForeignKey("Product")]
        public int ProductId { get; set; }

        [DataType(DataType.Date)]
        public DateTime StartTime { get; set; }

        [DataType(DataType.Date)]
        public DateTime EndTime { get; set; }

        [JsonIgnore]
        public virtual Product Product { get; set; }
    }
}
