using UnityEngine;

public class PlayParticle : MonoBehaviour
{
    [SerializeField] ParticleSystem _ghostParticle;
    [SerializeField] ParticleSystem _candytParticle;

    ParticleSystem particleToPlay;

    private void Awake()
    {
        this.transform.parent.parent.TryGetComponent(out House house);
        house.OnSetGhost += setParticleToPlay;
    }

    void setParticleToPlay(bool isGhost)
    {
        if (isGhost)
        {
            particleToPlay = _ghostParticle;
        }
        else
        {
            particleToPlay = _candytParticle;

        }

    }
    public void PlayingParticle()
    {
        particleToPlay.Play();
    }

}


