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

public class SocketIOScript : MonoBehaviour {
	public string serverURL = "http://localhost:3000";

	public InputField uiInput = null;
	public Button uiSend = null;
	public Text uiChatLog = null;

	protected Socket socket = null;
	protected List<string> chatLog = new List<string> (); 

	void Destroy() {
		DoClose ();
	}

	// Use this for initialization
	void Start () {
		DoOpen ();

		uiSend.onClick.AddListener(() => {
			SendChat(uiInput.text);
			uiInput.text = "";
			uiInput.ActivateInputField();
		});
	}
	
	// Update is called once per frame
	void Update () {
		lock (chatLog) {
			if (chatLog.Count > 0) {
				string str = uiChatLog.text;
				foreach (var s in chatLog) {
					str = str + "\n" + s;
				}
				uiChatLog.text = str;
				chatLog.Clear ();
			}
		}
	}

	void DoOpen() {
		if (socket == null) {
			socket = IO.Socket (serverURL);
			socket.On (Socket.EVENT_CONNECT, () => {
				//Debug.Log ("Socket.IO connected");
				lock(chatLog) {
					chatLog.Add("Socket.IO connected.");
				}
			});
			socket.On ("chat", (data) => {
				//Debug.Log(data);

				string str = data.ToString();
				//Debug.Log(str);

				ChatData chat = JsonConvert.DeserializeObject<ChatData> (str);
				string strChatLog = "user#" + chat.id + ": " + chat.msg;
				//Debug.Log (strChatLog);

				lock(chatLog) {
					chatLog.Add(strChatLog);
				}
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
