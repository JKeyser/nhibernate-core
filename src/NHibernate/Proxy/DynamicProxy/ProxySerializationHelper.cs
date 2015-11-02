using System;
using System.Runtime.Serialization;
using System.Reflection;
using System.Security;


namespace NHibernate.Proxy.DynamicProxy
{
	public static class ProxySerializationHelper
	{

		[SecuritySafeCritical()]
		public static void AddBaseTypeDataToSerializationInfo(Object proxyObject, System.Type baseType, SerializationInfo info, StreamingContext context)
		{
			MemberInfo[] serializableMembers = FormatterServices.GetSerializableMembers(baseType, context);
			Object[] data = FormatterServices.GetObjectData(proxyObject, serializableMembers);
			for (int memberIndex = 0; memberIndex < serializableMembers.Length; memberIndex++)
			{
				FieldInfo nextField = (FieldInfo)serializableMembers[memberIndex];
				info.AddValue(nextField.Name, data[memberIndex], nextField.FieldType);
			}
		}

		[SecuritySafeCritical()]
		public static void AddSerializationInfoDataToBaseType(Object proxyObject, System.Type baseType, SerializationInfo info, StreamingContext context)
		{
			MemberInfo[] serializableMembers = FormatterServices.GetSerializableMembers(baseType, context);
			Object[] data = new Object[serializableMembers.Length];
			for (int memberIndex = 0; memberIndex < serializableMembers.Length; memberIndex++)
			{
				FieldInfo nextField = (FieldInfo)serializableMembers[memberIndex];
				data[memberIndex] = info.GetValue(nextField.Name, nextField.FieldType);
			}
			FormatterServices.PopulateObjectMembers(proxyObject, serializableMembers, data);
		}

	}
}