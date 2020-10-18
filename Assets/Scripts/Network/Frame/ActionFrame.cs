using System;
using UnityEngine;

namespace Network.Frame
{
    [Serializable]
    public class ActionFrame: BaseFrame
    {
        public PlayerAction[] actions;

        public ActionFrame(PlayerAction action)
        {
            frameType = "ActionFrame";
            actions = new[] {action};
        }

        public static ActionFrame FromActionMovement(string userId, Vector2 movement)
        {
            var action = new PlayerAction
            {
                userId = userId,
                Movement = movement
            };
            return new ActionFrame(action);
        }
    }
}
