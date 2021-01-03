using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Models.DTO
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime SignedAt { get; set; }
        public DateTime? ArchivedAt { get; set; }
        public List<SimpleProductDTO> Products { get; set; }
        public List<SimpleTeamDTO> TeamsLeaded { get; set; }
        public List<SimpleTeamDTO> TeamParticipated { get; set; }
    }
}
