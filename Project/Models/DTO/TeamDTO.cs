using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Models.DTO
{
    public class TeamDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime CreatedAt { get; set; }

        public string TeamCode { get; set; } = System.Guid.NewGuid().ToString();

        public SimpleUserDTO TeamLeader { get; set; }

        public List<SimpleUserDTO> Members { get; set; }
        
        public SimpleProductDTO Product { get; set; }
    }
}
