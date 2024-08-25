/// <summary>
/// <see href="https://github.com/arogozine/TupleAsJsonArray" />
/// <para>The nuget package doesn't support .NET Standard 2.0, but the code does.</para>
/// </summary>

using System.Runtime.CompilerServices;

namespace OtomadHelper.Helpers.TupleAsJsonArray;

/// <summary>
/// JSON Tuple to Array Converter Factory
/// </summary>
public class TupleConverterFactory : JsonConverterFactory {
	/// <summary>
	/// Determines whether the converter instance can convert the specified object type.
	/// </summary>
	/// <param name="typeToConvert">
	/// The type of the object to check whether it can be converted by this converter instance.
	/// </param>
	/// <returns>true if the instance can convert the specified object type; otherwise, false.</returns>
	public override bool CanConvert(Type typeToConvert) {
		if (typeToConvert.IsAbstract || !typeToConvert.IsGenericType)
			return false;
		return TupleReflector.TupleTypes.Contains(typeToConvert.GetGenericTypeDefinition());
	}

	/// <summary>
	/// Creates a converter for a specified type.
	/// </summary>
	/// <param name="typeToConvert">The type handled by the converter.</param>
	/// <param name="options">The serialization options to use.</param>
	/// <returns>A converter for which T is compatible with typeToConvert.</returns>
	public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options) {
		Type converterType = TupleReflector.GetTupleConverter(typeToConvert);
		return (JsonConverter)Activator.CreateInstance(converterType);
	}
}

internal static class TupleReflector {
	public static readonly HashSet<Type> TupleTypes = new(new Type[] {
			typeof(Tuple<>),
			typeof(Tuple<,>),
			typeof(Tuple<,,>),
			typeof(Tuple<,,,>),
			typeof(Tuple<,,,,>),
			typeof(Tuple<,,,,,>),
			typeof(Tuple<,,,,,,>),
			typeof(Tuple<,,,,,,,>),

			typeof(ValueTuple<>),
			typeof(ValueTuple<,>),
			typeof(ValueTuple<,,>),
			typeof(ValueTuple<,,,>),
			typeof(ValueTuple<,,,,>),
			typeof(ValueTuple<,,,,,>),
			typeof(ValueTuple<,,,,,,>),
			typeof(ValueTuple<,,,,,,,>)
	});
	public static Type GetTupleConverter(Type typeToConvert) {
		Type[] genericTupleArgs = typeToConvert.GetGenericArguments();

		if (typeToConvert.IsClass) {
			// Tuple
			return genericTupleArgs.Length switch {
				1 => typeof(TupleConverter<>).MakeGenericType(genericTupleArgs),
				2 => typeof(TupleConverter<,>).MakeGenericType(genericTupleArgs),
				3 => typeof(TupleConverter<,,>).MakeGenericType(genericTupleArgs),
				4 => typeof(TupleConverter<,,,>).MakeGenericType(genericTupleArgs),
				5 => typeof(TupleConverter<,,,,>).MakeGenericType(genericTupleArgs),
				6 => typeof(TupleConverter<,,,,,>).MakeGenericType(genericTupleArgs),
				7 => typeof(TupleConverter<,,,,,,>).MakeGenericType(genericTupleArgs),
				_ => throw new NotSupportedException(),
			};
		} else {
			// Value Tuple
			return genericTupleArgs.Length switch {
				1 => typeof(ValueTupleConverter<>).MakeGenericType(genericTupleArgs),
				2 => typeof(ValueTupleConverter<,>).MakeGenericType(genericTupleArgs),
				3 => typeof(ValueTupleConverter<,,>).MakeGenericType(genericTupleArgs),
				4 => typeof(ValueTupleConverter<,,,>).MakeGenericType(genericTupleArgs),
				5 => typeof(ValueTupleConverter<,,,,>).MakeGenericType(genericTupleArgs),
				6 => typeof(ValueTupleConverter<,,,,,>).MakeGenericType(genericTupleArgs),
				7 => typeof(ValueTupleConverter<,,,,,,>).MakeGenericType(genericTupleArgs),
				_ => throw new NotSupportedException(),
			};
		}
	}

