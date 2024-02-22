using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace cYo.Projects.ComicRack.Engine
{
    public class VirtualTag : IVirtualTag
    {
        public VirtualTag()
        {
        }

        public VirtualTag(int id, string name, string description, string captionFormat, bool isEnabled = false, bool isDefault = false)
        {
            ID = id;
            Name = name;
            Description = description;
            CaptionFormat = captionFormat;
            IsEnabled = isEnabled;
            IsDefault = isDefault;
        }

        [DefaultValue(0)]
        public int ID { get; set; }

        [DefaultValue("")]
        public string Name { get; set; }

        [DefaultValue("")]
        public string Description { get; set; }

        [DefaultValue("")]
        public string CaptionFormat { get; set; }

        [DefaultValue(false)]
        public bool IsEnabled { get; set; }

        [XmlIgnore]
        [DefaultValue(false)]
        public bool IsDefault { get; set; }

        [XmlIgnore]
        public string PropertyName => $"VirtualTag{ID:00}";

        [XmlIgnore]
        public string DisplayMember => $"{Name}{(IsEnabled ? " (Enabled)" : "")}";

        public override bool Equals(object obj)
        {
            VirtualTag vtag = obj as VirtualTag;
            if (vtag != null)
            {
                return Equals(vtag);
            }
            return false;
        }

        public bool Equals(VirtualTag vtag)
        {
            if (vtag != null)
            {
                return vtag.ID == ID && vtag.Name == Name && vtag.Description == Description
                    && vtag.CaptionFormat == CaptionFormat && vtag.IsEnabled == IsEnabled && vtag.IsDefault == IsDefault;
            }
            return false;
        }
    }
}


