using System.Collections.Generic;
using Network.Frame;
using UnityEngine;

namespace Network.Controller.PlayerManger
{
    [RequireComponent(typeof(NetworkManager))]
    public class SimplePlayerManager: MonoBehaviour
    {
        public GameObject playerObject;
    
        private NetworkManager _wsManager;
        private PlayerStatus[] _shadowStatus;

        private void Start()
        {
            _wsManager = GetComponent<NetworkManager>();
            _wsManager.OnStatusFrame += InitStatus;
            _wsManager.OnActionFrame += ApplyAction;
        }
    
        private Dictionary<string, GameObject> _playerInstances;
	
        private void InitStatus(StatusFrame frame) {
            // Destroy children 
            for (var i = 0; i < transform.childCount ; i++)
                Destroy (transform.GetChild (0).gameObject);
        
            // Create players' GameObject
            var playerInstances = new Dictionary<string, GameObject>();
            foreach (var initStatus in frame.status) {
                var position = new Vector3(initStatus.Position.x, 0f, initStatus.Position.y);
                var player = Instantiate(playerObject, transform);
                player.transform.position = position;
                player.GetComponent<ShadowController>().shadowPosition = initStatus.Position;
                playerInstances.Add(initStatus.id, player);
            }
            _playerInstances = playerInstances;
        }
	
        private void ApplyAction(ActionFrame actionFrame)
        {
            // Update GameObjects' status
            foreach (var action in actionFrame.actions)
            {
                var gameObj = _playerInstances[action.id];
                var movement = action.Movement;
                var shadowController = gameObj.GetComponent<ShadowController>();
                shadowController.shadowPosition += movement;
            }
        }

    }
}
