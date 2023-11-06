using UnityEngine;

[CreateAssetMenu(fileName = "MusicData", menuName = "Custom/MusicSO")]
public class MusicDataSO : ScriptableObject
{
    public AudioClip[] Clips;

    public AudioClip GetClip(string name)
    {
        foreach (var clip in Clips)
        {
            if (clip.name == name)
            {
                return clip;
            }
        }
        return null;
    }
}

