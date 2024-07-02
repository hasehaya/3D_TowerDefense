using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class FPSToText :MonoBehaviour
{
    Text text;

    int second = 1;

    int frameCount = 0; // Updateが呼ばれた回数カウント用
    float oldTime = 0.0f; // 前回フレームレートを表示してからの経過時間計算用

    private void Start()
    {
#if UNITY_EDITOR || DEBUG
        // テキストコンポーネントを取得
        text = GetComponent<Text>();

        // テキストを初期化
        text.text = "0 FPS";
        var parent = transform.parent;
        DontDestroyOnLoad(parent);
#else
        Destroy(gameObject);
#endif
    }

#if UNITY_EDITOR || DEBUG
    void Update()
    {
        // Updateが呼ばれた回数を加算
        frameCount++;

        // 前フレームからの経過時間を計算：Time.realtimeSinceStartupはゲーム開始時からの経過時間（秒）
        float time = Time.realtimeSinceStartup - oldTime;

        // 指定時間を超えたらテキスト更新
        if (time >= second)
        {
            // フレームレートを計算
            float fps = frameCount / time;

            // 計算したフレームレートを小数点1桁まで丸めてテキスト表示
            text.text = string.Format("{0:F1} FPS", fps);

            // カウントと経過時間をリセット
            frameCount = 0;
            oldTime = Time.realtimeSinceStartup;
        }
    }
#endif
}
