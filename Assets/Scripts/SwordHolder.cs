using UnityEngine;

public class SwordHolder : MonoBehaviour
{

    public GameObject sword;
    public Sprite swordUp;
    public Sprite swordDown;
    public Sprite swordLeft;
    public Sprite swordRight;
    public AudioClip[] slashes;
    [Range(0f, 1f)] public float slashVol;
    Globals globals;
    AudioSource audioSource;

    void Start()
    {
        globals = Globals.Instance;
        audioSource = GetComponent<AudioSource>();
    }

    public void Slash(Face.Faces face)
    {
        Turn(face);
        SlashSound();
        sword.GetComponent<SpriteRenderer>().sprite =
            Face.Select(face, swordUp, swordDown, swordLeft, swordRight);
        int currentTick = globals.currenTick % 2;
        sword.GetComponent<Animator>().SetBool("Reverse", currentTick == 1);
        sword.GetComponent<Animator>().SetTrigger("Slash");
    }

    public void SlashSound()
    {
        int currentTick = globals.currenTick % 2;
        audioSource.pitch = Random.Range(0.75f, 1.25f);
        audioSource.PlayOneShot(slashes[currentTick], slashVol);
    }

    public void Turn(Face.Faces face)
    {
        sword.GetComponent<Transform>().eulerAngles = Face.ToRotation(face);
        sword.GetComponent<Animator>().SetInteger("Face", Face.ToInt(face));
    }
}
