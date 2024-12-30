using MiniExcelLibs;
using System.Reflection.Emit;
using System.Reflection;

namespace MauiExcel.Services;

internal class MiniExcelService
{
	public static void Save(Stream stream, IEnumerable<IEnumerable<string>> data)
	{
		var items = new List<object?>();
		var dataList = data.ToList();
		var dynamicType = CreateTypeWithProperties(dataList.First().Count());
		foreach (var rows in dataList)
		{
			var instance = Activator.CreateInstance(dynamicType);
			var index = 1;
			foreach (var value in rows)
			{
				var property = dynamicType.GetProperty($"Property{index}");
				property?.SetValue(instance, value);
				index++;
			}

			items.Add(instance);
		}

		stream.SaveAs(items);
	}

	public static void Save(Stream stream, IEnumerable<Data> data)
	{
		stream.SaveAs(data);
	}


	static Type CreateTypeWithProperties(int propertyCount)
	{
		// Define a dynamic assembly and module
		var assemblyName = new AssemblyName("DynamicAssembly");
		var assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);
		var moduleBuilder = assemblyBuilder.DefineDynamicModule("DynamicModule");

		// Define a public class named "DynamicClass"
		var typeBuilder = moduleBuilder.DefineType("DynamicClass", TypeAttributes.Public);

		// Add properties to the class
		for (var i = 1; i <= propertyCount; i++)
		{
			var propertyName = $"Property{i}";
			var fieldBuilder = typeBuilder.DefineField($"_{propertyName}", typeof(string), FieldAttributes.Private);

			var propertyBuilder = typeBuilder.DefineProperty(propertyName, PropertyAttributes.HasDefault, typeof(string), null);

			// Define the getter
			var getter = typeBuilder.DefineMethod(
				$"get_{propertyName}",
				MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig,
				typeof(string),
				Type.EmptyTypes
			);
			var getterIL = getter.GetILGenerator();
			getterIL.Emit(OpCodes.Ldarg_0);
			getterIL.Emit(OpCodes.Ldfld, fieldBuilder);
			getterIL.Emit(OpCodes.Ret);
			propertyBuilder.SetGetMethod(getter);

			// Define the setter
			var setter = typeBuilder.DefineMethod(
				$"set_{propertyName}",
				MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig,
				null,
				new[] { typeof(string) }
			);
			var setterIL = setter.GetILGenerator();
			setterIL.Emit(OpCodes.Ldarg_0);
			setterIL.Emit(OpCodes.Ldarg_1);
			setterIL.Emit(OpCodes.Stfld, fieldBuilder);
			setterIL.Emit(OpCodes.Ret);
			propertyBuilder.SetSetMethod(setter);
		}

		// Create the type
		return typeBuilder.CreateType();
	}
}