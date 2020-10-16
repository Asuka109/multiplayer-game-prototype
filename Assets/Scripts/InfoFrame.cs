using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
internal class InfoFrame: BaseFrame
{
    [SerializeField]
    public new string FrameType = "InfoFrame";
    [SerializeField]
    public string Info;

    public InfoFrame(string Info) {
        this.Info = Info;
    }
}
