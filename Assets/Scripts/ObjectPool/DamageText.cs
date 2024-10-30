using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;

public class DamageText :MonoBehaviour, IPoolable
{
    public Text damageText; // Text コンポーネントをアタッチ
    private float displayDuration = 1f;
    private float floatSpeed = 1f;
    private Color textColor;
    private float elapsedTime = 0f;
    private bool isActive = false;

    public void SetDamage(float damage)
    {
        damageText.text = Mathf.Abs(damage).ToString();

        if (damage >= 0)
        {
            damageText.color = Color.red;
        }
        else
        {
            damageText.color = Color.green;
        }

        textColor = damageText.color;
    }

    public void OnObjectReuse()
    {
        elapsedTime = 0f;
        isActive = true;
        textColor = damageText.color;
    }

    public void OnObjectReturn()
    {
        isActive = false;
    }

    void Update()
    {
        if (!isActive)
            return;

        // カメラの方向を向く
        if (Camera.main != null)
        {
            transform.LookAt(Camera.main.transform);
            transform.Rotate(0, 180, 0);
        }

        // 上昇する
        transform.position += Vector3.up * floatSpeed * Time.deltaTime;

        // 経過時間を更新
        elapsedTime += Time.deltaTime;

        // フェードアウト
        float alpha = Mathf.Lerp(1f, 0f, elapsedTime / displayDuration);
        textColor.a = alpha;
        damageText.color = textColor;

        if (elapsedTime >= displayDuration)
        {
            // オブジェクトプールに戻す
            ObjectPool<DamageText>.Instance.ReturnObject(this);
        }
    }
}
