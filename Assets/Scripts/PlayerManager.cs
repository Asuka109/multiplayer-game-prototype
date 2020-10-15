using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
internal class Frame {
    [SerializeField]
    public int x;
    [SerializeField]
    public int y;
    [SerializeField]
    public string id;
}

public class PlayerManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var status = new PlayerStatus {
            id = "ad",
            pos = Vector2.zero
        };
        PlayerStatus[] statusArr = { status };
        var SyncFrame = new StatusFrame {
            status = statusArr
        };
        string json = JsonUtility.ToJson(SyncFrame, true);
        Debug.Log(json);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
