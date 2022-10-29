using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildPairs : MonoBehaviour
{
    public static BuildPairs inst;

    public Dictionary<KeyCode, Sprite> buildPairs = new();

    [SerializeField] Sprite red;
    [SerializeField] Sprite green;
    [SerializeField] Sprite blue;

    private void Awake()
    {
        if (inst == null)
        {
            inst = this;
        }
        else
        {
            return;
        }
        buildPairs.Add(KeyCode.R, red);
        buildPairs.Add(KeyCode.G, green);
        buildPairs.Add(KeyCode.B, blue);
    }
}
