// --------------------------------------------------------------------------
// <copyright file="MachineService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2013.8.7">Created</change>
// </history>

namespace Fanex.Maintenance.Service.Impl.AppLog
{
    using System.Collections.Generic;
    using Fanex.Maintenance.Core.Model;
    using Fanex.Maintenance.Repository.Criteria.Log;
    using Fanex.Maintenance.Repository.Impl.AppLog;
    using Fanex.NAP.Core.Command;

    /// <summary>
    /// Machine Service.
    /// </summary>
    public class MachineService
    {
        private readonly MachineRepository _machineRepository = new MachineRepository();

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

            return _machineRepository.Fetch(criteria);
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

            return _machineRepository.Fetch(criteria);
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

            return _machineRepository.Get<Machine>(criteria);
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

            return _machineRepository.InsertMachine(criteria);
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

            return _machineRepository.UpdateMachine(criteria);
        }
    }
}