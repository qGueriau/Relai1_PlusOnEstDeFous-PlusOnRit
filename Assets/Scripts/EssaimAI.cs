using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EssaimAI : MonoBehaviour
{
    public float speed = 5f;
    public int HP = 5;


    public bool isMaster = false;
    public EssaimAI master = null;

    bool dead = false;
    public float speedDeath = 2f;
    public bool dyingByTransparency = true;


   



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(dead)
        {
            this.transform.position = Vector3.Lerp(this.transform.position, new Vector3(this.transform.position.x, -2.5f, this.transform.position.z), speedDeath * Time.deltaTime );
            if (this.transform.position.y < 2f) Destroy(this.gameObject);
            /*if (dyingByTransparency)
            {
                Renderer rend = this.GetComponent<Renderer>();
                rend.material.color = new Color(rend.material.color.r, rend.material.color.g, rend.material.color.b, Mathf.Clamp((2 + this.transform.position.y) * 0.5f, 0, 1f));

            }*/
        }
        else
        {
            if (isMaster)
            {
                this.transform.position += Time.deltaTime * speed * new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            }
            else
            {
                Vector3 deplacement = EssaimManager.instance.GetDeplacement(this);

                float deplacementSqrMagnitude = deplacement.sqrMagnitude;
                if (deplacementSqrMagnitude > speed* speed)
                {
                    deplacement *= speed * speed / deplacementSqrMagnitude;
                }
                this.transform.position += deplacement*Time.deltaTime;
            }
        }
        
    }

    public void Die()
    {
        dead = true;
    }
}
