using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class SoundManager : MonoBehaviour
{

    public List<AudioClip> sounds;
    public Dictionary<string,AudioClip> soundsDict;

    public AudioSource audioSource = new AudioSource();
    public AudioSource bgMusic = new AudioSource();
    public AudioSource[] soundList = new AudioSource[5];
     // add more audio sources if you add more sounds then add the name of the sound into loadSounds.txt 1 sound per line
     // then you call play sound by ID  where you need to and it should play the sound 
     // need to create audio sources on each object or access the audio sources on here
    void Start() 
    {
        //sounds = new List<AudioClip>();
        soundsDict = new Dictionary<string, AudioClip>();
        LoadSounds();
    }


    private void LoadSounds()
    {
        TextAsset txt = (TextAsset)Resources.Load("Sounds/loadSounds", typeof(TextAsset));
        string[] lines = Regex.Split(txt.text, "\n|\r|\r\n");

        foreach (string line in lines)
        {
            //sounds.Add(Resources.Load<AudioClip>("Sounds/" + line));
            soundsDict[line] = Resources.Load<AudioClip>("Sounds/" + line);

        }
    }
    public void SetToggleAudioLooping(AudioSource audioSource, bool toggle)
    {
        audioSource.loop = toggle;
    }

    public void PlayBackgroundMusic()
    {
        if (!bgMusic.isPlaying)
        {
            bgMusic.Play();
            bgMusic.loop = true;
        }
    }
    public void StopBackgroundMusic()
    {
        if (bgMusic.isPlaying)
        {
            bgMusic.Stop();
        }
    }
    public void PlaySound(int i) 
    {
        //Debug.Log(i);

        if (audioSource != null && !audioSource.isPlaying)
        {
            audioSource.clip = sounds[i];
            //s.loop = true;
            audioSource.Play();
        }
        // else { s.Stop(); }
    }

    public void PlaySoundByName(AudioSource audioSource, string name)
    {
        //Debug.Log(i);

        if (audioSource != null && !audioSource.isPlaying && soundsDict.ContainsKey(name))
        {
            audioSource.clip = soundsDict[name];
            //s.loop = true;
            audioSource.Play();
        }
        // else { s.Stop(); }
    }

    public void PlaySoundsByID(AudioSource s, int i)
    {
        //Debug.Log(i);

        if (s != null && !s.isPlaying)
        {
            s.clip = sounds[i];
            //s.loop = true;
            s.Play();
        }
        // else { s.Stop(); }
    }
    public void StopSound(AudioSource asource)
    {
        if (asource != null)
            asource.Stop();
    }

}


