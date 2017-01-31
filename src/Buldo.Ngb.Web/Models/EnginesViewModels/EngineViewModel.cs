using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Buldo.Ngb.Bot.EnginesManagement;

namespace Buldo.Ngb.Web.Models.EnginesViewModels
{
    public class EngineViewModel
    {
        public int Id { get; set; }

        [Required]
        [DataType(DataType.Url)]
        public string Address { get; set; }

        [Required]
        [DataType(DataType.Text)]
        public string Login { get; set; }

        [Required]
        [DataType(DataType.Text)]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public EngineViewModel(EngineInfo info)
        {
            Id = info.Id;
            Address = info.Address;
            Name = info.Name;
            Login = info.Login;
        }

        public EngineInfo ToEngineInfo()
        {
            return new EngineInfo()
            {
                Id = Id,
                Address = Address,
                Login = Login,
                Name = Name,
                Password = Password,
            };
        }
    }
}
