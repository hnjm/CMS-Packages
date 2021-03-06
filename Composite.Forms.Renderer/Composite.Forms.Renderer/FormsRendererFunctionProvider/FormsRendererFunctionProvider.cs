﻿using System.Collections.Generic;
using Composite.Forms.Renderer.FormsRendererFunctionProvider.Foundation;
using Composite.Forms.Renderer.FormsRendererFunctionProvider.Functions;
using Composite.Functions;
using Composite.Functions.Plugins.FunctionProvider;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.ObjectBuilder;

namespace Composite.Forms.Renderer.FormsRendererFunctionProvider
{
	[ConfigurationElementType(typeof(FormsRendererFunctionProviderData))]
	public class FormsRendererFunctionProvider : IFunctionProvider
	{
		private EntityTokenFactory _entityTokenFactory;
		private List<IFunction> _functions = null;

		public FormsRendererFunctionProvider(string providerName)
		{
			_entityTokenFactory = new EntityTokenFactory(providerName);
		}

		public FunctionNotifier FunctionNotifier
		{
			set { } // List is static
		}

		public IEnumerable<IFunction> Functions
		{
			get
			{
				if (_functions == null)
				{
					InitializeStaticTypeFunctions();
				}

				foreach (var function in _functions)
				{
					yield return function;
				}
			}
		}

		private void InitializeStaticTypeFunctions()
		{
			_functions = new List<IFunction>
			             	{
			             		new FormsRendererFunction(_entityTokenFactory),
			             		new FormsRendererControlFunction(_entityTokenFactory),
			             		new FormsRendererPropertyFunction(_entityTokenFactory),
			             		new FormEmailFunction(_entityTokenFactory),
			             		new JoinEmailsFunction(_entityTokenFactory)
			             	};
		}
	}


	[Assembler(typeof(FormsRendererFunctionProviderAssembler))]
	public sealed class FormsRendererFunctionProviderData : FunctionProviderData
	{
	}

	public sealed class FormsRendererFunctionProviderAssembler : IAssembler<IFunctionProvider, FunctionProviderData>
	{
		public IFunctionProvider Assemble(IBuilderContext context, FunctionProviderData objectConfiguration, IConfigurationSource configurationSource, ConfigurationReflectionCache reflectionCache)
		{
			return new FormsRendererFunctionProvider(objectConfiguration.Name);
		}
	}
}