using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Token_Based_Authentication_17_3.Models
{
    public class EmployeeViewModel
    {
        [Required]
        [MaxLength(256)]
        public string Username { get; set; }
       
    }
}