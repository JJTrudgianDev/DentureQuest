using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager_02 : MonoBehaviour
{
    public TextMeshProUGUI textObject;
    private string[] dialogueLines;
    private int currentLineIndex = 0;
    private bool dialogueActive = false;

    public bool isGamePaused;
    public bool isMouseLocked;

    public MapManager Level;

   // public string[] LevelOneText;
   // public string[] LevelTwoText;
   // public string[] LevelThreeText;
   // public string[] LevelFourText;
   // public string[] LevelFiveText;
   // public string[] LevelSixText;

    // Update is called once per frame
    void Update()
    {
        if (dialogueActive)
        {
            if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Return) || Input.GetMouseButtonDown(0))
            {
                if (currentLineIndex < dialogueLines.Length - 1)
                {
                    currentLineIndex++;
                    DisplayNextLine();
                }
                else
                {
                    // Dialogue has ended
                    EndDialogue();
                }
            }
        }
    }

    public void StartDialogue(string[] dialogue)
    {
        dialogueLines = dialogue;
        currentLineIndex = 0;
        dialogueActive = true;
        textObject.gameObject.SetActive(true);
        textObject.text = dialogueLines[currentLineIndex];

        pauseGame();

        
    }

    private void pauseGame()
    {
        // Pause the game and lock the mouse cursor
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        isGamePaused = true;
        isMouseLocked = true; 
    }

    private void resumeGame()
    {
        // Resume the game and unlock the mouse cursor
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        isGamePaused = false;
        isMouseLocked = false;
    }

    private void DisplayNextLine()
    {
        textObject.text = dialogueLines[currentLineIndex];
    }

    private void EndDialogue()
    {
        dialogueActive = false;
        textObject.gameObject.SetActive(false);

        // Disable the panel containing the UI
        transform.parent.gameObject.SetActive(false);

        resumeGame();
    }
}
