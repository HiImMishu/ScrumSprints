using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Models.DTO
{
    public class ItemDTO
    {
        public int ItemId { get; set; }

        [Required(ErrorMessage = "Please setup item description.")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Description too long.")]
        public string Description { get; set; }

        [DataType(DataType.Date)]
        public DateTime AddedAt { get; set; } = DateTime.Now;

        [StringLength(maximumLength: 1000, ErrorMessage = "Status edscription too long.")]
        public string Status { get; set; } = "ToDo";

        public BacklogDTO Sprint { get; set; }

        public SimpleUserDTO ModifiedBy { get; set; }
    }
}
