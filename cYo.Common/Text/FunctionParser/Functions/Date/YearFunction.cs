using cYo.Common.Text.FunctionParser.Functions.Boolean;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cYo.Common.Text.FunctionParser.Functions.Date
{
    public record YearFunctionParameters(string dateInText) : FunctionParameter;

    [FunctionDefinition("year")]
    public class YearFunction(string name) : FunctionBase<YearFunctionParameters, string>(name)
    {

        protected override Func<YearFunctionParameters, string> Function => param =>
        {
            if (string.IsNullOrEmpty(param.dateInText))
                return string.Empty;

            if (DateTime.TryParse(param.dateInText, out DateTime result))
                return result.Year.ToString();

            throw new ArgumentException("Can't parse date");
        };
    }
}
