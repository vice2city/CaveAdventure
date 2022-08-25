using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Rendering.Universal;

public class PlayerManager : MonoBehaviour
{
    public GameObject light2D;
    public Vector2 moveSpeed;
    public float initialLightTime;

    private Rigidbody2D rigidbody2d;
    private Animator animator; 

    private Vector2 movement;
    private int keyCount;
    private int playerState;
    private float lightTime;
    private bool pauseTime;
    private Queue<Vector2Int> obtainedKey;
    private static readonly int MoveX = Animator.StringToHash("moveX");
    private static readonly int MoveY = Animator.StringToHash("moveY");
    private static readonly int Speed = Animator.StringToHash("speed");

    private GameObject keyboardToast;
    private static readonly int AnimSpeed = Animator.StringToHash("animSpeed");

    private Light2D globalLight;
    private float lightIntensity;

    public enum TimeChangeType {Add, Replace};

    // Start is called before the first frame update
    private void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        obtainedKey = new Queue<Vector2Int>();
        keyboardToast = transform.Find("keyboardToast").gameObject;
        globalLight = GameObject.FindWithTag("GlobalLight").GetComponent<Light2D>();
        lightIntensity = globalLight.intensity;

        keyCount = 0;
        lightTime = initialLightTime;
        playerState = 0;
        AudioManager.instance.PlayBGM(0);
        pauseTime = false;
        keyboardToast.SetActive(false);
    }

    // Update is called once per frame
    private void Update()
    {
        if (GameManager.instance.IsGamePause()) return;
        movement.x = Input.GetAxis("Horizontal");
        movement.y = Input.GetAxis("Vertical");

        if (movement != Vector2.zero)
        {
            animator.SetFloat(MoveX, movement.x);
            animator.SetFloat(MoveY, movement.y);
        }
        animator.SetFloat(Speed, movement.magnitude);

        light2D.SetActive(!(GameManager.instance.IsCaveFinish(playerState) || pauseTime));
    }

    private void FixedUpdate()
    {
        if (GameManager.instance.IsGamePause()) return;
        
        var position = rigidbody2d.position;
        position.x += moveSpeed.x * movement.x * Time.deltaTime;
        position.y += moveSpeed.y * movement.y * Time.deltaTime;

        rigidbody2d.MovePosition(position);

        if (GameManager.instance.IsCaveFinish(playerState) || pauseTime) return;
        lightTime -= Time.deltaTime;
        ChangeLightIntensity();
        if (lightTime > 0) return;
        lightTime = 0.0f;
        PauseLightTime(true);
        GameManager.instance.GameOver();
    }

    public void GetKey(Vector2Int n)
    {
        obtainedKey.Enqueue(n);
        keyCount = obtainedKey.Count;
    }

    public Vector2Int UseKey()
    {
        keyCount = obtainedKey.Count-1;
        return obtainedKey.Dequeue();
    }

    public void ChangePlayerState(int n)
    {
        playerState = n;
        AudioManager.instance.PlayBGM(n);
    }

    public void ChangeLightTime(float d, TimeChangeType t = TimeChangeType.Add)
    {
        switch (t)
        {
            case TimeChangeType.Add when lightTime + d < 0:
                lightTime = 0;
                return;
            case TimeChangeType.Add:
                lightTime += d;
                break;
            case TimeChangeType.Replace:
                lightTime = d;
                break;
            default:
                break;
        }
    }

    public float GetLightTime() 
    { 
        return lightTime; 
    }  

    public int GetKeyCount()
    {
        return keyCount;
    }

    public void PauseLightTime(bool b)
    {
        pauseTime = b;
    }

    public bool IsTimePause()
    {
        return GameManager.instance.IsCaveFinish(playerState) || pauseTime;
    }

    public void ShowKeyboardToast(string key = "E")
    {
        var text = keyboardToast.transform.Find("Button").Find("Text").gameObject;
        string tip;
        switch (key)
        {
            case "E": 
                tip = "<sprite=\"Tips\" index=2>"; 
                break;
            default: 
                tip = key;
                break;
        }

        text.GetComponent<TextMeshProUGUI>().text = tip;
        keyboardToast.SetActive(true);
    }
    
    public void ShutKeyboardToast()
    {
        keyboardToast.SetActive(false);
    }

    public void StepsAudio()
    {
        AudioManager.instance.PlayStepsAudio();
    }
    
    public void ChangeStepsAnim(float speed=1f)
    {
        animator.SetFloat(AnimSpeed, speed);
    }

    private void ChangeLightIntensity()
    {
        if (lightTime > 30) return;
        globalLight.intensity = lightIntensity / 30 * lightTime;
    }
}
