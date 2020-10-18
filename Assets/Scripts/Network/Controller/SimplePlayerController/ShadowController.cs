using UnityEngine;

namespace Network.Controller.SimplePlayerController
{
    public class ShadowController: MonoBehaviour
    {
        [Range(1f, 15f)]
        public float lerpSpeed = 6f;
        
        public Vector2 shadowPosition;

        private Vector2 _remotePosition;
        public Vector2 RemotePosition
        {
            get => _remotePosition;
            set
            {
                _remotePosition = value;
                shadowPosition = value;
            }
        }

        private void Update()
        {
            var targetPos = new Vector3(shadowPosition.x, 0f, shadowPosition.y);
            var newPos = Vector3.Lerp(transform.position, targetPos, lerpSpeed * Time.deltaTime);
            transform.position = newPos;
        }
    }
}