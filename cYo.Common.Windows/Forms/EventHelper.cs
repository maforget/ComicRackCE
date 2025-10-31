using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;

namespace cYo.Common.Windows.Forms
{
	public static class EventHelper
	{
		/// <summary>
		/// <para>Safely subscribes to a control event by name:</para>
		/// <para>
		/// - Unsubscribes the handler first to prevent duplicates<br/>
		/// - Subscribes the handler<br/>
		/// - Automatically removes the handler when the control is disposed
		/// </para>
		/// </summary>
		public static void SafeSubscribe(this Component control, string eventName, Delegate handler)
		{
			if (control is null || string.IsNullOrEmpty(eventName) || handler is null)
				throw new ArgumentNullException();

			// Get event metadata
			EventInfo eventInfo = control.GetType().GetEvent(eventName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			if (eventInfo == null)
				throw new ArgumentException($"Event '{eventName}' not found on control type {control.GetType().Name}.");

			// Convert handler to correct delegate type if necessary
			Delegate convertedHandler = handler;
			if (!eventInfo.EventHandlerType.IsInstanceOfType(handler))
				convertedHandler = Delegate.CreateDelegate(eventInfo.EventHandlerType, handler.Target, handler.Method);

			// Remove and add handler
			eventInfo.RemoveEventHandler(control, convertedHandler);
			eventInfo.AddEventHandler(control, convertedHandler);

			void OnDisposed(object sender, EventArgs e)
			{
				if (sender is Component c)
				{
					eventInfo.RemoveEventHandler(c, convertedHandler); // Remove original handler
					c.Disposed -= OnDisposed; // unsuscribe to this OnDisposed event
				}
			}

			control.Disposed -= OnDisposed; // Remove previous Disposed handler (in case called multiple times)
			control.Disposed += OnDisposed; // Add disposal cleanup
		}
	}
}
