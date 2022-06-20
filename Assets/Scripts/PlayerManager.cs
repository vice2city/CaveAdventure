using System.Collections.Generic;
using UnityEngine;

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
    private Queue<int> obtainedKey;
    private static readonly int MoveX = Animator.StringToHash("moveX");
    private static readonly int MoveY = Animator.StringToHash("moveY");
    private static readonly int Speed = Animator.StringToHash("speed");

    public enum TimeChangeType {Add, Replace};

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        obtainedKey = new Queue<int>();

        keyCount = 0;
        lightTime = initialLightTime;
        playerState = 0;
        pauseTime = false;
    }

    // Update is called once per frame
    void Update()
    {
        movement.x = Input.GetAxis("Horizontal");
        movement.y = Input.GetAxis("Vertical");

        if (movement != Vector2.zero)
        {
            animator.SetFloat(MoveX, movement.x);
            animator.SetFloat(MoveY, movement.y);
        }
        animator.SetFloat(Speed, movement.magnitude);

        light2D.SetActive(!(GameManager.Instance.IsCaveFinish(playerState) || pauseTime));
    }

    void FixedUpdate()
    {
        Vector2 position = rigidbody2d.position;
        position.x += moveSpeed.x * movement.x * Time.deltaTime;
        position.y += moveSpeed.y * movement.y * Time.deltaTime;

        rigidbody2d.MovePosition(position);

        if (GameManager.Instance.IsCaveFinish(playerState) || pauseTime) return;
        lightTime -= Time.deltaTime;
        if(lightTime <= 0) GameManager.Instance.GameEnd();
    }

    public void GetKey(int n)
    {
        obtainedKey.Enqueue(n);
        keyCount = obtainedKey.Count;
    }

    public int UseKey()
    {
        keyCount = obtainedKey.Count-1;
        return obtainedKey.Dequeue();
    }

    public void ChangePlayerState(int n)
    {
        playerState = n;
    }

    public int GetPlayerState()
    {
        return playerState;
    }

    public void ChangeLightTime(float d, TimeChangeType t = TimeChangeType.Add)
    {
        if(t == TimeChangeType.Add)
        {
            if (lightTime + d < 0)
            {
                lightTime = 0;
                return;
            }
            lightTime += d;
        }else if(t == TimeChangeType.Replace)
        {
            lightTime = d;
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
}
