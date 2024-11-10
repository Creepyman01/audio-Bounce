using System.IO;
using UnityEngine;
using UnityEngine.AI;

public class AudioBounce : MonoBehaviour
{
    public GameObject Playa;
    public GameObject AudioSource;
    public float Distance = 10;
    public float MaxVolume;
    float LerpusAmountsus;
    float PathDistance;

    NavMeshTriangulation Triangulation;
    NavMeshPath Pathsus;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Triangulation = NavMesh.CalculateTriangulation();
        Pathsus = new NavMeshPath();
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(Playa.transform.position, this.transform.position) <= Distance)
        {
            if (NavMesh.CalculatePath(this.transform.position, Playa.transform.position, NavMesh.AllAreas, Pathsus))
            {
                PathDistance = 0;
                if ((Pathsus.status != NavMeshPathStatus.PathInvalid) && (Pathsus.corners.Length > 1))
                {
                    for (int i = 1; i < Pathsus.corners.Length; ++i)
                    {
                        PathDistance += Vector3.Distance(Pathsus.corners[i - 1], Pathsus.corners[i]);
                    }
                }

                float ReverseDistance = Mathf.Clamp((Vector3.Distance(Playa.transform.position, AudioSource.transform.position) * -1) + 4, 1, 4);
                AudioSource.GetComponent<AudioSource>().volume = (((PathDistance * -1) + Distance) / Distance * MaxVolume) / ReverseDistance;

                LerpusAmountsus = (Vector3.Distance(Playa.transform.position, Pathsus.corners[Pathsus.corners.Length - 2]) / (Distance / (Distance / 10))) * PathDistance / 10;

                if (Pathsus.corners.Length >= 3)
                {
                    if (Pathsus.corners.Length >= 4)
                    {
                        if (Vector3.Distance(Pathsus.corners[Pathsus.corners.Length - 2], Pathsus.corners[Pathsus.corners.Length - 3]) > 1.5f)
                        {
                            AudioSource.transform.position = Vector3.Lerp(Pathsus.corners[Pathsus.corners.Length - 3], Pathsus.corners[Pathsus.corners.Length - 2], LerpusAmountsus);
                        }
                        else
                        {
                            AudioSource.transform.position = Vector3.Lerp(Pathsus.corners[Pathsus.corners.Length - 4], Pathsus.corners[Pathsus.corners.Length - 2], LerpusAmountsus);
                        }
                    }
                    else
                    {
                        AudioSource.transform.position = Vector3.Lerp(this.transform.position, Pathsus.corners[Pathsus.corners.Length - 2], LerpusAmountsus);
                    }
                }
                else
                {
                    AudioSource.transform.position = this.transform.position;
                }
            }
        }
        else
        {
            AudioSource.GetComponent<AudioSource>().volume = 0;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(this.transform.position, Distance);
    }
}
