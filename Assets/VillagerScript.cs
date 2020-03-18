using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using Cinemachine;

public class VillagerScript : MonoBehaviour
{
    public VillagerData data;
    public DialogueData dialogue;

    public bool villagerIsTalking;

    private TMP_Animated animatedText;
    private DialogueAudio dialogueAudio;
    private Animator animator;
    public Renderer eyesRenderer;

    public Transform particlesParent;

    void Start()
    {
        dialogueAudio = GetComponent<DialogueAudio>();
        animator = GetComponent<Animator>();
        animatedText = InterfaceManager.instance.animatedText;
        animatedText.onEmotionChange.AddListener((newEmotion) => EmotionChanger(newEmotion));
        animatedText.onAction.AddListener((action) => SetAction(action));
    }

    public void EmotionChanger(Emotion e)
    {
        animator.SetTrigger(e.ToString());

        if (e == Emotion.suprised)
            eyesRenderer.material.SetTextureOffset("_BaseMap", new Vector2(.33f, 0));

        if (e == Emotion.angry)
            eyesRenderer.material.SetTextureOffset("_BaseMap", new Vector2(.66f, 0));

        if(e== Emotion.sad)
            eyesRenderer.material.SetTextureOffset("_BaseMap", new Vector2(.33f, -.33f));
    }

    public void SetAction(string action)
    {
        if(action == "shake")
        {
            Camera.main.GetComponent<CinemachineImpulseSource>().GenerateImpulse();
        }
        else
        {
            PlayParticle(action);

            if (action == "sparkle")
            {
                dialogueAudio.effectSource.clip = dialogueAudio.sparkleClip;
                dialogueAudio.effectSource.Play();
            }else if(action == "rain")
            {
                dialogueAudio.effectSource.clip = dialogueAudio.rainClip;
                dialogueAudio.effectSource.Play();
            }
        }
    }

    public void PlayParticle(string x)
    {
        if (particlesParent.Find(x + "Particle") == null)
            return;
        particlesParent.Find(x + "Particle").GetComponent<ParticleSystem>().Play();
    }

    public void Reset()
    {
        animator.SetTrigger("normal");
        eyesRenderer.material.SetTextureOffset("_BaseMap", Vector2.zero);
    }

    public void TurnToPlayer(Vector3 playerPos)
    {
        transform.DOLookAt(playerPos, Vector3.Distance(transform.position, playerPos) / 5);
        string turnMotion = isRightSide(transform.forward, playerPos, Vector3.up) ? "rturn" : "lturn";
        animator.SetTrigger(turnMotion);
    }

    //https://forum.unity.com/threads/left-right-test-function.31420/
    public bool isRightSide(Vector3 fwd, Vector3 targetDir, Vector3 up)
    {
        Vector3 right = Vector3.Cross(up.normalized, fwd.normalized);        // right vector
        float dir = Vector3.Dot(right, targetDir.normalized);
        return dir > 0f;
    }
}
