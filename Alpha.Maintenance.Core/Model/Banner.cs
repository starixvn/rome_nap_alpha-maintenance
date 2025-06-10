namespace Alpha.Maintenance.Core.Model
{
    using System;

    public class Banner
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public bool IsActive { get; set; }

        public DateTime ReleasedDate { get; set; }

        public DateTime ExpirationDate { get; set; }

        public string Environments { get; set; }

        public string Sites { get; set; }

        public string Desktop { get; set; }

        public string Mobile { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}
