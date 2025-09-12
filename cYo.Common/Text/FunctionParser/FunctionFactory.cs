using cYo.Common.Collections;
using cYo.Common.Reflection;
using cYo.Common.Text.FunctionParser.Functions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime;
using System.Text;
using System.Windows.Forms;


namespace cYo.Common.Text.FunctionParser
{
    public class FunctionFactory
    {
        private static readonly Lazy<FunctionFactory> instance = new Lazy<FunctionFactory>();
        public static FunctionFactory Functions => instance.Value;
        private Dictionary<string, Type> FunctionTypes;

        public FunctionFactory()
        {
            FunctionTypes = Register();
            WriteUsafeInfo();
        }

        [Conditional("DEBUG")]
        private void WriteUsafeInfo()
        {
            ExtractInfo(FunctionTypes).ForEach(f =>
            {
                Debug.WriteLine($"Category: {f.Key}");
                f.Value.ForEach(x => Debug.WriteLine($"    {x.Value}"));
            });
        } 


        public IFunction CreateFunction(string name)
        {
            if (FunctionTypes.TryGetValue(name, out Type functionType))
                return Activator.CreateInstance(functionType, name) as IFunction;

            throw new NotImplementedException($"{name} Function not implemented.");
        }

        public static Dictionary<string, Type> Register()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            return Register(assembly);
        }

        private static Dictionary<string, Type> Register(Assembly assembly)
        {
            var type = typeof(IFunction);

            var functions = (from t in Assembly.GetAssembly(type).GetTypes()
                             where !t.IsAbstract && type.IsAssignableFrom(t)
                             select Register(t)).Where(x => x.name != null);

            return functions.ToDictionary(x => x.name, x => x.type);
        }

        private static (string name, Type type) Register(Type t)
        {
            string name = t.GetAttribute<FunctionDefinitionAttribute>()?.Name.ToLower();

            if (!string.IsNullOrEmpty(name))
                return (name, t);

            return (name, t);
        }

        #region Extracting usage info via Reflection
        public static Dictionary<string, Dictionary<string, string>> ExtractInfo(Dictionary<string, Type> functionTypes)
        {
            Dictionary<string, Dictionary<string, string>> CategoryInfo = new();

            foreach (KeyValuePair<string, Type> item in functionTypes)
            {
                string name = item.Key;
                string @namespace = item.Value.Namespace.Split('.').LastOrDefault();
                CategoryInfo.TryGetValue(@namespace, out var categoryParameters);

                Type paramType = item.Value.GetProperties().Select(x => x.PropertyType).Where(x => x.IsSubclassOf(typeof(FunctionParameter))).FirstOrDefault();
                Type returnType = FindReturnType(item.Value);
                ParameterInfo[] param = paramType?.GetConstructors().FirstOrDefault()?.GetParameters();
                StringBuilder sb = new();
                sb.Append($"${name}<");

                for (int i = 0; i < param.Length; i++)
                {
                    ParameterInfo p = param[i];
                    string separator = i == param.Length - 1 ? ">" : ", ";
                    sb.Append($"{p.ParameterType.Name} {p.Name}{separator}");
                }
                //sb.Append($" - return type is a {returnType.Name}"); //All return type will probably all be strings

                if (categoryParameters != null)
                    categoryParameters.Add(name, sb.ToString());
                else
                    categoryParameters = new Dictionary<string, string>() { { name, sb.ToString() } };

                CategoryInfo[@namespace] = categoryParameters;
            }

            return CategoryInfo;
        }

        private static Type FindReturnType(Type type)
        {
            Type ret;
            var test = type.BaseType.GetGenericArguments();
            ret = test.LastOrDefault();

            if (test.Count() != 2 && type.BaseType != typeof(FunctionBase<,>))
                ret = FindReturnType(type.BaseType);

            return ret;
        } 
        #endregion
    }
}
