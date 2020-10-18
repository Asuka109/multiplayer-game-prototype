using System;
using UnityEngine;

namespace Network.Controller.SimplePlayerController
{
    public class ShadowController: MonoBehaviour
    {
        [Range(1f, 15f)]
        public float lerpSpeed = 6f;
        
        public Vector2 shadowPosition;
        public bool remotePositionApplyImmediately = false;
        public float maxOffset = 5f;

        public bool displayPositionMark = true;
        private Transform _remotePositionMark;
        private Transform _shadowPositionMark;
        
        private Vector2 _remotePosition;
        public Vector2 RemotePosition
        {
            get => _remotePosition;
            set
            {
                _remotePosition = value;
                if (remotePositionApplyImmediately)
                    shadowPosition = value;
            }
        }

        private void Start()
        {
            if (displayPositionMark)
            {
                _remotePositionMark = GameObject.CreatePrimitive(PrimitiveType.Sphere).transform;
                _remotePositionMark.localScale *= 0.2f;
                _shadowPositionMark = GameObject.CreatePrimitive(PrimitiveType.Sphere).transform;
                _shadowPositionMark.localScale *= 0.4f;
            }
        }

        private void Update()
        {
            // Correct the deviation
            var positionOffset = Vector2.Distance(RemotePosition, shadowPosition);
            if (!remotePositionApplyImmediately && positionOffset > maxOffset)
                shadowPosition = RemotePosition;
            // Lerp transform.position to shadowPosition
            var targetPos = new Vector3(shadowPosition.x, 0f, shadowPosition.y);
            var newPos = Vector3.Lerp(transform.position, targetPos, lerpSpeed * Time.deltaTime);
            transform.position = newPos;
            // Update positionMark.position
            _remotePositionMark.position = new Vector3(RemotePosition.x, 0f, RemotePosition.y);
            _shadowPositionMark.position = new Vector3(shadowPosition.x, 0f, shadowPosition.y);
        }
    }
}