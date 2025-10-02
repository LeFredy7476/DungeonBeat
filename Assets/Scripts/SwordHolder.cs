using UnityEngine;

public class SwordHolder : MyMonoBehaviour
{

    public GameObject sword;
    public Sprite swordUp;
    public Sprite swordDown;
    public Sprite swordLeft;
    public Sprite swordRight;
    public AudioClip[] attacks;
    [Range(0f, 1f)] public float attackVol;
    AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void Attack(Face.Faces face)
    {
        Turn(face);
        AttackSound();
        sword.GetComponent<SpriteRenderer>().sprite =
            Face.Select(face, swordUp, swordDown, swordLeft, swordRight);
        int currentTick = globals.currenTick % 2;
        sword.GetComponent<Animator>().SetBool("Reverse", currentTick == 1);
        sword.GetComponent<Animator>().SetTrigger("Slash");
    }

    public void AttackSound()
    {
        int currentTick = globals.currenTick % 2;
        audioSource.pitch = Random.Range(0.75f, 1.25f);
        audioSource.PlayOneShot(attacks[currentTick], attackVol);
    }

    public void Turn(Face.Faces face)
    {
        sword.GetComponent<Transform>().eulerAngles = Face.ToRotation(face);
        sword.GetComponent<Animator>().SetInteger("Face", Face.ToInt(face));
    }
}
