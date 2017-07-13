using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Buldo.Ngb.Web.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    

    public class ApplicationSetting
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Key { get; set; }

        public string Value { get; set; }
    }
}
