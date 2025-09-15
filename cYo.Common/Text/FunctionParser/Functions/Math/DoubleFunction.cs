using cYo.Common.Text.FunctionParser.Functions.Boolean;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cYo.Common.Text.FunctionParser.Functions.Math
{
    public record DoubleFunctionParameters(string doubleInText) : FunctionParameter;

    [FunctionDefinition(name: "double")]
    public class DoubleFunction(string name) : FunctionBase<DoubleFunctionParameters, double>(name)
    {
        protected override Func<DoubleFunctionParameters, double> Function => param =>
        {
            if (string.IsNullOrWhiteSpace(param.doubleInText))
                return -1.0d;

            if (Double.TryParse(param.doubleInText, out double result))
                return result;

			return -1.0d;
		};
    }
}
