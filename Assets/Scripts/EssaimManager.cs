using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EssaimManager : MonoBehaviour
{
    public static EssaimManager instance;

    public EssaimAI essaimMaitre;
    public Transform essaimParent;
    public GameObject prefabEssaim;

    public List<EssaimAI> listEssaim = new List<EssaimAI>();
    public List<Ennemi> listEnnemis = new List<Ennemi>();



    public float followMasterPower = 4f;
    public float followMasterMaxForce = 5f;
    public float avoidMasterPower = 7f;
    public float distanceMinToAvoidMaster = 3f;

    public float avoidOtherPower = 2f;
    public float distanceMinToAvoid = 15f;

    public float avoidBatimentPower = 3f;
    public float distanceMinToAvoidBatiment = 15f;

    public float distanceToConversion = 3f;

    public float attrackedByThingsToconvertPower = 4f;
    public float attrackedByThingsToconvertrMaxForce = 3f;
    public float avoidThingsToconvertPower = 3f;
    public float radiusToGoTryConvert = 5f;


    public int nbCivilStart = 300;
    public int nbvoitureStart = 50;
    public int nbBusStart = 25;

    public int nbSoldatStart = 0;
    public int nbTankStart = 0;

    public float probaPopCivilBase = 0.02f;
    public float probaPopVoitureBase = 0.005f;
    public float probaPopBusBase = 0.001f;

    public float probaPopCivilForEveryMemberEssaim = 0f;
    public float probaPopVoitureBaseForEveryMemberEssaim = 0f;
    public float probaPopBusBaseForEveryMemberEssaim = 0f;



    public float probaPopSoldatBase = -0.5f;
    public float probaPopTankBase = -1f;
    public float probaHelicoBase = -2f;

    public float probaPopSoldatForEveryMemberEssaim = 0.01f;
    public float probaPopTankForEveryMemberEssaim = 0f;
    public float probaPopHelicoForEveryMemberEssaim = 0f;


    /*public float probaPopSoldat = 0.02f;
    public float probaPopTank = 0.005f;
    public float probaPopHelicoptere = 0.001f;*/

    public bool activateAvoidanceBatiment = true;

    public GameObject prefabCivil;
    public GameObject prefabVoiture;
    public GameObject prefabBus;
    public GameObject prefabSoldat;
    public GameObject prefabTank;

    public Transform ennemiParent;
    public float raidusMinimumaroundEssaim = 50f;


    public Transform miniMapCamera;
    public float miniMapCameraStartZoom = 50f;
    public float miniMapCameraZoomEveryNewMember = 0.05f;

    public float score;
    public float time;

    public Text essaimSize;
    public Text timeScore;
    public Text scoreText;

    public float multiplicateurScoreBase = 1f;
    public float multiplicateurScoreMorForEachMemberOfEssaim = 0.01f;

    bool isInit = false;

    public GameObject panelPerdu;
    public Text timeScorePerdu;
    public Text scoreTextPerdu;

    public GameObject panelGagner;
    public Text timeScoreGagner;
    public Text scoreTextGagner;

    public static int nbCivil;
    public static int nbVoiture;
    public static int nbBus;
    public static int nbSoldat;
    public static int nbTank;

    public int nbMaxCivil = 200;
    public int nbMaxVoiture = 75;
    public int nbMaxBus = 35;
    public int nbMaxSoldat = 50;
    public int nbMaxTank = 25;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        listEssaim.Add(essaimMaitre.GetComponent<EssaimAI>());
       
    }

    // Update is called once per frame
    void Update()
    {

        


        if(Map.instance.isInit)
        {
            if(!isInit) GeneratePopInitial();

            if (listEssaim.Count>200)
            {
                panelGagner.SetActive(true);
            }
            else if (listEssaim.Count == 0)
            {
                panelPerdu.SetActive(true);
            }
            else
            {
                time += Time.deltaTime;
                score += Time.deltaTime * (multiplicateurScoreBase + multiplicateurScoreMorForEachMemberOfEssaim * listEssaim.Count);
                if(essaimMaitre!=null) miniMapCamera.transform.position = new Vector3(essaimMaitre.transform.position.x, miniMapCameraStartZoom + this.listEssaim.Count * miniMapCameraZoomEveryNewMember, essaimMaitre.transform.position.z);

            }



            float randomCivil = Random.Range(0, 1f);
            float randomVoiture = Random.Range(0, 1f);
            float randomBus = Random.Range(0, 1f);
            float randomSoldat = Random.Range(0, 1f);
            float randomTank = Random.Range(0, 1f);
            float randomHelico = Random.Range(0, 1f);

            if (nbCivil < nbMaxCivil && randomCivil < probaPopCivilBase + listEssaim.Count * probaPopCivilForEveryMemberEssaim) PopCivil(true);
            if (nbVoiture < nbMaxVoiture && randomVoiture < probaPopVoitureBase + listEssaim.Count * probaPopVoitureBaseForEveryMemberEssaim) PopVoiture(true);
            if (nbBus < nbMaxBus && randomBus < probaPopBusBase + listEssaim.Count * probaPopBusBaseForEveryMemberEssaim) PopBus(true);
            if (nbSoldat < nbMaxSoldat && randomSoldat < probaPopSoldatBase + listEssaim.Count * probaPopSoldatForEveryMemberEssaim) PopSoldat(true);
            if (nbTank < nbMaxTank && randomTank < probaPopTankBase + listEssaim.Count * probaPopTankForEveryMemberEssaim) PopTank(true);

        }



        

        timeScore.text = "Temps : " + time.ToString("F2");
        timeScorePerdu.text = "Temps : " + time.ToString("F2");
        timeScoreGagner.text = "Temps : " + time.ToString("F2");
        essaimSize.text = "Essaim : " + listEssaim.Count;
        scoreText.text = "Score : " + score.ToString("F2");
        scoreTextPerdu.text = "Score : " + score.ToString("F2");
        scoreTextGagner.text = "Score : " + score.ToString("F2");

        
#if UNITY_EDITOR

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Kill(essaimMaitre);
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            for (int i = 0; i < 10; i++)
            {
                GameObject essaimAI = GameObject.Instantiate(prefabEssaim, essaimParent);
                essaimAI.transform.position = essaimMaitre.transform.position + new Vector3(Random.Range(-5f, 5f), 0, Random.Range(-5f, 5f));
                EssaimAI essaimAiScript = essaimAI.GetComponent<EssaimAI>();
                essaimAiScript.isMaster = false;
                essaimAiScript.master = essaimMaitre;
                listEssaim.Add(essaimAiScript);
            }
        }

