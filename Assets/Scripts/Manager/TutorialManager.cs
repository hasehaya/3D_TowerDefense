using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class TutorialManager
{
    private static TutorialManager instance;
    public static TutorialManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new TutorialManager();
            }
            return instance;
        }
    }

    int messageIndex;

    public TutorialManager()
    {
        messageIndex = 0;
        ShowMessage();

        NoticeManager.OnNotice += ShowMessageByNotice;
        FacilityManager.OnPurchase += ShowMessageByPurchase;
    }

    ~TutorialManager()
    {
        NoticeManager.OnNotice -= ShowMessageByNotice;
        FacilityManager.OnPurchase -= ShowMessageByPurchase;
    }

    public static void DestroyInstance()
    {
        instance = null;
    }

    private void ShowMessage()
    {
        MessageWindowManager.Instance.ShowMessagesWithId(SharedSceneData.StageNum, messageIndex);
        messageIndex++;
    }

    private void ShowMessageByNotice(NoticeManager.NoticeType noticeType)
    {
        switch (SharedSceneData.StageNum)
        {
            case 4:
            if (messageIndex == 1 && noticeType == NoticeManager.NoticeType.Climb)
            {
                ShowMessage();
                break;
            }
            break;
        }
    }

    private void ShowMessageByPurchase()
    {
        if (SharedSceneData.StageNum == 1 && messageIndex == 1)
        {
            ShowMessage();
        }
    }
}
