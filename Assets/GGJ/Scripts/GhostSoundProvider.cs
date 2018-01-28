using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostSoundProvider : MonoBehaviour
{
    public Animator[] Animators;

    public AudioClip[] AudioClips;

    public static GhostSoundProvider FindOnScene()
    {
        return FindObjectOfType<GhostSoundProvider>();
    }

    private List<int> _availableIndexes = new List<int>();

    private void Awake()
    {
        for (int i = 0; i < Animators.Length; i++)
        {
            _availableIndexes.Add(i);
        }
    }

    public int GetAvailableIndex()
    {
        var index = UnityEngine.Random.Range(0, _availableIndexes.Count);
        var value = _availableIndexes[index];
        _availableIndexes.RemoveAt(index);

        return value;
    }

}
