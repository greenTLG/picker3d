using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    public static ParticleManager Instance { get; set; }

    public List<Particle> particles = new List<Particle>();
    public Dictionary<string, ParticleSystem[]> particlePool;
    public Dictionary<string, bool> particleRandomChoose;

    private void Awake()
    {
        Instance = this;

        particlePool = new Dictionary<string, ParticleSystem[]>();
        particleRandomChoose = new Dictionary<string, bool>();

        for (int i = 0; i < particles.Count; i++)
        {
            if (particles[i].particles == null || particles.Count == 0)
                continue;
            particlePool.Add(particles[i].tag, particles[i].particles);
            particleRandomChoose.Add(particles[i].tag, particles[i].chooseRandomOne);
        }


    }

    public void Play(string tag, Vector3 pos)
    {
        if (!particlePool.ContainsKey(tag))
        {
            //Debug.Log(tag + " particle doesn't exist");
            return;
        }
        ParticleSystem[] particles = particlePool[tag];
        if (particleRandomChoose[tag])
        {
            int index = Random.Range(0, particles.Length);

            particles[index].transform.localPosition = pos;
            particles[index].Play();
        }
        else
        {
            for (int i = 0; i < particles.Length; i++)
            {
                particles[i].transform.position = pos;
                particles[i].Play();
            }
        }
    }
}
[System.Serializable]
public class Particle
{
    public string tag;
    public ParticleSystem[] particles = new ParticleSystem[0];
    public bool chooseRandomOne = false;
}