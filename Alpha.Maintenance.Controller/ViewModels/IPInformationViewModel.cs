namespace Alpha.Maintenance.Controller.ViewModels
{
    using System.Collections.Generic;
    using Core.Model;
    using Newtonsoft.Json;

    public class IPInformationViewModel
    {
        public IPInformationViewModel(IEnumerable<APIInfo> apis)
        {
            APIInfoJson = JsonConvert.SerializeObject(apis);
        }

        public string APIInfoJson { get; }
    }
}
