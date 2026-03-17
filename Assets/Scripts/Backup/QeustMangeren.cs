using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestMangeren : MonoBehaviour
{
    public static QuestMangeren Instance { get; private set; } //make it global
    public Transform uiQuestHolder;
    public RectTransform uiQuestPanel; //don't got to do .getcommpoment

    [System.Serializable]
    private class Quests
    {
        [Header("Settings")]
        public int questId;
        public QuestBasic questScript;

        [HideInInspector] public bool complete = false;
        [HideInInspector] public bool isActive = false;
        [HideInInspector] public RectTransform uiQuestPanel;
    }

    [SerializeField] private Quests[] quests;

    //ui things
    private List<RectTransform> uiQuestsPanels = new List<RectTransform>();
    private float spaceBetweenQuest = .2f;

    void Awake()
    {
        if (Instance == null) {Instance = this;}
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // give all script the id
        foreach (Quests quest in quests)
        {
            quest.questScript.questId = quest.questId;
        }
    }

    Quests GetQuests(int id)
    {
        //try finding the quest
        foreach (Quests quest in quests)
        {
            if (quest.questId != id) continue;

            return quest;  
        }

        Debug.LogWarning("quest id: " + id + " is not valid or qeust don't exist");
        return null;
    }

    //add the quest
    public void AddQuests(int questId)
    {
        //get the qeust
        Quests questToAdd = GetQuests(questId);
        
        //debug
        if (questToAdd == null) {return;}
        if (questToAdd.complete || questToAdd.isActive) {Debug.LogWarning("can't reactive or give it again a qeust"); return;}

        //start it
        questToAdd.isActive = true; //debounce

        //ui*
        GameObject newGuestPanel = Instantiate(uiQuestPanel.gameObject, uiQuestHolder);
        RectTransform rectTransform = newGuestPanel.GetComponent<RectTransform>();

        questToAdd.questScript.questText = newGuestPanel.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        questToAdd.uiQuestPanel = rectTransform;
        uiQuestsPanels.Add(rectTransform);
        ResetUi();
        newGuestPanel.gameObject.SetActive(true);

        questToAdd.questScript.ActiveQuest();
    }

    public void RemoveQuests(int questId)
    {
        print(questId); //debug so u know for assement 

        //get the qeust
        Quests questToRemove = GetQuests(questId);
        
        //debug
        if (questToRemove == null) {return;}
        if (!questToRemove.isActive) {Debug.LogWarning("can't delete a quest that is not active"); return;}

        //remove it
        questToRemove.isActive = false;
        questToRemove.complete = true;

        //Ui*
        uiQuestsPanels.Remove(questToRemove.uiQuestPanel);
        Destroy(questToRemove.uiQuestPanel.gameObject);
        ResetUi();
    }

    void ResetUi()
    {
        float yOffset = 0;

        for (int i = 0; i < uiQuestsPanels.Count; i++)
        {
            // get the ui
            RectTransform currentUi = uiQuestsPanels[i];
            
            //set the pivot debug
            currentUi.pivot = new Vector2(0.5f, 1f);

            float panelHeight = currentUi.rect.height;

            currentUi.anchoredPosition = new Vector2(0, -yOffset);
            yOffset += panelHeight + spaceBetweenQuest;
        }
    }
}
