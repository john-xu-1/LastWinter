using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AudioHandler : MonoBehaviour
{
    public AudioClip[] MusicList;
    public AudioSource MusicSource;
    private int MusicTrack = 0;
    public static AudioHandler AHSingleton;

    private float trackStartTime;

    //public LevelHandler LH;

    public SpriteRenderer MuteIcon;
    public Sprite MuteOn;
    public Sprite MuteOff;
    [SerializeField] private static bool muted;

    public string MuteKey = "Mute";
    [SerializeField] private bool paused = false;

    public bool GetPaused() { return paused; }

    // Start is called before the first frame update
    void Start()
    {

        if (!AHSingleton)
        {
            AHSingleton = this;
            DontDestroyOnLoad(gameObject);
            startClip(MusicSource, MusicList[0]);

            //muted = MemoryHandler.GetBool(MuteKey);
            MusicSource.mute = muted;
        }
        else
        {
            Destroy(gameObject);
        }

    }

    // Update is called once per frame
    void Update()
    {
        //if (!MuteIcon)
        //{
        //    GameObject MuteIconObj = GameObject.FindGameObjectWithTag("Mute");
        //    if (MuteIconObj) MuteIcon = MuteIconObj.GetComponent<SpriteRenderer>();

        //}
        //else setMute(muted);

        //if (DebugController.DC.DisplayDebugPanel)
        //{
        //    if (!LH) LH = FindObjectOfType<LevelHandler>();
        //    else LH.DebugText.text = (int)GetRemainingTime() + " / " + ((int)MusicList[MusicTrack].length).ToString();
        //}
        if (!MusicSource.isPlaying  /*&& !paused*/)
        {
            MusicTrack += 1;
            MusicTrack = MusicTrack % MusicList.Length;
            startClip(MusicSource, MusicList[MusicTrack]);
            //if (!LevelHandler.CampaignMode) startClip(MusicSource, MusicList[MusicTrack]);
            //else paused = true;

            //FindObjectOfType<LevelHandler>().AudioChanged(this);
        }
    }

    public void RestartTracks()
    {
        MusicTrack = 0;
        MusicSource.Stop();
        paused = true;
        //startClip(MusicSource, MusicList[MusicTrack]);
    }

    public void StartMusic()
    {
        paused = false;
        startClip(MusicSource, MusicList[MusicTrack]);
    }

    private void startClip(AudioSource source, AudioClip clip)
    {

        trackStartTime = Time.fixedTime;
        source.clip = clip;
        source.Play();
    }

    public float GetRemainingTime()
    {
        return trackStartTime + MusicList[MusicTrack].length - Time.fixedTime;
    }

    public void ToggelMute()
    {
        muted = !muted;
        setMute(muted);
    }

    private void setMute(bool mute)
    {
        muted = mute;
        //MemoryHandler.SetBool(MuteKey, muted);
        if (muted)
        {
            MuteIcon.sprite = MuteOn;
        }
        else
        {
            MuteIcon.sprite = MuteOff;
        }

        MusicSource.mute = muted;
    }
}
