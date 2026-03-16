using TMPro;
using UnityEngine;

public class QuestBasic : MonoBehaviour
{
    [HideInInspector] public int questId;
    [HideInInspector] public TextMeshProUGUI questText;

    protected QeustMangeren qeustMangeren;
    protected bool isActive = false;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void Start()
    {
        qeustMangeren = QeustMangeren.Instance;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (!isActive) {return;}
    }

    public virtual void ActiveQuest()
    {
        isActive = true;
    }

    protected void OnComplete()
    {
        qeustMangeren.RemoveQuests(questId);
    }
}
