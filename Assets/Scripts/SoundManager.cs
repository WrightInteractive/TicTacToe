using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviourSingleton<SoundManager>
{
    [Tooltip("GameObject that has AudioSource")][SerializeField] GameObject audioParent;
    [Tooltip("List of sounds")][SerializeField] List<Sound> sounds = new List<Sound>();
    

    //Sets the an AudioSource for every sound, this allows multiple sounds to play at the same time and not stop the previous sound
    public void Start()
    {
        for (int i = 0; i < sounds.Count; i++)
        {
            GameObject _obj = new GameObject("Sound_" + i + "_" + sounds[i].name);
            _obj.transform.parent = audioParent.transform;
            sounds[i].SetSource(_obj.AddComponent<AudioSource>());
        }
    }


    //Iterates through all the sounds in the sound list, will play the matching sound by name comparison
    public void PlaySound(string _name)
    {
        for (int i = 0; i < sounds.Count; i++)
        {
            if(sounds[i].name == _name)
            {
                sounds[i].Play();
                return;
            }
        }

        Debug.LogError("Sound " + _name + "could not be found");
    }
}

[System.Serializable]
public class Sound
{
    [Tooltip("Name of sound")]public string name;
    [Tooltip("Audioclip of sound")]public AudioClip audioClip;
    [Tooltip("Volume of sound")][Range(0,1)] public float volume;
    [Tooltip("Pitch of sound")][Range(0,1)] public float pitch;
    private AudioSource source;     //AudioSource that is set in the Start function of the SoundManager


    //Sets the audiosource for the Sound
    public void SetSource(AudioSource _source)
    {
        source = _source;
        source.clip = audioClip;
    }

    //Plays the audioclip with set variables
    public void Play()
    {
        source.volume = volume;
        source.pitch = pitch;
        source.Play();
    }

}
