using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
internal class ActionFrame: BaseFrame
{
    [SerializeField]
    public new string FrameType = "ActionFrame";
    [SerializeField]
    public PlayerAction[] actions;
}
