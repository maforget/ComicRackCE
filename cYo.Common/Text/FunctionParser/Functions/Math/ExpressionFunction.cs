using System;
using System.Linq;
using System.Reflection;
using DynamicExpresso;

namespace cYo.Common.Text.FunctionParser.Functions.Math
{
    public record ExpressionFunctionParameters(string value) : FunctionParametersEval(value);

    [FunctionDefinition("expr")]
    public class ExpressionFunction(string Name) : FunctionBase<ExpressionFunctionParameters, string>(Name)
    {
        protected override Func<ExpressionFunctionParameters, string> Function => param => param.StringEval;
    }
}
