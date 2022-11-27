using UnityEngine;
[RequireComponent(typeof(AudioSource))]
public class AudioYB : MonoBehaviour
{
    AudioSource source;
    bool load;
    bool play;
    bool playLoop;
    float volumeScale;
    private void Awake() => source = GetComponent<AudioSource>();
    private void Update()
    {
        if (source.loop) { source.loop = false; loop = true; }
        Loop(loop);
    }
    void PlayAfter()
    {
        play = false;
        playLoop = true;
        source.Play();
    }
    void LoadAfter(AudioClip clip)
    {
        source.clip = clip;
        load = true;
        if (play) PlayAfter();
    }

    public void Play()
    {
        if (load) { source.Play(); playLoop = true; }

        else play = true;
    }
    public void Play(string name)
    {
        Clip clip = AudioStreamCash.Find(name);
        if (clip == null) return;

        load = false;
        play = true;
        StartCoroutine(clip.GetFile(LoadAfter));
    }
    private void LoadShotAfter(AudioClip clip, float volumeScale)
    {
        source.PlayOneShot(clip, volumeScale);
    }

    public void PlayOneShot(string clipName, float volumeScale = 1)
    {
        var clip = AudioStreamCash.Find(clipName);
        if (clip == null)
            return;
        StartCoroutine(clip.GetFile(delegate (AudioClip audioClip) { LoadShotAfter(audioClip, volumeScale); }));
    }


    private void Loop(bool enable)
    {
        if (source.time == 0 && enable && playLoop) { source.Play(); }
    }
    public void Pause() => source.Pause();
    public void UnPause() => source.UnPause();
    public void Stop()
    {
        playLoop = false;
        source.Stop();
    }
    public bool isPlaying { get => source.isPlaying; }
    public bool loop { get; set; }
    public float volume { get => source.volume; set => source.volume = value; }
    public bool mute { get => source.mute; set => source.mute = value; }
    public float time { get => source.time; set => source.time = value; }
    public bool Enabled { get => source.enabled; set => source.enabled = value; }
    public string clip { get => source.clip.name; set => source.clip = AudioStreamCash.Find(value).Cash; }
}

