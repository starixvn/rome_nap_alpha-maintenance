namespace Alpha.Maintenance.Repository.Impl
{
    using System;
    using System.Collections.Generic;
    using Core.Model;

    public interface AnnouncementRepository
    {
        IEnumerable<Site> GetSites();
    }

    public class AnnouncementRepositoryImpl : AnnouncementRepository
    {
        public IEnumerable<Site> GetSites()
        {
            return new List<Site>
            {
                new Site { Id = 1, Name = "Site 1" }
            };
        }
    }
}