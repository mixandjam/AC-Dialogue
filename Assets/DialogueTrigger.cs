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

    public TMP_Animated tmp_animated;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {

            gameCam.SetActive(!gameCam.activeSelf);
            dialogueCam.SetActive(!dialogueCam.activeSelf);

            float dofWeight = dialogueCam.activeSelf ? 1 : 0;
            DOVirtual.Float(dialogueDof.weight, dofWeight, .8f, DialogueDOF);

            uiGroup.DOFade(dofWeight, .2f).SetDelay(dialogueCam.activeSelf ? .65f : 0);
            currentNpc.DOLookAt(transform.position, .5f);
            currentNpc.GetComponent<Animator>().SetTrigger("turn");

            StartCoroutine(Routine(.65f, dofWeight == 1));


        }
    }

    public IEnumerator Routine(float delay, bool active)
    {
        yield return new WaitForSeconds(delay);

        if (active)
        {
            uiGroup.transform.DOScale(.5f, .2f).From().SetEase(Ease.OutBack);
            tmp_animated.ReadText("<speed=30> Hmmmmm Wait wait are those <speed=5>. . . <pause=.1> <speed=200> <size=150%> NEW SHOES?!");
        }

    }

    public void DialogueDOF(float x)
    {
        dialogueDof.weight = x;
    }

}
