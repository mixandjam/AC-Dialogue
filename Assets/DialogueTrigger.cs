using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Rendering;
using UnityEngine.UI;
using TMPro;

public class DialogueTrigger : MonoBehaviour
{
    public Volume dialogueDof;

    public GameObject gameCam;
    public GameObject dialogueCam;

    public CanvasGroup uiGroup;

    public Transform currentNpc;

    public TextMeshProUGUI dialogueTextUI;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {

            gameCam.SetActive(!gameCam.active);
            dialogueCam.SetActive(!dialogueCam.active);

            float dofWeight = dialogueCam.active ? 1 : 0;
            DOVirtual.Float(dialogueDof.weight, dofWeight, .8f, DialogueDOF);

            uiGroup.DOFade(dofWeight, .2f).SetDelay(dialogueCam.active ? .65f : 0);
            if (dofWeight == 1) uiGroup.transform.DOScale(.5f, .2f).From().SetEase(Ease.OutBack).SetDelay(.65f);

            if (dofWeight == 1)
            {
                currentNpc.DOLookAt(transform.position, .5f);
                currentNpc.GetComponent<Animator>().SetTrigger("turn");

                dialogueTextUI.maxVisibleCharacters = 0;
                DOVirtual.Float(0, dialogueTextUI.text.Length, 3, DialogueMaxVisibleChars).SetDelay(.8f);
            }


        }
    }

    public void DialogueDOF(float x)
    {
        dialogueDof.weight = x;
    }

    public void DialogueMaxVisibleChars(float x)
    {
        dialogueTextUI.maxVisibleCharacters = (int)x;
    }
}
