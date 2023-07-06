using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;

static public class Extender
{
    static public Vector3 SetX(this Vector3 v, float x)
    {
        return new Vector3(x, v.y, v.z);
    }

    static public Vector3 SetY(this Vector3 v, float y)
    {
        return new Vector3(v.x, y, v.z);
    }

    static public Vector3 SetZ(this Vector3 v, float z)
    {
        return new Vector3(v.x, v.y, z);
    }

    static public Color SetAlpha(this Color c, float a)
    {
        return new Color(c.r, c.g, c.b, a);
    }

    static public List<T> Randomise<T>(this List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int randomIndex = Random.Range(i, list.Count);
            T temp = list[i];
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }

        return list;
    }

    public static AudioSource PlayClipAtPoint(AudioClip clip, Vector3 position, float volume = 1.0f, AudioMixerGroup group = null, bool pitch = false, bool destroy = true)
    {
        if (clip == null) return null;
        GameObject gameObject = new GameObject("One shot audio");
        gameObject.transform.position = position;
        AudioSource audioSource = (AudioSource)gameObject.AddComponent(typeof(AudioSource));
        if (group != null)
            audioSource.outputAudioMixerGroup = group;
        audioSource.clip = clip;
        if (pitch) audioSource.pitch = UnityEngine.Random.Range(0.9f, 1.1f);
        audioSource.spatialBlend = 0f;
        audioSource.volume = volume;
        audioSource.Play();
        if (destroy) UnityEngine.Object.Destroy(gameObject, clip.length * (Time.timeScale < 0.009999999776482582 ? 0.01f : Time.timeScale));
        return audioSource;
    }

    public static bool IsNullOrEmpty<T>(this IEnumerable<T> enumerable)
    {
        if (enumerable == null)
        {
            return true;
        }
        /* If this is a list, use the Count property for efficiency. 
         * The Count property is O(1) while IEnumerable.Count() is O(N). */
        var collection = enumerable as ICollection<T>;
        if (collection != null)
        {
            return collection.Count < 1;
        }
        return !enumerable.Any();
    }

    public static void CleanPlay(this ParticleSystem p)
    {
        var main = p.main;
        main.stopAction = ParticleSystemStopAction.Destroy;
        p.transform.SetParent(null);
        p.Play();
    }
}