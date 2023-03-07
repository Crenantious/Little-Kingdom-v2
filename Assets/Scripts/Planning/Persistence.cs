using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPersist
{
    public string Key { get; }
    public string Value { get; }
    public Dictionary<string, object> OnSave();
    public void OnLoad(Dictionary<string, object> data);
    public string GetName();
    protected void Register()
    {
        Persistence.Register(this);
    }
}

public class PersistKey : IEnumerable<object>
{
    private List<object> parts;

    public object getPart() ...

    public string ToString()
    {
        return ""; // parts joined
    }
}


public class Persist : IPersist
{
    private static readonly string baseKey = "x.y.z";

    public string Key { get; }

    public Persist()
    {
        this.Key = baseKey + "." + Persistence.Get(baseKey).Count;
        new Guid(Guid.NewGuid().ToString());
    }

    public string GetName()
    {
        return "Persist";
    }
}

public class Persistence
{
    private static JsonStorage storage;
    private static Dictionary<PersistKey, IPersist> registeredPersistObjects = new();

    public static void SaveGame()
    {
        foreach(IPersist persistObject in registeredPersistObjects)
        {
            Dictionary<string, object>  data = persistObject.OnSave();
            SaveData(persistObject.GetName(), data);
        }

        storage.save(Json.toJson(registeredPersistObjects));
    }

    private static void SaveData(string objectName, Dictionary<string, object> data)
    {
        // Serialise data
    }

    public static void Register(IPersist persistObject)
    {
        if (registeredPersistObjects.ContainsKey(key))
        {
            throw new Exception();
        }

        registeredPersistObjects.Add(persistObject);
    }

    public static T Get<T>(PersistKey key)
    {
        try
        {
            typeof(T).GetMethod("unserialise")
        }
    }
}

Dictionary jsonValue;

foreach (object part in key)
{
    object jsonObject = jsonValue.get(part);

    if (jsonObject == Dictionary)
    {
        jsonValue = ((Dictionary) jsonObject);
    }
    else if (o == List)
    {

    }
    else
    {
        
    }
}
// level1.chunk.entities.n
// level1.entities.
// level1.entities.
/*
{
    "players": {
        "<id>": {
            "name": "Player 1",
            "buildings": [id1, id2...],
            // ...
        },
        {
            "name": "Player 2",
            "buildings": [],
            // ...
        },
    },
    "buildings": {
        "<id>": {
            ...
        }
    },
}

Players : IPersist, ISerialisable {
    Dictionary<Guid, Player> players;

    Key = new PersistKey("players");
}

Player : ISerialisable {
    

    Dictionary<string, object> serialise() {
        return {
            "id": ...,
            ...
            "buildings: [Guid, Guid],
        };
    }

    public Player(name, ...)
    {
        this.id = id;
        this.name = name;
    }

    static Player unserialise(Dict) {
        return new Player(data.id, data.name);
    }
}

Player.unserialise(data) -> Player;


Buildings {
    Dict<guid, Building> buildings

    static {
        foreach(Dict data in Persistence.get("buildings")) {
            Building.unserialise(data);
        }
    }

    Get(id) {
          return dict.get(id);
    }
}

Building {
    public Building(Guid id) {}
}