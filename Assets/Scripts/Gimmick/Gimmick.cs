using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gimmick : MonoBehaviour
{

    private float nearDistance = 10.0f;
    private float waitTime = 10.0f;
    bool waiting = false;
    bool isShowNotice = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        checkDistance();
    }

    public void WaitPlayerExcute()
    {
        waiting = true;
        HideNotice();
        Player.Instance.setCanMove(false);
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

    void checkDistance()
    {
        if(waiting)
        {
            NoticeManager.Instance.HideAllNotice();
            return;
        }
        var playerPosition = Player.Instance.transform.position;
        if (Vector3.Distance(transform.position, playerPosition) < nearDistance)
        {
            isShowNotice = true;
            ShowNotice();
            if (Fairy.Instance.getCanUse())
            {
                NoticeManager.Instance.ShowFuncNotice(NoticeManager.NoticeType.Fairy, useFairy);
            }

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

    void  useFairy()
    {
        waiting = true;
        Fairy.Instance.setCanUse(false);
        Fairy.Instance.setTargetPosition(this.gameObject.transform.position);
        Invoke(nameof(Excute), waitTime);
        Invoke(nameof(FinishFairy), waitTime);
    }

    void FinishFairy()
    {
        Fairy.Instance.setCanUse(true);
    }

    void FinishPlayerAction()
    {
        Player.Instance.setCanMove(true);
    }

}
