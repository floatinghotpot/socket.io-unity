using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using Quobject.SocketIoClientDotNet.Client;

using WebSocket4Net;

public class TestWebSocketScript : MonoBehaviour {
	protected WebSocket4Net.WebSocket ws = null;

	void Destroy() {
		DoClose ();
	}

	// Use this for initialization
	void Start () {
		Debug.Log("Start()");

		DoOpen ();
	}

	protected void DoOpen()
	{
		ws = new WebSocket4Net.WebSocket ("ws://echo.websocket.org");
		ws.Opened += ws_Opened;
		ws.DataReceived += ws_DataReceived;
		ws.MessageReceived += ws_MessageReceived;
		ws.Error += ws_Error;
		ws.Closed += ws_Closed;
		ws.Open ();
	}

	protected void DoClose() 
	{
		if (ws != null) {
			try {
				ws.Close();
			} catch( Exception e) {
				Debug.Log ("DoClose exception: " + e.Message);
			}
		}
	}

	void ws_Opened (object sender, EventArgs e)
	{
		Debug.Log("ws_Opened " + ws.SupportBinary);
	}

	void ws_DataReceived(object sender, DataReceivedEventArgs e)
	{
		Debug.Log("ws_DataReceived " + e.Data);
	}

	void ws_MessageReceived(object sender, MessageReceivedEventArgs e)
	{
		Debug.Log("ws_MessageReceived e.Message= " + e.Message);
	}

	void ws_Closed(object sender, EventArgs e)
	{
		Debug.Log("ws_Closed");
		ws.Opened -= ws_Opened;
		ws.Closed -= ws_Closed;
		ws.MessageReceived -= ws_MessageReceived;
		ws.DataReceived -= ws_DataReceived;
		ws.Error -= ws_Error;
	}

	void ws_Error(object sender, SuperSocket.ClientEngine.ErrorEventArgs e)
	{
		Debug.Log("websocket error" + e.Exception);
	}

	// Update is called once per frame
	void Update () {

	}

	void OnGUI() {
		GUI.Box (new Rect (20, 20, 200, 180), "WebSocket");

		if (GUI.Button (new Rect (40, 60, 160, 40), "Test 1")) {
			if (ws != null) {
				ws.Send ("Test 1");
			}
		}

		if (GUI.Button (new Rect (40, 120, 160, 40), "Test 2")) {
			if (ws != null) {
				ws.Send ("Test 2");
			}
		}

	}

}
