using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cYo.Common.Text.FunctionParser
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class FunctionDefinitionAttribute: Attribute
    {
        public string Name { get; set; }

        public FunctionDefinitionAttribute(string name)
        {
            Name = name;
        }
    }
}
