using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace Token_Based_Authentication_17_3.Models
{
    public class RequestViewModel
    {
        public string ID { get; set; }
        [Required]
        [MaxLength(256)]
        public string UserName { get; set; }
        [Required]
        [MaxLength(50)]
        public string EquipmentType { get; set; }
        public string DateRequest { get; set; }
        [MaxLength(10)]
        public string Status { get; set; }
    }
    public class TypeRequestViewModel
    {
        public IList<string> Types { get; set; }
        public RequestViewModel Requests { get; set; }
    }
    
    public class TickRequestViewModel
    {
        public string IDRequest { get; set; }
        public string UserName { get; set; }
        public IList<EquipmentViewModel> listequip { get; set; }
    }
}