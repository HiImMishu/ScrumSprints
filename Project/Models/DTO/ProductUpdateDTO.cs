using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Models.DTO
{
    public class ProductUpdateDTO
    {
        [Required(ErrorMessage = "Please setup product id.")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Please setup product name.")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Name too long.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please setup product dev team id.")]
        public int DevTeam { get; set; }
    }
}
