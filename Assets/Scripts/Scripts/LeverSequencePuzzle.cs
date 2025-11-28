using UnityEngine;
using System.Collections.Generic;

public class LeverSequencePuzzle : MonoBehaviour
{
    [System.Serializable]
    public class LeverStep
    {
        public LeverScript lever;
        [HideInInspector] public bool lastState = false;
    }

    public List<LeverStep> sequence = new List<LeverStep>();
    public GameObject lightsObject;
    public MonoBehaviour rotationController;
    public LeverPuzzleInteraction interaction; // assign in Inspector

    int currentIndex = 0;
    bool puzzleCompleted = false;

    void Start()
    {
        if (lightsObject) lightsObject.SetActive(false);
        if (rotationController) rotationController.enabled = false;
    }

    void Update()
    {
        foreach (var s in sequence)
        {
            if (!s.lever) continue;
            bool current = s.lever.LeverState;

            if (!s.lastState && current)
                OnLeverPulled(s.lever);

            s.lastState = current;
        }
    }

    void OnLeverPulled(LeverScript pulled)
    {
        if (puzzleCompleted) return;

        if (sequence[currentIndex].lever == pulled)
        {
            currentIndex++;

            if (currentIndex == 1 && lightsObject)
                lightsObject.SetActive(true);

            if (currentIndex >= sequence.Count)
            {
                ActivateGame();
                puzzleCompleted = true;
            }
        }
        else
        {
            ResetPuzzle(true);
        }
    }

    void ResetPuzzle(bool exitView)
    {
        currentIndex = 0;

        foreach (var s in sequence)
        {
            if (!s.lever) continue;
            var anim = s.lever.StickGameObject.GetComponent<Animator>();
            s.lever.LeverState = false;
            if (anim) anim.Play("LeverOffAnimation");
            s.lastState = false;
        }

        if (lightsObject) lightsObject.SetActive(false);
        if (rotationController) rotationController.enabled = false;
        puzzleCompleted = false;

        if (exitView && interaction)
            interaction.ExitInteraction();
    }

    void ActivateGame()
    {
        if (lightsObject) lightsObject.SetActive(true);
        if (rotationController) rotationController.enabled = true;
    }
}
