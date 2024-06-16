using cYo.Common.Runtime;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cYo.Common
{
    public class CommonConfiguration
    {
        public enum ResizingEngine
        {
            Legacy,
            MagicScaler
        }

        [DefaultValue(ResizingEngine.MagicScaler)]
        public ResizingEngine ImageResizing
        {
            get;
            set;
        }

        private static CommonConfiguration defaultConfig;

        public static CommonConfiguration Default => defaultConfig ?? (defaultConfig = IniFile.Default.Register<CommonConfiguration>());

        public CommonConfiguration()
        {
			ImageResizing = ResizingEngine.MagicScaler;
        }
    }
}
