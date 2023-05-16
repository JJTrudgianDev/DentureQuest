using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PreLevelOneDialogue : MonoBehaviour
{
    public DialogueManager_02 dialogueManager;
    public string[] dialogueArray;

    public string[] LevelOneText;
    public string[] LevelTwoText;
    public string[] LevelThreeText;
    public string[] LevelFourText;
    public string[] LevelFiveText;
    public string[] LevelSixText;

    public MapManager Level;

    private void Start()
    {
        // Start the dialogue after a delay (for demonstration purposes)
        Invoke("StartDialogue", 0f);
    }

    private void StartDialogue()
    {
        
        
       if (Level.unlockedLevel == 1)
       {
           dialogueArray = LevelOneText;
       }
       else if (Level.unlockedLevel == 2)
       {
           dialogueArray = LevelTwoText;
       }
       else if (Level.unlockedLevel == 3)
       {
           dialogueArray = LevelThreeText;
       }
       else if (Level.unlockedLevel == 4)
       {
           dialogueArray = LevelFourText;
       }
       else if (Level.unlockedLevel == 5)
       {
           dialogueArray = LevelFiveText;
       }
       else if (Level.unlockedLevel == 6)
       {
           dialogueArray = LevelSixText;
       }
        
       

        dialogueManager.StartDialogue(dialogueArray);
    }
}
