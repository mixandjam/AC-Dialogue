using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Experimental.GraphView;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using System;

public class DialogueGraph : EditorWindow
{
    private DialogueGraphView _graphView;

    [MenuItem("Custom Tools/Dialogue Graph")]
    public static void OpenDialogueGraphWindow()
    {
        var window = GetWindow<DialogueGraph>();
        window.titleContent = new GUIContent(text: "Dialogue Graph");
    }

    private void OnEnable()
    {
        GenerateToolbar();
        var quickToolVisualTree = Resources.Load<VisualTreeAsset>("DialogueGraphWindow");
        quickToolVisualTree.CloneTree(rootVisualElement);
        ConstructGraphView();
    }

    private void ConstructGraphView()
    {
        _graphView = new DialogueGraphView
        {
            name = "Dialogue Graph"
        };

        _graphView.StretchToParentSize();
        rootVisualElement.Q<VisualElement>("graph-container").Add(_graphView);

        _graphView.RegisterCallback<MouseUpEvent>(e => UpdateSelection());
    }

    private void UpdateSelection()
    {
        var selections = _graphView.selection;
        rootVisualElement.Q<VisualElement>("dialogue-editor").style.display = selections.Count > 0 ? DisplayStyle.Flex : DisplayStyle.None;
    }

    private void GenerateToolbar()
    {
        var toolbar = new Toolbar();

        toolbar.Add(new ToolbarButton(() => RequestData(true)) { text = "Save Data" });
        toolbar.Add(new ToolbarButton(() => RequestData(false)) { text = "Load Data" });

        var nodeCreateButton = new ToolbarButton(clickEvent: () => { _graphView.CreateNode("Dialogue Node"); });
        nodeCreateButton.text = "Create Node";
        toolbar.Add(nodeCreateButton);

        rootVisualElement.Add(toolbar);
    }

    private void RequestData(bool save)
    {

        var saveUtility = GraphSaveUtility.GetInstance(_graphView);
        if (save)
            saveUtility.SaveGraph();
        else
        {
            string path = EditorUtility.OpenFilePanel("Load Graph", "", "asset");
            if (string.IsNullOrEmpty(path))
                return;
            saveUtility.LoadGraph(path);
        }
    }

    private void SaveData()
    {
        throw new NotImplementedException();
    }
}
