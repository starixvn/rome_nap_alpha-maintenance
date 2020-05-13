namespace Alpha.Maintenance.Core.Enums
{
    using System.ComponentModel;

    public enum BannerFilterBy : byte
    {
        [Description("Released Date")]
        ReleasedDate = 1,

        [Description("Expiration Date")]
        ExpirationDate = 2,

        [Description("Created Date")]
        CreatedDate = 3
    }
}