	public static TRest GenerateTuple<TRest>(object[] values) {
		return (TRest)Activator.CreateInstance(typeof(TRest), values);
	}
}

/// <summary>
/// Tuple Converter Helper Base Class
/// </summary>
/// <typeparam name="TTuple">Class Tuple or Value Tuple</typeparam>
public abstract class TupleConverterBase<TTuple> : JsonConverter<TTuple>
	where TTuple : ITuple, IStructuralComparable, IStructuralEquatable, IComparable {
	/// <summary>
	/// Writes Value in the Tuple
	/// </summary>
	/// <typeparam name="T">Tuple Element Type</typeparam>
	/// <param name="writer">Writer</param>
	/// <param name="value">Tuple Value</param>
	/// <param name="options">Existing Options</param>
	protected void WriteValue<T>(Utf8JsonWriter writer, T value, JsonSerializerOptions options) {
		JsonConverter<T>? converter = (JsonConverter<T>)options.GetConverter(typeof(T));

		if (converter == null) {
			JsonSerializer.Serialize(writer, value, options);
		} else {
			converter.Write(writer, value, options);
		}
	}

	/// <summary>
	/// Read Value in the Array
	/// </summary>
	/// <typeparam name="T">Tuple Element Type</typeparam>
	/// <param name="reader">Reader</param>
	/// <param name="options">Existing Options</param>
	/// <returns>Deserialized Value</returns>
	protected T ReadValue<T>(ref Utf8JsonReader reader, JsonSerializerOptions options) {
		JsonConverter<T>? converter = (JsonConverter<T>)options.GetConverter(typeof(T));

		return converter == null ? JsonSerializer.Deserialize<T>(ref reader, options)! :
			converter.Read(ref reader, typeof(T), options)!;
	}
}

/// <summary>
/// 
/// </summary>
/// <typeparam name="T1">The type of the tuple's only component.</typeparam>
public class TupleConverter<T1> : TupleConverterBase<Tuple<T1>> {
	public override Tuple<T1> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
		if (reader.TokenType != JsonTokenType.StartArray) {
			throw new JsonException();
		}

		reader.Read();
		T1 value = ReadValue<T1>(ref reader, options);
		reader.Read(); // End of Array
		return new Tuple<T1>(value);
	}

	public override void Write(Utf8JsonWriter writer, Tuple<T1> value, JsonSerializerOptions options) {
		writer.WriteStartArray();
		WriteValue(writer, value.Item1, options);
		writer.WriteEndArray();
	}
}

/// <summary>
/// 
/// </summary>
/// <typeparam name="T1">The type of the tuple's first component.</typeparam>
/// <typeparam name="T2">The type of the tuple's second component.</typeparam>
public class TupleConverter<T1, T2> : TupleConverterBase<Tuple<T1, T2>> {
	public override Tuple<T1, T2> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
		T1 t1;
		T2 t2;

		if (reader.TokenType != JsonTokenType.StartArray) {
			throw new JsonException();
		}

		reader.Read();
		t1 = ReadValue<T1>(ref reader, options);

		reader.Read();
		t2 = ReadValue<T2>(ref reader, options);

		if (!reader.Read() || reader.TokenType != JsonTokenType.EndArray) {
			throw new JsonException();
		}

		return new Tuple<T1, T2>(t1, t2);
	}

	public override void Write(Utf8JsonWriter writer, Tuple<T1, T2> value, JsonSerializerOptions options) {
		writer.WriteStartArray();
		WriteValue(writer, value.Item1, options);
		WriteValue(writer, value.Item2, options);
		writer.WriteEndArray();
	}
}

/// <summary>
/// 
/// </summary>
/// <typeparam name="T1">The type of the tuple's first component.</typeparam>
/// <typeparam name="T2">The type of the tuple's second component.</typeparam>
/// <typeparam name="T3">The type of the tuple's third component.</typeparam>
public class TupleConverter<T1, T2, T3> : TupleConverterBase<Tuple<T1, T2, T3>> {
	public override Tuple<T1, T2, T3> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
		T1 t1;
		T2 t2;
		T3 t3;

