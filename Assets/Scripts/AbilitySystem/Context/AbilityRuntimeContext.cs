using System.Collections.Generic;
using System;
using UnityEngine;

public class AbilityRuntimeContext
{
    public GameObject Caster { get; set; }
    public GameObject Target { get; set; }

    public bool IsSuccessful { get; set; } = true; // Default success state

    // Event registry: Maps event names to lists of event handlers
    private readonly Dictionary<string, Action<object>> eventRegistry = new Dictionary<string, Action<object>>();

    // Subscribe to an event
    public void SubscribeEvent(string eventName, Action<object> handler)
    {
        if (!eventRegistry.ContainsKey(eventName))
        {
            eventRegistry[eventName] = null;
        }

        eventRegistry[eventName] += handler;
    }

    // Unsubscribe from an event
    public void UnsubscribeEvent(string eventName, Action<object> handler)
    {
        if (eventRegistry.ContainsKey(eventName))
        {
            eventRegistry[eventName] -= handler;
        }
    }

    // Trigger an event
    public void TriggerEvent(string eventName, object eventData = null)
    {
        if (eventRegistry.ContainsKey(eventName) && eventRegistry[eventName] != null)
        {
            eventRegistry[eventName].Invoke(eventData);
        }
    }
}
