using UnityEngine;

namespace GGJ.Scripts.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Notes/SingleNoteConfiguration", fileName = "SingleNoteConfiguration")]
    public class SingleNoteConfiguration : ScriptableObject
    {
        public AudioClip Note;

        public Sprite Sprite;
    }
}