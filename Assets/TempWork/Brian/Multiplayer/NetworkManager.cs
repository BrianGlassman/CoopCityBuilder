using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Gamedev.TV corse - Unity Multiplayer: Intermediate C# Coding & Networking

public class NetworkManager : Mirror.NetworkManager
{
    public override void OnClientConnect()
    {
        base.OnClientConnect();

        print("I connected");
    }

    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        base.OnServerAddPlayer(conn);

        print($"Added player, count is now {numPlayers}");
    }
}
