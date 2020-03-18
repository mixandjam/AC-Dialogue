using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TMP_Animated))]
public class TestingTextParser : MonoBehaviour
{

    [TextArea] public string masterText;
    private TMP_Animated animatedText;
    private void Awake()
    {
        animatedText = GetComponent<TMP_Animated>();
    }

}
