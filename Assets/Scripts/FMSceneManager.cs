using UnityEngine;
using UnityEngine.SceneManagement;

public class FMSceneManager : MonoBehaviour
{
    public void GoToStart()
    {
        FMInternalMode mode = FMClientSessionData.Instance.InternalMode;
        string scene = mode.Equals(FMInternalMode.OffLine) ? "IntroLoader" : "Home";
        SceneManager.LoadScene(scene);
    }

    public void LoadOnlineIntroLoader()
    {
        //setup "online mode"
        FMClientSessionData.Instance.InternalMode = FMInternalMode.OnLine;
        SceneManager.LoadScene("IntroLoaderOnline", LoadSceneMode.Single);
    }

    public void GoToAchievements()
    {
        SceneManager.LoadScene("PlayfabAchivements");
    }

    public void GoToStore()
    {
        SceneManager.LoadScene("Store");
    }

    public void GoToInventory()
    {
        SceneManager.LoadScene("Inventory");
    }

    public void GoToStageBuilder()
    {
        SceneManager.LoadScene("StageBuilder", LoadSceneMode.Single);
    }

    public void GoToBuilderTest()
    {
        SceneManager.LoadScene("BuilderTest", LoadSceneMode.Single);
    }

    public void GoTo3DPlayTest()
    {
        SceneManager.LoadScene("Test3D"); //Test3D is the beta stage
    }


    public void LoadStageTester()
    {
        Debug.Log("TODO add tester back");
        //SceneManager.LoadScene("StageTest", LoadSceneMode.Single);
    }
}
