using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Btn : MonoBehaviour
{
    private Button btn;

    void Start()
    {
        btn = GetComponent<Button>();
        btn.onClick.AddListener(TaskButton);
    }

    private void TaskButton()
    {
        SceneManager.LoadScene(1);
    }
}
