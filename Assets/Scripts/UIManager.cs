using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIManager : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Slider SliderSFX, SliderMusic;
    public GameObject ImageSFX, ImageMusic, GameOverPanel;
    public bool PointerOverUI;
    public Text Dwarf, Food, Bed, Fun, Ore, Storage, EnergyIn, EnergyOut, GameOverDaysPlayed, Day, Time;
    private void Start()
    {

    }
    public void DigCommand()
    {
        GameManager.instance.GameMode = GameModes.Dig;
    }

    string[] periods = new string[] { "Morning", "Noon", "Evening", "Night" };

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

    public void ShowGameOverPanel()
    {
        GameOverDaysPlayed.text = GameManager.instance.DayCycle.DayCounter.ToString();
        GameOverPanel.SetActive(true);
    }

    public void BackToMainMenu()
    {

    }

    public void UpdateDayTimeUI()
    {
        Day.text = GameManager.instance.DayCycle.DayCounter.ToString();
        Time.text = periods[GameManager.instance.DayCycle.PeriodCounter];
    }

}
