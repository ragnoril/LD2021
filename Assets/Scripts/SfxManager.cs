using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SoundClips
{
    BuildingBuilt,//0
    BuildingBroken,//1
    ButtonClick1,//2
    ButtonClic2,//3
    Digging1,//4
    Digging2,//5
    EnergyLow,//6
    TaskAdd1,//7
    TaskAdd2,//8
    TaskCancel,//9
    WorkerDead1,//10
    WorkerDead2,//11
    WorkerDead3,//12
    WorkerNewTask,//13
    WorkerHungry,//14
    WorkerMoody1,//15
    WorkerMoody2,//16
    WorkerMoody3,//17
    WorkerMoody4,//18
    WorkerSleepy//19
};

public class SfxManager : MonoBehaviour
{
    public AudioSource Player;

    public AudioClip[] Clips;
    /*
     *  0 button click
     *  1 coin collect
     *  2 crash
     *  3 level completed
     *  4 car turning
     */


    public bool IsPlaying;


    public void PlaySfx(int id)
    {
        if (!IsPlaying)
            return;

        if (id >= Clips.Length)
            return;

        if (Clips[id] == null)
            return;

        if (Player == null)
            GetAudioSource();

        Player.PlayOneShot(Clips[id]);
    }

    public void GetAudioSource()
    {
        Player = GetComponent<AudioSource>();
    }
    public void SetVolume(float val)
    {
        Player.volume = val;
    }

    public float GetVolume()
    {
        return Player.volume;
    }



}
