using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace Token_Based_Authentication_17_3.Models
{
    public class EquipmentViewModel
    {
        public string ID { get; set; }
        [Required]
        [MaxLength(50)]
        public string Type { get; set; }
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        [Required]
        [MaxLength(10)]
        public string Status { get; set; }
        [MaxLength(200)]
        public string Description { get; set; }
        [MaxLength(10)]
        public string UserName { get; set; }

    }
    public class AssignModel
    {
        [Required]
        [MaxLength(10)]
        public string IDEquip { get; set; }
        [Required]
        [MaxLength(10)]
        public string UserName { get; set; }
    }
}