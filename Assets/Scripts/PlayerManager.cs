using System;
using System.Threading;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityWebSocket;

public class PlayerManager : MonoBehaviour
{
	public UnityEngine.Object PlayerObject;
	private PlayerStatus[] ShadowPlayers;
	
    void Start()
    {
		// 创建实例
		string address = "ws://localhost:3000";
		WebSocket socket = new WebSocket(address);

		// 注册回调
		socket.OnOpen += (sender, e) => Debug.Log("WebSocket: OnOpen");
		socket.OnClose += (sender, e) => Debug.Log("WebSocket: OnClose");
		socket.OnMessage += (object sender, MessageEventArgs e) => {
			BaseFrame frame = JsonUtility.FromJson<BaseFrame>(e.Data);
			switch (frame.FrameType)
			{
				case "StatusFrame":
					this.InitStatus((StatusFrame)frame);
					break;
				case "ActionFrame":
					break;
				case "InfoFrame":
					break;
				default:
					Debug.Log("WebSocket: Received unknow frame.");
					break;
			}
		};
		socket.OnError += (sender, e) => Debug.Log("WebSocket: OnError");;

		// 连接
		socket.ConnectAsync();

		// 发送数据（两种方式）
		var readyFrame = new InfoFrame("Ready");
		var jsonStr = JsonUtility.ToJson(readyFrame);
		socket.SendAsync(jsonStr); // 发送 string 类型数据

		// 关闭连接
		//socket.CloseAsync();
    }

	void InitStatus(StatusFrame frame) {
		this.ShadowPlayers = frame.status;
		foreach (PlayerStatus player in frame.status) {
			GameObject.Instantiate(this.PlayerObject);
		}
	}

    void Update()
    {
        
    }
}
