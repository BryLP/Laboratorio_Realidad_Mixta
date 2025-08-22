using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Meta.XR.MRUtilityKit;
using JetBrains.Annotations;

public class DisplayLabel : MonoBehaviour
{
    public Transform rayStartPoint;
    public float rayLength = 5;
    public MRUKAnchor.SceneLabels labelFilter;
    public GameObject Objeto_Spawn;

    //Para escribir en el nombre del objeto que se esta apuntando con el raycast, es una etiqueta normal de unity
    public TMPro.TextMeshPro debugText;

    //Guarda el ultimo objeto que se instancia para que no se deba crear uno nuevo cada frame que pasa
    private GameObject spawnedObject;

    //Para obtener la poscion del visor
    public Transform centerEyeVisor;

    //Declaro la mano con la que colocare la mesa
    public OVRHand rightHand;
    public TMPro.TextMeshPro pinchText;
    private GameObject RealObject;

    //Variables de uso global para la instanciacion de la mesa de trabajo
    Vector3 hitPoint;
    Vector3 hitNormal;
    Vector3 direccionVectorNormalizado;
    string label;

    //Variable para activar o desactivar mecanica de colocar mesa
    bool colocarMesa;
    bool tableIsSet = false;


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
         
        
        if(colocarMesa == true){

            //Detecto si con la mano se esta haciendo el pinch o no, para saber si colocar la mesa o no
            if (rightHand.GetFingerIsPinching(OVRHand.HandFinger.Index) == false && tableIsSet == false){
                pinchText.text = "Pinch False :c";

                Ray ray = new Ray(rayStartPoint.position, rayStartPoint.forward);
                LabelFilter filter = new LabelFilter { SceneLabels = labelFilter };

                MRUKRoom room = MRUK.Instance.GetCurrentRoom();
                bool hasHit = room.Raycast(ray, rayLength, filter, out RaycastHit hit, out MRUKAnchor anchor);



                if (hasHit)
                {
                    hitPoint = hit.point;
                    hitNormal = hit.normal;

          

                    //Para obtener la etiqueta y saber que objeto , pared o techo es lo que esta apuntando el raycast
                    label = anchor.Label.ToString();

                    //Colocamos al texto o etquita de unity la poicion del ray  cast y su rotacion, ademas asignamos el valor de la etiquta obtenida antes 
                    //para saber que objeto es el que estamos apuntando
                    //debugText.transform.position = hitPoint;
                    //debugText.transform.rotation = Quaternion.LookRotation(-hitNormal);
                    debugText.text = label;



                    //Si el objeto aun no se ha creado, se crea con instantiate y con la posision obtenida del los datos del raycast
                    if(spawnedObject == null)
                    {
                        spawnedObject = Instantiate(Objeto_Spawn, hitPoint, Quaternion.LookRotation(-hitNormal));

                        //Desactivo todos los box Colliders de la mesa para que esta no tenga colisiones con objetos en el entorno y sea solo como un objeto fantasma
                        BoxCollider[] spawnedObjectColliders = spawnedObject.GetComponentsInChildren<BoxCollider>();
                        foreach (BoxCollider col in spawnedObjectColliders)
                        {
                            col.enabled = false;
                        }

                        //Obtener todos los mesh renderer de los hijos de spawnedObject, es decir de sus componentes del modelo, cube, cube001, etc y de ellos obtener su
                        //mesh renderer para modificarlos todos

                        MeshRenderer[] renderers_hijos = spawnedObject.GetComponentsInChildren<MeshRenderer>();

                        foreach (MeshRenderer renderer in renderers_hijos)
                        {
                            //Esto es lo que obtuve para cada componenete del modelo
                            //comp1_material = comp1.GetComponent<MeshRenderer>().material;

                            //Se obtiene el material del modelo, se obtiene su color, se mofiica en este el canal alfa y despues se vuelve a asignar al modelo
                            Material material = renderer.material;
                            Color colortransparente = material.color;
                            colortransparente.a = 0.5f;
                            material.color = colortransparente;
                    
                            //Esta podria ser otra forma donde creo un color desde 0 en blanco y le modifico
                            //El canal alfa para despues asignar ese color al modelo que quiero hacer transparente
                            //color_transparente = Color.white;
                            //color_transparente.a = 0.1f;
                            //material.color = color_transparente;
                        }


                        //Creo un vector que apunte siempre en direccion al visor para despues usarlo y que la mesa caundo este en el suelo o en el techo
                        //antes de colocarla siempre apunte al visor, normalizamos este vector para que sea de longitud 1 y no cause problemas
                        Vector3 direccionVector = centerEyeVisor.position - spawnedObject.transform.position;
                        direccionVector.y = 0;
                        direccionVectorNormalizado = direccionVector.normalized;



                    } //Si ya se creo y esta guardado en spawnedObject ya solo lo reajustamos con los dartos actuales del raycast
                    else
                    {
                        spawnedObject.transform.position = hitPoint;

                        //Actualizamos la posicion del vector
                        Vector3 direccionVector = centerEyeVisor.position - spawnedObject.transform.position;
                        direccionVector.y = 0;
                        direccionVectorNormalizado = direccionVector.normalized;

                        if (label == "FLOOR")
                        {
                            spawnedObject.transform.rotation = Quaternion.LookRotation(-direccionVectorNormalizado);
                        }
                        else if (label == "CEILING")
                        {
                            spawnedObject.transform.rotation = Quaternion.LookRotation(-direccionVectorNormalizado);
                        }
                        else
                        {
                            spawnedObject.transform.rotation = Quaternion.LookRotation(-hitNormal);
                        }
                
                    }

                }

            }else{
                pinchText.text = "Pinch True c:";

                if (RealObject == null){

                    if (label == "FLOOR")
                    {
                        RealObject = Instantiate(Objeto_Spawn, hitPoint, Quaternion.LookRotation(-direccionVectorNormalizado));
                        tableIsSet = true;
                        Destroy(spawnedObject);
                    }
                    else if (label == "CEILING")
                    {
                        RealObject = Instantiate(Objeto_Spawn, hitPoint, Quaternion.LookRotation(-direccionVectorNormalizado));
                        tableIsSet = true;
                        Destroy(spawnedObject);
                    }
                    else
                    {
                        RealObject = Instantiate(Objeto_Spawn, hitPoint, Quaternion.LookRotation(-hitNormal));
                        tableIsSet = true;
                        Destroy(spawnedObject);
                    }
                

                    Rigidbody rb = RealObject.AddComponent<Rigidbody>();
                    rb.useGravity = true;
                }
            }
        }
    
    


    }

    public void ActivarColocarMesa(){
        colocarMesa = true;
    }

    public void DesactivarColocarMesa(){
        colocarMesa = false;
        Destroy(spawnedObject);
        spawnedObject = null;
        Destroy(RealObject);
        RealObject = null;
        tableIsSet = false;

    }
}
