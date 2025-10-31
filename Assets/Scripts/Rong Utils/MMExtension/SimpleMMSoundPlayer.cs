using UnityEngine;
using MoreMountains.Tools;

public class SimpleMMSoundPlayer : MonoBehaviour
{
    [Header("Sound Settings")]
    public AudioClip Clip;
    // The audio clip that will be played through MMSoundManager

    [Range(0f, 1f)]
    public float Volume = 1f;
    // Volume level of the sound (0 = silent, 1 = full volume)

    [Range(-3f, 3f)]
    public float Pitch = 1f;
    // Pitch adjustment of the sound (1 = normal pitch)

    public MMSoundManager.MMSoundManagerTracks Track = MMSoundManager.MMSoundManagerTracks.Music;
    // The track to play the sound on (Music, SFX, UI, etc.)

    public bool Loop = false;
    // Whether the sound should loop continuously

    [Header("3D Sound Settings")]
    [Range(0f, 1f)]
    public float SpatialBlend = 0f;
    // 0 = 2D (no spatialization), 1 = fully 3D

    public float MinDistance = 1f;
    // Minimum distance at which the sound is heard at full volume

    public float MaxDistance = 500f;
    // Maximum distance beyond which the sound is no longer audible

    public bool PlayOnStart = false;
    private AudioSource myAudio;

    private void Start()
    {
        // Automatically play the sound when this GameObject starts
        if (PlayOnStart)
        {
            PlayClip();
        }
    }

    public void PlayClip(AudioClip clip)
    {
        Clip = clip;
        PlayClip();
    }

    public void PlayClip()
    {
        // If no clip is assigned, warn and stop
        if (Clip == null)
        {
            Debug.LogWarning($"{name}: No AudioClip assigned to SimpleMMSoundPlayer.");
            return;
        }

        // Play a sound in 3D space using MMSoundManager
        myAudio = MMSoundManagerSoundPlayEvent.Trigger(
            Clip,                       // AudioClip to play
            Track,                      // Which track to play on
            transform.position,         // Position in world space
            loop: Loop,
            volume: Volume,
            pitch: Pitch,
            spatialBlend: SpatialBlend, // How 3D the sound is
            minDistance: MinDistance,   // Min distance for full volume
            maxDistance: MaxDistance    // Max distance before fading out
        );
    }

    /// <summary>
    /// Stops the currently playing sound.
    /// </summary>
    public void StopClip()
    {
        if (myAudio != null)
        {
            myAudio.Stop();
        }
        else
        {
            Debug.LogWarning($"{name}: No sound is currently playing to stop.");
        }
    }
}
