using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class PuzzleController : MonoBehaviour
{
  public static PuzzleController Instance { get; private set; }

  private List<PuzzleVariable> puzzleVariables = new();
  public DoorController door;

  private void Awake()
  {
    if (Instance != null && Instance != this)
    {
      Destroy(gameObject);
      return;
    }

    Instance = this;
    DontDestroyOnLoad(gameObject);
  }

  public void SetVarible(string id, bool value)
  {
    // Set the variable in the puzzle
    Debug.Log($"Setting variable {id} to {value}");
    puzzleVariables.Add(new PuzzleVariable { id = id, value = value });
  }

  public void UpdateVariable(string id, bool value)
  {
    // Update the variable in the puzzle
    Debug.Log($"Updating variable {id} to {value}");
    for (int i = 0; i < puzzleVariables.Count; i++)
    {
      if (puzzleVariables[i].id == id)
      {
        Debug.Log($"Updating variable {id} to {value} Correctly");
        puzzleVariables[i] = new PuzzleVariable { id = id, value = value };
        break;
      }
    }

    CheckPuzzleVariables();
  }

  void CheckPuzzleVariables()
  {
    Debug.Log("Checking puzzle variables...");

    bool allTrue = true;

    // Check the puzzle variables and perform actions based on their values
    foreach (var variable in puzzleVariables)
    {
      if (variable.value == false)
      {
        allTrue = false;
        return;
      }
    }

    if (allTrue) PuzzleComplete();
  }

  void CleanVariables()
  {
    Debug.Log("Cleaning puzzle variables...");
    puzzleVariables.Clear();
  }

  public async void PuzzleComplete()
  {
    Debug.Log("All puzzle variables are true!");
    Debug.Log("Puzzle complete");

    // Clean up the puzzle variables
    CleanVariables();

    if (door == null)
    {
      Debug.LogWarning("Puzzle door is null. Attempting to set door...");
      if (await SetDoor())
      {
        Debug.Log("Puzzle door set successfully.");
      }
      else
      {
        Debug.LogError("Failed to set puzzle door. Cannot open door.");
        return;
      }
      return;
    }

    door.OpenDoor();
    door = null;
  }

  // For testing purposes, force the puzzle to be complete
  public void ForcePuzzleComplete()
  {
    Debug.Log("Forcing puzzle completion...");
    PuzzleComplete();
  }

  async Task<bool> SetDoor()
  {
    try
    {
      await SceneController.Instance.WaitUntilLevelIsLoaded();
      LevelVariables levelVariables = await SceneController.Instance.GetLevelVariables();
      door = levelVariables.door;
      return true;
    }
    catch (System.Exception e)
    {
      Debug.LogError("<color=red>Error finding puzzle door: </color>" + e.Message);
      return false;
    }
  }

  public async void InitialState()
  {
    Debug.Log("Setting default state for puzzle variables...");
    CleanVariables();
    await SetDoor();
  }
}


struct PuzzleVariable
{
  public string id;
  public bool value;
}
