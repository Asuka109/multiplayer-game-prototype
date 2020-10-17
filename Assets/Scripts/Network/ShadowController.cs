using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Network
{
    public class ShadowController: MonoBehaviour
    {
        public Vector2 shadowPosition;

        private void Update()
        {
            var targetPos = new Vector3(shadowPosition.x, 0f, shadowPosition.y);
            var newPos = Vector3.Lerp(transform.position, targetPos, 3f * Time.deltaTime);
            transform.position = newPos;
        }
    }
}