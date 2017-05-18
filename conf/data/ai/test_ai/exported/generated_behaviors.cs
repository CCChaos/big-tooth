// ---------------------------------------------------------------------
// This file is auto-generated, so please don't modify it by yourself!
// Export file: exported/generated_behaviors.cs
// ---------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace behaviac
{
	class AgentExtra_Generated
	{
		private static Dictionary<string, FieldInfo> _fields = new Dictionary<string, FieldInfo>();
		private static Dictionary<string, PropertyInfo> _properties = new Dictionary<string, PropertyInfo>();
		private static Dictionary<string, MethodInfo> _methods = new Dictionary<string, MethodInfo>();

		public static object GetProperty(behaviac.Agent agent, string property)
		{
			Type type = agent.GetType();
			string propertyName = type.FullName + property;
			if (_fields.ContainsKey(propertyName))
			{
				return _fields[propertyName].GetValue(agent);
			}

			if (_properties.ContainsKey(propertyName))
			{
				return _properties[propertyName].GetValue(agent, null);
			}

			while (type != typeof(object))
			{
				FieldInfo field = type.GetField(property, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
				if (field != null)
				{
					_fields[propertyName] = field;
					return field.GetValue(agent);
				}

				PropertyInfo prop = type.GetProperty(property, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
				if (prop != null)
				{
					_properties[propertyName] = prop;
					return prop.GetValue(agent, null);
				}

				type = type.BaseType;
			}
			Debug.Check(false, "No property can be found!");
			return null;
		}

		public static void SetProperty(behaviac.Agent agent, string property, object value)
		{
			Type type = agent.GetType();
			string propertyName = type.FullName + property;
			if (_fields.ContainsKey(propertyName))
			{
				_fields[propertyName].SetValue(agent, value);
				return;
			}

			if (_properties.ContainsKey(propertyName))
			{
				_properties[propertyName].SetValue(agent, value, null);
				return;
			}

			while (type != typeof(object))
			{
				FieldInfo field = type.GetField(property, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
				if (field != null)
				{
					_fields[propertyName] = field;
					field.SetValue(agent, value);
					return;
				}

				PropertyInfo prop = type.GetProperty(property, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
				if (prop != null)
				{
					_properties[propertyName] = prop;
					prop.SetValue(agent, value, null);
					return;
				}

				type = type.BaseType;
			}
			Debug.Check(false, "No property can be found!");
		}

		public static object ExecuteMethod(behaviac.Agent agent, string method, object[] args)
		{
			Type type = agent.GetType();
			string methodName = type.FullName + method;
			if (_methods.ContainsKey(methodName))
			{
				return _methods[methodName].Invoke(agent, args);;
			}

			while (type != typeof(object))
			{
				MethodInfo m = type.GetMethod(method, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
				if (m != null)
				{
					_methods[methodName] = m;
					return m.Invoke(agent, args);
				}

				type = type.BaseType;
			}
			Debug.Check(false, "No method can be found!");
			return null;
		}
	}

}
