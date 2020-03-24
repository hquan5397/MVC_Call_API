using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Token_Based_Authentication_17_3.Models
{
    public class EquipEmployViewModel
    {
        public string IDEquipment { get; set; }
        public string UserName { get; set; }
        public IList<EmployeeViewModel> Emps { get; set; }
        public IList<EquipmentViewModel> Eqps { get; set; }
    }
}