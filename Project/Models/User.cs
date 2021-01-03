using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Models
{
    public class User
    {
        public int id { get; set; }
        
        [Required]
        [StringLength(maximumLength:100, ErrorMessage = "First Name too long.")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(maximumLength: 100, ErrorMessage = "Last Name too long.")]
        public string LastName { get; set; }


        [Required]
        [DataType(DataType.EmailAddress)]
        [StringLength(maximumLength: 100, ErrorMessage = "Email too long.")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [StringLength(200, MinimumLength = 8, ErrorMessage = "Password must contain 8 characters")]
        public byte[] PasswordHash { get; set; }

        public byte[] PasswordSalt { get; set; }

        [DataType(DataType.Date)]
        public DateTime SignedAt { get; set; } = DateTime.Now;

        [DataType(DataType.Date)]
        public DateTime? ArchivedAt { get; set; }

        [InverseProperty("TeamLeader")]
        public ICollection<Team> LeadedTeams { get; set; }

        [JsonIgnore]
        public List<UserTeam> UserTeams { get; set; }
    }
}
