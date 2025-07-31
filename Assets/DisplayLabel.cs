using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Meta.XR.MRUtilityKit;

public class DisplayLabel : MonoBehaviour
{
    public Transform rayStartPoint;
    public float rayLength = 5;
    public MRUKAnchor.SceneLabels labelFilter;
    public GameObject Objeto_Spawn;

    //Guarda el ultimo objeto que se instancia para que no se deba crear uno nuevo cada frame que pasa
    private GameObject spawnedObject;


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = new Ray(rayStartPoint.position, rayStartPoint.forward);
        LabelFilter filter = new LabelFilter { SceneLabels = labelFilter };

        MRUKRoom room = MRUK.Instance.GetCurrentRoom();
        bool hasHit = room.Raycast(ray, rayLength, filter, out RaycastHit hit, out MRUKAnchor anchor);

        if (hasHit)
        {
            Vector3 hitPoint = hit.point;
            Vector3 hitNormal = hit.normal;

            //Si el objeto aun no se ha creado, se crea con instantiate y con la posision obtenida del los datos del raycast
            if(spawnedObject == null)
            {
                spawnedObject = Instantiate(Objeto_Spawn, hitPoint, Quaternion.LookRotation(-hitNormal));


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
                


            } //Si ya se creo y esta guardado en spawnedObject ya solo lo reajustamos con los dartos actuales del raycast
            else
            {
                spawnedObject.transform.position = hitPoint;
                spawnedObject.transform.rotation = Quaternion.LookRotation(-hitNormal);
            }

        }


    }
}
