using UnityEngine;
using UnityEngine.SceneManagement;

//code to move between scenes from menu
public class MenuManager : MonoBehaviour
{

    public void ExitGame(){
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    public void OpenTutorial(){
        SceneManager.LoadScene("Tutorial");
    }

    public void StartGame(){
        SceneManager.LoadScene("MainGame");
    }
}
