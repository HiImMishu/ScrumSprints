using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Models.DTO
{
    public class ProductDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please setup product name.")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Name too long.")]
        public string Name { get; set; }

        public SimpleUserDTO Owner { get; set; }

        public SimpleTeamDTO DevTeam { get; set; }

        public List<BacklogDTO> Backlogs { get; set; }

        public List<ItemDTO> ProductItems { get; set; }
    }
}
