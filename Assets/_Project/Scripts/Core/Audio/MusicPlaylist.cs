using UnityEngine;
using Sirenix.OdinInspector;

namespace DaftAppleGames.Darskerry.Core.Audio
{
    [CreateAssetMenu(fileName = "MusicPlaylist", menuName = "Daft Apple Games/Audio/Music Playlist", order = 1)]
    public class MusicPlaylist : ScriptableObject
    {
        // Public serializable properties
        [BoxGroup("Music")] public MusicClip[] musicClips;

    }
}