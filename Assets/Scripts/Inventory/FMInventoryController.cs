using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FMInventoryController : MonoBehaviour
{
    [SerializeField]


    void Start()
    {
        
    }


    void Update()
    {
        
    }

    public void BackToHome()
    {
        SceneManager.LoadScene("Home");
    }
}
