using System;
using Network.Frame;
using UnityEngine;
using UnityWebSocket;

namespace Network
{
	public class NetworkManager : MonoBehaviour
	{
		public WebSocket Socket { get; private set; }
		public string serverUri;
		public string userId;

		public event StatusFrameDelegate OnStatusFrame;
		public delegate void StatusFrameDelegate(StatusFrame frame);
		public event ActionFrameDelegate OnActionFrame;
		public delegate void ActionFrameDelegate(ActionFrame frame);
		public event InfoFrameDelegate OnInfoFrame;
		public delegate void InfoFrameDelegate(InfoFrame frame);

		public enum RoomStatus
		{
			Idle, Playing
		}

		public RoomStatus Status { get; private set; }
		
		private void Start()
		{
			// ONLY FOR DEMO
			userId = Guid.NewGuid().ToString();
			
			if (string.IsNullOrEmpty(serverUri))
				throw new ApplicationException ("Server Uri cannot be empty.");
			if (string.IsNullOrEmpty(userId))
				throw new ApplicationException ("User Id cannot be empty.");
				
			// Create websocket client instance
			Socket = new WebSocket(serverUri);

			// Register callbacks
			Socket.OnOpen += (sender, e) => Debug.Log("WebSocket: OnOpen");
			Socket.OnClose += (sender, e) => Debug.Log("WebSocket: OnClose");
			Socket.OnError += (sender, e) => Debug.Log("WebSocket: OnError");
			Socket.OnMessage += (sender, e) => {
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

			// Exit application while abort by server or lost connect
			Status = RoomStatus.Idle;
			OnInfoFrame += UpdateStatusByFrame;
			Socket.OnClose += (sender, e) => { 
				Status = RoomStatus.Idle;
				Application.Quit();
			};
			Socket.OnError += (sender, e) => { 
				Status = RoomStatus.Idle;
				Application.Quit();
			};

			// Start connect
			Socket.ConnectAsync();

			// Send Ready sign
			var readyFrame = new InfoFrame(userId, "ready");
			var jsonStr = JsonUtility.ToJson(readyFrame);
			Socket.SendAsync(jsonStr);
		}

		private void UpdateStatusByFrame(InfoFrame frame)
		{
			switch (frame.info)
			{
				case "ready":
					Status = RoomStatus.Playing;
					break;
				case "exit":
					Status = RoomStatus.Idle;
					Application.Quit();
					break;
			}
		}

		private void OnDestroy()
		{
			// Close connect
			Socket.CloseAsync();
		}

	}
}
