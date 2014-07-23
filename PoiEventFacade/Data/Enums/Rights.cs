using System;

namespace PoiEventNetwork.Data.Enums
{
    [Flags]
    public enum Rights
    {
        None        = 0,
        Registered  = 1,
        Administer  = Registered | 2 
    }
}