using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pieton : Ennemi
{
    public float speedDeath;

    public bool isInit = false;
    public Vector3 targetPosition;

   

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(dead)
        {
            this.transform.position = Vector3.Lerp(this.transform.position, new Vector3(this.transform.position.x, -2.5f, this.transform.position.z), speedDeath * Time.deltaTime);
            if (this.transform.position.y < 2f) Destroy(this.gameObject);
        }
        else
        {
           


            if(!isInit || Vector3.SqrMagnitude(this.transform.position- targetPosition)<1f)
            {
                targetPosition = this.transform.position + 10 * new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));
                isInit = true;
            }
            this.transform.position += speed * Time.deltaTime * (targetPosition - this.transform.position).normalized;

            UpdateLoop();
        }
        
    }


}
