using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class BaseHpView :MonoBehaviour
{
    [SerializeField] Text hpText;

    public void UpdateHpText(int currentHp, int maxHp)
    {
        hpText.text = currentHp.ToString() + "/" + maxHp.ToString();
    }
}
