using UnityEngine;
using System.Text;
using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

using NetMQ;
using NetMQ.Sockets;

public static class SocketExtensions {
	public static string Recv(this NetMQSocket socket) {
		StringBuilder sb = new StringBuilder();
		bool more = true;
		while(more) {
			var frame = socket.ReceiveFrameString(out more);
			sb.Append(frame);
		}

		return sb.ToString();
	}

	public static void Send(this NetMQSocket socket, string packet) {
		if(socket is PublisherSocket) {
			socket.SendMoreFrame("pubsub").SendFrame(packet);
		} 
		else {
			socket.SendFrame(packet);
		}
	}

	public static string SerializeObject<T>(this T obj) {
		var binaryFormatter = new BinaryFormatter();

		using(var mem = new MemoryStream()) {
			try {
				binaryFormatter.Serialize(mem, obj);
				mem.Flush();
				mem.Seek(0, SeekOrigin.Begin);
				using(var reader = new StreamReader(mem)) {
					return reader.ReadToEnd();
				}
			}
			catch(SerializationException e) {
				Debug.LogError("Failed to serialize object: " + e.Message);
				return string.Empty;
			}
		}
	}

	public static T DeserializeObject<T>(string data) {
		var binaryFormatter = new BinaryFormatter();

		using(var mem = new MemoryStream())
		using(var writer = new StreamWriter(mem)) {
			writer.Write(data);
			writer.Flush();
			mem.Seek(0, SeekOrigin.Begin);

			try {
				return (T)binaryFormatter.Deserialize(mem);
			} 
			catch(SerializationException e) {
				Debug.LogError("Failed to deserialize object: " + e.Message);
				return default(T);
			} 
			catch(Exception e) {
				Debug.LogError("General Exception when deserializing object: " + e.Message);
				return default(T);
			}
		}
	}
}