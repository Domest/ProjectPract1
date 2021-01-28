using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PractProj1
{
    [Table("valute_curs")]
    public class SendModel
    {
        //public int ID { get; set; }
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public string Nominal { get; set; }
        public string NumCode { get; set; }
        public string CharCode { get; set; }
    }
}
