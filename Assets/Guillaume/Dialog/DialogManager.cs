using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
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
    public GameManager GM;
    public VolumeSettings MenuPause;
    public RuntimeDialogGraph CurrentDialogGraph;

    public List<PnjScript> PNJ = new List<PnjScript>();

    private bool IsDialogueStarted;





    [Header("Localization")]
    public TextAsset LocalizationNameCSV;
    public TextAsset LocalizationDialogCSV;
    private Dictionary<string, string> localizedText = new Dictionary<string, string>();
    private GameLanguage CurrentLanguage = GameLanguage.French;
    private TMP_Dropdown LanguageDropdown;

    [System.Serializable]
    public class DialogView
    {
        public GameObject Root;
        public TextMeshProUGUI SpeakerTextOne;
        public TextMeshProUGUI SpeakerTextTwo;
        public TextMeshProUGUI BodyText;
        public Transform ChoiceContainer;
        public Image SpeakerImageOne;
        public Image SpeakerImageTwo;
        public Image BG;
        public Image TextBox;
    }

    [Header("UI Views")]
    public Image NextDialog;
    public DialogView PanelView;
    public Button ChoiceButtonPrefab;
    public Sprite OnLeft;
    public Sprite OnRight;
    public Sprite TextBoxNarrateur;


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

        for (int i = 0; i < PNJ.Count; i++)
        {
            RuntimeDialogGraph rdg = PNJ[i].dialogGraph;
            foreach (var node in rdg.AllNodes)
            {
                nodeLookup[node.NodeId] = node;
            }
        }

        if (PanelView.Root)
        {
            PanelView.Root.SetActive(false);
            PanelView.BG.gameObject.SetActive(false);
        }

        MenuPause = FindFirstObjectByType<VolumeSettings>();
        LanguageDropdown = MenuPause.LanguageDropdown;
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


        if (GM.CanShowDialogue)
        {
            if (!PanelView.Root.activeInHierarchy && CurrentDialogGraph)
            {
                PanelView.Root.SetActive(true);
                PanelView.BG.gameObject.SetActive(true);
                GM.IsInDialogue = true;
                ShowNode(CurrentDialogGraph.EntryNodeId);
            }
            else if (!string.IsNullOrEmpty(currentNode.NextNodeId) && currentNode != null && currentNode.Choices.Count == 0 && Pointer.current.press.wasPressedThisFrame && !MenuPause.IsInPause)
            {
                ShowNode(currentNode.NextNodeId);
            }
        }
    }

    private void ShowNode(string nodeId)
    {
        StopAllCoroutines();
        NextDialog.gameObject.SetActive(false);
        if (!nodeLookup.ContainsKey(nodeId))
        {
            EndDialogue();
            GameManager.Instance.IsInDialogue = false;
            GameManager.Instance.CanShowDialogue = false;
            return;
        }
        currentNode = nodeLookup[nodeId];

        DialogView targetView = GetViewForMode(currentNode.Mode);

        if (currentView != targetView)
        {
            if (currentView != null && currentView.Root != null)
            {
                currentView.Root.SetActive(false);
                currentView.BG.gameObject.SetActive(false);
            }

            targetView.Root.SetActive(true);
            currentView = targetView;
        }

        if (currentNode.Mode == DialogueMode.Narrateur)
        {
            currentView.SpeakerTextOne.gameObject.SetActive(false);
            currentView.SpeakerTextTwo.gameObject.SetActive(false);
            currentView.TextBox.sprite = TextBoxNarrateur;
            currentView.SpeakerImageOne.gameObject.SetActive(false);
            currentView.SpeakerImageTwo.gameObject.SetActive(false);
        }
        else
        {
            currentView.SpeakerTextOne.gameObject.SetActive(true);
            currentView.SpeakerTextTwo.gameObject.SetActive(true);
            currentView.SpeakerImageOne.gameObject.SetActive(true);
            currentView.SpeakerImageTwo.gameObject.SetActive(true);
            if (currentNode.IsSpeakerOnLeft != false)
            {
                currentView.TextBox.sprite = OnLeft;
                currentView.SpeakerTextTwo.gameObject.SetActive(false);
                currentView.SpeakerTextOne.gameObject.SetActive(true);
            }
            else if (currentNode.IsSpeakerOnLeft == false)
            {
                currentView.TextBox.sprite = OnRight;
                currentView.SpeakerTextOne.gameObject.SetActive(false);
                currentView.SpeakerTextTwo.gameObject.SetActive(true);
            }

            if (currentView.SpeakerTextOne != null)
                currentView.SpeakerTextOne.text = GetText(currentNode.SpeakerNameOne);

            if (currentView.SpeakerTextTwo != null)
                currentView.SpeakerTextTwo.text = GetText(currentNode.SpeakerNameTwo);
        }


        if (currentView.BodyText != null)
            StartCoroutine(ShowText());

        if (currentView.SpeakerImageOne != null)
            currentView.SpeakerImageOne.sprite = currentNode.SpeakerSpriteOne;

        if (currentView.SpeakerImageTwo != null)
            currentView.SpeakerImageTwo.sprite = currentNode.SpeakerSpriteTwo;

        if (currentView.BG != null)
            currentView.BG.sprite = currentNode.BackgroundSprite;

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

    IEnumerator ShowText()
    {
        string DialogueText = "";

        if (currentView.BodyText != null)
        {
            DialogueText = GetText(currentNode.DialogueText);
        }

        for (int i = 0; i <= DialogueText.Length; i++)
        {
            currentView.BodyText.text = DialogueText.Substring(0, i);
            yield return new WaitForSeconds(MenuPause.textSpeed);
        }

        NextDialog.gameObject.SetActive(true);
        Animator animator = NextDialog.GetComponent<Animator>();
        animator.SetTrigger("Lancer");
    }

    private void EndDialogue()
    {
        if (currentView != null && currentView.Root != null)
        {
            currentView.Root.SetActive(false);
            currentView.BG.gameObject.SetActive(false);

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
            case DialogueMode.Discussion:
            default: return PanelView;
        }
    }
}