		if (reader.TokenType != JsonTokenType.StartArray) {
			throw new JsonException();
		}

		reader.Read();
		t1 = ReadValue<T1>(ref reader, options);

		reader.Read();
		t2 = ReadValue<T2>(ref reader, options);

		reader.Read();
		t3 = ReadValue<T3>(ref reader, options);

		if (!reader.Read() || reader.TokenType != JsonTokenType.EndArray) {
			throw new JsonException();
		}

		return new Tuple<T1, T2, T3>(t1, t2, t3);
	}

	public override void Write(Utf8JsonWriter writer, Tuple<T1, T2, T3> value, JsonSerializerOptions options) {
		writer.WriteStartArray();
		WriteValue(writer, value.Item1, options);
		WriteValue(writer, value.Item2, options);
		WriteValue(writer, value.Item3, options);
		writer.WriteEndArray();
	}
}

/// <summary>
/// 
/// </summary>
/// <typeparam name="T1">The type of the tuple's first component.</typeparam>
/// <typeparam name="T2">The type of the tuple's second component.</typeparam>
/// <typeparam name="T3">The type of the tuple's third component.</typeparam>
/// <typeparam name="T4">The type of the tuple's fourth component.</typeparam>
public class TupleConverter<T1, T2, T3, T4> : TupleConverterBase<Tuple<T1, T2, T3, T4>> {
	public override Tuple<T1, T2, T3, T4> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
		T1 t1;
		T2 t2;
		T3 t3;
		T4 t4;

		if (reader.TokenType != JsonTokenType.StartArray) {
			throw new JsonException();
		}

		reader.Read();
		t1 = ReadValue<T1>(ref reader, options);

		reader.Read();
		t2 = ReadValue<T2>(ref reader, options);

		reader.Read();
		t3 = ReadValue<T3>(ref reader, options);

		reader.Read();
		t4 = ReadValue<T4>(ref reader, options);

		if (!reader.Read() || reader.TokenType != JsonTokenType.EndArray) {
			throw new JsonException();
		}

		return new Tuple<T1, T2, T3, T4>(t1, t2, t3, t4);
	}

	public override void Write(Utf8JsonWriter writer, Tuple<T1, T2, T3, T4> value, JsonSerializerOptions options) {
		writer.WriteStartArray();
		WriteValue(writer, value.Item1, options);
		WriteValue(writer, value.Item2, options);
		WriteValue(writer, value.Item3, options);
		WriteValue(writer, value.Item4, options);
		writer.WriteEndArray();
	}
}

/// <summary>
/// 
/// </summary>
/// <typeparam name="T1">The type of the tuple's first component.</typeparam>
/// <typeparam name="T2">The type of the tuple's second component.</typeparam>
/// <typeparam name="T3">The type of the tuple's third component.</typeparam>
/// <typeparam name="T4">The type of the tuple's fourth component.</typeparam>
/// <typeparam name="T5">The type of the tuple's fifth component.</typeparam>
public class TupleConverter<T1, T2, T3, T4, T5> : TupleConverterBase<Tuple<T1, T2, T3, T4, T5>> {
	public override Tuple<T1, T2, T3, T4, T5> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
		T1 t1;
		T2 t2;
		T3 t3;
		T4 t4;
		T5 t5;

		if (reader.TokenType != JsonTokenType.StartArray) {
			throw new JsonException();
		}

		reader.Read();
		t1 = ReadValue<T1>(ref reader, options);

		reader.Read();
		t2 = ReadValue<T2>(ref reader, options);

		reader.Read();
		t3 = ReadValue<T3>(ref reader, options);

		reader.Read();
		t4 = ReadValue<T4>(ref reader, options);

		reader.Read();
		t5 = ReadValue<T5>(ref reader, options);

		if (!reader.Read() || reader.TokenType != JsonTokenType.EndArray) {
			throw new JsonException();
		}

