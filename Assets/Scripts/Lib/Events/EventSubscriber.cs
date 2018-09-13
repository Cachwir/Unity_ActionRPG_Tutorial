using System.Collections.Generic;

public delegate void EventSubscriber_EventCallback(); // this long name is here to avoid conflicts with other libs, but you don't need to care about it

/**
 * Class EventsSubscriber
 * This class subscribes your entity's events and calls them whenever you need (in the order you suscribed them in) !
 * 
 * Example :
 * EventSubscriber eventSubscriber = new EventSuscriber();
 * eventSubscriber.Subscribe("myEvent", delegate() {
 *     // do stuff
 * });
 * // some time later
 * eventSubscriber.Trigger("myEvent"); // will call the delegate function declared up there
 * 
 * /!\ Warning /!\ 
 * Subscribed events are not removed after being called ! If you need that, use CallAndEmpty(eventName) or Empty(eventName).
 */
public class EventSubscriber {
    
    protected Dictionary<string, List<EventSubscriber_EventCallback>> registeredEvents = new Dictionary<string, List<EventSubscriber_EventCallback>>();

    public const int INSERT_POSITION_BEFORE = 0;
    public const int INSERT_POSITION_AFTER = int.MaxValue;

    /**
     * Adds a callback to execute when the eventName event is triggered.
     * @string      eventName                   The event't name
     * @function    callback                    The function to execute
     * @int         insertPosition (optionnal)  The position to insert the function in the functions to execute list
     */
    public void Subscribe(string eventName, EventSubscriber_EventCallback callback, int insertPosition = INSERT_POSITION_AFTER)
    {
        string key = eventName;
        List<EventSubscriber_EventCallback> subscribedEvents = GetValue(eventName);

        if (insertPosition == INSERT_POSITION_BEFORE)
        {
            subscribedEvents.Insert(0, callback);
        }
        else if (insertPosition == INSERT_POSITION_AFTER)
        {
            subscribedEvents.Add(callback);
        }
        else
        {
            subscribedEvents.Insert(insertPosition, callback);
        }
        
        registeredEvents.Remove(key);
        registeredEvents.Add(key, subscribedEvents);
    }

    /**
     * Calls all the callback functions subscribed for the given eventName
     * @string eventName The event't name
     */
    public void Trigger(string eventName)
    {
        foreach (EventSubscriber_EventCallback subscribedEvent in GetValue(eventName))
        {
            subscribedEvent();
        }
    }

    /**
     * Removes all the subscribed events for the given eventName
     * @string eventName The event't name
     */
    public void Empty(string eventName)
    {
        registeredEvents.Remove(eventName);
    }

    /**
     * Triggers the event, then removes all its subscribed events
     * @string eventName The event't name
     */
    public void CallAndEmpty(string eventName)
    {
        Trigger(eventName);
        Empty(eventName);
    }

    protected List<EventSubscriber_EventCallback> GetValue(string eventName)
    {
        List<EventSubscriber_EventCallback> subscribedEvents = new List<EventSubscriber_EventCallback>();
        registeredEvents.TryGetValue(eventName, out subscribedEvents);
        return subscribedEvents ?? new List<EventSubscriber_EventCallback>();
    }
}
