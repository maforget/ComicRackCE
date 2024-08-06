using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using DynamicExpresso;

namespace cYo.Common.Text.FunctionParser
{
    public interface IFunction
    {
        string Name { get; }
        void SetParameters(params object[] args);
        object Result { get; }
        string ResultAsText { get; }
    }
}
