using UnityEngine;
using System.Collections.Generic;
using System;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

public class VRPNInputManager
{
    public Dictionary<string, string> ButtonMap = new Dictionary<string, string>();
    public Dictionary<string, string> AnalogMap = new Dictionary<string, string>();
    public Dictionary<string, string> SixdofMap = new Dictionary<string, string>();

    public Dictionary<string, bool> ButtonValues = new Dictionary<string, bool>();
    public Dictionary<string, float> AnalogValues = new Dictionary<string, float>();
    public Dictionary<string, Sixdof> SixdofValues = new Dictionary<string, Sixdof>();

    private Config config;
	private NetworkManager networkManager;

	private InputData data;
    // Use this for initialization
	public void Start(Config c) {

        //data = new InputData();

        //ButtonMap.Add("clear", "WiiMote0[0]"); // Home button
        //ButtonMap.Add("paint", "WiiMote0[3]"); // A button
        //ButtonMap.Add("rotate", "WiiMote0[16]"); // nunchuck-z button
        ButtonMap.Add("A_button", "WiiMote0[3]"); // A button
        ButtonMap.Add("boost", "WiiMote0[17]"); // nunchuck-c button
        ButtonMap.Add("reset", "WiiMote0[0]"); // Home button
        ButtonMap.Add("B_button", "WiiMote0[4]"); // B button
        ButtonMap.Add("left", "WiiMote0[7]"); // Left Analog
        ButtonMap.Add("right", "WiiMote0[8]"); // Right Analog
        ButtonMap.Add("up", "WiiMote0[10]"); // Up Analog
        ButtonMap.Add("down", "WiiMote0[9]"); // Down Analog
        ButtonMap.Add("jump", "WiiMote0[16]"); // nunchuck-z button
		ButtonMap.Add("increase", "WiiMote0[6]"); // + button
		ButtonMap.Add("decrease", "WiiMote0[5]"); // - button

        AnalogMap.Add("x", "WiiMote0[22]"); // nunchuck-x
        AnalogMap.Add("y", "WiiMote0[21]"); // nunchuck-y

        SixdofMap.Add("wand", "WiiMote0[0]"); // WiiMote0
        //SixdofMap.Add("view", "ShortGlasses[0]"); // short glasses

        //AnalogMap.Add("accelerator", "WiiMote0[69]");

        //SixdofMap.Add("wheel", "WiiMote0[0]");

        foreach(var button in ButtonMap.Values) {
            ButtonValues.Add(button, false);
        }

        foreach(var analog in AnalogMap.Values) {
            AnalogValues.Add(analog, 0.0f);
        }

        foreach(var sixdof in SixdofMap.Values) {
            SixdofValues.Add(sixdof, new Sixdof());
        }

		config = c;
		//networkManager = new NetworkManager();
		//networkManager.Start(config);
    }

    // Update is called once per frame
    public void Update() {
        string device;
        int channel;

		if(config.IsMaster) {
			foreach(var button in ButtonMap.Values) {
				GetDeviceAndChannel(button, out device, out channel);
				ButtonValues[button] = config.GetButtonValue(device, channel);
			}

			foreach(var analog in AnalogMap.Values) {
				GetDeviceAndChannel(analog, out device, out channel);
				AnalogValues[analog] = config.GetAnalogValue(device, channel);
			}

			foreach(var sixdof in SixdofMap.Values) {
				GetDeviceAndChannel(sixdof, out device, out channel);
				SixdofValues[sixdof] = config.GetSixdofValue(device, channel);
			}
		}
    }

	public void Sync() {
//		if(config.IsMaster) {
//			for(int i = 0; i < config.NumMachines - 1; i++) {
//				var packet = networkManager.SyncSocket.Recv();
//
//				if(packet != string.Empty) {
//					networkManager.SyncSocket.Send(packet);
//				}
//			}
//
//			var dataPacket = data.SerializeObject();
//			networkManager.PubsubSocket.Send(dataPacket);
//		} 
//		else {
//			var packet = " ";
//			networkManager.SyncSocket.Send(packet);
//			packet = networkManager.SyncSocket.Recv();
//		}

		if(config.IsMaster) {
			var data = new InputData {
				buttonValues = ButtonValues,
				analogValues = AnalogValues,
				sixdofValues = SixdofValues,
				terminated = CaVR.Terminated
			};

			var serialized = data.SerializeObject();
			networkManager.PubsubSocket.Send(serialized);
		} 
		else {
			var packet = networkManager.PubsubSocket.Recv();
			var data = SocketExtensions.DeserializeObject<InputData>(packet);

			ButtonValues = data.buttonValues;
			AnalogValues = data.analogValues;
			SixdofValues = data.sixdofValues;
		}
	}

	void OnDestroy() {
		networkManager.Destroy();
		networkManager = null;
	}

    public bool GetButtonValue(string name) {
        return ButtonValues[ButtonMap[name]];
    }

    public float GetAnalogValue(string name) {
        return AnalogValues[AnalogMap[name]];
    }

    public Sixdof GetSixdofValue(string name) {
        return SixdofValues[SixdofMap[name]];
    }

    private static void GetDeviceAndChannel(string addr, out string device, out int channel) {
        var split = addr.Split('[');
        device = split[0].Replace("]", string.Empty);
        var channelStr = split[1].Replace("]", string.Empty);
        channel = int.Parse(channelStr);
    }

	[Serializable]
	public struct InputData {
		
		//[SerializableAttribute]
		public Dictionary<string, bool> buttonValues;

		//[SerializableAttribute]
		public Dictionary<string, float> analogValues;

		//[SerializableAttribute]
		public Dictionary<string, Sixdof> sixdofValues;

		//[SerializableAttribute]
		public bool terminated;
	}
}
