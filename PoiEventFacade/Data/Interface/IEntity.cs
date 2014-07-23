using System.ComponentModel.DataAnnotations;
using PoiEventNetwork.Data.Context;

namespace PoiEventNetwork.Data.Interface
{
    public interface IEntity : IValidatableObject
    {
        [Key]
        long Id { get; }

        void InitDel(DataContext ctx);
    } 
}