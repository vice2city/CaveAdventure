using UnityEngine;

public class SwitchPuzzle : MonoBehaviour
{
    public enum BlockColor
    {
        Red,
        Blue,
        Yellow,
        Purple
    }
    
    public Sprite defaultSprite;
    public Sprite noneSprite;
    public PuzzleManager.BlockColorGroup colorGroup;
    public BlockColor color;
    
    private SpriteRenderer[] blocks;
    private BoxCollider2D[] blocksCoil;

    private bool isFormer;
    // Start is called before the first frame update
    private void Start()
    {
        blocks = GetComponentsInChildren<SpriteRenderer>();
        blocksCoil = GetComponentsInChildren<BoxCollider2D>();
        isFormer = PuzzleManager.instance.GetColorState(colorGroup);
        CheckState();
    }

    // Update is called once per frame
    private void Update()
    {
        var state = PuzzleManager.instance.GetColorState(colorGroup);
        if (isFormer==state) return;
        isFormer = state;
        CheckState();
    }

    private void CheckState()
    {
        if ((color==BlockColor.Red&&isFormer)||(color==BlockColor.Blue&&!isFormer) ||
            (color==BlockColor.Yellow&&isFormer)||(color==BlockColor.Purple&&!isFormer)) ChangeBlockState(true);
        else ChangeBlockState(false);
    }

    private void ChangeBlockState(bool state)
    {
        foreach (var block in blocks)
        {
            block.sprite = state ? defaultSprite : noneSprite;
        }

        foreach (var coil in blocksCoil)
        {
            coil.enabled = state;
        }
    }
}
