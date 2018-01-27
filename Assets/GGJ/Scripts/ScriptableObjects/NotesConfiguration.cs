using UnityEngine;

namespace GGJ.Scripts.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Notes/NotesConfiguration", fileName = "NotesConfiguration")]
    public class NotesConfiguration : ScriptableObject
    {
        public SingleNoteConfiguration[] NoteConfigurations;
    }
}