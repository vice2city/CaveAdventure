using UnityEngine;

public class AreaLightManager : MonoBehaviour
{
    public int caveId;

    public void ChangeAreaLightState(bool state)
    {
        this.gameObject.SetActive(state);
    }
}
