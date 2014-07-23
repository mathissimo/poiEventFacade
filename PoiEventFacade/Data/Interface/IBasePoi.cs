using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using PoiEventNetwork.Data.POI;

namespace PoiEventNetwork.Data.Interface
{
    public interface IBasePoi : ITagAble, IEquatable<BasePoi>
    {
        [Required]
        string Name { get; set; }

        ObservableCollection<Tag> Tags { get; }
    }
}