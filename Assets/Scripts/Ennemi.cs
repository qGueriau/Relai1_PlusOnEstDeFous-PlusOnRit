using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ennemi : MonoBehaviour
{
    public int scoreIfConverted;
    public float speed;
    public int nbToConvert;
    public int nbAfterConvertion;
    public float convertionTaux = 0;
    public float speedConversion = 0.5f;
    public float speedConversionGoDown = 0.25f;
    public float radius;

    protected bool dead = false;

    public List<Renderer> renderers;

    public Color normalColour;
    // Start is called before the first frame update


    public bool canShoot = false;

    public float range = 15f;
    public int damage = 1;
    public float timeBetweenShoot = 1;
    public EssaimAI currentTarget = null;
    float countDownShoot = 10f;

    public GameObject weaponItem;

    public enum Type
    {
        Civil, Voiture, Bus, Soldat, Tank
    }
    public Type type;
    void Awake()
    {
        normalColour = renderers[0].material.color;
    }

    // Update is called once per frame
    protected void UpdateLoop()
    {

        if (canShoot)
        {
            countDownShoot += Time.deltaTime;
            if (countDownShoot > timeBetweenShoot)
            {
                if (currentTarget == null || Vector3.SqrMagnitude(this.transform.position- currentTarget.transform.position)> range* range)
                {
                    currentTarget = EssaimManager.instance.GetTarget(this);
                }

                if (currentTarget != null)
                {
                    weaponItem.transform.LookAt(new Vector3(currentTarget.transform.position.x, weaponItem.transform.position.y, currentTarget.transform.position.z));


                    currentTarget.HP -= damage;
                    if (currentTarget.HP <= 0) EssaimManager.instance.Kill(currentTarget);
                    countDownShoot = 0;
                }
            }
        }



        bool isGettingConvert = EssaimManager.instance.isGettingConversion(this);
        if (isGettingConvert)
        {
            convertionTaux += speedConversion * Time.deltaTime;
        }
        else
        {
            convertionTaux -= speedConversionGoDown * Time.deltaTime;
        }
        if (convertionTaux < 0) convertionTaux = 0;

        foreach(Renderer rend in renderers)
        {
            rend.material.color = Color.Lerp(normalColour, Color.red, convertionTaux);
        }


        if (convertionTaux >= 1f)
        {
            Die();

            EssaimManager.instance.score += scoreIfConverted;

            if (type == Type.Civil) EssaimManager.nbCivil--;
            if (type == Type.Voiture) EssaimManager.nbVoiture--;
            if (type == Type.Bus) EssaimManager.nbBus--;
            if (type == Type.Soldat) EssaimManager.nbSoldat--;
            if (type == Type.Tank) EssaimManager.nbTank--;
        }
    }

    void Die()
    {
        dead = true;
        EssaimManager.instance.doConvert(this);
    }
}
