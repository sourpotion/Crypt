using UnityEngine;

public class Quest1 : QuestBasic
{
    
    protected override void Update()
    {
        base.Update();

        if (Input.GetKeyDown(KeyCode.M))
        {
            OnComplete();
        }
    }

    public override void ActiveQuest()
    {
        questText.text = "click M";
        base.ActiveQuest();
    }

    protected override void OnComplete()
    {
        base.OnComplete();

        qeustMangeren.AddQuests(2);
    }
}
