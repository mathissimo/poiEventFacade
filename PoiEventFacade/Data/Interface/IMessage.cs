using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using PoiEventNetwork.Data.Context;

namespace PoiEventNetwork.Data.Interface
{
    public interface IMessage : IEntity 
    {
        string Name { get; set; }
        string Text { get; set; }
        User User { get; set; }
        Event Event { get; set; }
    }
}