		return new Tuple<T1, T2, T3, T4, T5>(t1, t2, t3, t4, t5);
	}

	public override void Write(Utf8JsonWriter writer, Tuple<T1, T2, T3, T4, T5> value, JsonSerializerOptions options) {
		writer.WriteStartArray();
		WriteValue(writer, value.Item1, options);
		WriteValue(writer, value.Item2, options);
		WriteValue(writer, value.Item3, options);
		WriteValue(writer, value.Item4, options);
		WriteValue(writer, value.Item5, options);
		writer.WriteEndArray();
	}
}

/// <summary>
/// 
/// </summary>
/// <typeparam name="T1">The type of the tuple's first component.</typeparam>
/// <typeparam name="T2">The type of the tuple's second component.</typeparam>
/// <typeparam name="T3">The type of the tuple's third component.</typeparam>
/// <typeparam name="T4">The type of the tuple's fourth component.</typeparam>
/// <typeparam name="T5">The type of the tuple's fifth component.</typeparam>
/// <typeparam name="T6">The type of the tuple's sixth component.</typeparam>
public class TupleConverter<T1, T2, T3, T4, T5, T6> : TupleConverterBase<Tuple<T1, T2, T3, T4, T5, T6>> {
	public override Tuple<T1, T2, T3, T4, T5, T6> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
		T1 t1;
		T2 t2;
		T3 t3;
		T4 t4;
		T5 t5;
		T6 t6;

		if (reader.TokenType != JsonTokenType.StartArray) {
			throw new JsonException();
		}

		reader.Read();
		t1 = ReadValue<T1>(ref reader, options);

		reader.Read();
		t2 = ReadValue<T2>(ref reader, options);

		reader.Read();
		t3 = ReadValue<T3>(ref reader, options);

		reader.Read();
		t4 = ReadValue<T4>(ref reader, options);

		reader.Read();
		t5 = ReadValue<T5>(ref reader, options);

		reader.Read();
		t6 = ReadValue<T6>(ref reader, options);

		if (!reader.Read() || reader.TokenType != JsonTokenType.EndArray) {
			throw new JsonException();
		}

		return new Tuple<T1, T2, T3, T4, T5, T6>(t1, t2, t3, t4, t5, t6);
	}

	public override void Write(Utf8JsonWriter writer, Tuple<T1, T2, T3, T4, T5, T6> value, JsonSerializerOptions options) {
		writer.WriteStartArray();
		WriteValue(writer, value.Item1, options);
		WriteValue(writer, value.Item2, options);
		WriteValue(writer, value.Item3, options);
		WriteValue(writer, value.Item4, options);
		WriteValue(writer, value.Item5, options);
		WriteValue(writer, value.Item6, options);
		writer.WriteEndArray();
	}
}

/// <summary>
/// 
/// </summary>
/// <typeparam name="T1">The type of the tuple's first component.</typeparam>
/// <typeparam name="T2">The type of the tuple's second component.</typeparam>
/// <typeparam name="T3">The type of the tuple's third component.</typeparam>
/// <typeparam name="T4">The type of the tuple's fourth component.</typeparam>
/// <typeparam name="T5">The type of the tuple's fifth component.</typeparam>
/// <typeparam name="T6">The type of the tuple's sixth component.</typeparam>
/// <typeparam name="T7">The type of the tuple's seventh component.</typeparam>
public class TupleConverter<T1, T2, T3, T4, T5, T6, T7> : TupleConverterBase<Tuple<T1, T2, T3, T4, T5, T6, T7>> {
	public override Tuple<T1, T2, T3, T4, T5, T6, T7> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
		T1 t1;
		T2 t2;
		T3 t3;
		T4 t4;
		T5 t5;
		T6 t6;
		T7 t7;

		if (reader.TokenType != JsonTokenType.StartArray) {
			throw new JsonException();
		}

		reader.Read();
		t1 = ReadValue<T1>(ref reader, options);

		reader.Read();
		t2 = ReadValue<T2>(ref reader, options);

		reader.Read();
		t3 = ReadValue<T3>(ref reader, options);

		reader.Read();
		t4 = ReadValue<T4>(ref reader, options);

		reader.Read();
		t5 = ReadValue<T5>(ref reader, options);

		reader.Read();
		t6 = ReadValue<T6>(ref reader, options);

