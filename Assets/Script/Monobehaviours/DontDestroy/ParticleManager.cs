using UnityEngine;
using System.Collections.Generic;

public class ParticleManager : MonoBehaviour
{
    //  public variables
    public static ParticleManager Instance;
    public List<ParticleSystem> p_systems;
    //  private variables

    //  private methods
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            return;
        }
        Destroy(this.gameObject);
    }


    //  public methods

    /// <summary>
    /// Starts a particle system up
    /// </summary>
    /// <param name="p"></param>
    public void StartParticles(ParticleType p) 
    {
        p_systems[(int)p].Play();
    }

    /// <summary>
    /// Stops a particle system
    /// </summary>
    /// <param name="p"></param>
    public void StopParticles(ParticleType p) 
    {
        p_systems[(int)p].Stop();
    }
}

public enum ParticleType { SandStorm };