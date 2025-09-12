using cYo.Common.Text.FunctionParser.Functions.Boolean;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cYo.Common.Text.FunctionParser.Functions.Math
{
    public record IntFunctionParameters(string intInText) : FunctionParameter;

    [FunctionDefinition(name: "int")]
    public class IntFunction(string name) : FunctionBase<IntFunctionParameters, int>(name)
    {
        protected override Func<IntFunctionParameters, int> Function => param =>
        {
            if (string.IsNullOrWhiteSpace(param.intInText))
                return -1;

            if (Int32.TryParse(param.intInText, out int result))
                return result;

            return -1;
		};
    }
}
