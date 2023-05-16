using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.XR.WSA;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MainMenu : MonoBehaviour
{
    public GameObject AcceptInvitation;
    public GameObject Play;

    public GameObject SeeDetails;
    public GameObject Options;

    public GameObject DeclineInvitation;
    public GameObject Quit;

    public GameObject HearingAids;
    public GameObject Volume;

    public GameObject Glasses;
    public GameObject NativeResolution;

    public GameObject Parkinsons;
    public GameObject Sensitivity;

    public GameObject MainMenuPanel;
    public GameObject OptionsPanel;




    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Play.SetActive(false);
        Options.SetActive(false);
        Quit.SetActive(false);
      
        StartCoroutine(toggleMouse());
    }

    public IEnumerator toggleMouse()
    {
        Debug.Log("start");
        yield return new WaitForSeconds(2f);
        Debug.Log("gksdf");
        Cursor.lockState = CursorLockMode.None;
        yield return null;

    }

    public void DoMap()
    {
        SceneManager.LoadScene("map");
    }

    public void DoMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void PlayPointerEnter() 
    {
        AcceptInvitation.SetActive(false);
        Play.SetActive(true);

    }

    public void PlayPointerExit()
    {
        AcceptInvitation.SetActive(true);
        Play.SetActive(false);
    }

    public void OptionsPointerEnter()
    {
        SeeDetails.SetActive(false);
        Options.SetActive(true);
    }

    public void OptionsPointerExit()
    {
        Options.SetActive(false);
        SeeDetails.SetActive(true);
    }

    public void QuitPointerEnter()
    {
        Quit.SetActive(true);
        DeclineInvitation.SetActive(false);
    }

    public void QuitPointerExit()
    {
        DeclineInvitation.SetActive(true);
        Quit.SetActive(false);
    }

    public void VolumePointerEnter()
    {
        Volume.SetActive(true);
        HearingAids.SetActive(false);
    }

    public void VolumePointerExit()
    {
        HearingAids.SetActive(true);
        Volume.SetActive(false);
    }

    public void ResPointerEnter()
    {
       NativeResolution.SetActive(true);
       Glasses.SetActive(false);
    }

    public void ResPointerExit()
    {
        Glasses.SetActive(true);
        NativeResolution.SetActive(false);
    }

    public void SensPointerEnter()
    {
        Sensitivity.SetActive(true);
        Parkinsons.SetActive(false);
    }

    public void SensPointerExit()
    {
        Parkinsons.SetActive(true);
        Sensitivity.SetActive(false);
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
