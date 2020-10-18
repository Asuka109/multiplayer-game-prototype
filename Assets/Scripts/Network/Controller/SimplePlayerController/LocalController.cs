using System;
using Network.Frame;
using UnityEngine;

namespace Network.Controller.SimplePlayerController
{
    [RequireComponent(typeof(NetworkManager))]
    public class LocalController : MonoBehaviour
    {
        public GameObject PlayerGameObject
        {
            get => _player;
            set
            {
                _player = value;
                _shadowController = _player.GetComponent<ShadowController>();
            }
        }
        private GameObject _player;
        private ShadowController _shadowController;
        private NetworkManager _wsManager;
        
        private void Start()
        {
            _wsManager = GetComponent<NetworkManager>();
        }

        private void Update()
        {
            HandleUserInput();
        }
	
        private const float InputHandingInterval = 0.05f;
        private float _lastInputTime = 0f;
	
        private void HandleUserInput()
        {
            if (!_shadowController) return;
            // Throttled handler: 20 per sec
            if (Time.time - _lastInputTime < InputHandingInterval) return;
            _lastInputTime = Time.time;
            // Apply user input locally
            var moveX = Input.GetAxisRaw("Horizontal");
            var moveY = Input.GetAxisRaw("Vertical");
            var movement = new Vector2(moveX, moveY);
            _shadowController.shadowPosition += movement;
            // Send user input to server
            var frame = ActionFrame.FromActionMovement(_wsManager.userId, movement);
            var jsonStr = JsonUtility.ToJson(frame);
            _wsManager.Socket.SendAsync(jsonStr);
        }
    }
}