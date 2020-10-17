using Network.Frame;
using UnityEngine;
using UnityWebSocket;

namespace Network
{
	public class NetworkManager : MonoBehaviour
	{
		public WebSocket Socket { get; private set; }
	
		public event StatusFrameDelegate OnStatusFrame;
		public delegate void StatusFrameDelegate(StatusFrame frame);
		public event ActionFrameDelegate OnActionFrame;
		public delegate void ActionFrameDelegate(ActionFrame frame);
		public event InfoFrameDelegate OnInfoFrame;
		public delegate void InfoFrameDelegate(InfoFrame frame);

		private void Start()
		{
			// Create websocket client instance
			const string address = "ws://localhost:3000";
			var socket = new WebSocket(address);
			Socket = socket;

			// Register callbacks
			socket.OnOpen += (sender, e) => Debug.Log("WebSocket: OnOpen");
			socket.OnClose += (sender, e) => Debug.Log("WebSocket: OnClose");
			socket.OnError += (sender, e) => Debug.Log("WebSocket: OnError");
			socket.OnMessage += (sender, e) => {
				var frame = JsonUtility.FromJson<BaseFrame>(e.Data);
				switch (frame.frameType)
				{
					case "StatusFrame":
						var statusFrame = JsonUtility.FromJson<StatusFrame>(e.Data);
						OnStatusFrame?.Invoke(statusFrame);
						break;
					case "ActionFrame":
						var actionFrame = JsonUtility.FromJson<ActionFrame>(e.Data);
						OnActionFrame?.Invoke(actionFrame);
						break;
					case "InfoFrame":
						var infoFrame = JsonUtility.FromJson<InfoFrame>(e.Data);
						OnInfoFrame?.Invoke(infoFrame);
						break;
					default:
						Debug.Log("WebSocket: Received unknow frame. \nRaw Data: " + e.Data);
						break;
				}
			};

			// Start connect
			socket.ConnectAsync();

			// Send Ready sign
			var readyFrame = new InfoFrame("Ready");
			var jsonStr = JsonUtility.ToJson(readyFrame);
			socket.SendAsync(jsonStr);
		}

		private void OnDestroy()
		{
			// Close connect
			Socket.CloseAsync();
		}

		private void Update()
		{
			HandleUserInput();
		}
	
		private const float InputHandingInterval = 0.05f;
		private float _lastInputTime = 0f;
	
		private void HandleUserInput()
		{
			// Throttled handler: 20 per sec
			if (Time.time - _lastInputTime < InputHandingInterval) return;
			_lastInputTime = Time.time;
			// Send user input to server
			var moveX = Input.GetAxisRaw("Horizontal");
			var moveY = Input.GetAxisRaw("Vertical");
			var frame = ActionFrame.FromActionMovement("asd", new Vector2(moveX, moveY));
			var jsonStr = JsonUtility.ToJson(frame);
			Socket.SendAsync(jsonStr);
		}
	}
}
