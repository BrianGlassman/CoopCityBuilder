using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkPlayer : Mirror.NetworkBehaviour
{
    [Mirror.SyncVar]
    [SerializeField]
    private string displayName = "Missing Name";
    
    [Mirror.Server]
    public void SetDisplayName(string displayName) { this.displayName = displayName; }
}
