using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Gamedev.TV corse - Unity Multiplayer: Intermediate C# Coding & Networking

public class NetworkManager : Mirror.NetworkManager
{
    [SerializeField] HexGrid grid;

    /* Not used for now, might use later
    public override void OnClientConnect()
    {
        base.OnClientConnect();
    }
    */

    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        // Run the base version
        base.OnServerAddPlayer(conn);

        // Get the player and assign a name
        NetworkPlayer player =  conn.identity.GetComponent<NetworkPlayer>();
        player.SetDisplayName($"Player {numPlayers}");

        player.GetComponent<Clicker>().grid = grid;
    }
}
