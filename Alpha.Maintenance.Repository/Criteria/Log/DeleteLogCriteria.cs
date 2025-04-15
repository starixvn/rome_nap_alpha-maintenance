using System;
using Fanex.Data.Repository;

namespace Alpha.Maintenance.Repository.Criteria.Log
{
    public class DeleteLogCriteria : NonQueryCommand
    {
        private const string SettingKeyName = "Logging_Del";

        public DateTime From { get; set; }

        public override bool IsValid() => true;

        public override string GetSettingKey() => SettingKeyName;
    }
}
