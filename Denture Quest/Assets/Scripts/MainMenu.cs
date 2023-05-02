using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenu : MonoBehaviour
{
    public TextMeshProUGUI AcceptInvitation;
    public TextMeshProUGUI Play;

    public TextMeshProUGUI SeeDetails;
    public TextMeshProUGUI Options;

    public TextMeshProUGUI DeclineInvitation;
    public TextMeshProUGUI Quit;

    public TextMeshProUGUI HearingAids;
    public TextMeshProUGUI Volume;

    public TextMeshProUGUI Glasses;
    public TextMeshProUGUI NativeResolution;

    public TextMeshProUGUI Parkinsons;
    public TextMeshProUGUI Sensitivity;

    public GameObject MainMenuPanel;
    public GameObject OptionsPanel;

    void Start()
    {
        Play.enabled = false;
    }
    public void DoMap()
    {
        SceneManager.LoadScene("map");
    }

    public void PlayPointerEnter() 
    {
        AcceptInvitation.enabled = false;
        Play.enabled = true;
    }

    public void PlayPointerExit()
    {
        AcceptInvitation.enabled = true;
        Play.enabled = false;
    }

    public void OptionsPointerEnter()
    {
        SeeDetails.enabled = false;
        Options.enabled = true;
    }

    public void OptionsPointerExit()
    {
        Options.enabled = false;
        SeeDetails.enabled = true;
    }

    public void QuitPointerEnter()
    {
        Quit.enabled = true;
        DeclineInvitation.enabled = false;
    }

    public void QuitPointerExit()
    {
        DeclineInvitation.enabled = true;
        Quit.enabled = false;
    }

    public void VolumePointerEnter()
    {
        Volume.enabled = true;
        HearingAids.enabled = false;
    }

    public void VolumePointerExit()
    {
        HearingAids.enabled = true;
        Volume.enabled = false;
    }

    public void ResPointerEnter()
    {
       NativeResolution.enabled = true;
       Glasses.enabled = false;
    }

    public void ResPointerExit()
    {
        Glasses.enabled = true;
        NativeResolution.enabled = false;
    }

    public void SensPointerEnter()
    {
        Sensitivity.enabled = true;
        Parkinsons.enabled = false;
    }

    public void SensPointerExit()
    {
        Parkinsons.enabled = true;
        Sensitivity.enabled = false;
    }

    public void DoQuitGame()
    {
        Application.Quit();
    }

    public void GoToOptions()
    {
        MainMenuPanel.SetActive (false);
        OptionsPanel.SetActive (true);
    }

    public void GoBack()
    {
        MainMenuPanel.SetActive (true);
        OptionsPanel.SetActive (false);
    }
}
