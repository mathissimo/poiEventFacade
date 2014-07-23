using System.Collections.ObjectModel;

namespace PoiEventNetwork.Data.Interface
{
    public interface INormPoi : IBasePoi
    {
        Coordinate Location { get; set; }
        ObservableCollection<Event> Events { get; }
    }
}