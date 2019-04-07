﻿using System.Collections.Generic;
using System.Threading.Tasks;

namespace ClipboardMachinery.Core.Repository.LazyProvider {

    public interface ILazyDataProvider {

        /// <summary>
        /// Query database for a batch of clips with internal offset to retrieve history items.
        /// After successful query moves offset counter by batch size.
        /// </summary>
        /// <typeparam name="M">Type of model that the query instance should be mapped to and returned back</typeparam>
        /// <returns>An enumerable of queried clips mapped to M model</returns>
        Task<IEnumerable<M>> GetNextBatchAsync<M>();

        /// <summary>
        /// Specifies a filter that will be applied to each batch query.
        /// NOTE: This is only experimental, more in-depth implementation will be needed once we start working on search.
        /// </summary>
        /// <param name="name">Name of a tag to filter or null if you do no with to filter by this property.</param>
        /// <param name="value">Value of a tag to filter or null if you do no with to filter by this property.</param>
        void ApplyTagFilter(string name, string value);

        /// <summary>
        /// Resets the offset counter to the beginning of the history.
        /// </summary>
        void Reset();

    };

}
