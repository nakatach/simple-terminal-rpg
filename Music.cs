using System;
using NAudio.Wave;

public class Music
{
    private WaveOutEvent outputDevice;
    private AudioFileReader audioFileReader;

    public Music()
    {
        outputDevice = new WaveOutEvent();
    }

    public void PlayMusic(string path)
    {
        StopMusic();

        audioFileReader = new AudioFileReader(path);
        outputDevice.Init(audioFileReader);
        outputDevice.Play();
    }

    public void StopMusic()
    {
        if (outputDevice.PlaybackState == PlaybackState.Playing)
        {
            outputDevice.Stop();
        }
    }

    public void ChangeMusic(string path)
    {
        PlayMusic(path);
    }
}
