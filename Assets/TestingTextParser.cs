using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TestingTextParser : MonoBehaviour
{
    [TextArea] public string masterText;
    public TextMeshProUGUI gui;
    // Start is called before the first frame update
    void Start()
    {
        ReadText(masterText);
    }

    void ReadText(string text)
    {
        string[] subText = text.Split('<', '>');
        string displayText = "";
        for (int i = 0; i < subText.Length; i += 2)
        {
            displayText += subText[i];
        }
        int subCounter = 0;
        int visibleCounter = 0;
        float speed = 10;
        gui.text = displayText;
        gui.maxVisibleCharacters = 0;
        StartCoroutine(Read());
        IEnumerator Read()
        {
            while (subCounter < subText.Length)
            {
                if (subCounter % 2 == 1)
                {
                    EvaluateCommand();
                }
                else
                {

                    while (visibleCounter < subText[subCounter].Length)
                    {
                        visibleCounter++;
                        gui.maxVisibleCharacters++;
                        yield return new WaitForSeconds(1f / speed);
                    }
                    visibleCounter = 0;
                }
                subCounter++;
            }
            yield return null;
        }
        void EvaluateCommand()
        {
            string command = subText[subCounter].ToLower();
            if (command.Length > 0)
            {
                if (command.StartsWith("speed"))
                {
                    speed = float.Parse(command.Split('=')[1]);
                }
                else if (command.StartsWith("emotion"))
                {
                    // set emotion
                }
            }
        }
    }
}
