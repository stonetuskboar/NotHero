using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadScene : MonoBehaviour
{
    public Button button;


    
    public void loadscene()
    {

        SceneManager.LoadScene("Game");
    }
}
