using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cYo.Common.Text.FunctionParser.Functions.Text
{
    public record SubStringFunctionParameters(string text, int startIndex, int length) : FunctionParameter;

    [FunctionDefinition("substring")]
    public class SubStringFunction(string name) : FunctionBase<SubStringFunctionParameters, string>(name)
    {
        protected override Func<SubStringFunctionParameters, string> Function => param => Param.text.Substring(Param.startIndex, Param.length);

        //This is to intercept the args validity check, to change them to actual int type instead of th eincoming text
        protected override bool AreValid(params object[] args)
        {
            bool isValid = base.AreValid(args);

            if (isValid)
            {
                string theText = args[0] as string;
                string startText = args[1] as string;
                string lengthText = args[2] as string;

                int.TryParse(startText, out int startInt);
                int.TryParse(lengthText, out int lengthInt);

                if (startInt < 0 || startInt + 1 >= theText.Length)
                    return false;

                //Set the max length if it's -1 or longer than the text
                if (lengthInt < 0 || (lengthInt - startInt) > (theText.Length - startInt))
                    lengthInt = theText.Length - startInt;

                Arguments = [theText, startInt, lengthInt];
            }

            return isValid;
        }
    }
}