#endif
        if(essaimMaitre!=null)
        {
            foreach (Batiment bat in Map.instance.listBatiments)
            {
                if (bat.transform.position.z < essaimMaitre.transform.position.z)
                {
                    bat.opacityTarget = 0.25f;
                }
                else
                {
                    bat.opacityTarget = 1f;
                }
            }
        }
        



    }

    public Vector3 GetDeplacement(EssaimAI essaim)
    {
        Vector3 deplacement = Vector3.zero;

        Vector3 vectorToMaster = (essaimMaitre.transform.position - essaim.transform.position);
        float distanceToMaster = vectorToMaster.sqrMagnitude;

        if (distanceToMaster < distanceMinToAvoidMaster)
        {
            deplacement -= avoidMasterPower * vectorToMaster.normalized * (distanceMinToAvoidMaster - distanceToMaster) / distanceMinToAvoidMaster;
        }
        else
        {
            deplacement += Mathf.Clamp(followMasterPower * (distanceToMaster - distanceMinToAvoidMaster), 0f, followMasterMaxForce) * vectorToMaster.normalized ;
        }



        for (int i = 0; i < listEssaim.Count; i++)
        {
            EssaimAI toCompare = listEssaim[i];
            Vector3 vectorToVoisin = (toCompare.transform.position - essaim.transform.position);
            float distanceToVoisin = vectorToVoisin.sqrMagnitude;
            if (distanceToVoisin < distanceMinToAvoid)
            {
                deplacement -= avoidOtherPower * vectorToVoisin.normalized * (distanceMinToAvoid - distanceToVoisin) / distanceMinToAvoid;
            }
            
        }


        for (int i=0; i< listEnnemis.Count; i++)
        {
            Ennemi toCompare = listEnnemis[i];
            Vector3 vectorToEnnemi = (toCompare.transform.position - essaim.transform.position);
            float distanceToEnnemi = vectorToEnnemi.sqrMagnitude;
            if (distanceToEnnemi < toCompare.radius)
            {
                deplacement -= avoidThingsToconvertPower * vectorToEnnemi.normalized * (toCompare.radius - distanceToEnnemi) / toCompare.radius;
            }
            else if(distanceToEnnemi < toCompare.radius + radiusToGoTryConvert)
            {
                deplacement += (Mathf.Clamp(attrackedByThingsToconvertPower * (radiusToGoTryConvert - (distanceToEnnemi - toCompare.radius))/ radiusToGoTryConvert, 0f, attrackedByThingsToconvertrMaxForce)) * vectorToEnnemi.normalized;
            }
        }

        if(activateAvoidanceBatiment)
        {
            foreach (Batiment batiment in Map.instance.listBatiments)
            {
               
                Vector3 vectorToBatiment = (batiment.transform.position - essaim.transform.position);
                float distanceToBatiment = vectorToBatiment.sqrMagnitude;
                if (distanceToBatiment < distanceMinToAvoidBatiment)
                {
                    deplacement -= avoidBatimentPower * vectorToBatiment.normalized * (distanceMinToAvoidBatiment - distanceToBatiment) / distanceMinToAvoid;
                }

            }
        }
        



        return deplacement;
    }

    public bool isGettingConversion(Ennemi ennemi)
    {
        int nbClose = 0;
        for (int i = 0; i < listEssaim.Count; i++)
        {
            EssaimAI toCompare = listEssaim[i];
            Vector3 vectorToVoisin = (toCompare.transform.position - ennemi.transform.position);
            float distanceToVoisin = vectorToVoisin.sqrMagnitude;
            if (distanceToVoisin < ennemi.radius + distanceToConversion)
            {
                nbClose++;
                if(nbClose>=ennemi.nbToConvert)
                {
                    return true;
                }
            }
        }
        return false;
    }

    public void doConvert(Ennemi ennemi)
    {
        for(int i=0; i<ennemi.nbAfterConvertion; i++)
        {
            GameObject essaimAI = GameObject.Instantiate(prefabEssaim, essaimParent);
            essaimAI.transform.position = new Vector3(ennemi.transform.position.x, essaimMaitre.transform.position.y, ennemi.transform.position.z) + ennemi.radius * new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));
            EssaimAI essaimAiScript = essaimAI.GetComponent<EssaimAI>();
            essaimAiScript.isMaster = false;
            essaimAiScript.master = essaimMaitre;
            listEssaim.Add(essaimAiScript);
        }
        listEnnemis.Remove(ennemi);
    }

    public void GeneratePopInitial()
    {
        /*nbCivilStart = 300;
    public int nbvoitureStart = 50;
    public int nbBusStart = 25;*/

        for(int i=0; i<nbCivilStart; i++)
        {
            PopCivil(false);
        }

        for (int i = 0; i < nbvoitureStart; i++)
        {
            PopVoiture(false);
        }
        for (int i = 0; i < nbBusStart; i++)
        {
            PopBus(false);
        }
        for (int i = 0; i < nbSoldatStart; i++)
        {
            PopSoldat(false);
        }
        for (int i = 0; i < nbTankStart; i++)
        {
            PopTank(false);
        }

        isInit = true;
    }

    public void PopCivil(bool notTooMuchClose)
    {
        GameObject civil = GameObject.Instantiate(prefabCivil, ennemiParent);
        bool positionOk = false;
        while(!positionOk)
        {
            Vector3 position = new Vector3(Map.instance.sizeX * Random.Range(-0.5f, 0.5f), 0, Map.instance.sizeZ * Random.Range(-0.5f, 0.5f));
            if(notTooMuchClose && Vector3.SqrMagnitude(position-essaimMaitre.transform.position)< raidusMinimumaroundEssaim* raidusMinimumaroundEssaim)
            {
                positionOk = false;
            }
            else
            {
                positionOk = true;
                civil.transform.position = position;
            }
        }
        listEnnemis.Add(civil.GetComponent<Ennemi>());
        nbCivil++;
    }

    public void PopSoldat(bool notTooMuchClose)
    {
        GameObject soldat = GameObject.Instantiate(prefabSoldat, ennemiParent);
        bool positionOk = false;
        while (!positionOk)
        {
            Vector3 position = new Vector3(Map.instance.sizeX * Random.Range(-0.5f, 0.5f), 0, Map.instance.sizeZ * Random.Range(-0.5f, 0.5f));
            if (notTooMuchClose && Vector3.SqrMagnitude(position - essaimMaitre.transform.position) < raidusMinimumaroundEssaim * raidusMinimumaroundEssaim)
            {
                positionOk = false;
            }
            else
            {
                positionOk = true;
                soldat.transform.position = position;
            }
        }
        listEnnemis.Add(soldat.GetComponent<Ennemi>());
        nbSoldat++;
    }

    public void PopVoiture(bool notTooMuchClose)
    {
        GameObject voiture = GameObject.Instantiate(prefabVoiture, ennemiParent);
        bool positionOk = false;
        Map.Voronoi voronoi = null;
        while (!positionOk)
        {
            voronoi = Map.instance.quartiers[Random.Range(0, Map.instance.quartiers.Count - 1)];
            if (voronoi.voisins.Count == 0 || (notTooMuchClose && Vector3.SqrMagnitude(new Vector3(voronoi.position.x,0, voronoi.position.y) - essaimMaitre.transform.position) < raidusMinimumaroundEssaim * raidusMinimumaroundEssaim))
            {
                positionOk = false;
            }
            else
            {
                positionOk = true;
                voiture.transform.position = new Vector3(voronoi.position.x, 0, voronoi.position.y);
               
            }
        }
        Vehicule vehicule = voiture.GetComponent<Vehicule>();
        vehicule.currentPoint = voronoi;
        vehicule.targetPoint = voronoi.voisins[Random.Range(0, voronoi.voisins.Count-1)];
        Debug.LogWarning((vehicule.currentPoint == null) + "  " + (vehicule.targetPoint == null));
        listEnnemis.Add(vehicule);
        nbVoiture++;
    }

    public void PopBus(bool notTooMuchClose)
    {
        GameObject bus = GameObject.Instantiate(prefabBus, ennemiParent);
        bool positionOk = false;
        Map.Voronoi voronoi = null;
        while (!positionOk)
        {
            voronoi = Map.instance.quartiers[Random.Range(0, Map.instance.quartiers.Count - 1)];
            if (voronoi.voisins.Count == 0 || (notTooMuchClose && Vector3.SqrMagnitude(new Vector3(voronoi.position.x, 0, voronoi.position.y) - essaimMaitre.transform.position) < raidusMinimumaroundEssaim * raidusMinimumaroundEssaim))
            {
                positionOk = false;
            }
            else
            {
                positionOk = true;
                bus.transform.position = new Vector3(voronoi.position.x, 0, voronoi.position.y);

            }
        }
        Vehicule vehicule = bus.GetComponent<Vehicule>();
        vehicule.currentPoint = voronoi;
        vehicule.targetPoint = voronoi.voisins[Random.Range(0, voronoi.voisins.Count - 1)];
        Debug.LogWarning((vehicule.currentPoint == null) + "  " + (vehicule.targetPoint == null));
        listEnnemis.Add(vehicule);
        nbBus++;
    }

    public void PopTank(bool notTooMuchClose)
    {
        GameObject tank = GameObject.Instantiate(prefabTank, ennemiParent);
        bool positionOk = false;
        Map.Voronoi voronoi = null;
        while (!positionOk)
        {
            voronoi = Map.instance.quartiers[Random.Range(0, Map.instance.quartiers.Count - 1)];
            if (voronoi.voisins.Count == 0 || (notTooMuchClose && Vector3.SqrMagnitude(new Vector3(voronoi.position.x, 0, voronoi.position.y) - essaimMaitre.transform.position) < raidusMinimumaroundEssaim * raidusMinimumaroundEssaim))
            {
                positionOk = false;
            }
            else
            {
                positionOk = true;
                tank.transform.position = new Vector3(voronoi.position.x, 0, voronoi.position.y);

            }
        }
        Vehicule vehicule = tank.GetComponent<Vehicule>();
        vehicule.currentPoint = voronoi;
        vehicule.targetPoint = voronoi.voisins[Random.Range(0, voronoi.voisins.Count - 1)];
        Debug.LogWarning((vehicule.currentPoint == null) + "  " + (vehicule.targetPoint == null));
        listEnnemis.Add(vehicule);
        nbTank++;
    }

    public EssaimAI GetTarget(Ennemi ennemi)
    {
        EssaimAI target = null;
        float shortestDistance = ennemi.range* ennemi.range;

        foreach(EssaimAI essaim in listEssaim)
        {
            float distance = Vector3.SqrMagnitude(essaim.transform.position - ennemi.transform.position);
            if(distance< shortestDistance)
            {
                target = essaim;
                shortestDistance = distance;

            }
        }
        return target;
    }

    public void Kill(EssaimAI essaim)
    {
        try
        {
            if (essaim == essaimMaitre)
            {
                listEssaim[0].Die();
                essaimMaitre.isMaster = false;
                Transform cameraParent = listEssaim[0].transform.GetChild(1);
                Vector3 savePosition1 = cameraParent.localPosition;
                Quaternion saveRotation1 = cameraParent.localRotation;
                Vector3 savePosition2 = cameraParent.GetChild(0).localPosition;
                Quaternion saveRotation2 = cameraParent.GetChild(0).localRotation;
                Transform camera = cameraParent.GetChild(0).GetChild(0);

                Vector3 savePositionCamera = camera.position;
                Quaternion saveRotationCamera = camera.rotation;

                camera.SetParent(null);
                cameraParent.SetParent(null);
                if (listEssaim.Count > 1)
                {
                    essaimMaitre = listEssaim[1];
                    essaimMaitre.isMaster = true;
                    cameraParent.SetParent(essaimMaitre.transform);
                    cameraParent.localPosition = savePosition1;
                    cameraParent.localRotation = saveRotation1;
                    cameraParent.GetChild(0).localPosition = savePosition2;
                    cameraParent.GetChild(0).localRotation = saveRotation2;
                    camera.SetParent(cameraParent.GetChild(0));
                    camera.position = savePositionCamera;
                    camera.rotation = saveRotationCamera;
                }
                else
                {
                    camera.SetParent(null);
                }
                listEssaim.Remove(essaim);
            }
            else
            {
                essaim.Die();
                listEssaim.Remove(essaim);
            }
        }
        catch (System.Exception e )
        { 
        }

        
        
    }
}
