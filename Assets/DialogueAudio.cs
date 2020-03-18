
using UnityEngine;
using TMPro;
using DG.Tweening;

public class DialogueAudio : MonoBehaviour
{
    public TMP_Animated tmp_animated;

    public Transform mouthQuad;

    public AudioClip[] voices;
    public AudioClip[] punctuations;
    [Space]
    public AudioSource voiceSource;
    public AudioSource punctuationSource;
    public AudioSource effectSource;

    // Start is called before the first frame update
    void Start()
    {
        tmp_animated.onEmotionChange.AddListener((newEmotion) => PlayEffect(newEmotion));
        tmp_animated.onTextReveal.AddListener((newChar) => ReproduceSound(newChar));
    }

    public void ReproduceSound(char c)
    {

        if (char.IsPunctuation(c) && !punctuationSource.isPlaying)
        {
            voiceSource.Stop();
            punctuationSource.clip = punctuations[Random.Range(0, punctuations.Length)];
            punctuationSource.Play();
        }

        if (char.IsLetter(c) && !voiceSource.isPlaying)
        {
            punctuationSource.Stop();
            voiceSource.clip = voices[Random.Range(0, voices.Length)];
            voiceSource.Play();

            mouthQuad.localScale = new Vector3(1, 0, 1);
            mouthQuad.DOScaleY(1, .2f).OnComplete(() => mouthQuad.DOScaleY(0, .2f));
        }

    }

    public void PlayEffect(Emotion e)
    {

    }
}
