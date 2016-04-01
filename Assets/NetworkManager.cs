using System;
using System.Threading;

using NetMQ;
using NetMQ.Sockets;

public class NetworkManager {

	public NetMQSocket SyncSocket { get; private set; }
	public NetMQSocket PubsubSocket { get; private set; }

	private Config config;
	// Use this for initialization
	public void Start(Config c) {
        AsyncIO.ForceDotNet.Force();

		config = c;

		var masterKey = string.Empty;
	    foreach(var arg in Environment.GetCommandLineArgs()) {
			if(arg.Contains("--cavr_master=")) {
				masterKey = arg.Split('=')[1];
                break;
            }
        }
			
			
		if(config.IsMaster) {
			var host = config.GetLuaStateValue("machines.master.address").ToString();
			SyncSocket = new ResponseSocket(host);
			PubsubSocket = new PublisherSocket(host);
        }

        else {
			SyncSocket = new RequestSocket(masterKey);
			PubsubSocket = new SubscriberSocket(masterKey);

			(PubsubSocket as SubscriberSocket).Subscribe("pubsub");
        }
	}
	
	// Update is called once per frame
	public void Update() {
	
	}

	public void Destroy() {
		SyncSocket.Dispose();
		PubsubSocket.Dispose();
		SyncSocket = null;
		PubsubSocket = null;
	}
}
