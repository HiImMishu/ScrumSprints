using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Models
{
    public class Product
    {
        [JsonIgnore]
        public int Id { get; set; }

        [Required(ErrorMessage = "Please setup product name.")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Name too long.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please setup owner id")]
        [ForeignKey("User")]
        public int OwnerId { get; set; }

        [ForeignKey("Team")]
        [JsonIgnore]
        public int? DevTeam { get; set; }

        [JsonIgnore]
        public virtual Team Team { get; set; }
    }
}
