using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueCreatorControl : MonoBehaviour
{

    private TMP_InputField inputField;

    public TMP_InputField.TextSelectionEvent onTextSelection { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        inputField = GetComponent<TMP_InputField>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTextSelect(string text)
    {
        print(text);
    }
}
