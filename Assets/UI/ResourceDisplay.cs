using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Heavily drawn from gamedev.tv Multiplayer course, Resource Generation
// https://www.gamedev.tv/courses/1086379/lectures/25044287

public class ResourceDisplay : MonoBehaviour
{
    public NetworkPlayer player;

    private TMPro.TextMeshProUGUI TMP;

    private void Awake()
    {
        TMP = GetComponentInChildren<TMPro.TextMeshProUGUI>();
    }

    private void Update()
    {
        // Have to get player during Update instead of Start/Awake because of a race condition during player creation
        if (player == null && Mirror.NetworkClient.connection != null && Mirror.NetworkClient.connection.identity != null)
        {
            // Keep trying to get the player until it works
            if (Mirror.NetworkClient.connection.identity.TryGetComponent<NetworkPlayer>(out player))
            {
                // Subscribe to the event
                player.ClientOnResourceUpdated += HandleResourceUpdate;
                // Sync with the initial state
                HandleResourceUpdate(player.GetResources());
            }
        }
    }

    private void OnDestroy()
    {
        if (player != null) player.ClientOnResourceUpdated -= HandleResourceUpdate;
    }

    private void SetText(string text)
    {
        TMP.text = text;
    }

    private void HandleResourceUpdate(Dictionary<ResourceType, int> vals)
    {
        SetText(
            $"Workers: {vals[ResourceType.freeWorkers]}/{vals[ResourceType.maxWorkers]}" +
            $"\nPopulation: {vals[ResourceType.population]}/{vals[ResourceType.housing]}" +
            $"\nFood: {vals[ResourceType.food]}" +
            $"\nLumber: {vals[ResourceType.lumber]}"
            );
    }
}
