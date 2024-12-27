using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

using MessagePack;

public class SettingsManager : MonoBehaviour
{
    /// <summary>
    /// LZ4 Compression for MessagePack
    /// </summary>
    public static MessagePackSerializerOptions LZ4_OPTIONS = MessagePackSerializerOptions.Standard.WithCompression(MessagePackCompression.Lz4BlockArray);

    public static SettingsObject Settings { get; set; } = new SettingsObject();

    public static void ApplySettings()
    {
        string path = $@"{Application.dataPath}\settings.json";

        if (!File.Exists(path))
        {
            ExceptionManager.Throw("The settings file 'settings.json' doesn't exists in Ingame directory. Please reinstall the game and try again.", "SettingsManager");
            return;
        }

        string json = File.ReadAllText(path);
        byte[] buffer = MessagePackSerializer.ConvertFromJson(json);
        Settings = MessagePackSerializer.Deserialize<SettingsObject>(buffer);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
