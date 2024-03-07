using System.Collections.Generic;
using Alpha.Maintenance.Core.Model;

namespace Alpha.Maintenance.Controller.ViewModels
{
    public class TokenViewModel
    {
        public IDictionary<string, SiteToken> NewTokens { get; set; }

        public IDictionary<string, SiteToken> OldTokens { get; set; }
    }
}