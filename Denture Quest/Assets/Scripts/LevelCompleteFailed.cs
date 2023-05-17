using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelCompleteFailed : MonoBehaviour
{
    public GameObject MenuButton01;
    public GameObject MenuButton02;

    public GameObject ContinueButton01;
    public GameObject ContinueButton02;



    // Start is called before the first frame update
    void Start()
    {
        MenuButton02.SetActive(false);
        ContinueButton02.SetActive(false);
    }

    public void MenuPointerEnter()
    {
        MenuButton01.SetActive(false);
        MenuButton02.SetActive(true);
    }

    public void MenuPointerExit()
    {
        MenuButton01.SetActive(true);
        MenuButton02.SetActive(false);
    }

    public void ContinuePointerEnter()
    {
        ContinueButton01.SetActive(false);
        ContinueButton02.SetActive(true);
    }

    public void ContinuePointerExit()
    {
        ContinueButton01.SetActive(true);
        ContinueButton02.SetActive(false);
    }
}
