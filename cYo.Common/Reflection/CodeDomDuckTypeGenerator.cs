using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using Microsoft.CSharp;

namespace cYo.Common.Reflection
{
	internal class CodeDomDuckTypeGenerator : IDuckTypeGenerator
	{
		private class ReferenceList
		{
			private readonly List<string> list = new List<string>();

			private static readonly Assembly mscorlib = typeof(object).Assembly;

			public bool AddReference(Assembly assembly)
			{
				if (!list.Contains(assembly.Location) && assembly != mscorlib)
				{
					list.Add(assembly.Location);
					return true;
				}
				return false;
			}

			public void AddReference(Type type)
			{
				AddReference(type.Assembly);
				if (type.BaseType.Assembly != mscorlib)
				{
					AddReference(type.BaseType);
				}
			}

			public void SetToCompilerParameters(CompilerParameters parameters)
			{
				foreach (string item in list)
				{
					parameters.ReferencedAssemblies.Add(item);
				}
			}
		}

		private const string TypePrefix = "Duck";

		private const string CommonNamespace = "DynamicDucks";

		public Type[] CreateDuckTypes(Type interfaceType, Type[] duckedTypes)
		{
			string text = $"{CommonNamespace}.{interfaceType.Name}";
			CodeCompileUnit codeCompileUnit = new CodeCompileUnit();
			CodeNamespace codeNamespace = new CodeNamespace(text);
			codeCompileUnit.Namespaces.Add(codeNamespace);
			CodeTypeReference codeTypeReference = new CodeTypeReference(interfaceType);
			ReferenceList referenceList = new ReferenceList();
			for (int i = 0; i < duckedTypes.Length; i++)
			{
				Type type = duckedTypes[i];
				CodeTypeReference type2 = new CodeTypeReference(type);
				referenceList.AddReference(type);
				CodeTypeDeclaration codeTypeDeclaration = new CodeTypeDeclaration(TypePrefix + i);
				codeNamespace.Types.Add(codeTypeDeclaration);
				codeTypeDeclaration.TypeAttributes = TypeAttributes.Public;
				codeTypeDeclaration.BaseTypes.Add(codeTypeReference);
				CodeMemberField codeMemberField = new CodeMemberField(type2, "_obj");
				codeTypeDeclaration.Members.Add(codeMemberField);
				CodeFieldReferenceExpression codeFieldReferenceExpression = new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), codeMemberField.Name);
				CodeConstructor codeConstructor = new CodeConstructor();
				codeTypeDeclaration.Members.Add(codeConstructor);
				codeConstructor.Attributes = MemberAttributes.Public;
				codeConstructor.Parameters.Add(new CodeParameterDeclarationExpression(type2, "obj"));
				codeConstructor.Statements.Add(new CodeAssignStatement(codeFieldReferenceExpression, new CodeArgumentReferenceExpression("obj")));
				MethodInfo[] methods = interfaceType.GetMethods();
				foreach (MethodInfo methodInfo in methods)
				{
					if ((methodInfo.Attributes & MethodAttributes.SpecialName) == 0)
					{
						CodeMemberMethod codeMemberMethod = new CodeMemberMethod();
						codeTypeDeclaration.Members.Add(codeMemberMethod);
						codeMemberMethod.Name = methodInfo.Name;
						codeMemberMethod.ReturnType = new CodeTypeReference(methodInfo.ReturnType);
						codeMemberMethod.PrivateImplementationType = codeTypeReference;
						referenceList.AddReference(methodInfo.ReturnType);
						ParameterInfo[] parameters = methodInfo.GetParameters();
						CodeArgumentReferenceExpression[] array = new CodeArgumentReferenceExpression[parameters.Length];
						int num = 0;
						ParameterInfo[] array2 = parameters;
						foreach (ParameterInfo parameterInfo in array2)
						{
							referenceList.AddReference(parameterInfo.ParameterType);
							CodeParameterDeclarationExpression value = new CodeParameterDeclarationExpression(parameterInfo.ParameterType, parameterInfo.Name);
							codeMemberMethod.Parameters.Add(value);
							array[num++] = new CodeArgumentReferenceExpression(parameterInfo.Name);
						}
						CodeMethodInvokeExpression codeMethodInvokeExpression = new CodeMethodInvokeExpression(codeFieldReferenceExpression, methodInfo.Name, array);
						if (methodInfo.ReturnType == typeof(void))
						{
							codeMemberMethod.Statements.Add(codeMethodInvokeExpression);
						}
						else
						{
							codeMemberMethod.Statements.Add(new CodeMethodReturnStatement(codeMethodInvokeExpression));
						}
					}
				}
				PropertyInfo[] properties = interfaceType.GetProperties();
				foreach (PropertyInfo propertyInfo in properties)
				{
					CodeMemberProperty codeMemberProperty = new CodeMemberProperty();
					codeTypeDeclaration.Members.Add(codeMemberProperty);
					codeMemberProperty.Name = propertyInfo.Name;
					codeMemberProperty.Type = new CodeTypeReference(propertyInfo.PropertyType);
					codeMemberProperty.Attributes = MemberAttributes.Public;
					codeMemberProperty.PrivateImplementationType = new CodeTypeReference(interfaceType);
					referenceList.AddReference(propertyInfo.PropertyType);
					ParameterInfo[] indexParameters = propertyInfo.GetIndexParameters();
					CodeArgumentReferenceExpression[] array3 = new CodeArgumentReferenceExpression[indexParameters.Length];
					int num2 = 0;
					ParameterInfo[] array4 = indexParameters;
					foreach (ParameterInfo parameterInfo2 in array4)
					{
						CodeParameterDeclarationExpression value2 = new CodeParameterDeclarationExpression(parameterInfo2.ParameterType, parameterInfo2.Name);
						codeMemberProperty.Parameters.Add(value2);
						referenceList.AddReference(parameterInfo2.ParameterType);
						CodeArgumentReferenceExpression codeArgumentReferenceExpression = new CodeArgumentReferenceExpression(parameterInfo2.Name);
						array3[num2++] = codeArgumentReferenceExpression;
					}
					if (propertyInfo.CanRead)
					{
						codeMemberProperty.HasGet = true;
						if (array3.Length == 0)
						{
							codeMemberProperty.GetStatements.Add(new CodeMethodReturnStatement(new CodePropertyReferenceExpression(codeFieldReferenceExpression, propertyInfo.Name)));
						}
						else
						{
							codeMemberProperty.GetStatements.Add(new CodeMethodReturnStatement(new CodeIndexerExpression(codeFieldReferenceExpression, array3)));
						}
					}
					if (propertyInfo.CanWrite)
					{
						codeMemberProperty.HasSet = true;
						if (array3.Length == 0)
						{
							codeMemberProperty.SetStatements.Add(new CodeAssignStatement(new CodePropertyReferenceExpression(codeFieldReferenceExpression, propertyInfo.Name), new CodePropertySetValueReferenceExpression()));
						}
						else
						{
							codeMemberProperty.SetStatements.Add(new CodeAssignStatement(new CodeIndexerExpression(codeFieldReferenceExpression, array3), new CodePropertySetValueReferenceExpression()));
						}
					}
				}
				EventInfo[] events = interfaceType.GetEvents();
				foreach (EventInfo eventInfo in events)
				{
					StringBuilder stringBuilder = new StringBuilder();
					stringBuilder.Append("public event " + eventInfo.EventHandlerType.FullName + " @" + eventInfo.Name + "{");
					stringBuilder.Append("add    {" + codeMemberField.Name + "." + eventInfo.Name + "+=value;}");
					stringBuilder.Append("remove {" + codeMemberField.Name + "." + eventInfo.Name + "-=value;}");
					stringBuilder.Append("}");
					referenceList.AddReference(eventInfo.EventHandlerType);
					codeTypeDeclaration.Members.Add(new CodeSnippetTypeMember(stringBuilder.ToString()));
				}
			}
			CSharpCodeProvider cSharpCodeProvider = new CSharpCodeProvider();
			StringWriter stringWriter = new StringWriter();
			cSharpCodeProvider.GenerateCodeFromCompileUnit(codeCompileUnit, stringWriter, new CodeGeneratorOptions());
			string value3 = stringWriter.ToString();
			Console.WriteLine(value3);
			CompilerParameters compilerParameters = new CompilerParameters();
			compilerParameters.GenerateInMemory = true;
			compilerParameters.ReferencedAssemblies.Add(interfaceType.Assembly.Location);
			referenceList.SetToCompilerParameters(compilerParameters);
			CompilerResults compilerResults = cSharpCodeProvider.CompileAssemblyFromDom(compilerParameters, codeCompileUnit);
			if (compilerResults.Errors.Count > 0)
			{
				StringWriter stringWriter2 = new StringWriter();
				foreach (CompilerError error in compilerResults.Errors)
				{
					stringWriter2.WriteLine(error.ErrorText);
				}
				throw new Exception("Compiler-Errors: \n\n" + stringWriter2);
			}
			Assembly compiledAssembly = compilerResults.CompiledAssembly;
			Type[] array5 = new Type[duckedTypes.Length];
			for (int num3 = 0; num3 < duckedTypes.Length; num3++)
			{
				array5[num3] = compiledAssembly.GetType($"{text}.{TypePrefix}{num3}");
			}
			return array5;
		}
	}
}
