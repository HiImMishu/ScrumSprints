using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Models
{
    public class Team
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please setup Team name.")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Name too long.")]
        public string Name { get; set; }

        [DataType(DataType.Date)]
        public DateTime CreatedAt { get; set; }

        public string TeamCode { get; set; } = System.Guid.NewGuid().ToString();

        [ForeignKey("User")]
        public int LeaderId { get; set; }

        [JsonIgnore]
        public User TeamLeader { get; set; }

        public List<UserTeam> UserTeams { get; set; }
    }
}
