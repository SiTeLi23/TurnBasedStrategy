using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitRagdoll : MonoBehaviour
{
    [SerializeField] private Transform ragdollRootBone;



    public void Setup(Transform originalRootBone) 
    {
        MatchAllChildTransforms(originalRootBone, ragdollRootBone);

        ApplyExplosionToRagdoll(ragdollRootBone, 300f,transform.position, 10f);
        
    }


    //cycle all the childs
    private void MatchAllChildTransforms(Transform root,Transform clone) 
    {

        //checking throuhgh all the directly level's child first
       foreach(Transform child in root) 
        {

            Transform cloneChild = clone.Find(child.name);

            if (cloneChild != null) 
            {
                cloneChild.position = child.position;
                cloneChild.rotation = child.rotation;

                //checking throught all the next level's child if there's a next level
                MatchAllChildTransforms(child, cloneChild);


            }

        }
    
    }




    private void ApplyExplosionToRagdoll(Transform root, float explosionForce, Vector3 explosionPosition, float explosionRange)
    {
        foreach (Transform child in root)
        {
            if (child.TryGetComponent<Rigidbody>(out Rigidbody childrigidBody))
            {

                childrigidBody.AddExplosionForce(explosionForce, explosionPosition, explosionRange);
            }

            //make sure next and other levels of childs also get that explosion force
            ApplyExplosionToRagdoll(child, explosionForce, explosionPosition, explosionRange);

        }



    }
}
