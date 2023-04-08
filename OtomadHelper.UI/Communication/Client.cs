using System;
using System.IO;
using System.IO.Pipes;
using System.Text;
using System.Threading;

namespace OtomadHelper.UI.Communication;
internal class PipeClient {
	private readonly Thread thread;
	public string send = "";
	private readonly object lockObject = new();

	internal PipeClient() {
		thread = new Thread(StartClient);
		Start();
	}

	private void StartClient() {
		try {
			lock (lockObject) {
				using NamedPipeClientStream pipeClient = new(".", "OtomadHelper", PipeDirection.InOut);
				pipeClient.Connect();
				using StreamReader streamReader = new(pipeClient);
				while (true) {
					string received = streamReader.ReadLine();
					if (received != null)
						ClientReceived(received);
					if (!string.IsNullOrEmpty(send)) {
						byte[] data = Encoding.UTF8.GetBytes(send);
						send = "";
						pipeClient.Write(data, 0, data.Length);
					}
				}
			}
		} catch (ThreadInterruptedException) { }
	}

	public delegate void ClientReceivedType(string text);
	public event ClientReceivedType ClientReceived;

	public void Start() {
		thread.Start();
	}

	public void Close() {
		thread.Interrupt();
	}
}
