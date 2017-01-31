using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Buldo.Ngb.Bot.EnginesManagement;

namespace Buldo.Ngb.Web.Models.EnginesViewModels
{
    public class EnginesListViewModel
    {
        public List<EngineViewModel> Engines { get; } = new List<EngineViewModel>();
    }
}
