using System.Collections;
using UnityEngine;

public class Quest2 : QuestBasic
{
    private float questDurationWait = 3f;

    public override void ActiveQuest()
    {
        questText.text = "wait 3 sec";
        base.ActiveQuest();

        StartCoroutine(WaitQuest());
    }

    IEnumerator WaitQuest()
    {
        yield return new WaitForSeconds(questDurationWait);

        OnComplete();
    }
}
