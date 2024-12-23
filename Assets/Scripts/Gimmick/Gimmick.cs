using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class Gimmick :MonoBehaviour
{

    private float nearDistance = 10.0f;
    private float waitTime = 3.0f;
    bool waiting = false;
    bool isShowNotice = false;

    protected virtual void Update()
    {
        CheckDistance();
    }

    public void WaitPlayerExcute()
    {
        waiting = true;
        HideNotice();
        Player.Instance.SetCanMove(false);
        Invoke(nameof(Excute), waitTime);
        Invoke(nameof(FinishPlayerAction), waitTime);
    }

    protected virtual void Excute()
    {

    }

    protected virtual void ShowNotice()
    {

    }

    protected virtual void HideNotice()
    {

    }

    void CheckDistance()
    {
        if (waiting)
        {
            return;
        }
        var playerPosition = Player.Instance.transform.position;
        if (Vector3.Distance(transform.position, playerPosition) < nearDistance)
        {
            isShowNotice = true;
            ShowNotice();
            /*
            if (Fairy.Instance.getCanUse())
            {
                NoticeManager.Instance.ShowFuncNotice(NoticeManager.NoticeType.Fairy, UseFairy);
            }
            */

        }
        else
        {
            if (isShowNotice)
            {
                HideNotice();
                NoticeManager.Instance.HideNotice(NoticeManager.NoticeType.Fairy);
            }
        }
    }

    void UseFairy()
    {
        waiting = true;
        Fairy.Instance.SetCanUse(false);
        Fairy.Instance.setTargetPosition(this.gameObject.transform.position);
        Invoke(nameof(Excute), waitTime);
        Invoke(nameof(FinishFairy), waitTime);
    }

    void FinishFairy()
    {
        Fairy.Instance.SetCanUse(true);
    }

    void FinishPlayerAction()
    {
        Player.Instance.SetCanMove(true);
    }

}
