using TMPro;
using UnityEngine;

public class QuestBasic : MonoBehaviour
{
    public int questId;
    [HideInInspector] public TextMeshProUGUI questText;

    protected QuestMangeren qeustMangeren;
    protected bool isActive = false;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void Start()
    {
        qeustMangeren = QuestMangeren.Instance;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (!isActive) {return;}
    }

    //when this quest is active
    public virtual void ActiveQuest()
    {
        isActive = true;
    }

    //when this quest is done funtion
    protected virtual void OnComplete()
    {
        qeustMangeren.RemoveQuests(questId);
    }
}
