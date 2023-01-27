using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] private Button btnRestart;

    private void Awake()
    {
        btnRestart.onClick.AddListener(() =>
        {
            SceneManager.LoadScene(0);
        });
    }
}
