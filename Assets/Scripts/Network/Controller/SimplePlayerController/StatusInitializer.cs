using System.Collections.Generic;
using Network.Frame;
using UnityEngine;

namespace Network.Controller.SimplePlayerController
{
    [RequireComponent(typeof(NetworkManager))]
    public class StatusInitializer : MonoBehaviour
    {
        public GameObject playerPrefab;
        
        [Header("ShadowController")]
        [Range(1f, 15f)]
        public float lerpSpeed = 6f;
    
        private NetworkManager _wsManager;
        private Rect _fullscreenRect;
        private GUIStyle _labelStyle;

        private void Start()
        {
            _wsManager = GetComponent<NetworkManager>();
            _wsManager.OnStatusFrame += InitStatus;
            
            _fullscreenRect = new Rect(0.0f, 0.0f, Screen.width, Screen.height);
            _labelStyle = new GUIStyle { fontSize = 50, alignment = TextAnchor.MiddleCenter };
        }
    
        private Dictionary<string, GameObject> _playerInstances;
	
        private void InitStatus(StatusFrame frame) {
            // Destroy children 
            if (_playerInstances != null) foreach (var instance in _playerInstances.Values)
                Destroy(instance);
        
            var playerInstances = new Dictionary<string, GameObject>();
            foreach (var initStatus in frame.status) {
                // Create GameObject
                var position = new Vector3(initStatus.Position.x, 0f, initStatus.Position.y);
                var player = Instantiate(playerPrefab, transform);
                player.transform.position = position;
                player.name = initStatus.userId;
                var shadowController = player.AddComponent<ShadowController>();
                var remoteController = player.AddComponent<RemoteController>();
                if (initStatus.userId == _wsManager.userId)
                {
                    player.AddComponent<LocalController>();
                    shadowController.remotePositionApplyImmediately = false;
                }
                shadowController.lerpSpeed = lerpSpeed;
                shadowController.RemotePosition = initStatus.Position;
                shadowController.shadowPosition = initStatus.Position;
                
                playerInstances.Add(initStatus.userId, player);
            }
            _playerInstances = playerInstances;
        }

        private void OnGUI()
        {
            if (_wsManager.Status != NetworkManager.RoomStatus.Idle) return;
            GUI.DrawTexture(_fullscreenRect, Texture2D.whiteTexture, ScaleMode.StretchToFill);
            GUI.Label(_fullscreenRect, "Waiting Player", _labelStyle);
        }
    }
}