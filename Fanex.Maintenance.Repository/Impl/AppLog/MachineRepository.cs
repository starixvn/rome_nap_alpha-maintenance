// --------------------------------------------------------------------------
// <copyright file="MachineRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2013.8.7">Created</change>
// </history>

namespace Fanex.Maintenance.Repository.Impl.AppLog
{
    using System.Collections;
    using Fanex.Data;
    using Fanex.Maintenance.Core.Model;
    using Fanex.Maintenance.Repository.Criteria.Log;
    using Fanex.Maintenance.Repository.Criteria.Misc;
    using Fanex.NAP.Core.Command;

    /// <summary>
    /// Machine Repository.
    /// </summary>
    public class MachineRepository : BaseRepository<Machine>
    {
        /// <summary>
        /// Insert machine.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Command result.</returns>
        public CommandResult InsertMachine(InsertMachineCriteria criteria)
        {
            Hashtable hash = criteria.GetSpParams();

            hash = (new ResultCriteria()).AddSpParams(hash);

            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                db.ExecuteNonQuery(hash);
                return GetCommandResult(hash);
            }
        }

        /// <summary>
        /// Update machine.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Command result.</returns>
        public CommandResult UpdateMachine(UpdateMachineCriteria criteria)
        {
            Hashtable hash = criteria.GetSpParams();
            hash = (new ResultCriteria()).AddSpParams(hash);

            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                db.ExecuteNonQuery(hash);
                return GetCommandResult(hash);
            }
        }
    }
}