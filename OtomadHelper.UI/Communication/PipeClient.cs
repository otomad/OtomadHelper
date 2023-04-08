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
	private readonly NamedPipeClientStream pipeClient = new(".", "OtomadHelper", PipeDirection.InOut);

	internal PipeClient() {
		thread = new Thread(StartClient);
		Start();
	}

	private void StartClient() {
		try {
			lock (lockObject) {
				ClientReceived("Wait for connection");
				if (!pipeClient.IsConnected) pipeClient.Connect();
				using StreamReader reader = new(pipeClient, Encoding.UTF8);
				using StreamWriter writer = new(pipeClient, Encoding.UTF8);
				writer.AutoFlush = true;
				ClientReceived("Connected");
				while (pipeClient.IsConnected) {
					string received = reader.ReadLine();
					if (received != null) ClientReceived(received);
					/*if (!string.IsNullOrEmpty(send)) {
						writer.WriteLine(send);
						send = "";
					}
					Thread.Sleep(100);*/
				}
			}
		} catch (ThreadInterruptedException) { }
		catch (ObjectDisposedException) { }
		catch (IOException) { ClientReceived("Disconnected"); }
	}

	public void Send(string text) {
		byte[] data = Encoding.UTF8.GetBytes(text);
		pipeClient.Write(data, 0, data.Length);
	}

	public delegate void ClientReceivedType(string text);
	public event ClientReceivedType ClientReceived;

	public void Start() {
		thread.Start();
	}

	public void Close() {
		thread.Interrupt();
		pipeClient.Close();
	}
}
