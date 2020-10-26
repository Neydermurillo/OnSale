using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace OnSaleCommon.Entities
{
    public class Country
    {

        public int Id { get; set; }
        //anotaciones
        [MaxLength(50, ErrorMessage="The filed {0} must contain less than {1}characteres.")]
        [Required]
        public string Name { get; set; }

        public ICollection<Department> Departments { get; set; }
        //propiedad de lectura que me permite saber que propiedades tiene un pais 
        [DisplayName("Departments Number")]
        public int DepartmentsNumber => Departments == null ? 0 : Departments.Count;

    }
}
