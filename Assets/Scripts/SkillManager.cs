using UnityEngine;

// -1:spare 0:shoes 1:timeStone 2:bomb
public class SkillManager : MonoBehaviour
{
    public float spareVolume;
    public float shoesSpeed;
    public float timeStoneVolume;
    public GameObject bomb;

    private GameObject player;
    private PlayerManager controller;
    
    private bool shoesState;
    private Vector2 moveSpeed;
    private GameObject bombObj;
    // Start is called before the first frame update
    private void Start()
    {
        player = GameManager.instance.GetPlayer();
        controller = player.GetComponent<PlayerManager>();
        shoesState = false;
        moveSpeed = controller.moveSpeed;
    }

    // Update is called once per frame
    private void Update()
    {
        if (GameManager.instance.IsGamePause()) return;
 
        if (Input.GetKeyDown(KeyCode.Q))
        {
            var result = GameManager.instance.UseSpare();
            if (result)
            {
                AudioManager.instance.PlaySfx(AudioManager.instance.useSpare);
                controller.ChangeLightTime(spareVolume);
            }
        }
        
        if (GameManager.instance.IsSkillObtained(0)&&
            (Input.GetKeyDown(KeyCode.LeftShift)||Input.GetKeyDown(KeyCode.RightShift)))
            ChangeShoesState(!shoesState);

        if (GameManager.instance.IsSkillObtained(1) && Input.GetKeyDown(KeyCode.Space))
            if (!controller.IsTimePause() && !GameManager.instance.IsGamePause()) 
                controller.ChangeLightTime(timeStoneVolume);
        
        
        if (GameManager.instance.IsSkillObtained(2)&&Input.GetKeyDown(KeyCode.F))
            CreateBomb();
    }
    
    private void ChangeShoesState(bool state)
    {
        AudioManager.instance.PlaySfx(AudioManager.instance.useShoes);
        shoesState = state;
        controller.moveSpeed = state ? moveSpeed*shoesSpeed : moveSpeed;
        controller.ChangeStepsAnim(state ? 1.5f : 1f);
    }

    private void CreateBomb()
    {
        if (bombObj) return;
        bombObj = Instantiate(bomb, transform);
        bombObj.transform.position = player.transform.position - new Vector3(0,0.3f,0);
    }
}
