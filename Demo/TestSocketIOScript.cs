using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Quobject.SocketIoClientDotNet.Client;

public class TestSocketIOScript : MonoBehaviour {
	protected Socket socket = null;

	void Destroy() {
		DoClose ();
	}

	// Use this for initialization
	void Start () {
		DoOpen ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void DoOpen() {
		if (socket == null) {
			socket = IO.Socket ("http://localhost:7000");
			socket.On (Socket.EVENT_CONNECT, ws_OnConnected);
			socket.On ("rpc_ret", (data) => {
				Debug.Log(data);
			});
			socket.On ("notify", (data) => {
				Debug.Log(data);
			});
		}
	}

	void DoClose() {
		if (socket != null) {
			socket.Disconnect ();
			socket = null;
		}
	}

	void DoLogin() {
		if (socket != null) {
			socket.Emit ("rpc", "login");
		}
	}

	void ws_OnConnected() {
		Debug.Log ("Socket.IO connected");
	}

	void OnGUI() {
		GUI.Box (new Rect (320, 20, 200, 220), "SocketIO");

		if (GUI.Button (new Rect (340, 60, 160, 40), "Open")) {
			DoOpen ();
		}

		if (GUI.Button (new Rect (340, 120, 160, 40), "login")) {
			DoLogin ();
		}

		if (GUI.Button (new Rect (340, 180, 160, 40), "Close")) {
			DoClose ();
		}

	}
}
