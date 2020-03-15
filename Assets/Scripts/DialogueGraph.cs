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
    private string _fileName = "New Chart";

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

        var fileNameTextField = new TextField("File Name:");
        fileNameTextField.SetValueWithoutNotify(_fileName);
        fileNameTextField.MarkDirtyRepaint();
        fileNameTextField.RegisterValueChangedCallback(evt => _fileName = evt.newValue);
        toolbar.Add(fileNameTextField);

        toolbar.Add(new ToolbarButton(() => RequestData(true)) { text = "Save Data" });
        toolbar.Add(new ToolbarButton(() => RequestData(false)) { text = "Load Data" });

        var nodeCreateButton = new ToolbarButton(clickEvent: () => { _graphView.CreateNode("Dialogue Node"); });
        nodeCreateButton.text = "Create Node";
        toolbar.Add(nodeCreateButton);

        rootVisualElement.Add(toolbar);
    }

    private void RequestData(bool save)
    {
        if (string.IsNullOrEmpty(_fileName))
        {
            EditorUtility.DisplayDialog("Invalid file name!", "Please enter a valid file name.", "OK");
            return;
        }

        var saveUtility = GraphSaveUtility.GetInstance(_graphView);
        if (save)
            saveUtility.SaveGraph(_fileName);
        else
            saveUtility.LoadGraph(_fileName);
    }

    private void SaveData()
    {
        throw new NotImplementedException();
    }
}
