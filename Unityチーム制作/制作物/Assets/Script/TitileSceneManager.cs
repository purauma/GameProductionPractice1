using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using TMPro;

public class TitileSceneManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI anyButtonText;
    [SerializeField] private float blinkSpeed = 2.0f;

    void Update()
    {
        // 文字の点滅処理
        if (anyButtonText != null)
        {
            float alpha = Mathf.Abs(Mathf.Sin(Time.time * blinkSpeed));
            anyButtonText.color = new Color(anyButtonText.color.r, anyButtonText.color.g, anyButtonText.color.b, alpha);
        }

        // キーボード入力チェック
        if (Keyboard.current != null && Keyboard.current.anyKey.wasPressedThisFrame)
        {
            ChangeScene();
            return;
        }

        // ゲームパッド入力チェック
        if (Gamepad.current != null)
        {
            foreach (var control in Gamepad.current.allControls)
            {
                if (control is UnityEngine.InputSystem.Controls.ButtonControl button && button.wasPressedThisFrame)
                {
                    ChangeScene();
                    return;
                }
            }
        }
    }

    private void ChangeScene()
    {
        // プレイシーンへ遷移
        SceneManager.LoadScene("EndoScene");
    }
}