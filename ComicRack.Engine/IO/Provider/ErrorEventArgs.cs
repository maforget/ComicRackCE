using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cYo.Projects.ComicRack.Engine.IO.Provider
{
    public class ErrorEventArgs : EventArgs
    {
        public string Message { get; }

        public ErrorEventArgs(string errorMessage)
        {
            Message = errorMessage;
        }
    }
}
