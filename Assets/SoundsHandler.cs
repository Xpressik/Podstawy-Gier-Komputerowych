using UnityEngine;
using System.Collections;

public class SoundsHandler : MonoBehaviour {

    public AudioClip moveSound;

    public AudioClip suppliesSound;

    public AudioClip incorrectMoveSound;

    public AudioClip notEnoughSupplies;

    public AudioClip buildingPlacement;

    public AudioClip destruction;

    private AudioSource source;

	void Awake()
    {
        source = GetComponent<AudioSource>();
    }
    
    public void PlayMoveSound()
    {
        source.PlayOneShot(moveSound);
    }

    public void PlaySuppliesSound()
    {
        source.PlayOneShot(suppliesSound);
    }

    public void PlayIncorrectMoveSound()
    {
        source.PlayOneShot(incorrectMoveSound);
    }

    public void PlayNotEnoughSuppliesSound()
    {
        source.PlayOneShot(notEnoughSupplies);
    }

    public void PlayBuildingPlacement()
    {
        source.PlayOneShot(buildingPlacement);
    }

    public void PlayDestructionSound()
    {
        source.PlayOneShot(destruction);
    }
}
