using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Batiment : MonoBehaviour
{
    public float radius;
    public List<Renderer> renderers;
    public float opacityTarget = 1f;
    public float currentOpacity = 1f;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        currentOpacity = Mathf.Lerp(currentOpacity, opacityTarget, Time.deltaTime);
        Color normalColor = renderers[0].material.color;
        foreach(Renderer rend in renderers)
        {
            rend.material.color = new Color(normalColor.r, normalColor.g, normalColor.b, currentOpacity);
        }
    }
}
