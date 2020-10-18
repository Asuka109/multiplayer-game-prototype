using System;
using UnityEngine;

namespace Network.Frame
{
    [Serializable]
    public class PlayerStatus
    {
        public string userId;
        public int[] pos;

        public Vector2 Position
        {
            get => new Vector2(pos[0] / 1000f, pos[1] / 1000f);
            set => pos = new[] { Convert.ToInt32(value.x * 1000), Convert.ToInt32(value.y * 1000) };
        }
    }
}
