using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Models.DTO
{
    public class TeamSignoutDTO
    {
        [Required]
        public int UserId { get; set; }

        [Required]
        public int TeamId { get; set; }
    }
}
