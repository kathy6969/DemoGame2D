using UnityEngine;
using System.Collections;

public class CotManager : MonoBehaviour
{
    [Header("Objects Setup")]
    [SerializeField] private GameObject[] cotObjects;  // Object 1 và 3 (có tag "cot" và va chạm ban đầu)
    [SerializeField] private GameObject[] hiddenObjects; // Object 2 và 4 (ẩn ban đầu)

    [Header("Settings")]
    [SerializeField] private float delayBeforeShow = 1f; // Thời gian đợi trước khi hiện 2,4

    private bool isBossDefeated = false;

    void Start()
    {
        // Đảm bảo object 2 và 4 ẩn lúc đầu
        foreach (GameObject obj in hiddenObjects)
        {
            obj.SetActive(false);
        }
    }

    // Gọi khi player chạm vào object có tag "cot"
    public void OnCotTouched()
    {
        if (isBossDefeated) return; // Nếu boss đã chết, không làm gì

        // Ẩn object 1 và 3
        foreach (GameObject obj in cotObjects)
        {
            obj.SetActive(false);
        }

        // Sau 'delayBeforeShow' giây, hiện object 2 và 4
        StartCoroutine(ShowHiddenObjectsAfterDelay());
    }

    private IEnumerator ShowHiddenObjectsAfterDelay()
    {
        yield return new WaitForSeconds(delayBeforeShow);

        foreach (GameObject obj in hiddenObjects)
        {
            obj.SetActive(true);
            // Kích hoạt va chạm nếu chưa có
            if (obj.GetComponent<Collider2D>() != null)
            {
                obj.GetComponent<Collider2D>().enabled = true;
            }
        }
    }

    // Gọi khi boss bị tiêu diệt
    public void OnBossDefeated()
    {
        isBossDefeated = true;

        // Ẩn object 2 và 4
        foreach (GameObject obj in hiddenObjects)
        {
            obj.SetActive(false);
        }

        // Hiện lại object 1 và 3 nếu cần
        // foreach (GameObject obj in cotObjects)
        // {
        //     obj.SetActive(true);
        // }
    }
}