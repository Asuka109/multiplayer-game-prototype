using System.Collections.Generic;
using Network.Frame;
using UnityEngine;

namespace Network.Controller.SimplePlayerController
{
    [RequireComponent(typeof(NetworkManager))]
    public class RemoteController: MonoBehaviour
    {
        public GameObject playerPrefab;
        public float moveSpeed = 0.2f; 
        
        [Header("ShadowController")]
        [Range(1f, 15f)]
        public float lerpSpeed = 6f;
    
        private NetworkManager _wsManager;
        private LocalController _localController;

        private void Start()
        {
            _wsManager = GetComponent<NetworkManager>();
            _wsManager.OnStatusFrame += InitStatus;
            _wsManager.OnActionFrame += ApplyAction;
            _localController = GetComponent<LocalController>();
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
                // Set the initial value of the shadowController.lerpSpeed
                var shadowController = player.GetComponent<ShadowController>();
                if (shadowController == null)
                {
                    shadowController = player.AddComponent<ShadowController>();
                    shadowController.lerpSpeed = lerpSpeed;
                }
                shadowController.RemotePosition = initStatus.Position;
                shadowController.shadowPosition = initStatus.Position;
                // Update LocalController's GameObject
                Debug.Log("_localController: " + _localController);
                if (_localController != null)
                    _localController.PlayerGameObject = player;
                
                playerInstances.Add(initStatus.userId, player);
            }
            _playerInstances = playerInstances;
        }
	
        private void ApplyAction(ActionFrame actionFrame)
        {
            // Update GameObjects' status
            foreach (var action in actionFrame.actions)
            {
                var gameObj = _playerInstances[action.userId];
                var movement = action.Movement;
                var shadowController = gameObj.GetComponent<ShadowController>();
                shadowController.RemotePosition += movement * moveSpeed;
            }
        }

    }
}
