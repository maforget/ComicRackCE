using System;
using System.Linq;
using System.Reflection;
using DynamicExpresso;

namespace cYo.Common.Text.FunctionParser.Functions.Boolean
{
    public record IfFunctionParameters(string condition, string ifTrue, string ifFalse) : FunctionParametersEval(condition);

    [FunctionDefinition("if")]
    public class IfFunction(string name) : FunctionBase<IfFunctionParameters, string>(name)
    {
        protected override Func<IfFunctionParameters, string> Function => param =>
        {
            //if condition doesn't resolve to a true or false, return an empty string
            if (param.BoolEval is null)
                return string.Empty; 

            return param.BoolEval == true ? param.ifTrue : param.ifFalse;
        };
    }
}
