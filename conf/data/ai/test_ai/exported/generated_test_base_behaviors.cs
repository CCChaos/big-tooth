// ---------------------------------------------------------------------
// This file is auto-generated, so please don't modify it by yourself!
// Export file: exported/generated_test_base_behaviors.cs
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

	// Source file: test_base

	class DecoratorLoop_bt_test_base_node0 : behaviac.DecoratorLoop
	{
		public DecoratorLoop_bt_test_base_node0()
		{
			m_bDecorateWhenChildEnds = true;
		}
		protected override int GetCount(Agent pAgent)
		{
			return -1;
		}
	}

	class Parallel_bt_test_base_node1 : behaviac.Parallel
	{
		public Parallel_bt_test_base_node1()
		{
			m_failPolicy = behaviac.FAILURE_POLICY.FAIL_ON_ONE;
			m_succeedPolicy = behaviac.SUCCESS_POLICY.SUCCEED_ON_ALL;
			m_exitPolicy = behaviac.EXIT_POLICY.EXIT_ABORT_RUNNINGSIBLINGS;
			m_childFinishPolicy = behaviac.CHILDFINISH_POLICY.CHILDFINISH_LOOP;
		}
	}

	class Action_bt_test_base_node2 : behaviac.Action
	{
		public Action_bt_test_base_node2()
		{
		}
		protected override EBTStatus update_impl(behaviac.Agent pAgent, behaviac.EBTStatus childStatus)
		{
			((AgentBase)pAgent).AI_Attack();
			return EBTStatus.BT_SUCCESS;
		}
	}

	class Action_bt_test_base_node3 : behaviac.Action
	{
		public Action_bt_test_base_node3()
		{
		}
		protected override EBTStatus update_impl(behaviac.Agent pAgent, behaviac.EBTStatus childStatus)
		{
			((AgentBase)pAgent).AI_Idle();
			return EBTStatus.BT_SUCCESS;
		}
	}

	class Action_bt_test_base_node4 : behaviac.Action
	{
		public Action_bt_test_base_node4()
		{
		}
		protected override EBTStatus update_impl(behaviac.Agent pAgent, behaviac.EBTStatus childStatus)
		{
			((AgentBase)pAgent).AI_Sleep();
			return EBTStatus.BT_SUCCESS;
		}
	}

	public static class bt_test_base
	{
		public static bool build_behavior_tree(BehaviorTree bt)
		{
			bt.SetClassNameString("BehaviorTree");
			bt.SetId(-1);
			bt.SetName("test_base");
#if !BEHAVIAC_RELEASE
			bt.SetAgentType("AgentBase");
#endif
			// children
			{
				DecoratorLoop_bt_test_base_node0 node0 = new DecoratorLoop_bt_test_base_node0();
				node0.SetClassNameString("DecoratorLoop");
				node0.SetId(0);
#if !BEHAVIAC_RELEASE
				node0.SetAgentType("AgentBase");
#endif
				bt.AddChild(node0);
				{
					Parallel_bt_test_base_node1 node1 = new Parallel_bt_test_base_node1();
					node1.SetClassNameString("Parallel");
					node1.SetId(1);
#if !BEHAVIAC_RELEASE
					node1.SetAgentType("AgentBase");
#endif
					node0.AddChild(node1);
					{
						Action_bt_test_base_node2 node2 = new Action_bt_test_base_node2();
						node2.SetClassNameString("Action");
						node2.SetId(2);
#if !BEHAVIAC_RELEASE
						node2.SetAgentType("AgentBase");
#endif
						node1.AddChild(node2);
						node1.SetHasEvents(node1.HasEvents() | node2.HasEvents());
					}
					{
						Action_bt_test_base_node3 node3 = new Action_bt_test_base_node3();
						node3.SetClassNameString("Action");
						node3.SetId(3);
#if !BEHAVIAC_RELEASE
						node3.SetAgentType("AgentBase");
#endif
						node1.AddChild(node3);
						node1.SetHasEvents(node1.HasEvents() | node3.HasEvents());
					}
					{
						Action_bt_test_base_node4 node4 = new Action_bt_test_base_node4();
						node4.SetClassNameString("Action");
						node4.SetId(4);
#if !BEHAVIAC_RELEASE
						node4.SetAgentType("AgentBase");
#endif
						node1.AddChild(node4);
						node1.SetHasEvents(node1.HasEvents() | node4.HasEvents());
					}
					node0.SetHasEvents(node0.HasEvents() | node1.HasEvents());
				}
				bt.SetHasEvents(bt.HasEvents() | node0.HasEvents());
			}
			return true;
		}
	}

}
