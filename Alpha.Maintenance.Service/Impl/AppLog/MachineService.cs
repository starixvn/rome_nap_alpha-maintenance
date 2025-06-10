namespace Alpha.Maintenance.Service.Impl.AppLog
{
    using System.Collections.Generic;
    using Alpha.Maintenance.Core.Model;
    using Alpha.Maintenance.Repository.Criteria.Log;
    using Fanex.Data.Repository;

    /// <summary>
    /// Machine Service.
    /// </summary>
    public class MachineService
    {
        private readonly IDynamicRepository _dynamicRepository = new DynamicRepository();

        private MachineService()
        {
        }

        /// <summary>
        /// Create machine service.
        /// </summary>
        /// <returns>Machine service.</returns>
        public static MachineService Create()
        {
            return new MachineService();
        }

        /// <summary>
        /// Get all machines.
        /// </summary>
        /// <returns>List of machine.</returns>
        public IEnumerable<Machine> GetAll()
        {
            GetAllMachinesCriteria criteria = new GetAllMachinesCriteria();

            return _dynamicRepository.Fetch<Machine>(criteria);
        }

        /// <summary>
        /// Get machines by machine type.
        /// </summary>
        /// <param name="machineType">Machine type.</param>
        /// <returns>List of machine.</returns>
        public IEnumerable<Machine> GetByType(bool? machineType)
        {
            GetMachinesByTypeCriteria criteria = new GetMachinesByTypeCriteria
            {
                MachineType = machineType
            };

            return _dynamicRepository.Fetch<Machine>(criteria);
        }

        /// <summary>
        /// Get machine by machine id.
        /// </summary>
        /// <param name="id">Machine ID.</param>
        /// <returns>Machine object.</returns>
        public Machine GetMachine(int id)
        {
            GetMachineByIdCriteria criteria = new GetMachineByIdCriteria
            {
                MachineId = id
            };

            return _dynamicRepository.Get<Machine>(criteria);
        }

        /// <summary>
        /// Insert machine.
        /// </summary>
        /// <param name="machineName">Machine name.</param>
        /// <param name="machineType">Machine type.</param>
        /// <param name="timeZone">Machine time zone.</param>
        /// <returns>Command result.</returns>
        public CommandResult InsertMachine(string machineName, bool machineType, short timeZone, string machineIP)
        {
            InsertMachineCriteria criteria = new InsertMachineCriteria
            {
                MachineName = machineName,
                IsProduction = machineType,
                Timezone = timeZone,
                MachineIP = machineIP ?? string.Empty
            };

            _dynamicRepository.Execute(criteria);

            return new CommandResult
            {
                Code = criteria.ErrCode,
                Message = criteria.ErrMsg
            };
        }

        /// <summary>
        /// Update machine.
        /// </summary>
        /// <param name="machineId">Machine ID.</param>
        /// <param name="machineType">Machine type.</param>
        /// <param name="timeZone">Machine time zone.</param>
        /// <returns>Command result.</returns>
        public CommandResult UpdateMachine(int machineId, bool machineType, short timeZone, string machineIP)
        {
            UpdateMachineCriteria criteria = new UpdateMachineCriteria
            {
                MachineId = machineId,
                IsProduction = machineType,
                Timezone = timeZone,
                MachineIP = machineIP ?? string.Empty
            };

            _dynamicRepository.Execute(criteria);

            return new CommandResult
            {
                Code = criteria.ErrCode,
                Message = criteria.ErrMsg
            };
        }
    }
}