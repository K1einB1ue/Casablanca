using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EventManager : SingletonMono<EventManager>
{

    private static Dictionary<string, Events> EventLib = new Dictionary<string, Events>();

    public static void Eventlize(string Event, Action action){
        if(EventLib.TryGetValue(Event,out Events events)){
            events.AddAction(action);
        }
        else {
            EventLib.Add(Event, new Events(action));
        }
    }

    public static void Eventlize(string Event, Action action,int Hash) {
        if (EventLib.TryGetValue(Event, out Events events)) {
            events.AddAction(action,Hash);
        }
        else {
            EventLib.Add(Event, new Events(action, Hash));
        }
    }

    public static void Call(string Event) {
        if (EventLib.TryGetValue(Event,out Events events)){
            events.Call();
        }
    }

    public static void Call(string Event,int hash) {
        if(EventLib.TryGetValue(Event,out Events events)) {
            events.Call(hash);
        }
    }
}

public class EventBody
{
    private string Event;
    public EventBody(string EventName) {
        this.Event = EventName;
    }
    public void Call() {
        EventManager.Call(this.Event);
    }
    public void Call(int Hash) {
        EventManager.Call(this.Event, Hash);
    }
    public void Eventlize(Action action) {
        EventManager.Eventlize(this.Event, action);
    }
    public void Eventlize(Action action,int Hash) {
        EventManager.Eventlize(this.Event, action, Hash);
    }
}


public class Events {

    public Events(Action action) { this.actions.Add(action); }
    public Events(Action action,int Hash) {this.Hashaction.Add(Hash, action); }
    private List<Action> actions = new List<Action>();
    private Dictionary<int, Action> Hashaction = new Dictionary<int, Action>();
    
    public void AddAction(Action action) {
        this.actions.Add(action);
    }
    public void AddAction(Action action,int Hash) {
        this.Hashaction.Add(Hash, action);
    }

    public void Call() {
        for(int i = 0; i < actions.Count; i++) {
            actions[i]?.Invoke();
        }
    }
    public void Call(int Hash) {
        Hashaction[Hash]?.Invoke();
    }
}
