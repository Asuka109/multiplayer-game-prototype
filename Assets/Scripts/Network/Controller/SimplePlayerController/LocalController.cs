using System;
using Network.Frame;
using UnityEngine;

namespace Network.Controller.SimplePlayerController
{
    [RequireComponent(typeof(ShadowController))]
    public class LocalController : MonoBehaviour
    {
        public float moveSpeed = 0.2f;
        private GameObject _slotGameObject;
        private NetworkManager _wsManager;
        private ShadowController _shadowController;
        
        private void Start()
        {
            _slotGameObject = transform.parent.gameObject;
            _wsManager = _slotGameObject.GetComponent<NetworkManager>();
            _shadowController = GetComponent<ShadowController>();
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
            _shadowController.shadowPosition += movement * moveSpeed;
            // Send user input to server
            var frame = ActionFrame.FromActionMovement(_wsManager.userId, movement);
            var jsonStr = JsonUtility.ToJson(frame);
            _wsManager.Socket.SendAsync(jsonStr);
        }
    }
}