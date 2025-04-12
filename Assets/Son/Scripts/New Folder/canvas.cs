using UnityEngine;

public class KeepCanvasOrientation : MonoBehaviour
{
    private Vector3 initialGlobalScale;
    private Quaternion initialGlobalRotation;

    void Start()
    {
        // Lưu lại global scale ban đầu của Canvas
        initialGlobalScale = transform.lossyScale;
        // Lưu lại global rotation ban đầu của Canvas
        initialGlobalRotation = transform.rotation;
    }

    void LateUpdate()
    {
        if (transform.parent != null)
        {
            // Tính toán lại localScale sao cho global scale luôn giữ nguyên ban đầu
            Vector3 parentScale = transform.parent.lossyScale;
            float scaleX = initialGlobalScale.x / Mathf.Abs(parentScale.x);
            float scaleY = initialGlobalScale.y / Mathf.Abs(parentScale.y);
            float scaleZ = initialGlobalScale.z / Mathf.Abs(parentScale.z);
            transform.localScale = new Vector3(scaleX, scaleY, scaleZ);
        }

        // Đặt lại global rotation của Canvas về giá trị ban đầu để UI không quay theo player.
        transform.rotation = initialGlobalRotation;
    }
}
