using UnityEngine;

public class SimpleSound : MonoBehaviour
{
    [SerializeField]
    private AudioClip hitClip;

    [SerializeField]
    private float volume = 0.8f;

    public void PlayHitAt(Vector3 position)
    {
        if(hitClip != null)
        {
            AudioSource.PlayClipAtPoint(hitClip, position, volume);
        }
    }
}
