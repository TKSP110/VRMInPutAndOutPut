using UnityEngine;
namespace VRMIO
{
    public class VRMColliderObj2DController : MonoBehaviour
    {
        //collider2D向き固定
        // Update is called once per frame
        void FixedUpdate()
        {
            transform.rotation = Quaternion.identity;
        }
    }
}