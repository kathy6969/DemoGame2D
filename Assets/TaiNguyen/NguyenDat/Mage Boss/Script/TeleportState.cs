using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportState : State
{
    private BossStateMachine boss;
    private List<Transform> platforms = new List<Transform>(); // Dùng List thay vì mảng
    private float teleportCooldown = 3f; // Chờ 3 giây trước mỗi lần dịch chuyển
    private bool isTeleporting = false; // Ngăn spam dịch chuyển

    public TeleportState(BossStateMachine boss)
    {
        this.boss = boss;
    }

    public override void EnterState()
    {
        platforms.Clear(); // Xóa danh sách cũ để cập nhật lại platform
        GameObject[] platformObjects = GameObject.FindGameObjectsWithTag("Platform");
        foreach (GameObject obj in platformObjects)
        {
            platforms.Add(obj.transform); // Chuyển từ GameObject[] thành List<Transform>
        }

        if (platforms.Count > 0)
        {
            boss.StartCoroutine(TeleportRoutine());
        }
    }

    public override void ExitState()
    {
        boss.StopAllCoroutines();
        isTeleporting = false;
    }

    private IEnumerator TeleportRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(teleportCooldown); // Chờ 3 giây

            if (!isTeleporting)
            {
                isTeleporting = true;
                Teleport();
                isTeleporting = false;
            }
        }
    }

    public void Teleport()
    {
        Transform currentPlatform = FindClosestPlatform();
        if (currentPlatform == null) return;

        Vector3 newPosition = currentPlatform.position + new Vector3(0, 1f, 0); // Chuyển Vector2 thành Vector3
        boss.transform.position = newPosition;
    }
    private Transform FindClosestPlatform()
    {
        Transform bestPlatform = null;
        float minDistance = Mathf.Infinity;

        foreach (Transform platform in platforms)
        {
            float distance = Vector2.Distance(boss.transform.position, platform.position);
            if (distance > 1f && distance < minDistance) // Tránh chọn platform đang đứng
            {
                minDistance = distance;
                bestPlatform = platform;
            }
        }

        return bestPlatform;
    }
}
