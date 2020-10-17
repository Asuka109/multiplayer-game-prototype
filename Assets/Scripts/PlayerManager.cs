using System;
using System.Threading;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using Network;
using Network.Frame;
using UnityEngine;
using UnityEngine.Serialization;
using UnityWebSocket;

public class PlayerManager : MonoBehaviour
{
	public GameObject playerObject;
	
	private PlayerStatus[] _shadowStatus;
	private WebSocket _socket;

	private void Start()
    {
		// 创建实例
		const string address = "ws://localhost:3000";
		var socket = new WebSocket(address);
		this._socket = socket;

		// 注册回调
		socket.OnOpen += (sender, e) => Debug.Log("WebSocket: OnOpen");
		socket.OnClose += (sender, e) => Debug.Log("WebSocket: OnClose");
		socket.OnMessage += (object sender, MessageEventArgs e) => {
			var frame = JsonUtility.FromJson<BaseFrame>(e.Data);
			switch (frame.frameType)
			{
				case "StatusFrame":
					var statusFrame = JsonUtility.FromJson<StatusFrame>(e.Data);
					this.InitStatus(statusFrame);
					break;
				case "ActionFrame":
					var actionFrame = JsonUtility.FromJson<ActionFrame>(e.Data);
					this.UpdateStatus(actionFrame);
					break;
				case "InfoFrame":
					break;
				default:
					Debug.Log("WebSocket: Received unknow frame. \n" + e.Data);
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

	private Dictionary<string, GameObject> _playerInstances;
	
	private void InitStatus(StatusFrame frame) {
		for (var i = 0; i < transform.childCount ; i++) {
			Destroy (transform.GetChild (0).gameObject);
		}

		this._shadowStatus = frame.status;
		var playerInstances = new Dictionary<string, GameObject>();
		foreach (var initStatus in frame.status) {
			var position = new Vector3(initStatus.Position.x, 0f, initStatus.Position.y);
			var player = GameObject.Instantiate(this.playerObject, this.transform);
			player.transform.position = position;
			player.GetComponent<ShadowController>().shadowPosition = initStatus.Position;
			playerInstances.Add(initStatus.id, player);
		}
		this._playerInstances = playerInstances;
	}
	
	private void UpdateStatus(ActionFrame actionFrame)
	{
		foreach (var action in actionFrame.actions)
		{
			var gameObj = this._playerInstances[action.id];
			var movement = action.Movement;
			var shadowController = gameObj.GetComponent<ShadowController>();
			Debug.Log(gameObj.name);
			shadowController.shadowPosition += movement;
		}
	}

	private void Update()
	{
		UserInput();
	}

	private float _lastInputTime = 0f;
	
	private void UserInput()
	{
		if (Time.time - _lastInputTime < 0.05f) return;
		_lastInputTime = Time.time;
		var moveX = Input.GetAxisRaw("Horizontal");
		var moveY = Input.GetAxisRaw("Vertical");
		var frame = ActionFrame.FromActionMovement("asd", new Vector2(moveX, moveY));
		var jsonStr = JsonUtility.ToJson(frame);
		this._socket.SendAsync(jsonStr);
	}
}
