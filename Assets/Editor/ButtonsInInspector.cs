using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(PuzzleController))]
public class ButtonsInInspector : Editor
{
  public override void OnInspectorGUI()
  {
    DrawDefaultInspector();

    PuzzleController myScript = (PuzzleController)target;
    if (GUILayout.Button("Force Puzzle Complete"))
    {
      myScript.ForcePuzzleComplete();
    }
  }
}