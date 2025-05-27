using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightController : MonoBehaviour
{
  public Light2D lightSource;

  public void SetLightOff()
  {
    if (lightSource != null)
    {
      StartCoroutine(LightOff());
    }
  }

  public void SetLightOn()
  {
    if (lightSource != null)
    {
      StartCoroutine(LightOn());
    }
  }

  IEnumerator LightOff()
  {
    while (lightSource.intensity > 0)
    {
      lightSource.intensity -= 0.1f;
      yield return new WaitForSeconds(0.1f);
    }
  }

  IEnumerator LightOn()
  {
    while (lightSource.intensity < 1)
    {
      lightSource.intensity += 0.1f;
      yield return new WaitForSeconds(0.1f);
    }
  }

}
