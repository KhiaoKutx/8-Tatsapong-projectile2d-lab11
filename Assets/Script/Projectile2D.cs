using UnityEngine;

public class Projectile2D : MonoBehaviour
{
    [SerializeField] Transform shootPoint;        // จุดที่ยิงกระสุนออกมา
    [SerializeField] GameObject target;           // วัตถุเป้า (ใช้แสดงตำแหน่งที่คลิก)
    [SerializeField] Rigidbody2D bulletPrefab;    // พรีแฟบกระสุน (ต้องมี Rigidbody2D ติดอยู่)

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // สร้าง Ray จากหน้าจอไปในโลก
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Debug.DrawRay(ray.origin, ray.direction * 5f, Color.red, 5f);

            // ตรวจจับการชนจาก Ray
            RaycastHit2D hit = Physics2D.GetRayIntersection(ray, Mathf.Infinity);

            if (hit.collider != null)
            {
                // ย้าย target ไปตำแหน่งที่คลิก
                target.transform.position = new Vector2(hit.point.x, hit.point.y);
                Debug.Log("hit " + hit.collider.name);

                // คำนวณความเร็วกระสุน
                Vector2 projectileVelocity = CalculateProjectileVelocity(shootPoint.position, hit.point, 1f);

                // ยิงกระสุน
                Rigidbody2D shootBullet = Instantiate(bulletPrefab, shootPoint.position, Quaternion.identity);
                shootBullet.linearVelocity = projectileVelocity; // หรือใช้ linearVelocity ก็ได้ถ้าเป็น Physics2D

                // หมายเหตุ: ต้องแน่ใจว่าพรีแฟบ bulletPrefab มี Rigidbody2D และไม่มีการคอลไลเดอร์ block เส้นทาง
            }
        }
    }

    // ฟังก์ชันคำนวณความเร็วเริ่มต้นของกระสุน
    Vector2 CalculateProjectileVelocity(Vector2 origin, Vector2 target, float time)
    {
        Vector2 distance = target - origin;

        float velocityX = distance.x / time;
        float velocityY = distance.y / time + 0.5f * Mathf.Abs(Physics2D.gravity.y) * time;

        return new Vector2(velocityX, velocityY);
    }
}
