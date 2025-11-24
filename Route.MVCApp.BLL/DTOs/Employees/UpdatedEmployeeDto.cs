using Route.MVCApp.DAL.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Route.MVCApp.BLL.DTOs.Employees
{
    public class UpdatedEmployeeDto : EmployeeBaseDto
    {
        public int Id { get; set; }
    }
}