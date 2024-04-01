using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource music;
    public AudioSource sound;
    public AudioClip bgMusic;
    public AudioClip bossMusic;
    public AudioClip jump;
    public AudioClip dead;
    public AudioClip gem;
    public AudioClip dash;
    public AudioClip sword;
    public AudioClip blip;
    public bool canPlaySound;
    public bool canPlayMusic;

    private void Start()
    {
        canPlaySound = true;
        canPlayMusic = true;
    }

    public void ToggleMusic()
    {
        if (canPlayMusic)
        {
            canPlayMusic = false;
            music.Pause();
        }
        else
        {
            canPlayMusic = true;
            music.Play();
        }
    }

    public void ToggleSound()
    {
        if (canPlaySound)
        {
            canPlaySound = false;
        }
        else
        {
            canPlaySound = true;
        }
    }

    public void Background()
    {
        if (canPlayMusic)
        {
            music.clip = bgMusic;
            music.Play();
        }        
    }

    public void Boss()
    {
        if (canPlayMusic)
        {
            music.clip = bossMusic;
            music.Play();
        }        
    }

    public void Blip()
    {
        if (canPlaySound)
        {
            sound.PlayOneShot(blip);
        }
    }

    public void Jump()
    {
        if (canPlaySound)
        {
            sound.PlayOneShot(jump);
        }
    }

    public void Dead()
    {
        if (canPlaySound)
        {
            sound.PlayOneShot(dead);
        }        
    }

    public void Gem()
    {
        if (canPlaySound)
        {
            sound.PlayOneShot(gem);
        }       
    }

    public void Dash()
    {
        if (canPlaySound)
        {
            sound.PlayOneShot(dash);
        }        
    }

    public void Sword()
    {
        if (canPlaySound)
        {
            sound.PlayOneShot(sword);
        }
    }
}
