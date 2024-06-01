<<<<<<< HEAD
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static Dictionary<string, AudioClip> soundDictionary = new Dictionary<string, AudioClip>();
    public static List<AudioSource> backgroundSongs = new List<AudioSource>();

    private void Awake()
    {
        soundDictionary = new Dictionary<string, AudioClip> {
        {"coins", Resources.Load<AudioClip>("Sounds/InventorySounds/coins")},
        {"open", Resources.Load<AudioClip>("Sounds/InventorySounds/open")},
        {"error", Resources.Load<AudioClip>("Sounds/InventorySounds/error")},
        {"load", Resources.Load<AudioClip>("Sounds/InventorySounds/load")},
        {"loot", Resources.Load<AudioClip>("Sounds/InventorySounds/loot")},
        {"next", Resources.Load<AudioClip>("Sounds/InventorySounds/next")},
        {"pick", Resources.Load<AudioClip>("Sounds/InventorySounds/pick")},
        {"openHide", Resources.Load<AudioClip>("Sounds/InventorySounds/openHide")},
        {"menuButton", Resources.Load<AudioClip>("Sounds/InventorySounds/menuButton")},
        {"startGame", Resources.Load<AudioClip>("Sounds/InventorySounds/startGame")},
        {"GameOver", Resources.Load<AudioClip>("Sounds/InventorySounds/GameOver")},
        {"openLine", Resources.Load<AudioClip>("Sounds/InventorySounds/openLine")},

        {"army", Resources.Load<AudioClip>("Sounds/CardSounds/army")},
        {"crash", Resources.Load<AudioClip>("Sounds/CardSounds/crash")},
        {"damege1", Resources.Load<AudioClip>("Sounds/CardSounds/damege1")},
        {"damege2", Resources.Load<AudioClip>("Sounds/CardSounds/damege2")},
        {"hp", Resources.Load<AudioClip>("Sounds/CardSounds/hp")},
        {"lock", Resources.Load<AudioClip>("Sounds/CardSounds/lock")},
        {"take", Resources.Load<AudioClip>("Sounds/CardSounds/take")},

        {"Game", Resources.Load<AudioClip>("Sounds/Ost/8-Bit_Game_mp3")},
        {"Game_Land", Resources.Load<AudioClip>("Sounds/Ost/8_Bit_Game_Land_mp3")},
        {"neon-gaming", Resources.Load<AudioClip>("Sounds/Ost/neon-gaming-128925")},
        {"system-error", Resources.Load<AudioClip>("Sounds/Ost/system-error-sever-made--raritet-esche-na-frutike-delal-tipa")},
        {"Upbeat", Resources.Load<AudioClip>("Sounds/Ost/Upbeat_8_Bit_mp3")},

        {"slime", Resources.Load<AudioClip>("Sounds/EnemySounds/slime")},
        {"mag", Resources.Load<AudioClip>("Sounds/EnemySounds/mag")},
        {"goblin", Resources.Load<AudioClip>("Sounds/EnemySounds/goblin")},
        {"piphiy", Resources.Load<AudioClip>("Sounds/EnemySounds/piphiy")},
        {"undead", Resources.Load<AudioClip>("Sounds/EnemySounds/undead")},
    };
    }

    public static void PlaySound(string soundName, float volume = 1.0f, bool loop = false)
    {
        if (soundDictionary.ContainsKey(soundName))
        {
            AudioClip sound = soundDictionary[soundName];

            if (loop) StopBackgroundMusic();

            GameObject soundObject = new GameObject("SoundObject");
            AudioSource audioSource = soundObject.AddComponent<AudioSource>();
            audioSource.clip = sound;
            audioSource.volume = volume * (float)(PixelSpawner.volume / 14);
            audioSource.loop = loop;
            audioSource.Play();

            if (loop) backgroundSongs.Add(audioSource);
            else Destroy(soundObject, sound.length);
        }
        else Debug.LogWarning("Sound with name " + soundName + " not found in sound dictionary.");
    }
    public static void StopBackgroundMusic()
    {
        foreach (AudioSource bgSound in backgroundSongs)
        {
            bgSound.Stop();
            Destroy(bgSound.gameObject);
        }
        backgroundSongs.Clear();
    }
    public static void UpdateAllSoundVolumes()
    {
        foreach (AudioSource audioSource in FindObjectsOfType<AudioSource>())
        {
            audioSource.volume = (float)(PixelSpawner.volume / 14);
        }
    }

}
=======
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static Dictionary<string, AudioClip> soundDictionary = new Dictionary<string, AudioClip>();
    public static List<AudioSource> backgroundSongs = new List<AudioSource>();

    private void Awake()
    {
        soundDictionary = new Dictionary<string, AudioClip> {
        {"coins", Resources.Load<AudioClip>("Sounds/InventorySounds/coins")},
        {"open", Resources.Load<AudioClip>("Sounds/InventorySounds/open")},
        {"error", Resources.Load<AudioClip>("Sounds/InventorySounds/error")},
        {"load", Resources.Load<AudioClip>("Sounds/InventorySounds/load")},
        {"loot", Resources.Load<AudioClip>("Sounds/InventorySounds/loot")},
        {"next", Resources.Load<AudioClip>("Sounds/InventorySounds/next")},
        {"pick", Resources.Load<AudioClip>("Sounds/InventorySounds/pick")},
        {"openHide", Resources.Load<AudioClip>("Sounds/InventorySounds/openHide")},
        {"menuButton", Resources.Load<AudioClip>("Sounds/InventorySounds/menuButton")},
        {"startGame", Resources.Load<AudioClip>("Sounds/InventorySounds/startGame")},
        {"GameOver", Resources.Load<AudioClip>("Sounds/InventorySounds/GameOver")},
        {"openLine", Resources.Load<AudioClip>("Sounds/InventorySounds/openLine")},

        {"army", Resources.Load<AudioClip>("Sounds/CardSounds/army")},
        {"crash", Resources.Load<AudioClip>("Sounds/CardSounds/crash")},
        {"damege1", Resources.Load<AudioClip>("Sounds/CardSounds/damege1")},
        {"damege2", Resources.Load<AudioClip>("Sounds/CardSounds/damege2")},
        {"hp", Resources.Load<AudioClip>("Sounds/CardSounds/hp")},
        {"lock", Resources.Load<AudioClip>("Sounds/CardSounds/lock")},
        {"take", Resources.Load<AudioClip>("Sounds/CardSounds/take")},

        {"Game", Resources.Load<AudioClip>("Sounds/Ost/8-Bit_Game_mp3")},
        {"Game_Land", Resources.Load<AudioClip>("Sounds/Ost/8_Bit_Game_Land_mp3")},
        {"neon-gaming", Resources.Load<AudioClip>("Sounds/Ost/neon-gaming-128925")},
        {"system-error", Resources.Load<AudioClip>("Sounds/Ost/system-error-sever-made--raritet-esche-na-frutike-delal-tipa")},
        {"Upbeat", Resources.Load<AudioClip>("Sounds/Ost/Upbeat_8_Bit_mp3")},

        {"slime", Resources.Load<AudioClip>("Sounds/EnemySounds/slime")},
        {"mag", Resources.Load<AudioClip>("Sounds/EnemySounds/mag")},
        {"goblin", Resources.Load<AudioClip>("Sounds/EnemySounds/goblin")},
        {"piphiy", Resources.Load<AudioClip>("Sounds/EnemySounds/piphiy")},
        {"undead", Resources.Load<AudioClip>("Sounds/EnemySounds/undead")},
    };
    }

    public static void PlaySound(string soundName, float volume = 1.0f, bool loop = false)
    {
        if (soundDictionary.ContainsKey(soundName))
        {
            AudioClip sound = soundDictionary[soundName];

            if (loop) StopBackgroundMusic();

            GameObject soundObject = new GameObject("SoundObject");
            AudioSource audioSource = soundObject.AddComponent<AudioSource>();
            audioSource.clip = sound;
            audioSource.volume = volume * (float)(PixelSpawner.volume / 14);
            audioSource.loop = loop;
            audioSource.Play();

            if (loop) backgroundSongs.Add(audioSource);
            else Destroy(soundObject, sound.length);
        }
        else Debug.LogWarning("Sound with name " + soundName + " not found in sound dictionary.");
    }
    public static void StopBackgroundMusic()
    {
        foreach (AudioSource bgSound in backgroundSongs)
        {
            bgSound.Stop();
            Destroy(bgSound.gameObject);
        }
        backgroundSongs.Clear();
    }
    public static void UpdateAllSoundVolumes()
    {
        foreach (AudioSource audioSource in FindObjectsOfType<AudioSource>())
        {
            audioSource.volume = (float)(PixelSpawner.volume / 14);
        }
    }

}
>>>>>>> 12fa110b21928b31eb4877f8d5b5df0b12480307
