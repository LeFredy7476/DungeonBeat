using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : GridEntity
{
    public void Unlock()
    {
        playerOnlyPass = true;
        GetComponent<Animator>().SetTrigger("Unlock");
    }
}
