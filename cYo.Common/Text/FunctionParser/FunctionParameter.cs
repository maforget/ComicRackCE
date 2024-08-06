using DynamicExpresso;
using System;
using System.Linq.Expressions;

namespace cYo.Common.Text.FunctionParser
{
    public abstract record FunctionParameter
    {
        public event EventHandler<ParameterEventArgs>  EvalError;
        protected virtual void OnEvalError(ParameterEventArgs e)
        {
            EvalError?.Invoke(this, e);
        }
    }

    public abstract record FunctionParametersEval(string Text) : FunctionParameter
    {
        private object eval = null;
        public virtual object Eval
        {
            get
            {
                if (eval == null)
                    eval = GetEval();

                return eval;
            }
        }
        [NonSerialized]
        private Func<bool> expression;

        public virtual string StringEval => Eval.ToString();
        public virtual bool? BoolEval => bool.TryParse(Text, out bool parsed) ? parsed : Eval as bool?;
        public virtual int IntEval => Eval as int? ?? 0;
        public virtual double DoubleEval => Eval as double? ?? 0.0;
        public virtual DateTime DateTimeEval => Eval as DateTime? ?? DateTime.MinValue;

        private object GetEval()
        {
            try
            {
                return new Interpreter().Eval(Text);
            }
            catch (Exception ex)
            {
                OnEvalError(new ParameterEventArgs(ex.Message));
                throw new Exception(ex.Message, ex);
            }
        }
    }

    public class ParameterEventArgs
    {
        public string Message { get; set; }

        public ParameterEventArgs(string message)
        {
            Message = message;
        }
    }
}
