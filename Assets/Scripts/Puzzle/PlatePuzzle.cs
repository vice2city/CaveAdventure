using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatePuzzle : MonoBehaviour
{
    private PlateManager[] plates;
    private int plateGoalNum;
    private int plateNum;

    private bool isFinished;
    // Start is called before the first frame update
    private void Start()
    {
        isFinished = false;
        plates = GetComponentsInChildren<PlateManager>();
        plateGoalNum = plates.Length;
    }

    // Update is called once per frame
    private void Update()
    {
        plateNum = 0;
        foreach (var plate in plates) if (plate.IsPlatePressed()) plateNum++;
        if (plateNum!=plateGoalNum) return;
        FinishPuzzle();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (isFinished) return;
        if (other.GetComponent<PlayerManager>()==null) return;
        ChangePlatesState(false);
    }

    public void ChangePlatesState(bool state)
    {
        foreach (var plate in plates) plate.ChangePlateState(state);
    }

    private void FinishPuzzle()
    {
        isFinished = true;
        foreach (var plate in plates) plate.FinishPlate();
    }
}
