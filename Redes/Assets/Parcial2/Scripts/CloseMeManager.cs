using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CloseMeManager : MonoBehaviour
{
    

    public IEnumerator GoToLobby(NetworkRunner runner)
    {
        yield return new WaitForSeconds(2);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        runner.Shutdown();
        SceneManager.LoadScene(0);
    }


}
