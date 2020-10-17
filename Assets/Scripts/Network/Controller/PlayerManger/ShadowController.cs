using UnityEngine;

namespace Network.Controller.PlayerManger
{
    public class ShadowController: MonoBehaviour
    {
        public Vector2 shadowPosition;
        [Range(1f, 15f)]
        public float lerpSpeed = 6f;

        private void Update()
        {
            var targetPos = new Vector3(shadowPosition.x, 0f, shadowPosition.y);
            var newPos = Vector3.Lerp(transform.position, targetPos, lerpSpeed * Time.deltaTime);
            transform.position = newPos;
        }
    }
}