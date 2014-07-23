using System;
using System.Collections.ObjectModel;
using PoiEventNetwork.Data.Abstract;

namespace PoiEventNetwork.Data.Interface
{
    public interface ITag : IEntity, IEquatable<Tag>
    {
        string Text { get; set; }
        ObservableCollection<AbsTagAble> Tagged { get; }
    }
}