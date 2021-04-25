using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SoundClips
{
    BuildingBuilt,
    BuildingBroken,
    ButtonClick1,
    ButtonClic2,
    Digging1,
    Digging2,
    EnergyLow,
    TaskAdd1,
    TaskAdd2,
    TaskCancel,
    WorkerDead1,
    WorkerDead2,
    WorkerDead3,
    WorkerNewTask,
    WorkerHungry,
    WorkerMoody1,
    WorkerMoody2,
    WorkerMoody3,
    WorkerMoody4,
    WorkerSleepy
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

}
