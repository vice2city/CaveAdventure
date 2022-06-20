using UnityEngine;

public class AreaLightManager : MonoBehaviour
{
    public GameObject[] areaLight;
    // Start is called before the first frame update
    void Start()
    {
        foreach (var item in areaLight)
        {
            item.SetActive(false);
        }
    }

    public void ChangeAreaLightState(int n, bool b)
    {
        areaLight[n].SetActive(b);
    }
}
