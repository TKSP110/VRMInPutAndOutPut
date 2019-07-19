using UnityEngine;
#if VRMIO
using VRMIO;
#endif
public class VRMCollider : MonoBehaviour
{
    public Transform ColliderObj2D;
    public Transform ColliderObj3D;
#if VRMIO
    public enum ColliderType : int
    {
        TYPE2D, TYPE3D
    }

    public void SetupVRMCollider(VroidData data, ColliderType type, Transform parent)
    {
        Transform col;
        //dataのサイズを元にコライダをparentオブジェクトにセットアップする
        if (type == ColliderType.TYPE2D)
        {
            col = Instantiate(ColliderObj2D, parent.position + new Vector3(0, data.modelheight, 0), parent.rotation, parent);
        }
        else
            col = Instantiate(ColliderObj3D, parent.position + new Vector3(0, data.modelheight, 0), parent.rotation, parent);

        col.localScale = new Vector3(data.width, data.height, data.width);

    }
#endif
}

