using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace TMPro
{
    public enum Emotion { happy, sad, suprised, angry };
    [System.Serializable] public class EmotionEvent : UnityEvent<Emotion> { }

    [System.Serializable] public class TextRevealEvent : UnityEvent<char> { }

    public class TMP_Animated : TextMeshProUGUI
    {

        [SerializeField] private float speed = 10;
        public EmotionEvent onEmotionChange;
        public TextRevealEvent onTextReveal;

        public void ReadText(string newText)
        {
            // split the whole text into parts based off the <> tags 
            // even numbers in the array are text, odd numbers are tags
            string[] subTexts = newText.Split('<', '>');

            // textmeshpro still needs to parse its built-in tags, so we only include noncustom tags
            string displayText = "";
            for (int i = 0; i < subTexts.Length; i++)
            {
                if (i % 2 == 0)
                    displayText += subTexts[i];
                else if (!isCustomTag(subTexts[i].Replace(" ", "")))
                    displayText += $"<{subTexts[i]}>";
            }
            // check to see if a tag is our own
            bool isCustomTag(string tag)
            {
                return tag.StartsWith("speed=") || tag.StartsWith("pause=") || tag.StartsWith("emotion=");
            }

            // send that string to textmeshpro and hide all of it, then start reading
            text = displayText;
            maxVisibleCharacters = 0;
            StartCoroutine(Read());

            IEnumerator Read()
            {
                int subCounter = 0;
                int visibleCounter = 0;
                while (subCounter < subTexts.Length)
                {
                    // if 
                    if (subCounter % 2 == 1)
                    {
                        yield return EvaluateTag(subTexts[subCounter].Replace(" ", ""));
                    }
                    else
                    {
                        while (visibleCounter < subTexts[subCounter].Length)
                        {
                            onTextReveal.Invoke(subTexts[subCounter][visibleCounter]);
                            visibleCounter++;
                            maxVisibleCharacters++;
                            yield return new WaitForSeconds(1f / speed);
                        }
                        visibleCounter = 0;
                    }
                    subCounter++;
                }
                yield return null;

                WaitForSeconds EvaluateTag(string tag)
                {
                    if (tag.Length > 0)
                    {
                        if (tag.StartsWith("speed="))
                        {
                            speed = float.Parse(tag.Split('=')[1]);
                        }
                        else if (tag.StartsWith("pause="))
                        {
                            return new WaitForSeconds(float.Parse(tag.Split('=')[1]));
                        }
                        else if (tag.StartsWith("emotion="))
                        {
                            onEmotionChange.Invoke((Emotion)System.Enum.Parse(typeof(Emotion), tag.Split('=')[1]));
                        }
                    }
                    return null;
                }
            }
        }
    }
}