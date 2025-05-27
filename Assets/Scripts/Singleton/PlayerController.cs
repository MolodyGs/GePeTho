using System.Threading.Tasks;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
  public static PlayerController Instance { get; private set; }

  // Player variables
  public GameObject playerPrefab;
  public GameObject player;

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

  public async Task SetPlayerPosition()
  {
    try
    {
      LevelVariables levelVariables = await SceneController.Instance.GetLevelVariables();
      GameObject playerPositionReference = levelVariables.playerPositionReference;
      player.transform.position = new Vector3(playerPositionReference.transform.position.x, playerPositionReference.transform.position.y, playerPositionReference.transform.position.z);
      playerPositionReference.GetComponent<SpriteRenderer>().enabled = false;
    }
    catch (System.Exception e)
    {
      throw new System.Exception("Player position reference not found!", e);
    }
  }

  public void SetPlayerActive(bool value)
  {
    try
    {
      player.GetComponent<SpriteRenderer>().enabled = value;
    }
    catch (System.Exception e)
    {
      throw new System.Exception("Player sprite renderer not found!", e);
    }
  }

  public void BlockMovement(bool value = true)
  {
    try
    {
      Debug.Log("bloqueando el movimiento?: " + value);
      player.GetComponent<Conditions>().blockMovement = value;
    }
    catch (System.Exception e)
    {
      throw new System.Exception("Player conditions not found!", e);
    }
  }

  public void InstantiatePlayer()
  {
    Debug.Log("Instantiating player...");
    try
    {
      player = Instantiate(playerPrefab, new(-100, -100, 0), Quaternion.identity);
    }
    catch (System.Exception e)
    {
      throw new System.Exception("Error loading player prefab!", e);
    }
  }

  public GameObject GetPlayer()
  {
    if (player != null)
    {
      return player;
    }
    else
    {
      Debug.LogWarning("Player not instantiated, searching by tag...");
      player = GameObject.FindWithTag("Player");
      if (player != null)
      {
        return player;
      }
      throw new System.Exception("Player not instantiated!");
    }
  }

  public void SetPlayerLightOn()
  {
    player.GetComponent<LightController>().SetLightOn();
  }

  public void SetPlayerLightOff()
  {
    player.GetComponent<LightController>().SetLightOff();
  }
}
