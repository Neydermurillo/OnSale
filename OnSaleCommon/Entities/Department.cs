using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace OnSaleCommon.Entities
{
    public class Department
    {
        public int Id { get; set; }

        [MaxLength(50, ErrorMessage = "The filed {0} must contain less than {1}characteres.")]
        [Required]
        public string Name { get; set; }

        //relacion un depeartamento tiene una coleccion de ciudades 
        public ICollection<City> Cities { get; set; }

        [DisplayName("Cities Number")]
        public int CitiesNumber => Cities == null ? 0 : Cities.Count; //proiedad de lectura si la coleccion de ciudades es nuela devuelve un cero, delo contrario devuelve cuenta las ciudades que tenemos 

        //este campo no se guarda en la base de datos 
        [JsonIgnore]//
        [NotMapped]
        public int IdCountry { get; set; }
    }

}
