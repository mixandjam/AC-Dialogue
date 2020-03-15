using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
public class DialogueEditorWindow : EditorWindow
{

    class DialogueBox
    {
        public string dialogue;
        public int index = 0;
        public DialogueBox(int i)
        {
            index = i;
        }
    }

    [MenuItem("Custom Tools/Dialogue Editor")]
    public static void ShowWindow()
    {
        // Opens the window, otherwise focuses it if it’s already open.
        var window = GetWindow<DialogueEditorWindow>();

        // Adds a title to the window.
        window.titleContent = new GUIContent("DialogueEditor");

        // Sets a minimum size to the window.
        window.minSize = new Vector2(250, 50);
    }

    private void OnEnable()
    {
        // Reference to the root of the window.
        var root = rootVisualElement;

        // Loads and clones our VisualTree (eg. our UXML structure) inside the root.
        var quickToolVisualTree = Resources.Load<VisualTreeAsset>("DialogueEditor");
        quickToolVisualTree.CloneTree(root);
        var layers = new List<DialogueBox>(100);
        for (int i = 0; i < 10; i++)
            layers.Add(new DialogueBox(i));

        System.Func<VisualElement> makeItem = () =>
                    {
                        var box = new VisualElement();
                        var dialogueBox = Resources.Load<VisualTreeAsset>("DialogueBox");
                        dialogueBox.CloneTree(box);
                        return box;
                    };

        var layerList = new ListView(layers, 75, makeItem, (e, a) =>
        {
            //(e.Q("layerName") as Label).text = $"Layer {a + 1}";
        });
        InitListView(layerList);
        root.Q<VisualElement>("dialogueBoxHolder").Add(layerList);

    }
    private void InitListView(ListView listView)
    {
        listView.selectionType = SelectionType.Multiple;
        listView.style.flexDirection = FlexDirection.ColumnReverse;

        //listView.onItemChosen += obj => Debug.Log(obj);
        //listView.onSelectionChanged += objects => Debug.Log(objects);

        listView.style.flexGrow = 1f;
        listView.style.flexShrink = 0f;
        listView.style.flexBasis = 0f;
    }
}
