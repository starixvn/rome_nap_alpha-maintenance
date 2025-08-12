namespace Alpha.Maintenance.Service
{
    using System.Collections.Generic;
    using Alpha.Maintenance.Core.Model;
    using Alpha.Maintenance.Repository.Criteria.SuperLimitDate;
    using Fanex.Data.Repository;

    public interface ISuperLimitDateService
    {
        CommandResult Delete(DeleteSuperLimitDateCriteria criteria);
        CommandResult Add(AddSuperLimitDateCriteria criteria);
        IEnumerable<SuperLimitDate> Get(GetAllSuperLimitDateCriteria criteria);
    }

    public class SuperLimitDateService : ISuperLimitDateService
    {
        private readonly IDynamicRepository repository;
        public SuperLimitDateService() : this(null)
        {

        }

        public SuperLimitDateService(IDynamicRepository repository)
        {
            this.repository = repository ?? new DynamicRepository();
        }

        public IEnumerable<SuperLimitDate> Get(GetAllSuperLimitDateCriteria criteria)
            => repository.Fetch<SuperLimitDate>(criteria);

        public CommandResult Delete(DeleteSuperLimitDateCriteria criteria)
        {
            repository.Execute(criteria);

            return new CommandResult
            {
                Code = criteria.ErrCode,
                Message = criteria.ErrMsg
            };
        }

        public CommandResult Add(AddSuperLimitDateCriteria criteria)
        {
            repository.Execute(criteria);

            return new CommandResult
            {
                Code = criteria.ErrCode,
                Message = criteria.ErrMsg
            };
        }
    }
}
