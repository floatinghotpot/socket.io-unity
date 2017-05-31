using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Quobject.SocketIoClientDotNet.Client;
using Newtonsoft.Json;

public class ChatData {
	public string id;
	public string msg;
};

public class TestSocketIOScript : MonoBehaviour {
	public InputField inputMsg = null;
	public Button btnSend = null;
	public Text textChatLog = null;

	protected Socket socket = null;

	void Destroy() {
		DoClose ();
	}

	// Use this for initialization
	void Start () {
		DoOpen ();

		btnSend.onClick.AddListener(() => SendChat(inputMsg.text));
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void DoOpen() {
		if (socket == null) {
			socket = IO.Socket ("http://localhost:3000");
			socket.On (Socket.EVENT_CONNECT, () => {
				Debug.Log ("Socket.IO connected");
			});
			socket.On ("chat", (data) => {
				//Debug.Log(data);

				string str = data.ToString();
				//Debug.Log(str);

				ChatData chat = JsonConvert.DeserializeObject<ChatData> (str);
				string strChatLog = "user#" + chat.id + ": " + chat.msg;
				Debug.Log (strChatLog);

				// TODO: show it in UI with main thread, UI not allow access from back thread
			});
		}
	}

	void DoClose() {
		if (socket != null) {
			socket.Disconnect ();
			socket = null;
		}
	}

	void SendChat(string str) {
		if (socket != null) {
			socket.Emit ("chat", str);
		}
	}
}
