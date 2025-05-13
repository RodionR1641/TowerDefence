using UnityEngine;
using UnityEngine.SceneManagement;

//functionality to go back to menu
public class TutorialManager : MonoBehaviour
{
    public void OpenMenu(){
        SceneManager.LoadScene("Menu");
    }
}
