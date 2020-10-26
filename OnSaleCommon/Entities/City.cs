using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace OnSaleCommon.Entities
{
    public class City
    {
        public int Id { get; set; }

        [MaxLength(50, ErrorMessage = "The filed {0} must contain less than {1}characteres.")]
        [Required]
        public string Name { get; set; }
        //este campo no se guarda en la base de datos 
        [JsonIgnore]//
        [NotMapped]
        public int IdDepartment { get; set; }
    }

}