		reader.Read();
		t7 = ReadValue<T7>(ref reader, options);

		if (!reader.Read() || reader.TokenType != JsonTokenType.EndArray) {
			throw new JsonException();
		}

		return new Tuple<T1, T2, T3, T4, T5, T6, T7>(t1, t2, t3, t4, t5, t6, t7);
	}

	public override void Write(Utf8JsonWriter writer, Tuple<T1, T2, T3, T4, T5, T6, T7> value, JsonSerializerOptions options) {
		writer.WriteStartArray();
		WriteValue(writer, value.Item1, options);
		WriteValue(writer, value.Item2, options);
		WriteValue(writer, value.Item3, options);
		WriteValue(writer, value.Item4, options);
		WriteValue(writer, value.Item5, options);
		WriteValue(writer, value.Item6, options);
		WriteValue(writer, value.Item7, options);
		writer.WriteEndArray();
	}
}

/// <summary>
/// 
/// </summary>
/// <typeparam name="T1">The type of the value tuple's only element.</typeparam>
public class ValueTupleConverter<T1> : TupleConverterBase<ValueTuple<T1>> {
	public override ValueTuple<T1> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
		if (reader.TokenType != JsonTokenType.StartArray) {
			throw new JsonException();
		}

		reader.Read();
		T1 value = ReadValue<T1>(ref reader, options);

		reader.Read(); // End of Array
		return new ValueTuple<T1>(value);
	}

	public override void Write(Utf8JsonWriter writer, ValueTuple<T1> value, JsonSerializerOptions options) {
		writer.WriteStartArray();
		WriteValue(writer, value.Item1, options);
		writer.WriteEndArray();
	}
}

/// <summary>
/// 
/// </summary>
/// <typeparam name="T1">The type of the value tuple's first element.</typeparam>
/// <typeparam name="T2">The type of the value tuple's second element.</typeparam>
public class ValueTupleConverter<T1, T2> : TupleConverterBase<ValueTuple<T1, T2>> {
	public override ValueTuple<T1, T2> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
		T1 t1;
		T2 t2;

		if (reader.TokenType != JsonTokenType.StartArray) {
			throw new JsonException();
		}

		reader.Read();
		t1 = ReadValue<T1>(ref reader, options);

		reader.Read();
		t2 = ReadValue<T2>(ref reader, options);

		if (!reader.Read() || reader.TokenType != JsonTokenType.EndArray) {
			throw new JsonException();
		}

		return (t1, t2);
	}

	public override void Write(Utf8JsonWriter writer, ValueTuple<T1, T2> value, JsonSerializerOptions options) {
		writer.WriteStartArray();
		WriteValue(writer, value.Item1, options);
		WriteValue(writer, value.Item2, options);
		writer.WriteEndArray();
	}
}

/// <summary>
/// 
/// </summary>
/// <typeparam name="T1">The type of the value tuple's first element.</typeparam>
/// <typeparam name="T2">The type of the value tuple's second element.</typeparam>
/// <typeparam name="T3">The type of the value tuple's third element.</typeparam>
public class ValueTupleConverter<T1, T2, T3> : TupleConverterBase<ValueTuple<T1, T2, T3>> {
	public override ValueTuple<T1, T2, T3> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
		T1 t1;
		T2 t2;
		T3 t3;

		if (reader.TokenType != JsonTokenType.StartArray) {
			throw new JsonException();
		}

		reader.Read();
		t1 = ReadValue<T1>(ref reader, options);

		reader.Read();
		t2 = ReadValue<T2>(ref reader, options);

		reader.Read();
		t3 = ReadValue<T3>(ref reader, options);

		if (!reader.Read() || reader.TokenType != JsonTokenType.EndArray) {
			throw new JsonException();
		}

		return (t1, t2, t3);
	}

	public override void Write(Utf8JsonWriter writer, ValueTuple<T1, T2, T3> value, JsonSerializerOptions options) {
		writer.WriteStartArray();
		WriteValue(writer, value.Item1, options);
		WriteValue(writer, value.Item2, options);
		WriteValue(writer, value.Item3, options);
		writer.WriteEndArray();
	}
}

