using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cYo.Common.Text.FunctionParser.Functions.Date
{
    public record DateFunctionParameters(string dateInText, string format) : FunctionParameter;

    [FunctionDefinition("date")]
    public class DateFormatFunction(string name) : FunctionBase<DateFunctionParameters, string>(name) 
    {
        protected override Func<DateFunctionParameters, string> Function => param =>
        {
            if (string.IsNullOrEmpty(param.dateInText))
                return string.Empty;

            if (DateTime.TryParse(param.dateInText, out DateTime result))
                return result.ToString(param.format);

            throw new ArgumentException("Can't parse date");
        };
    }
}
