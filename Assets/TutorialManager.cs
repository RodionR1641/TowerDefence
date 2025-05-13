using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour
{
    public void OpenMenu(){
        SceneManager.LoadScene("Menu");
    }
}
