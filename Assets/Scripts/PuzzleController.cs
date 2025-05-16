using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleController : MonoBehaviour
{
  private List<PuzzleVariable> puzzleVariables = new();
  public GameObject mainController;

  public void Start()
  {
    mainController = GameObject.Find("MainController");
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

    if (allTrue)
    {
      Debug.Log("All puzzle variables are true!");
      mainController.GetComponent<MainController>().PuzzleComplete();
    }
  }
}

struct PuzzleVariable
{
  public string id;
  public bool value;
}
