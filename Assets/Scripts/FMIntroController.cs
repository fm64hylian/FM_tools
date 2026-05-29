using UnityEngine;
using UnityEngine.SceneManagement;

public class FMIntroController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public void LoadStageBuilder() {
        Debug.Log("TODO add stagebuildr back");
        //SceneManager.LoadScene("StageBuilder", LoadSceneMode.Single);
    }

    public void LoadStageTester()
    {
        Debug.Log("TODO add tester back");
        //SceneManager.LoadScene("StageTest", LoadSceneMode.Single);
    }

    public void LoadOnlineIntroLoader()
    {
        SceneManager.LoadScene("IntroLoaderOnline", LoadSceneMode.Single);
    }
}
