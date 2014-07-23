
﻿using System;
using System.Collections.Generic;
﻿using System.Linq;
﻿using NLog;

namespace PoiEventNetwork.Data.Tools
{
    public static class GenericSearcher
    {
        #region Fields

        /// <summary>
        /// The static logger instance for this class.
        /// </summary>
        private static Logger s_Logger = NLog.LogManager.GetCurrentClassLogger();

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <param name="dbSet">The DbSet in which to look.</param>
        /// <param name="predicate">The predicate to match.</param>
        /// <returns>The entity if any matches.</returns>
        /// <exception cref="ArgumentException">If no entity matches.</exception>
        public static TEntity GetFirstOrDefaultByCriteria<TEntity>(IEnumerable<TEntity> dbSet, Func<TEntity, bool> predicate)
            where TEntity : class
        {
            var entity = dbSet.FirstOrDefault(predicate);

            if(entity == null)
                ExceptionHelper.ThrowArgumentException(s_Logger, "", new ArgumentException(string.Format("No entity of type {0} exists for the given criteria.", typeof(TEntity).ToString())));

            return entity;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <param name="dbSet">The DbSet in which to look.</param>
        /// <param name="predicate">The predicate to match.</param>
        /// <returns>The entity if any matches, null otherwise.</returns>
        public static TEntity TryGetFirstOrDefaultByCriteria<TEntity>(IEnumerable<TEntity> dbSet, Func<TEntity, bool> predicate)
            where TEntity : class
        {
            return dbSet.FirstOrDefault(predicate);
        }
    }
}

