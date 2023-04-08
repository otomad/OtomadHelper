using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OtomadHelper.Core.Communication {
	internal class PipeServer {
		private readonly Thread thread;
		public string send = "";
		private readonly object lockObject = new object();
		private NamedPipeServerStream pipeServer = new NamedPipeServerStream("OtomadHelper", PipeDirection.InOut, 1, PipeTransmissionMode.Message, PipeOptions.Asynchronous);

		internal PipeServer() {
			thread = new Thread(StartServer);
			Start();
		}

		private void StartServer() {
			ServerReceived("Wait for connection");
			try {
				pipeServer.WaitForConnection();
			} catch (IOException) { return; }
			ServerReceived("Connected");
			using (StreamReader reader = new StreamReader(pipeServer, Encoding.UTF8)) {
				using (StreamWriter writer = new StreamWriter(pipeServer, Encoding.UTF8)) {
					ServerReceived("Wait for pipe drain");
					while (true) {
						pipeServer.WaitForPipeDrain();
						string received = reader.ReadLine();
						if (received != null)
							ServerReceived(received);
						if (!string.IsNullOrEmpty(send)) {
							writer.WriteLine(send);
							writer.Flush();
							send = "";
						}
					}
				}
			}
		}

		public delegate void ServerReceivedType(string text);
		public event ServerReceivedType ServerReceived;

		public void Start() {
			thread.Start();
		}

		public void Close() {
			pipeServer.Close();
			pipeServer.Dispose();
		}
	}
}
