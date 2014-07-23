using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using PoiEventNetwork.Data.Interface;

namespace PoiEventNetwork.Data.Abstract
{
    public abstract class AbsTagAble : AbsEntity
    {
        #region Properties

        public virtual ObservableCollection<Tag> Tags { get; private set; }

        #endregion

        #region Constructor, Destructor

        protected AbsTagAble()
        {
            Tags = new ObservableCollection<Tag>();
        }

        protected AbsTagAble(IEnumerable<Tag> tags)
        {
            Tags = new ObservableCollection<Tag>(tags);
        }

        #endregion

        #region Methods

        public void InitAddTags(IEnumerable<string> labels)
        {
            //var tags = Tag.GetTagsFromLabels()
        }

        public void InitAddTags(IEnumerable<Tag> tags)
        {
            foreach (var tag in tags.Where(tag => !Tags.Contains(tag)))
            {
                Tags.Add(tag);
            }
        }

        /// <summary>
        /// This removes the tags from this object. It does however not neccesarily delete the tags.
        /// </summary>
        /// <param name="tags">The tags to remove</param>
        public void InitDelTags(IEnumerable<Tag> tags)
        {
            foreach (var tag in tags.Where(tag => Tags.Contains(tag)).ToArray())
            {
                Tags.Remove(tag);
            }
        }

        #endregion
    }
}
