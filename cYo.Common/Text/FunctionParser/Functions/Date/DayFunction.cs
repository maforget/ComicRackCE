using cYo.Common.Text.FunctionParser.Functions.Boolean;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cYo.Common.Text.FunctionParser.Functions.Date
{
    public record DayFunctionParameters(string dateInText) : FunctionParameter;

    [FunctionDefinition(name: "day")]
    public class DayFunction(string name) : FunctionBase<DayFunctionParameters, string>(name)
    {

        protected override Func<DayFunctionParameters, string> Function => param =>
        {
            if (string.IsNullOrEmpty(param.dateInText))
                return string.Empty;

            if (DateTime.TryParse(param.dateInText, out DateTime result))
                return result.Day.ToString("D4");

            throw new ArgumentException("Can't parse date");
        };
    }
}
