using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public enum GameLanguage
{
    French,
    English,
    Espanol
}

public class DialogManager : MonoBehaviour
{

    public static DialogManager Instance;
    public RuntimeDialogGraph RuntimeGraph;
    public GameManager GM;

    public List<PnjScript> CurrentObject = new List<PnjScript>();

    public bool IsDialogueStarted;

    public RuntimeDialogGraph CurrentDialogGraph;


    [Header("Localization")]
    public TextAsset LocalizationNameCSV;
    public TextAsset LocalizationDialogCSV;
    private Dictionary<string, string> localizedText = new Dictionary<string, string>();
    public GameLanguage CurrentLanguage = GameLanguage.French;
    public TMP_Dropdown LanguageDropdown;

    [System.Serializable]
    public class DialogView
    {
        public GameObject Root;
        public TextMeshProUGUI SpeakerText;
        public TextMeshProUGUI BodyText;
        public Transform ChoiceContainer;
        public Image SpeakerImage;
    }

    [Header("UI Views")]
    public DialogView PanelView;
    //public DialogView PopupView; 
    //public DialogView BubbleView;
    public Button ChoiceButtonPrefab;


    private Dictionary<string, RuntimeDialogNode> nodeLookup = new Dictionary<string, RuntimeDialogNode>();
    private RuntimeDialogNode currentNode;

    private DialogView currentView;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
        LoadLocalization();
    }

    private void Start()
    {

        for(int i = 0; i < CurrentObject.Count; i++)
        {
            RuntimeDialogGraph rdg = CurrentObject[i].dialogGraph;
            foreach (var node in rdg.AllNodes)
            {
                nodeLookup[node.NodeId] = node;
            }
        }

        if (PanelView.Root) PanelView.Root.SetActive(false);
        //if (PopupView.Root) PopupView.Root.SetActive(false);
        //if (BubbleView.Root) BubbleView.Root.SetActive(false);

        //if (!string.IsNullOrEmpty(RuntimeGraph.EntryNodeId))
        //{
        //    ShowNode(RuntimeGraph.EntryNodeId);
        //}
        //else
        //{
        //    EndDialogue();
        //}

    }

    public void SetLanguage(int languageIndex)
    {
        if (languageIndex == 0) CurrentLanguage = GameLanguage.French;
        else if (languageIndex == 1) CurrentLanguage = GameLanguage.English;
        else if (languageIndex == 2) CurrentLanguage = GameLanguage.Espanol;
        LoadLocalization();
        if (currentNode != null)
        {
            ShowNode(currentNode.NodeId);
        }
    }

    private void LoadLocalization()
    {
        if (LocalizationNameCSV == null || LocalizationNameCSV == null) return;
        localizedText.Clear();
        var namesLines = LocalizationNameCSV.text.Split('\n');
        var dialogLines = LocalizationDialogCSV.text.Split('\n');
        int columnIndex = 1;

        switch (CurrentLanguage)
        {
            case GameLanguage.French:
                columnIndex = 1;
                break;
            case GameLanguage.English:
                columnIndex = 2;
                break;
            case GameLanguage.Espanol:
                columnIndex = 3;
                break;
        }
        for (int i = 1; i < namesLines.Length; i++)
        {
            var line = namesLines[i].Trim();
            if (string.IsNullOrEmpty(line)) continue;

            var parts = line.Split(';');

            if (parts.Length > columnIndex)
            {
                string key = parts[0];
                string text = parts[columnIndex];

                text = text.Replace("\\n", "\n");

                if (!localizedText.ContainsKey(key))
                {
                    localizedText.Add(key, text);
                }
            }
        }

        for (int i = 1; i < dialogLines.Length; i++)
        {
            var line = dialogLines[i].Trim();
            if (string.IsNullOrEmpty(line)) continue;

            var parts = line.Split(';');

            if (parts.Length > columnIndex)
            {
                string key = parts[0];
                string text = parts[columnIndex];

                text = text.Replace("\\n", "\n");

                if (!localizedText.ContainsKey(key))
                {
                    localizedText.Add(key, text);
                }
            }
        }

    }

    private string GetText(string key)
    {
        if (string.IsNullOrEmpty(key)) return "";

        if (localizedText.TryGetValue(key, out string value))
        {
            return value;
        }

        Debug.LogWarning($"Clé de dialogue introuvable : {key}");
        return key;
    }

    private void Update()
    {

        if (Keyboard.current.pKey.wasPressedThisFrame && currentNode != null && currentNode.Choices.Count == 0 && GM.ShowDialogue)
        {
            if (!string.IsNullOrEmpty(currentNode.NextNodeId))
            {
                ShowNode(currentNode.NextNodeId);
            }
            else
            {
                EndDialogue();
            }
        }

        if (!PanelView.Root.activeInHierarchy && GM.ShowDialogue && Keyboard.current.eKey.wasPressedThisFrame && CurrentDialogGraph)
        {
            PanelView.Root.SetActive(true);
            ShowNode(CurrentDialogGraph.EntryNodeId);
        }
    }

    private void ShowNode(string nodeId)
    {
        if (!nodeLookup.ContainsKey(nodeId))
        {
            EndDialogue();
            return;
        }
        currentNode = nodeLookup[nodeId];

        DialogView targetView = GetViewForMode(currentNode.Mode);

        if (currentView != targetView)
        {
            if (currentView != null && currentView.Root != null)
                currentView.Root.SetActive(false);

            targetView.Root.SetActive(true);
            currentView = targetView;
        }

        if (currentView.SpeakerText != null)
            currentView.SpeakerText.text = GetText(currentNode.SpeakerName);

        if (currentView.BodyText != null)
            currentView.BodyText.text = GetText(currentNode.DialogueText);

        if (currentView.SpeakerImage != null)
            currentView.SpeakerImage.sprite = currentNode.SpeakerSprite;

        if (currentView.ChoiceContainer != null)
        {
            foreach (Transform child in currentView.ChoiceContainer)
                Destroy(child.gameObject);

            if (currentNode.Choices.Count > 0)
            {
                foreach (var choice in currentNode.Choices)
                {
                    Button button = Instantiate(ChoiceButtonPrefab, currentView.ChoiceContainer);
                    TextMeshProUGUI buttonText = button.GetComponentInChildren<TextMeshProUGUI>();

                    if (buttonText != null)
                        buttonText.text = GetText(choice.ChoiceText);

                    button.onClick.AddListener(() =>
                    {
                        if (!string.IsNullOrEmpty(choice.DestinationNodeId))
                            ShowNode(choice.DestinationNodeId);
                        else
                            EndDialogue();
                    });
                }
            }
        }
    }

    private void EndDialogue()
    {
        if (currentView != null && currentView.Root != null)
        {
            currentView.Root.SetActive(false);

            // Nettoyage des boutons
            if (currentView.ChoiceContainer != null)
            {
                foreach (Transform child in currentView.ChoiceContainer) Destroy(child.gameObject);
            }
        }
        currentView = null;
        currentNode = null;
    }

    private DialogView GetViewForMode(DialogueMode mode)
    {
        switch (mode)
        {
            //case DialogueMode.Popup: return PopupView;
            //case DialogueMode.Bulle: return BubbleView;
            case DialogueMode.Panel:
            default: return PanelView;
        }
    }
}
