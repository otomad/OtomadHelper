namespace OtomadHelper.Bridges;

public class WebMessageAcknowledgement {
	public void Ack(string json) =>
		Received?.Invoke(json);

	public delegate void ReceivedEventHandler(string json);
	public event ReceivedEventHandler? Received;
}