/// <summary>
/// 
/// </summary>
/// <typeparam name="T1">The type of the value tuple's first element.</typeparam>
/// <typeparam name="T2">The type of the value tuple's second element.</typeparam>
/// <typeparam name="T3">The type of the value tuple's third element.</typeparam>
/// <typeparam name="T4">The type of the value tuple's fourth element.</typeparam>
public class ValueTupleConverter<T1, T2, T3, T4> : TupleConverterBase<ValueTuple<T1, T2, T3, T4>> {
	public override ValueTuple<T1, T2, T3, T4> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
		T1 t1;
		T2 t2;
		T3 t3;
		T4 t4;

		if (reader.TokenType != JsonTokenType.StartArray) {
			throw new JsonException();
		}

		reader.Read();
		t1 = ReadValue<T1>(ref reader, options);

		reader.Read();
		t2 = ReadValue<T2>(ref reader, options);

		reader.Read();
		t3 = ReadValue<T3>(ref reader, options);

		reader.Read();
		t4 = ReadValue<T4>(ref reader, options);

		if (!reader.Read() || reader.TokenType != JsonTokenType.EndArray) {
			throw new JsonException();
		}

		return (t1, t2, t3, t4);
	}

	public override void Write(Utf8JsonWriter writer, ValueTuple<T1, T2, T3, T4> value, JsonSerializerOptions options) {
		writer.WriteStartArray();
		WriteValue(writer, value.Item1, options);
		WriteValue(writer, value.Item2, options);
		WriteValue(writer, value.Item3, options);
		WriteValue(writer, value.Item4, options);
		writer.WriteEndArray();
	}
}

/// <summary>
/// 
/// </summary>
/// <typeparam name="T1">The type of the value tuple's first element.</typeparam>
/// <typeparam name="T2">The type of the value tuple's second element.</typeparam>
/// <typeparam name="T3">The type of the value tuple's third element.</typeparam>
/// <typeparam name="T4">The type of the value tuple's fourth element.</typeparam>
/// <typeparam name="T5">The type of the value tuple's fifth element.</typeparam>
public class ValueTupleConverter<T1, T2, T3, T4, T5> : TupleConverterBase<ValueTuple<T1, T2, T3, T4, T5>> {
	public override ValueTuple<T1, T2, T3, T4, T5> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
		T1 t1;
		T2 t2;
		T3 t3;
		T4 t4;
		T5 t5;

		if (reader.TokenType != JsonTokenType.StartArray) {
			throw new JsonException();
		}

		reader.Read();
		t1 = ReadValue<T1>(ref reader, options);

		reader.Read();
		t2 = ReadValue<T2>(ref reader, options);

		reader.Read();
		t3 = ReadValue<T3>(ref reader, options);

		reader.Read();
		t4 = ReadValue<T4>(ref reader, options);

		reader.Read();
		t5 = ReadValue<T5>(ref reader, options);

		if (!reader.Read() || reader.TokenType != JsonTokenType.EndArray) {
			throw new JsonException();
		}

		return (t1, t2, t3, t4, t5);
	}

	public override void Write(Utf8JsonWriter writer, ValueTuple<T1, T2, T3, T4, T5> value, JsonSerializerOptions options) {
		writer.WriteStartArray();
		WriteValue(writer, value.Item1, options);
		WriteValue(writer, value.Item2, options);
		WriteValue(writer, value.Item3, options);
		WriteValue(writer, value.Item4, options);
		WriteValue(writer, value.Item5, options);
		writer.WriteEndArray();
	}
}

/// <summary>
/// 
/// </summary>
/// <typeparam name="T1">The type of the value tuple's first element.</typeparam>
/// <typeparam name="T2">The type of the value tuple's second element.</typeparam>
/// <typeparam name="T3">The type of the value tuple's third element.</typeparam>
/// <typeparam name="T4">The type of the value tuple's fourth element.</typeparam>
/// <typeparam name="T5">The type of the value tuple's fifth element.</typeparam>
/// <typeparam name="T6">The type of the value tuple's sixth element.</typeparam>
public class ValueTupleConverter<T1, T2, T3, T4, T5, T6> : TupleConverterBase<ValueTuple<T1, T2, T3, T4, T5, T6>> {
	public override ValueTuple<T1, T2, T3, T4, T5, T6> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
		T1 t1;
		T2 t2;
		T3 t3;
		T4 t4;
		T5 t5;
		T6 t6;

