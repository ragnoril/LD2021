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
    public Text Dwarf, Food, Bed, Fun, Ore, Storage, EnergyIn, EnergyOut;
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

    public void UpdateStatsUI()
    {
        Dwarf.text = GameManager.instance.Workers.Count.ToString();
        EnergyIn.text = GameManager.instance.GetEnergySupplyAmount().ToString();
        EnergyOut.text = GameManager.instance.GetEnergyDrainAmount().ToString();
        Bed.text = GameManager.instance.GetBedSupplyAmount().ToString();
        Food.text = GameManager.instance.GetDinerCapacity().ToString();
        Fun.text = GameManager.instance.GetEntertainmentCapacity().ToString();
        Storage.text = GameManager.instance.GetOreStorageCapacity().ToString();
        Ore.text = GameManager.instance.OreAmount.ToString();
    }

    public void CancelSelection()
    {
        GameManager.instance.GameMode = GameModes.Cancel;
    }

    public void AddDwarf()
    {
        GameManager.instance.CreateWorkers(1);
    }

}
