using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkPlayer : Mirror.NetworkBehaviour
{
    #region Resources
    // TODO remove this block once we don't need it for debugging anymore
    [SerializeField] private ResourceType SetResource = ResourceType.food;
    [SerializeField] private int Value = 5;
    [SerializeField] private bool Go;
    // end block

    private readonly Mirror.SyncDictionary<ResourceType, int> resources = new()
    {
        { ResourceType.maxWorkers, 0 },
        { ResourceType.freeWorkers, 0 },
        { ResourceType.housing, 0 },
        { ResourceType.population, 0 },
        { ResourceType.food, 0 },
        { ResourceType.lumber, 0 },
    };

    public Dictionary<ResourceType, int> GetResources()
    {
        return new Dictionary<ResourceType, int>(resources);
    }
    #endregion

    public override void OnStartClient()
    {
        base.OnStartClient();

        resources.Callback += OnResourceChange;
    }

    /// <summary> Event that other scripts can subscribe to so they get alerted when resources changes. </summary>
    public event System.Action<Dictionary<ResourceType, int>> ClientOnResourceUpdated;
    /// <summary>
    /// Callback that gets triggered whenever resources changes.
    /// </summary>
    /// <param name="op">What type of change was made</param>
    /// <param name="type">The changed key</param>
    /// <param name="val">The changed value</param>
    private void OnResourceChange(Mirror.SyncDictionary<ResourceType, int>.Operation op, ResourceType type, int val)
    {
        ClientOnResourceUpdated?.Invoke(GetResources());
    }

    private void Update()
    {
        // TODO remove this block once we don't need it for debugging anymore
        if (Go)
        {
            resources[SetResource] = Value;
            Go = false;
            print($"Set {SetResource} to {Value}");
        }
    }
}
