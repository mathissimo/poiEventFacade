namespace PoiEventNetwork.Data.Interface
{
    public interface ICityPoi : INormPoi
    {
        string Street { get; set; }
        string City { get; set; }
    }
}