		if (reader.TokenType != JsonTokenType.StartArray) {
			throw new JsonException();
		}

		reader.Read();
		t1 = ReadValue<T1>(ref reader, options);

		reader.Read();
		t2 = ReadValue<T2>(ref reader, options);

		reader.Read();
		t3 = ReadValue<T3>(ref reader, options);

		reader.Read();
		t4 = ReadValue<T4>(ref reader, options);

		reader.Read();
		t5 = ReadValue<T5>(ref reader, options);

		reader.Read();
		t6 = ReadValue<T6>(ref reader, options);

		if (!reader.Read() || reader.TokenType != JsonTokenType.EndArray) {
			throw new JsonException();
		}

		return (t1, t2, t3, t4, t5, t6);
	}

	public override void Write(Utf8JsonWriter writer, ValueTuple<T1, T2, T3, T4, T5, T6> value, JsonSerializerOptions options) {
		writer.WriteStartArray();
		WriteValue(writer, value.Item1, options);
		WriteValue(writer, value.Item2, options);
		WriteValue(writer, value.Item3, options);
		WriteValue(writer, value.Item4, options);
		WriteValue(writer, value.Item5, options);
		WriteValue(writer, value.Item6, options);
		writer.WriteEndArray();
	}
}

/// <summary>
/// 
/// </summary>
/// <typeparam name="T1">The type of the value tuple's first element.</typeparam>
/// <typeparam name="T2">The type of the value tuple's second element.</typeparam>
/// <typeparam name="T3">The type of the value tuple's third element.</typeparam>
/// <typeparam name="T4">The type of the value tuple's fourth element.</typeparam>
/// <typeparam name="T5">The type of the value tuple's fifth element.</typeparam>
/// <typeparam name="T6">The type of the value tuple's sixth element.</typeparam>
/// <typeparam name="T7">The type of the value tuple's seventh element.</typeparam>
public class ValueTupleConverter<T1, T2, T3, T4, T5, T6, T7> : TupleConverterBase<ValueTuple<T1, T2, T3, T4, T5, T6, T7>> {
	public override ValueTuple<T1, T2, T3, T4, T5, T6, T7> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
		T1 t1;
		T2 t2;
		T3 t3;
		T4 t4;
		T5 t5;
		T6 t6;
		T7 t7;

		if (reader.TokenType != JsonTokenType.StartArray) {
			throw new JsonException();
		}

		reader.Read();
		t1 = ReadValue<T1>(ref reader, options);

		reader.Read();
		t2 = ReadValue<T2>(ref reader, options);

		reader.Read();
		t3 = ReadValue<T3>(ref reader, options);

		reader.Read();
		t4 = ReadValue<T4>(ref reader, options);

		reader.Read();
		t5 = ReadValue<T5>(ref reader, options);

		reader.Read();
		t6 = ReadValue<T6>(ref reader, options);

		reader.Read();
		t7 = ReadValue<T7>(ref reader, options);

		if (!reader.Read() || reader.TokenType != JsonTokenType.EndArray) {
			throw new JsonException();
		}

		return (t1, t2, t3, t4, t5, t6, t7);
	}

	public override void Write(Utf8JsonWriter writer, ValueTuple<T1, T2, T3, T4, T5, T6, T7> value, JsonSerializerOptions options) {
		writer.WriteStartArray();
		WriteValue(writer, value.Item1, options);
		WriteValue(writer, value.Item2, options);
		WriteValue(writer, value.Item3, options);
		WriteValue(writer, value.Item4, options);
		WriteValue(writer, value.Item5, options);
		WriteValue(writer, value.Item6, options);
		WriteValue(writer, value.Item7, options);
		writer.WriteEndArray();
	}
}
