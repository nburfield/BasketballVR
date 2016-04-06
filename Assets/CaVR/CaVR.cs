using UnityEngine;
using System;
using System.Diagnostics;
using System.Text;
using System.Threading;
using NLua;

public class CaVR : MonoBehaviour {

	public static bool Terminated { get; private set; }
	private Config config;
	public VRPNInputManager InputManger { get; private set; }

	private Thread networkThread;
	// Use this for initialization
	void Start() {
		Terminated = false;
		config = new Config();
		config.Init();
		InputManger = new VRPNInputManager();
		InputManger.Start(config);

		UnityEngine.Debug.Log(Environment.GetCommandLineArgs()[0]);
		if(config.IsMaster) {
			//if(!Application.isEditor && (Environment.OSVersion.Platform == PlatformID.Unix || Environment.OSVersion.Platform == PlatformID.MacOSX || Environment.OSVersion.Platform == PlatformID.Win32NT)) { 
			    var machines = config.GetLuaStateValue("machines") as LuaTable;
                foreach(var machineKey in machines.Keys) {
                    var machine = machineKey.ToString();
                    if(machine == "master") {
                        continue;
                    }

                    var ssh = machines[machineKey + ".ssh"].ToString();

                    var cwd = Environment.CurrentDirectory;

                    var command = new StringBuilder();
					command.Append(ssh).Append(' ');
                    command.Append("'cd ").Append(cwd).Append(" && export DISPLAY=:0 &&");

                    foreach(var arg in Environment.GetCommandLineArgs()) {
                        command.Append(' ');
                        command.Append(arg);
                    }

                    command.Append(" --cavr_master=").Append(config.GetLuaStateValue("machines.master.address").ToString());

					command.Append("'");

					var commandStr = command.ToString();

					UnityEngine.Debug.Log("Command: " + commandStr);

                    var sshOptions = new ProcessStartInfo("ssh", command.ToString());
                    var process = new Process();
                    process.StartInfo = sshOptions;

                    if(!process.Start()) {
                        UnityEngine.Debug.LogError("Unable to launch SSH");
                    }

                    Thread.Sleep(1000 * 2);
                }
			//}
            //else {
            //    UnityEngine.Debug.LogWarning("Windows currently not supported for cross network CaVR");
            //}
		}

		networkThread = new Thread(() => {
			while(!Terminated) {
				if(config.IsMaster) {
					Pubsub();
				}	

				SyncInput();

				if(!config.IsMaster) {
					Pubsub();
				}
			}
		});

		//networkThread.Start();
	}

	// Update is called once per frame
	void Update() {
        InputManger.Update();
	}

	void Destroy() {
		Terminated = true;
	}

	private void Pubsub() {
		
	}

	private void SyncInput() {
		InputManger.Sync();
	}
		
}
