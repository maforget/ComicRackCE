using cYo.Common.Drawing;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cYo.Projects.ComicRack.Engine.IO.Provider
{
    public class ExportImageContainer
    {
        public required byte[] Data { get; init; }

        public Bitmap Bitmap => BitmapExtensions.BitmapFromBytes(Data);

        public required bool NeedsToConvert { get; init; }

    }
}
