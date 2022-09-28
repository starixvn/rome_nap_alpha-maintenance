namespace Alpha.Maintenance.Controller.ViewModels
{
    public class WinLossMonitorViewModel
    {
        public bool AccumulateWinLossJob { get; set; }

        public string LastId { get; set; }

        public bool ChangeAccumulatedWinLossJob { get; set; }

        public string LastCheckTime { get; set; }

        public string LastScanSettingDate { get; set; }

        public bool ChangeThresholdWinLossJob { get; set; }

        public string SuperUUS { get; set; }

        public string Environment { get; set; }

        public bool IsCreditJob { get; set; }

        public string ServiceName { get; set; }

        public bool HasHostPermission { get; set; }

        public string DBAlias { get; set; }
    }
}
