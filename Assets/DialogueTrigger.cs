using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Rendering;
using UnityEngine.UI;
using TMPro;
using Cinemachine;

public class DialogueTrigger : MonoBehaviour
{
    private DialogueAudio audio;

    public Volume dialogueDof;

    public GameObject gameCam;
    public GameObject dialogueCam;

    public CanvasGroup uiGroup;

    public VillagerScript currentVillager;

    public TMP_Animated tmp_animated;

    public Renderer eyesRenderer;
    public ParticleSystem particle;

    // Start is called before the first frame update
    void Start()
    {
        audio = GetComponent<DialogueAudio>();
        tmp_animated.onEmotionChange.AddListener((newEmotion) => EmotionChanger(newEmotion));
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

                if (dofWeight == 1)
                {

                    tmp_animated.text = string.Empty;
                    uiGroup.transform.localScale = Vector3.one / 2;

                    currentVillager.transform.DOLookAt(transform.position, .5f);
                    string turnMotion = isRightSide(currentVillager.transform.forward, transform.position, Vector3.up) ? "rturn" : "lturn";
                    currentVillager.GetComponent<Animator>().SetTrigger(turnMotion);
                }
                StartCoroutine(Routine(.8f, dofWeight == 1));

        }
    }

    public IEnumerator Routine(float delay, bool active)
    {
        yield return new WaitForSeconds(delay);

        if (active)
        {
            uiGroup.transform.DOScale(1, .5f).SetEase(Ease.OutBack);
            yield return new WaitForSeconds(.1f);
            tmp_animated.ReadText("<emotion=happy><speed=10>Hmmmmm<speed=50> Wait wait are those<speed=5>... <speed=50>Are those really<speed=5>... <pause=.1><speed=60><size=150%><emotion=suprised> <b>NEW SHOES?!</b>");
        }
        else
        {
            eyesRenderer.material.SetTextureOffset("_BaseMap", Vector2.zero);
            currentVillager.GetComponent<Animator>().SetTrigger("normal");
        }

    }

    public void DialogueDOF(float x)
    {
        dialogueDof.weight = x;
    }

    public void EmotionChanger(Emotion e)
    {
        if (e == Emotion.happy)
        {
            currentVillager.GetComponent<Animator>().SetTrigger("happy");
        }

        if (e == Emotion.suprised)
        {
            Camera.main.GetComponent<CinemachineImpulseSource>().GenerateImpulse();
            particle.Play();
            eyesRenderer.material.SetTextureOffset("_BaseMap", new Vector2(.33f, 0));
            currentVillager.GetComponent<Animator>().SetTrigger("dead");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Villager"))
        {
            currentVillager = other.GetComponent<VillagerScript>();
        }
    }

    //https://forum.unity.com/threads/left-right-test-function.31420/
    public bool isRightSide(Vector3 fwd, Vector3 targetDir, Vector3 up)
    {
        Vector3 right = Vector3.Cross(up.normalized, fwd.normalized);        // right vector
        float dir = Vector3.Dot(right, targetDir.normalized);
        return dir > 0f;
    }
}
