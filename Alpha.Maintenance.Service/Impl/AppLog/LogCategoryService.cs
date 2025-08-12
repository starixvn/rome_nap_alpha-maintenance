// --------------------------------------------------------------------------
// <copyright file="LogCategoryService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Edward.Nguyen" date="2012.9.20">Created</change>
// </history>

namespace Alpha.Maintenance.Service.Impl.AppLog
{
    using System.Collections.Generic;
    using System.Linq;
    using Alpha.Maintenance.Core.Model;
    using Alpha.Maintenance.Repository.Criteria.Log;
    using Fanex.Data.Repository;

    /// <summary>
    /// Log category service.
    /// </summary>
    public class LogCategoryService
    {
        private readonly IDynamicRepository dynamicRepository;

        private LogCategoryService()
        {
            dynamicRepository = new DynamicRepository();
        }

        /// <summary>
        /// Create log category service.
        /// </summary>
        /// <returns>Log category service.</returns>
        public static LogCategoryService Create()
        {
            return new LogCategoryService();
        }

        /// <summary>
        /// Get all category.
        /// </summary>
        /// <returns>List of log category.</returns>
        public IEnumerable<LogCategory> GetAll()
        {
            var criteria = new GetAllCategoryCriteria();
            var categories = dynamicRepository.Fetch<LogCategory>(criteria);

            return categories.OrderBy(logCategory =>
            {
                if (logCategory.CategoryName.ToLowerInvariant().StartsWith("alpha-"))
                {
                    return 0;
                }

                return 1;
            });
        }
    }
}