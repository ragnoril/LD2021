using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIManager : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Slider SliderSFX, SliderMusic;
    public GameObject ImageSFX, ImageMusic;
    public bool PointerOverUI;
    private void Start()
    {
        
    }
    public void DigCommand()
    {
        GameManager.instance.GameMode = GameModes.Dig;
    }

    public void BuildCommand(int buildingId)
    {
        GameManager.instance.GameMode = GameModes.Build;
        GameManager.instance.PrepareBuilding(buildingId);
    }

    public void SFXMuteCommand()
    {
        bool isPlaying = GameManager.instance.SfxPlayer.IsPlaying;
        ImageSFX.SetActive(isPlaying);
        GameManager.instance.SfxPlayer.IsPlaying = !isPlaying;
    }
    public void MusicMuteCommand()
    {
        bool isPlaying = GameManager.instance.MusicPlayer.IsPlaying;
        ImageMusic.SetActive(isPlaying);
        GameManager.instance.MusicPlayer.IsPlaying = !isPlaying;
    }

    public void ButtonClick()
    {
        GameManager.instance.SfxPlayer.PlaySfx(Random.Range(2, 4));
    }

    public void SetMusicVolume()
    {
        GameManager.instance.MusicPlayer.SetVolume(SliderMusic.value);
    }

    public void SetSFXVolume()
    {
        GameManager.instance.SfxPlayer.SetVolume(SliderSFX.value);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        PointerOverUI = false;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        PointerOverUI = true;
    }


}
