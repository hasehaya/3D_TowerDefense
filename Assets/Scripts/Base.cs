using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Base : MonoBehaviour
{
    Slider slider;
    [SerializeField] GameObject target;
    [SerializeField] int hp;
    [SerializeField] GameObject hpBar;
    void Start()
    {
        //SetDestination(GameManager.Instance.GetBase().transform);
        var parent = this.transform;
        var sliderObj = Instantiate(hpBar, parent);
        Vector3 pos = sliderObj.transform.position;
        float height = gameObject.GetComponent<BoxCollider>().size.y;
        pos.y += height + 7;
        sliderObj.transform.position = new Vector3(pos.x, pos.y, pos.z);
        ;
        slider = sliderObj.GetComponentInChildren<Slider>();
        slider.maxValue = hp;
        slider.value = hp;
    }

    public void TakeDamage(int damage)
    {
        hp -= damage;
        slider.value -= damage;
        if (hp <= 0)
        {
            Destroy(gameObject);
        }
    }
}
