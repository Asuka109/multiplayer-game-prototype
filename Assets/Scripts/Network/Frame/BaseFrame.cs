using System;
using UnityEngine;

namespace Network.Frame
{
    [Serializable]
    public class BaseFrame
    {
        public string frameType = "BaseFrame";
        public int id;
    }
}
