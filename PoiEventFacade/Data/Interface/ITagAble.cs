using System.Collections.Generic;

namespace PoiEventNetwork.Data.Interface
{
    public interface ITagAble : IEntity
    {
        void InitAddTags(IEnumerable<Tag> tags);
        void InitDelTags(IEnumerable<Tag> tags);
    }
}