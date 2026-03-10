using UnityEngine;


[ExecuteAlways]
public class SplineTool : MonoBehaviour
{
    [Header("Points")]
    public Transform StartPoint;
    public Transform EndPoint;

    [Header("Prefab")]
    public GameObject Prefab;

    [Header("Settings")]
    [Range(1, 100)]
    public int Count = 10;
    [Range(0f, 2f)]
    public float Sag = 0.3f;
    public Vector3 RotationOffset;
    public Vector3 ScaleOverride = Vector3.one;

    [Header("Options")]
    public bool AutoUpdate = true;
    public bool AlignToPath = true;

    void Update()
    {
        if (AutoUpdate)
            Rebuild();
    }

    [ContextMenu("Rebuild")]
    public void Rebuild()
    {
        if (StartPoint == null || EndPoint == null || Prefab == null)
            return;

        // clear old children
        while (transform.childCount > 0)
        {
            var child = transform.GetChild(0);
            if (Application.isPlaying)
                Destroy(child.gameObject);
            else
                DestroyImmediate(child.gameObject);
        }

        // spawn prefabs along the line
        for (int i = 0; i < Count; i++)
        {
            float t = (Count > 1) ? i / (float)(Count - 1) : 0.5f;

            Vector3 pos = Vector3.Lerp(StartPoint.position, EndPoint.position, t);

            // parabolic sag
            pos.y -= Sag * (4f * t * (1f - t));

            var obj = Instantiate(Prefab, pos, Quaternion.identity, transform);
            obj.name = Prefab.name + "_" + i;

            // align rotation to face next point
            if (AlignToPath && Count > 1)
            {
                Vector3 dir;
                bool isLast = (i == Count - 1);

                if (!isLast)
                {
                    // look forward to next position
                    float tNext = (i + 1) / (float)(Count - 1);
                    Vector3 nextPos = Vector3.Lerp(StartPoint.position, EndPoint.position, tNext);
                    nextPos.y -= Sag * (4f * tNext * (1f - tNext));
                    dir = (nextPos - pos).normalized;
                }
                else
                {
                    // last element: reuse direction from previous to current
                    float tPrev = (i - 1) / (float)(Count - 1);
                    Vector3 prevPos = Vector3.Lerp(StartPoint.position, EndPoint.position, tPrev);
                    prevPos.y -= Sag * (4f * tPrev * (1f - tPrev));
                    dir = (pos - prevPos).normalized;
                }

                if (dir != Vector3.zero)
                    obj.transform.rotation = Quaternion.LookRotation(dir) * Quaternion.Euler(RotationOffset);
            }
            else
            {
                obj.transform.rotation = Quaternion.Euler(RotationOffset);
            }

            obj.transform.localScale = ScaleOverride;
        }
    }

    [ContextMenu("Clear")]
    public void Clear()
    {
        while (transform.childCount > 0)
        {
            if (Application.isPlaying)
                Destroy(transform.GetChild(0).gameObject);
            else
                DestroyImmediate(transform.GetChild(0).gameObject);
        }
    }

    void OnDrawGizmos()
    {
        if (StartPoint == null || EndPoint == null) return;

        Gizmos.color = Color.cyan;
        Gizmos.DrawSphere(StartPoint.position, 0.1f);
        Gizmos.DrawSphere(EndPoint.position, 0.1f);

        // draw the sag curve
        Vector3 prev = StartPoint.position;
        for (int i = 1; i <= 20; i++)
        {
            float t = i / 20f;
            Vector3 p = Vector3.Lerp(StartPoint.position, EndPoint.position, t);
            p.y -= Sag * (4f * t * (1f - t));
            Gizmos.DrawLine(prev, p);
            prev = p;
        }
    }
}
