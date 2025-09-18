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
            }else if(ResizeObject != null && distanceOriginal == 0){
                distanceOriginal = Vector3.Distance(rightHand.transform.position, leftHand.transform.position);
            }else{
                //Calculando distancia entre manos
                distanceUpdate = Vector3.Distance(rightHand.transform.position, leftHand.transform.position);
                    
                //Obteniendo el cambio de la distancia entre las manos
                float cambio = distanceUpdate-distanceOriginal;
                
                //
                ResizeObject.transform.localScale = new Vector3(0.02f + cambio, 0.02f+cambio, 0.02f+cambio);
                pinchAmbasManosText.text = "Pinch en ambas manos activado c: Update Distance= "+distanceUpdate+"Original Distance= "+distanceOriginal+" cambio = "+cambio;
            }
            
        }else{
            distanceOriginal = 0;
        }
    }
    
}
