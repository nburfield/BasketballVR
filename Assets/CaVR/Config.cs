using UnityEngine;
using System.Collections.Generic;
using System.Linq;

using NLua;

public delegate bool ButtonCallback(int channel);
public delegate float AnalogCallback(int channel);
public delegate Sixdof SixdofCallback(int channel);

public class Config
{
    private Lua state = new Lua();

    private Dictionary<string, ButtonCallback> buttonCallbacks = new Dictionary<string, ButtonCallback>();
    private Dictionary<string, AnalogCallback> analogCallbacks = new Dictionary<string, AnalogCallback>();
    private Dictionary<string, SixdofCallback> sixdofCallbacks = new Dictionary<string, SixdofCallback>();

    private Dictionary<string, LuaTable> machines = new Dictionary<string, LuaTable>();

	public bool IsMaster { get; private set; }
	public int NumMachines { get; private set; }

    public void Init() {
        state.DoString(string.Format("HOSTNAME = '{0}'", System.Environment.MachineName));

        Debug.Log(string.Format("HOSTNAME = '{0}'", state["HOSTNAME"]));

        //System.IO.File.WriteAllText("log.txt", string.Format("{0}  {1}  {2}  {3}", Application.dataPath, Application.persistentDataPath, Application.streamingAssetsPath, Application.temporaryCachePath));
        state.DoFile(Application.streamingAssetsPath + "/example.lua");

		// machines
		var luaMachines = state["machines"] as LuaTable;
		NumMachines = luaMachines.Values.Count;
		var master = luaMachines["master"] as LuaTable;

		Debug.Log(string.Format("Machines: {0}  Master: {1}", luaMachines, master));

		IsMaster = System.Convert.ToString(master["hostname"]).Contains(System.Environment.MachineName);

		if(IsMaster) {
			var plugins = master["plugins"] as LuaTable;
			var vrpnEnabled = plugins["vrpn"] != null;

			if(vrpnEnabled) {
				var vrpn = state["vrpn"] as LuaTable;

				//System.IO.File.WriteAllText("log2.txt", string.Format("{0}  {1}", State["HOSTNAME"], vrpn));

				var buttons = vrpn["buttons"] as LuaTable;
				var analogs = vrpn["analogs"] as LuaTable;
				var sixdofs = vrpn["sixdofs"] as LuaTable;

				if(buttons != null) {
					foreach(var b in buttons.Values) {
						var button = b.ToString();
						Debug.Log("Button: " + button);
						var device = button.Split('@')[0];

						ButtonCallback callback = (int channel) => {
							return VRPN.vrpnButton(button, channel);
						};

						buttonCallbacks.Add(device, callback);
					}
				}

				if(analogs != null) {
					foreach(var a in analogs.Values) {
						var analog = a.ToString();
						Debug.Log("Analog: " + analog);

						var device = analog.Split('@')[0];

						AnalogCallback callback = (int channel) => {
							return (float)VRPN.vrpnAnalog(analog, channel);
						};

						analogCallbacks.Add(device, callback);
					}
				}

				if(sixdofs != null) {
					foreach(var s in sixdofs.Values) {
						var sixdof = s.ToString();
						Debug.Log("Sixdof: " + sixdof);

						var device = sixdof.Split('@')[0];

						SixdofCallback callback = (int channel) => {
							var pos = VRPN.vrpnTrackerPos(sixdof, channel);
							var rot = VRPN.vrpnTrackerQuat(sixdof, channel);

							pos.z *= -1;

							return new Sixdof {
								Position = pos,//new Vector3(pos.x, pos.y, -pos.z),
								Rotation = Quaternion.Inverse(rot),//new Quaternion(-rot.x, rot.z, rot.y, rot.w)
							};
						};

						sixdofCallbacks.Add(device, callback);
					}
				}
			}
		}

        foreach(string machine in luaMachines.Keys) {
            machines[machine] = luaMachines[machine] as LuaTable;
        }
    }

	public object GetLuaStateValue(string key) {
		return state[key];
	}

    public bool GetButtonValue(string device, int channel) {
		try {
        	return buttonCallbacks[device](channel);
		}
		catch(KeyNotFoundException) {
			Debug.LogError(string.Format("No Button Device Named {0}", device));
			return false;
		}
    }

    public float GetAnalogValue(string device, int channel) {
		try {
        	return analogCallbacks[device](channel);
		}
		catch(KeyNotFoundException) {
			Debug.LogError(string.Format("No Analog Device Named {0}", device));
			return 0.0f;
		}
	}

    public Sixdof GetSixdofValue(string device, int channel) {
		try {
			return sixdofCallbacks[device](channel);
		}
		catch(KeyNotFoundException) {
			Debug.LogError(string.Format("No Sixdof Device Named {0}", device));
			return Sixdof.Empty;
		}
    }

    public Dictionary<string, object> GetMachineConfiguration(string machine) {
		try {
        	return state.GetTableDict(machines[machine]).ToDictionary(v => v.Key.ToString(), v => v.Value);
		}
		catch(KeyNotFoundException) {
			Debug.LogError(string.Format("No Machine Configuration Named {0}", machine));
			return new Dictionary<string, object>();
		}
    }
}
