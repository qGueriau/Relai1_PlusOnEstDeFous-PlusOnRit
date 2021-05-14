using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    public static Map instance;

    public float sizeX = 300;
    public float sizeZ = 300;

    public int nbQuartiers = 500;

    public Transform roadParent;
    public Transform ground;

    public List<Voronoi> quartiers = new List<Voronoi>();


    public Transform batimentParent;
    public List<GameObject> batimentsPrefab;
    public List<Batiment> listBatiments;
    public int nbBatiments = 50;
    public float biggestRadiusBatiment = 8;

    public bool isInit = false;

    //public float distanceMinBetweeenQuartiers = 8f;
    public class Voronoi
    {
        public int id;
        public Vector2 position;
        public List<Voronoi> voisins;

        public Voronoi(int id, Vector2 position)
        {
            this.id = id;
            this.position = position;
            voisins = new List<Voronoi>();
        }
    }

    public GameObject roadPrefab;

    void Start()
    {
        instance = this;
        ground.transform.localScale = new Vector3(sizeX, 1, sizeZ);
        GenerateMap();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GenerateMap()
    {


        for (int i = 0; i < nbQuartiers; i++)
        {
            Vector2 position = new Vector2(sizeX * Random.Range(-0.49f, 0.49f), sizeZ * Random.Range(-0.49f, 0.49f));
            Voronoi newquartier = new Voronoi(i, position);

            foreach (Voronoi quartier in quartiers)
            {
                float distance = Vector2.SqrMagnitude(quartier.position - position);
                bool closest = true;
                foreach (Voronoi quartierAutre in quartiers)
                {
                    float distanceAutre = Vector2.SqrMagnitude(quartierAutre.position - position);
                    if (quartier != quartierAutre && distanceAutre < distance + 0.1f)
                    {
                        closest = false;
                        break;
                    }

                }
                if (closest)
                {
                    newquartier.voisins.Add(quartier);
                    quartier.voisins.Add(newquartier);
                }
            }
            quartiers.Add(newquartier);
        }

        /*foreach (Voronoi quartier in quartiers)
        {
            for (int idQuartier = quartier.voisins.Count - 1; idQuartier >= 0; idQuartier--)
            {
                Voronoi quartierVoisin = quartier.voisins[idQuartier];
                float distance = Vector2.SqrMagnitude(quartier.position - quartierVoisin.position);
                bool closest = true;
                foreach (Voronoi quartierAutre in quartiers)
                {
                    float distanceAutre = Vector2.SqrMagnitude(quartierAutre.position - quartier.position);
                    if (quartierVoisin != quartierAutre && quartier != quartierAutre && distanceAutre < distance + 0.1f)
                    {
                        closest = false;
                        break;
                    }

                }
                if (!closest)
                {
                    quartierVoisin.voisins.Remove(quartier);
                    quartier.voisins.RemoveAt(idQuartier);
                }
            }


        }*/

        foreach (Voronoi quartier in quartiers)
        {
            foreach (Voronoi quartierVoisin in quartier.voisins)
            {
                if (quartier.id < quartierVoisin.id)
                {
                    GameObject road = GameObject.Instantiate(roadPrefab, roadParent);
                    road.transform.position = 0.5f * new Vector3(quartier.position.x + quartierVoisin.position.x, 0f, quartier.position.y + quartierVoisin.position.y);
                    road.transform.LookAt(new Vector3(quartier.position.x, 0f, quartier.position.y));
                    road.transform.Rotate(90 * Vector3.up);
                    road.transform.localScale = new Vector3(Vector2.Distance(quartier.position, quartierVoisin.position), 1, 1);
                }
            }
        }

        for (int i = 0; i < nbBatiments; i++)
        {
            Vector2 position = new Vector2(sizeX * Random.Range(-0.48f, 0.48f), sizeZ * Random.Range(-0.48f, 0.48f));
            bool tooClose = false;
            foreach (Voronoi quartier in quartiers)
            {
                if(Vector2.SqrMagnitude(quartier.position- position)< biggestRadiusBatiment* biggestRadiusBatiment)
                {
                    
                    tooClose = true;
                    break;
                }
            }
            if(!tooClose && Vector2.SqrMagnitude(position)>64f )
            {
                GameObject batiment = GameObject.Instantiate(batimentsPrefab[Random.Range(0, batimentsPrefab.Count)], batimentParent);
                batiment.transform.position = new Vector3(position.x, 0, position.y);
                listBatiments.Add(batiment.GetComponent<Batiment>());
            }
            else
            {
                i--;
            }
        }

        isInit = true;
    }


}
