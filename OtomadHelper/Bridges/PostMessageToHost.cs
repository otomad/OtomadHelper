using System.Text.Json.Nodes;

namespace OtomadHelper.Bridges;

public class PostMessageToHost {
	public delegate void ReceivedEventHandler(JsonNode node);
	public event ReceivedEventHandler? Received;

	public PostMessageToHost() {
		Received += OnReceived;
	}

	public void PostMessage(string jsonString) {
		JsonNode? node = JsonNode.Parse(jsonString);
		if (node is null) return;
		Received?.Invoke(node);
	}

	private void OnReceived(JsonNode node) {
		
	}
}
