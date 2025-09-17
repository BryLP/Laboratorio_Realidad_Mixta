using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSizeWithHands : MonoBehaviour
{
    
    public OVRHand rightHand;
    public OVRHand leftHand;

    GameObject ResizeObject;
    //Para prueba
    public GameObject objectPrueba;
    public Transform posicion;


    public TMPro.TextMeshPro pinchAmbasManosText;
    float distanceOriginal;
    float distanceUpdate;
    float escala;


    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {   
        pinchAmbasManosText.text = "No hay pinch en ambas manos";
        
        if(rightHand.GetFingerIsPinching(OVRHand.HandFinger.Index) == true && leftHand.GetFingerIsPinching(OVRHand.HandFinger.Index) == true){

            if(ResizeObject == null){
                ResizeObject = Instantiate(objectPrueba, posicion.position, objectPrueba.transform.rotation);
                distanceOriginal = Vector3.Distance(rightHand.transform.position, leftHand.transform.position);
            }else{
                //Calculando distancia entre manos
                distanceUpdate = Vector3.Distance(rightHand.transform.position, leftHand.transform.position);

                //Obteniendo el cambio de la distancia entre las manos
                float cambio = distanceOriginal/ 
                
                //
                ResizeObject.transform.localScale = new Vector3(escala, escala, escala);
                pinchAmbasManosText.text = "Pinch en ambas manos activado c: "+distance;
            }
            
        } 
    }
    
}
