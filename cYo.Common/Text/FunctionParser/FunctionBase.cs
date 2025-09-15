using System;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;

namespace cYo.Common.Text.FunctionParser
{
    public abstract class FunctionBase<TParam, TResult>(string Name) : IFunction where TParam : FunctionParameter
    {
        public string Name { get; } = Name;
        public TParam Param { get; protected set; }
        protected object[] Arguments { get; set; }
        protected abstract Func<TParam, TResult> Function { get; }
        public int ParameterCount => typeof(TParam).GetConstructors().FirstOrDefault()?.GetParameters().Length ?? 0;
        public object Result { get ; private set ; }
        public string ResultAsText => Result.ToString();

        private object Execute()
        {
            if (Param != default(TParam))
                return Function.Invoke(Param);

            return default;
        }

        public event EventHandler Initialized;
        protected virtual void OnInitialized()
        {
            Initialized?.Invoke(this, EventArgs.Empty);
        }

        public event EventHandler Validated;
        protected virtual void OnValidated()
        {
            Validated?.Invoke(this, EventArgs.Empty);
        }

        public virtual void SetParameters(params object[] args)
        {
            Arguments = args;

            if (!AreValid(Arguments))
                return;

            Initialize(Arguments);
            RunFunction();
        }

        protected virtual void Initialize(params object[] args)
        {
            Param = Activator.CreateInstance(typeof(TParam), args) as TParam;
            OnInitialized();
        }

        protected virtual bool AreValid(params object[] args)
        {
            if (args.Length != ParameterCount)
                throw new ArgumentException($"Expected {ParameterCount} parameters, but got {args.Length}.");

            OnValidated();
            return true;
        }

        protected virtual void RunFunction()
        {
            Result = Execute();
        }
    }
}
