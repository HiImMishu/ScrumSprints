using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Models.DTO
{
    public class TeamUpdateDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please setup Team name.")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Name too long.")]
        public string Name { get; set; }

        public int TeamLeader { get; set; }
    }
}
