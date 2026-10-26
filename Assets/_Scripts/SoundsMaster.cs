using UnityEngine;

public class SoundsMaster : MonoBehaviour
{

    public AudioClip FireballExplosion;
    public AudioClip Fireballrelease;
    public AudioClip Sadtromboneacappella;
    public AudioClip Sadtrombone1;
    public AudioClip Sadtromboneshort;
    public AudioClip Sadtromboneplayback;
    public AudioClip Sadtrombonecartoon;
    public AudioClip Bell_miss1;
    public AudioClip Bell_miss2;
    public AudioClip Bell_miss3;
    public AudioClip Bell_miss4;
    public AudioClip Fireballflight2;
    public AudioClip Flightofthefireball3;
    public AudioClip FireballFlight;
    public AudioClip FireballHit2;
    public AudioClip Fireballhit3;
    public AudioClip Hittingafireball;
    public AudioClip Asimplebell;
    public AudioClip Scatteringofbones;
    public AudioClip Bonecrunching;

    private AudioSource audioSource;








    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }



    void Update()
    {

    }



    public void PlaySoundFireballExplosion()    //Звук взрыва огненного шара
    {
        audioSource.PlayOneShot(FireballExplosion);
    }

    public void PlaySoundFireballrelease()  //Звук выпускания огненного шара
    {
        audioSource.PlayOneShot(Fireballrelease);
    }
    public void PlaySoundSadtromboneacappella() //Звук проигрыша 1
    {
        audioSource.PlayOneShot(Sadtromboneacappella);
    }
    public void PlaySoundSadtrombone1() //Звук проигрыша 2
    {
        audioSource.PlayOneShot(Sadtrombone1);
    }
    public void PlaySoundSadtromboneshort() //Звук проигрыша 3
    {
        audioSource.PlayOneShot(Sadtromboneshort);
    }
    public void PlaySoundSadtromboneplayback() //Звук проигрыша 4
    {
        audioSource.PlayOneShot(Sadtromboneplayback);
    }
    public void PlaySoundSadtrombonecartoon() //Звук проигрыша 5
    {
        audioSource.PlayOneShot(Sadtrombonecartoon);
    }
    public void PlaySoundBell_miss1() //Звук промаха 1
    {
        audioSource.PlayOneShot(Bell_miss1);
    }
    public void PlaySoundBell_miss2() //Звук промаха 2
    {
        audioSource.PlayOneShot(Bell_miss2);
    }
    public void PlaySoundBell_miss3() //Звук промаха 3
    {
        audioSource.PlayOneShot(Bell_miss3);
    }
    public void PlaySoundBell_miss4() //Звук промаха 4
    {
        audioSource.PlayOneShot(Bell_miss4);
    }
    public void PlaySoundFireballflight2() //Звук полёта огненного шара 2
    {
        audioSource.PlayOneShot(Fireballflight2);
    }
    public void PlaySoundFlightofthefireball3() //Звук полёта огненного шара 3
    {
        audioSource.PlayOneShot(Flightofthefireball3);
    }
    public void PlaySoundFireballFlight() //Звук полёта огненного шара 1
    {
        audioSource.PlayOneShot(FireballFlight);
    }
    public void PlaySoundFireballHit2() //Звук попадания огненного шара 2
    {
        audioSource.PlayOneShot(FireballHit2);
    }
    public void PlaySoundFireballhit3() //Звук попадания огненного шара 3
    {
        audioSource.PlayOneShot(Fireballhit3);
    }
    public void PlaySoundHittingafireball() //Звук попадания огненного шара 1
    {
        audioSource.PlayOneShot(Hittingafireball);
    }
    public void PlaySoundAsimplebell() //Звук колокольчика (при попадании в красного скелета)
    {
        audioSource.PlayOneShot(Asimplebell);
    }
    public void PlaySoundScatteringofbones() //Звук попадания огненного шара в скелета (посмертно)
    {
        audioSource.PlayOneShot(Scatteringofbones);
    }
    public void PlaySoundBonecrunching()    //Звук попадания огненного шара в скелета (не посмертно)
    {
        audioSource.PlayOneShot(Bonecrunching);
    }




}
