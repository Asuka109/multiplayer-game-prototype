using System;

namespace Network.Frame
{
    [Serializable]
    public class InfoFrame: BaseFrame
    {
        public string userId;
        public string info;

        public InfoFrame(string userId, string info)
        {
            frameType = "InfoFrame";
            this.userId = userId;
            this.info = info;
        }
    }
}
