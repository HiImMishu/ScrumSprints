using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Models.DTO
{
    public class BacklogDTO
    {
        public int Id { get; set; }

        [Required]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sprint description too long.")]
        public string Description { get; set; }

        [DataType(DataType.Date)]
        public DateTime StartTime { get; set; }

        [DataType(DataType.Date)]
        public DateTime EndTime { get; set; }
    }
}
