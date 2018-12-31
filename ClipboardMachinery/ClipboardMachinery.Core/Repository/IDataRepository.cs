﻿using ClipboardMachinery.Core.Repository.LazyProvider;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ClipboardMachinery.Core.Repository {

    public interface IDataRepository : IDisposable {

        /// <summary>
        /// Create clip provider that can be used to restive batches of clips sored by descending date.
        /// </summary>
        /// <param name="batchSize">Size of a batch of clips</param>
        /// <returns>A instance of lazy clip provider</returns>
        ILazyDataProvider CreateLazyClipProvider(int batchSize);

        /// <summary>
        /// Create new clip with specified values and insert it into repository.
        /// </summary>
        /// <typeparam name="T">A type of data model that created clip instance will be mapped to and returned back</typeparam>
        /// <param name="content">Content of a clip</param>
        /// <param name="created">Date and time when clip was created</param>
        /// <param name="tags">Tags that clip have in format name=value</param>
        /// <returns>An instance of created clip mapped to T model</returns>
        Task<T> InsertClip<T>(string content, DateTime created, KeyValuePair<string, object>[] tags = null);

        /// <summary>
        /// Remove clip with corresponding id and all related tags.
        /// </summary>
        /// <param name="id">Id of a clip to remove</param>
        Task DeleteClip(int id);

        /// <summary>
        /// Remove tag with corresponding id
        /// </summary>
        /// <param name="id">Id of tag to remove</param>
        Task DeleteTag(int id);

    }

}
