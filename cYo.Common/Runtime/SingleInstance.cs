#define TRACE
using System;
using System.Diagnostics;
using System.ServiceModel;

namespace cYo.Common.Runtime
{
	[ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, IncludeExceptionDetailInFaults = true)]
	public class SingleInstance : ISingleInstance
	{
		private readonly string name;

		private readonly Action<string[]> StartNew;

		private readonly Action<string[]> StartLast;

		public SingleInstance(string name, Action<string[]> startNew, Action<string[]> startLast)
		{
			this.name = name;
			StartNew = startNew;
			StartLast = startLast;
		}

		public void Run(string[] args)
		{
			string arg = name;
			string text = $"net.pipe://localhost/{arg}";
			ServiceHost serviceHost = null;
			try
			{
				serviceHost = new ServiceHost(this, new Uri(text));
				serviceHost.AddServiceEndpoint(typeof(ISingleInstance), new NetNamedPipeBinding(), "SI");
				serviceHost.Open();
				try
				{
					StartNew(args);
				}
				catch (Exception ex)
				{
					Trace.WriteLine("Failed to start Program: " + ex.Message);
				}
				return;
			}
			catch (Exception)
			{
			}
			finally
			{
				try
				{
					serviceHost.Close();
				}
				catch
				{
				}
			}
			try
			{
				ChannelFactory<ISingleInstance> channelFactory = new ChannelFactory<ISingleInstance>(new NetNamedPipeBinding(), text + "/SI");
				ISingleInstance singleInstance = channelFactory.CreateChannel();
				singleInstance.InvokeLast(args);
			}
			catch
			{
			}
		}

		public void InvokeLast(string[] args)
		{
			if (StartLast != null)
			{
				StartLast(args);
			}
		}

		public void InvokeNew(string[] args)
		{
			if (StartNew != null)
			{
				StartNew(args);
			}
		}
	}
}
