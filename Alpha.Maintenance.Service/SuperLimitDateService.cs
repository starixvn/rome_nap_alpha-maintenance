namespace Alpha.Maintenance.Service
{
    using System.Collections.Generic;
    using Alpha.Maintenance.Core.Model;
    using Alpha.Maintenance.Repository;
    using Alpha.Maintenance.Repository.Criteria.SuperLimitDate;

    public interface ISuperLimitDateService
    {
        CommandResult Delete(DeleteSuperLimitDateCriteria criteria);
        CommandResult Add(AddSuperLimitDateCriteria criteria);
        IEnumerable<SuperLimitDate> Get(GetAllSuperLimitDateCriteria criteria);
    }

    public class SuperLimitDateService : ISuperLimitDateService
    {
        private readonly ISuperLimitDateRepository repository;
        public SuperLimitDateService() : this(null)
        {

        }

        public SuperLimitDateService(ISuperLimitDateRepository repository)
        {
            this.repository = repository ?? new SuperLimitDateRepository();
        }

        public IEnumerable<SuperLimitDate> Get(GetAllSuperLimitDateCriteria criteria)
            => repository.Get(criteria);

        public CommandResult Delete(DeleteSuperLimitDateCriteria criteria)
            => repository.ExecuteNonQuery(criteria);

        public CommandResult Add(AddSuperLimitDateCriteria criteria)
            => repository.ExecuteNonQuery(criteria);
    }
}
