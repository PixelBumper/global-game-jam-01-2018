using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MumbleCharacterMap", menuName = "GGJ/Humans", order = 1)]
public class MumbleCharacterMap : ScriptableObject
{
    [Serializable]
    private class MumbleCharacter
    {
        public AudioClip Mumble;
        public GameObject Human;
    }

    [SerializeField]
    private MumbleCharacter[] _map;

    private Dictionary<AudioClip, GameObject> _mumbleToHumans;
    public Dictionary<AudioClip, GameObject> MumbleToHumans
    {
        get
        {
            if(_mumbleToHumans == null)
            {
                _mumbleToHumans = new Dictionary<AudioClip, GameObject>(_map.Length);
                for (int i = 0; i < _map.Length; i++)
                {
                    var current = _map[i];
                    _mumbleToHumans[current.Mumble] = current.Human;
                }
            }

            return _mumbleToHumans;
        }
    }
}
