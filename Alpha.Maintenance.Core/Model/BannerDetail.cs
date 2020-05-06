namespace Alpha.Maintenance.Core.Model
{
    using Alpha.Maintenance.Core.Enums;

    public class BannerDetail
    {
        public BannerDetail(byte type, string languageCode, byte[] imageContent)
        {
            Type = (BannerType)type;
            LanguageCode = languageCode;
            ImageContent = imageContent;
        }

        public BannerType Type { get; }

        public string LanguageCode { get; }

        public byte[] ImageContent { get; }
    }
}
