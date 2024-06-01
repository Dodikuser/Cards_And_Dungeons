<<<<<<< HEAD
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;

public class StateManager
{
    public static void SaveState(Dictionary<string, object> state)
    {
        string json = JsonConvert.SerializeObject(state, Newtonsoft.Json.Formatting.Indented, new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Auto,
            PreserveReferencesHandling = PreserveReferencesHandling.Objects
        });
        File.WriteAllText(Application.persistentDataPath + "/saves/data.json", json);
    }

    public static Dictionary<string, object> LoadState()
    {
        string json = File.ReadAllText(Application.persistentDataPath + "/saves/data.json");
        return JsonConvert.DeserializeObject<Dictionary<string, object>>(json, new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Auto,
            PreserveReferencesHandling = PreserveReferencesHandling.Objects
        });
    }
}
=======
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;

public class StateManager
{
    public static void SaveState(Dictionary<string, object> state)
    {
        string json = JsonConvert.SerializeObject(state, Newtonsoft.Json.Formatting.Indented, new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Auto,
            PreserveReferencesHandling = PreserveReferencesHandling.Objects
        });
        File.WriteAllText(Application.persistentDataPath + "/saves/data.json", json);
    }

    public static Dictionary<string, object> LoadState()
    {
        string json = File.ReadAllText(Application.persistentDataPath + "/saves/data.json");
        return JsonConvert.DeserializeObject<Dictionary<string, object>>(json, new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Auto,
            PreserveReferencesHandling = PreserveReferencesHandling.Objects
        });
    }
}
>>>>>>> 12fa110b21928b31eb4877f8d5b5df0b12480307
