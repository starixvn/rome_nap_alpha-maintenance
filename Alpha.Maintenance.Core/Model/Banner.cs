namespace Alpha.Maintenance.Core.Model
{
    using System;

    public class Banner
    {
        public Banner(
            int id,
            string title,
            bool isActive,
            DateTime releasedDate,
            DateTime expirationDate,
            string environments,
            string sites,
            string desktop,
            string mobile,
            DateTime createdDate)
        {
            Id = id;
            Title = title;
            IsActive = isActive;
            ReleasedDate = releasedDate;
            ExpirationDate = expirationDate;
            Environments = environments;
            Sites = sites;
            Desktop = desktop;
            Mobile = mobile;
            CreatedDate = createdDate;
        }

        public Banner(
            int id,
            string title,
            bool isActive,
            DateTime releasedDate,
            DateTime expirationDate,
            string environments,
            string sites)
        {
            Id = id;
            Title = title;
            IsActive = isActive;
            ReleasedDate = releasedDate;
            ExpirationDate = expirationDate;
            Environments = environments;
            Sites = sites;
        }

        public int Id { get; }

        public string Title { get; }

        public bool IsActive { get; }

        public DateTime ReleasedDate { get; }

        public DateTime ExpirationDate { get; }

        public string Environments { get; }

        public string Sites { get; }

        public string Desktop { get; }

        public string Mobile { get; }

        public DateTime CreatedDate { get; }
    }
}
