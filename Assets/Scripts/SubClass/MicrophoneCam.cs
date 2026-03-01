using System;
using UnityEngine;

public class MicrophoneCam : AudioListeren
{
    [HideInInspector] public Action onEnemieHear;

    private Transform plr;
    private string plrTag = "Player"; 
    private float disantToBeHighLight = 50f;
    private bool isHighlighted = false;

    protected override void Start()
    {
        base.Start(); //keep old one

        //add new function
        plr = GameObject.FindGameObjectWithTag(plrTag).transform;
    }

    protected override void Update()
    {
        //keep old one
        base.Update();

        // add new lines
        if (!isHighlighted && CanBeHighLigted()) {ToggleHighlight();}
        else if (isHighlighted && !CanBeHighLigted()) {ToggleHighlight();}
    }

    protected override void OnHeardSomething(float amplutide)
    {
        onEnemieHear?.Invoke();
    }

    bool CanBeHighLigted()
    {
        if (plr == null) {return false;}
        
        float disantToPlr = (transform.position - plr.position).magnitude;
        return disantToPlr < disantToBeHighLight;
    }

    void ToggleHighlight()
    {
        isHighlighted = !isHighlighted;
        print(isHighlighted + " is highligt");
    }
}
