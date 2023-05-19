using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MapManager : MonoBehaviour
{
    public Character Character;
    public Pin StartPin;
    public Pin[] Pins;
    public Text SelectedLevelText;

    public DialogueManager_02 dialogueManager;

    public bool isGamePaused;
    public bool isMouseLocked;

    public GameObject[] pinTexts; // Array of UI text elements representing pins
    private bool isDialogueActive = false;

    public int unlockedLevel = 1;

    /// <summary>
    /// Use this for initialization
    /// </summary>
    private void Start()
    {

        // Initialize the game pause and mouse lock flags from the DialogueManager
        isGamePaused = dialogueManager.isGamePaused;
        isMouseLocked = dialogueManager.isMouseLocked;


        if (!PlayerPrefs.HasKey("LevelUnlocked"))
        {
            PlayerPrefs.SetInt("LevelUnlocked", 1);
        }

        unlockedLevel = PlayerPrefs.GetInt("LevelUnlocked");

        for (int i = 0; i < unlockedLevel; i++)
        {
            Pins[i].isUnlocked = true;
            if (i == unlockedLevel - 1)
            {
                if (i == 0)
                {
                    StartPin = Pins[0];
                }
                else
                {
                    StartPin = Pins[i - 1];
                }
            }

        }

        // Pass a ref and default the player Starting Pin
        Character.Initialise(this, StartPin);

    }


    /// <summary>
    /// This runs once a frame
    /// </summary>
    private void Update()
    {
        // Update the dialogue state
        CheckDialogueState();

        // makes sure that the script knows whether the game is paused and the mouse is locked
        isGamePaused = dialogueManager.isGamePaused;
        isMouseLocked = dialogueManager.isMouseLocked;
        // Only check input when the game is not paused and mouse movement is not locked
        if (isGamePaused || isMouseLocked)
            return;

        // Only check input when character is stopped
        if (Character.IsMoving) return;

        // First thing to do is try get the player input
        CheckForInput();
    }


    /// <summary>
    /// Check if the player has pressed a button
    /// </summary>
    private void CheckForInput()
    {
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            Character.TrySetDirection(Direction.Up);
        }
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            Character.TrySetDirection(Direction.Down);
        }
        else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Character.TrySetDirection(Direction.Left);
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            Character.TrySetDirection(Direction.Right);
        }
        else if (Input.GetKeyUp(KeyCode.E) || Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene(Character.CurrentPin.SceneToLoad);
        }

        if (Input.GetKeyUp(KeyCode.P))
        {
            PlayerPrefs.SetInt("LevelUnlocked", 1);
            SceneManager.LoadScene("map");
        }


    }

    private void CheckDialogueState()
    {
        isDialogueActive = dialogueManager.dialogueActive; 
    }
	
	/// <summary>
	/// Update the GUI text
	/// </summary>
	public void UpdateGui()
	{
        // Check if dialogue is active
        if (isDialogueActive == true)
        {
            // Disable all text elements
            for (int i = 0; i < pinTexts.Length; i++)
            {
                pinTexts[i].gameObject.SetActive(false);
            }
            return;
        }

        // Iterate through all pinTexts
        for (int i = 0; i < pinTexts.Length; i++)
        {
            // Check if the current pin matches and the next pin is unlocked
            if (Character.CurrentPin == Pins[i] && (i + 1 < Pins.Length && Pins[i + 1].isUnlocked))
            {
                pinTexts[i].gameObject.SetActive(true);
            }
            else
            {
                pinTexts[i].gameObject.SetActive(false);
            }
        }
    }
}
