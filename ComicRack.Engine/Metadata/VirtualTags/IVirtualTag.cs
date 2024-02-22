using System;
using System.Xml.Serialization;

namespace cYo.Projects.ComicRack.Engine
{
    public interface IVirtualTag
    {
        int ID { get; }
        string Name { get; set; }
        string Description { get; set; }
        string CaptionFormat { get; set; }
        bool IsEnabled { get; set; }
        string PropertyName { get; }
    }
}