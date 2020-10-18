using System;

namespace Network.Frame
{
    [Serializable]
    public class StatusFrame: BaseFrame
    {
        public PlayerStatus[] status;

        public StatusFrame()
        {
            frameType = "StatusFrame";
        }
    }
}
