using System.ComponentModel;

namespace cYo.Common.ComponentModel
{
	public interface IPath
	{
		string Path
		{
			get;
			set;
		}

        string Arguments
        {
            get;
            set;
        }

        string FullPath
        {
            get;
        }
    }
}
