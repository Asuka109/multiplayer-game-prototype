using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Network.Frame
{
    [Serializable]
    public class PlayerAction
    {
        public string userId;
        public int[] movement;

        public Vector2 Movement
        {
            get => new Vector2(movement[0] / 1000f, movement[1] / 1000f);
            set => movement = new[] { Convert.ToInt32(value.x * 1000), Convert.ToInt32(value.y * 1000) };
        }
    }
}
