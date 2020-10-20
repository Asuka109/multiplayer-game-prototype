using System;
using System.Collections.Generic;
using Network.Frame;
using UnityEngine;

namespace Network.Controller.SimplePlayerController
{
    [RequireComponent(typeof(ShadowController))]
    public class RemoteController: MonoBehaviour
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
            _wsManager.OnActionFrame += ApplyAction;
        }
    
        private void ApplyAction(ActionFrame actionFrame)
        {
            // Update GameObjects' status
            foreach (var action in actionFrame.actions)
            {
                if (name != action.userId) continue;
                var movement = action.Movement;
                _shadowController.RemotePosition += movement * moveSpeed;
                return;
            }
        }
    }
}
