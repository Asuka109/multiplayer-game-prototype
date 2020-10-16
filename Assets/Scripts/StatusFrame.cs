using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
internal class StatusFrame: BaseFrame
{
    [SerializeField]
    public new string FrameType = "StatusFrame";
    [SerializeField]
    public PlayerStatus[] status;
}
