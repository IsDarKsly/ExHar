using UnityEngine;

[System.Serializable]
public class Options
{
    // Graphics Settings
    public Vector2Int resolution = new Vector2Int(1920, 1080); // Default resolution
    public bool isFullscreen = false;                          // Fullscreen toggle

    // Audio Settings
    public float masterVolume = 1.0f; // Range [0.0, 1.0]
    public float musicVolume = 1.0f;  // Range [0.0, 1.0]
    public float effectsVolume = 1.0f; // Range [0.0, 1.0]
    public float voiceVolume = 1.0f;  // Range [0.0, 1.0]

    // Language Settings
    public string language = "English"; // Default language
}
