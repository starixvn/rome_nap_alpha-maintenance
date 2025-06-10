namespace Alpha.Maintenance.Repository.Criteria.Banners
{
    using System;
    using Fanex.Data;
    using Fanex.Data.Repository;

    public abstract class SaveBannerCriteria : NonQueryCommand
    {
        public string BannerName { get; set; }
        public bool IsActive { get; set; }
        public DateTime ReleasedDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string Environments { get; set; }
        public string Systems { get; set; }
        public int Creator { get; set; }

        [SpParam(IsSpecialType = true)]
        public byte[] DeskEnUsImage { get; set; }

        [SpParam(IsSpecialType = true)]
        public byte[] DeskViVnImage { get; set; }

        [SpParam(IsSpecialType = true)]
        public byte[] DeskZhCnImage { get; set; }

        [SpParam(IsSpecialType = true)]
        public byte[] DeskZhTwImage { get; set; }

        [SpParam(IsSpecialType = true)]
        public byte[] DeskThThImage { get; set; }

        [SpParam(IsSpecialType = true)]
        public byte[] DeskKoKrImage { get; set; }

        [SpParam(IsSpecialType = true)]
        public byte[] DeskJaJpImage { get; set; }

        [SpParam(IsSpecialType = true)]
        public byte[] MobileEnUsImage { get; set; }

        [SpParam(IsSpecialType = true)]
        public byte[] MobileViVnImage { get; set; }

        [SpParam(IsSpecialType = true)]
        public byte[] MobileZhCnImage { get; set; }

        [SpParam(IsSpecialType = true)]
        public byte[] MobileZhTwImage { get; set; }

        [SpParam(IsSpecialType = true)]
        public byte[] MobileThThImage { get; set; }

        [SpParam(IsSpecialType = true)]
        public byte[] MobileKoKrImage { get; set; }

        [SpParam(IsSpecialType = true)]
        public byte[] MobileJaJpImage { get; set; }
        public int ErrCode { get; set; }
        public string ErrMsg { get; set; }

        [SpParam(Ignored = true)]
        public bool IsDBStandby { get; set; }
        public override string GetSettingKey()
            => IsDBStandby ? "Age_Banner_Update_Standby" : "Age_Banner_Update";
    }
}
