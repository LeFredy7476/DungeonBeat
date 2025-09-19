using UnityEngine;

public class BeatMaker : MonoBehaviour
{

    public AudioSource audioSource;
    public AudioClip smallKick;
    public AudioClip bigKick;
    public AudioClip hitHat;
    [Range(0f, 1f)] public float kickPower;

    Globals globals;

    void Start()
    {
        globals = Globals.Instance;
    }

    void Update()
    {

    }

    public void Tick8()
    {
        if (globals.currentTick8 == 0)
        {
            if (globals.currenTick % 2 == 1) audioSource.PlayOneShot(smallKick, kickPower);
            else audioSource.PlayOneShot(bigKick, kickPower);
        }
        else if (globals.currentTick8 == 4)
        {
            audioSource.PlayOneShot(hitHat);
        }
    }
}
