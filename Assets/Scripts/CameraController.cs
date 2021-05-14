using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController instance;
    public float zoomValue = 2f;
    public float zoomValueStart = 2f;
    public float zoomValueEveryNewMember = 0.025f;
    public float speedChangeMaster = 2f;



    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {

        zoomValue = zoomValueStart + EssaimManager.instance.listEssaim.Count* zoomValueEveryNewMember;

        this.transform.parent.parent.localScale = zoomValue * Vector3.one;
        this.transform.localPosition = Vector3.Lerp(this.transform.localPosition, Vector3.zero, speedChangeMaster*Time.deltaTime);
        this.transform.localRotation = Quaternion.Lerp(this.transform.localRotation, Quaternion.identity, speedChangeMaster*Time.deltaTime);





    }
}
