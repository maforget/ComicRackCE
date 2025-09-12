using cYo.Common.Text.FunctionParser.Functions.Boolean;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cYo.Common.Text.FunctionParser.Functions.Date
{
    public record MonthFunctionParameters(string dateInText) : FunctionParameter;

    [FunctionDefinition("month")]
    public class Monthunction(string name) : FunctionBase<MonthFunctionParameters, string>(name)
    {

        protected override Func<MonthFunctionParameters, string> Function => param =>
        {
            if (string.IsNullOrEmpty(param.dateInText))
                return string.Empty;

            if (DateTime.TryParse(param.dateInText, out DateTime result))
                return result.Month.ToString("D4");

            throw new ArgumentException("Can't parse date");
        };
    }
}
