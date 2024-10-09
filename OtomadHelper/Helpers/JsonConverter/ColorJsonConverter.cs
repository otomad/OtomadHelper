using System.Windows.Media;

namespace OtomadHelper.Helpers;

public class ColorJsonConverter : JsonConverter<Color> {
	public override Color Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) =>
		System.Drawing.ColorTranslator.FromHtml(reader.GetString()).ToMediaColor();

	public override void Write(Utf8JsonWriter writer, Color value, JsonSerializerOptions options) =>
		writer.WriteStringValue(value.ToHex());
}
