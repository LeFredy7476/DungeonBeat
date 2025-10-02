
using UnityEngine;

public abstract class MyMonoBehaviour : MonoBehaviour
{
    protected Globals globals
    {
        get { return Globals.Instance; }
    }
}