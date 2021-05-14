using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vehicule : Ennemi
{
    public float speedDeath;


    public Map.Voronoi currentPoint;
    public Map.Voronoi targetPoint;

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
            
            if(Vector3.SqrMagnitude(this.transform.position- new Vector3(targetPoint.position.x,0, targetPoint.position.y)) <1f)
            {
                currentPoint = targetPoint;
                this.transform.position = new Vector3(currentPoint.position.x, 0, currentPoint.position.y);
                targetPoint = currentPoint.voisins[Random.Range(0, currentPoint.voisins.Count - 1)];
                this.transform.LookAt(new Vector3(targetPoint.position.x, 0, targetPoint.position.y));
                this.transform.Rotate(90 * Vector3.up);
            }
            this.transform.position += speed * Time.deltaTime * (new Vector3(targetPoint.position.x, 0, targetPoint.position.y) - this.transform.position).normalized;
            
            UpdateLoop();
        }
        
    }


}
