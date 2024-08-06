using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace cYo.Common.Text.FunctionParser.Functions.Text
{
    public record RegexMatchFunctionParameters(string inputText, string pattern) : FunctionParameter;

    [FunctionDefinition("RegexMatch")]
    public class RegexMatchFunction(string name) : FunctionBase<RegexMatchFunctionParameters, bool>(name)
    {
        protected override Func<RegexMatchFunctionParameters, bool> Function => param =>
        {
            return Regex.IsMatch(param.inputText, param.pattern, RegexOptions.IgnoreCase);
        };
    }
}
