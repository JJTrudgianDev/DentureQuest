using UnityEngine;
using UnityEngine.UI;

using UnityEngine.SceneManagement;

public class MapManager : MonoBehaviour
{
	public Character Character;
	public Pin StartPin;
    public Pin[] Pins;
	public Text SelectedLevelText;

	int unlockedLevel = 1;
	
	/// <summary>
	/// Use this for initialization
	/// </summary>
	private void Start()
	{

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
		if (Input.GetKeyDown(KeyCode.W))
		{
			Character.TrySetDirection(Direction.Up);
		}
		else if(Input.GetKeyDown(KeyCode.S))
		{
			Character.TrySetDirection(Direction.Down);
		}
		else if(Input.GetKeyDown(KeyCode.A))
		{
			Character.TrySetDirection(Direction.Left);
		}
		else if(Input.GetKeyDown(KeyCode.D))
		{
			Character.TrySetDirection(Direction.Right);
		}
        else if (Input.GetKeyUp(KeyCode.Return))
        {
			SceneManager.LoadScene(Character.CurrentPin.SceneToLoad);
        }

        if (Input.GetKeyUp(KeyCode.P))
        {
            PlayerPrefs.SetInt("LevelUnlocked", 1);
            SceneManager.LoadScene("map");
        }


    }

	
	/// <summary>
	/// Update the GUI text
	/// </summary>
	public void UpdateGui()
	{
		//SelectedLevelText.text = string.Format("Current Level: {0}", Character.CurrentPin.SceneToLoad);
	}
}
