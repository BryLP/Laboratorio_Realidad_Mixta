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
    Vector3 escalaActual;


    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {   
        pinchAmbasManosText.text = "No hay pinch en ambas manos";
        
        //Se detecta si se esta realizando el pinch con ambas manos
        if(rightHand.GetFingerIsPinching(OVRHand.HandFinger.Index) == true && leftHand.GetFingerIsPinching(OVRHand.HandFinger.Index) == true){

            //La primera vez que se hace el pinch y que no se ha generado el objeto 
            if(ResizeObject == null){
                //Se instancia el objeto 
                ResizeObject = Instantiate(objectPrueba, posicion.position, objectPrueba.transform.rotation);
            }else if(ResizeObject != null && distanceOriginal == 0){
                //Segunda vez en adelante que se hace el pinch con ambas manos, el objeto ya existe pero se quiere volver a modificar su tamaño
                //Reincio el calculo de la distancia entre las manos
                //Se actualiza la distancia Original, es decir se guarda la primera distancia entre las manos desde el momento justo donde se pinch doble, la distancia
                //Original entre las manos desde la primera vez que se hace el pinch, este sera el eje de referencia
                distanceOriginal = Vector3.Distance(rightHand.transform.position, leftHand.transform.position);

                //Obtenemos la escalas actuales del objeto
                escalaActual = ResizeObject.transform.localScale;
            }else{
                //Se actualiza el tamaño del objeto a partir del cambio de distancia entre ambas manos durante el pinch doble 

                //Calculando distancia entre manos
                distanceUpdate = Vector3.Distance(rightHand.transform.position, leftHand.transform.position);
                    
                //Obteniendo el cambio de la distancia entre las manos a partir de la original
                float cambio = distanceUpdate-distanceOriginal;
                

                //Actualizamos el tamaño del objeto
                ResizeObject.transform.localScale = new Vector3(escalaActual.x + cambio, escalaActual.y + cambio, escalaActual.z + cambio);
                pinchAmbasManosText.text = "Pinch en ambas manos activado c: Update Distance= "+distanceUpdate+"Original Distance= "+distanceOriginal+" cambio = "+cambio;
            }
            
        }else{
            distanceOriginal = 0;
        }
    }
    
}
