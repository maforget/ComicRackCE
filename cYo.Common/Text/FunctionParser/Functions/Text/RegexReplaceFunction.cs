using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace cYo.Common.Text.FunctionParser.Functions.Text
{
    public record RegexReplaceFunctionParameters(string inputText, string pattern, string replacement) : FunctionParameter;

    [FunctionDefinition("RegexReplace")]
    public class RegexReplaceFunction(string name) : FunctionBase<RegexReplaceFunctionParameters, string>(name)
    {
        protected override Func<RegexReplaceFunctionParameters, string> Function => param =>
        {
            return Regex.Replace(param.inputText, param.pattern, param.replacement, RegexOptions.IgnoreCase);
        };
    }
}
