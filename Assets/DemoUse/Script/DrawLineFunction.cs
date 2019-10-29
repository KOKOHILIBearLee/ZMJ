using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class CommonZheng
{
    public static Vector3 Round(Vector3 target, Vector3 direction, float radianper)
    {
        Vector3 t = target;
        Vector3.OrthoNormalize(ref t, ref direction);
        return Vector3.SlerpUnclamped(target, direction * target.magnitude, radianper * 4);
    }

    public static void GetGameSizePosition(List<GameObject> game, out Vector3 position, out Vector3 size)
    {
        List<BoxPlane> boxplanes = new List<BoxPlane>();
        for (int i = 0; i < game.Count; i++)
        {
            Vector3 tposition = GetGameMeshPosition(game[i]);
            Vector3 tsize = GetGameMeshSize(game[i]);
            if (tsize != Vector3.zero)
                boxplanes.Add(GetBoxPlane(GetGameMeshPosition(game[i]), GetGameMeshSize(game[i])));
        }
        Vector3 lposition = Vector3.zero;
        Vector3 lextent = Vector3.zero;
        GetBox(boxplanes, out lposition, out lextent);
        position = lposition;
        size = lextent;
    }

    public struct BoxPlane
    {
        public Vector3 forwardPlane;
        public Vector3 backPlane;
        public Vector3 rightPlane;
        public Vector3 leftPlane;
        public Vector3 upPlane;
        public Vector3 downPlane;
    }

    static BoxPlane GetBoxPlane(Vector3 position, Vector3 size)
    {
        BoxPlane boxplane = new BoxPlane();
        size = new Vector3(Mathf.Abs(size.x), Mathf.Abs(size.y), Mathf.Abs(size.z));
        boxplane.forwardPlane = new Vector3(position.x, position.y, position.z + size.z / 2);
        boxplane.backPlane = new Vector3(position.x, position.y, position.z - size.z / 2);
        boxplane.rightPlane = new Vector3(position.x + size.x / 2, position.y, position.z);
        boxplane.leftPlane = new Vector3(position.x - size.x / 2, position.y, position.z);
        boxplane.upPlane = new Vector3(position.x, position.y + size.y / 2, position.z);
        boxplane.downPlane = new Vector3(position.x, position.y - size.y / 2, position.z);
        return boxplane;
    }

    public static Vector3 GetGameMeshSize(GameObject game)
    {
        try
        {
            Renderer render = game.GetComponent<Renderer>();
            return render.bounds.size;
            //Mesh mesh = game.GetComponent<MeshFilter>().mesh;
            //return game.transform.TransformVector(2 * mesh.bounds.extents);
        }
        catch
        {
            return Vector3.zero;
        }
    }

    public static Vector3 GetGameMeshPosition(GameObject game)
    {
        try
        {
            Mesh mesh = game.GetComponent<MeshFilter>().mesh;
            return game.transform.TransformPoint(mesh.bounds.center);
        }
        catch
        {
            return game.transform.position;
        }
    }

    static void GetBox(List<BoxPlane> planes, out Vector3 position, out Vector3 size)
    {
        if (planes == null || planes.Count == 0)
        {
            position = Vector3.zero;
            size = Vector3.zero;
            return;
        }

        Vector3 forward = Vector3.forward * float.MinValue;
        Vector3 back = Vector3.forward * float.MaxValue;
        Vector3 right = Vector3.right * float.MinValue;
        Vector3 left = Vector3.right * float.MaxValue;
        Vector3 up = Vector3.up * float.MinValue;
        Vector3 down = Vector3.up * float.MaxValue;

        for (int i = 0; i < planes.Count; i++)
        {
            if (planes[i].forwardPlane.z >= forward.z)
                forward = planes[i].forwardPlane;
            if (planes[i].backPlane.z <= back.z)
                back = planes[i].backPlane;
            if (planes[i].rightPlane.x >= right.x)
                right = planes[i].rightPlane;
            if (planes[i].leftPlane.x <= left.x)
                left = planes[i].leftPlane;
            if (planes[i].upPlane.y >= up.y)
                up = planes[i].upPlane;
            if (planes[i].downPlane.y <= down.y)
                down = planes[i].downPlane;
        }


        position = new Vector3((right.x + left.x) / 2, (up.y + down.y) / 2, (forward.z + back.z) / 2);
        size = new Vector3(Mathf.Abs(right.x - left.x), Mathf.Abs(up.y - down.y), Mathf.Abs(forward.z - back.z));
    }

    static List<Vector4> regions = new List<Vector4>();

    public static void ExchangeMaterial(this Material self, int count)
    {
        self.shader = Shader.Find("Unlit/CullHole");

        self.SetInt("_Points_Num", count);

        List<Vector4> res = new List<Vector4>();
        for (int i = 0; i < count; i++)
            res.Add(new Vector4(0, 0, -10, 0));
        self.SetVectorArray("_Points", res.ToArray());
    }

    public static void SetRenderCullHoleRegion(this Material self, List<Vector4> regions, bool add = true)
    {
        if (self.shader.name == "Unlit/CullHole")
        {
            int num = self.GetInt("_Points_Num");
            if (num < regions.Count + 1)
                ExchangeMaterial(self, (regions.Count + 1) * 2);
            regions = new List<Vector4>();
            List<Vector4> result = new List<Vector4>();
            regions.ForEach(rf =>
            {
                result.Add(rf);
                regions.Add(rf);
            });
            self.SetInt("_Points_Num", result.Count);
            self.SetVectorArray("_Points", result.ToArray());
        }
        else if (add)
        {
            self = new Material(Shader.Find("Unlit/CullHole"));
            self.SetRenderCullHoleRegion(regions, false);
        }
    }

    public static void RefreshCull(this Material self)
    {
        if (self.shader.name == "Unlit/CullHole" && regions.Count != 0)
        {
            int num = self.GetInt("_Points_Num");
            if (num < regions.Count + 1)
                ExchangeMaterial(self, (regions.Count + 1) * 2);

            self.SetInt("_Points_Num", regions.Count);
            self.SetVectorArray("_Points", regions.ToArray());
        }
        else
        {
            Debug.LogError("无法设置CullHole材质");
        }
    }

    public static void AddRenderCullHoleRegion(this Material self, List<Vector2> regionss, RegionType type, float radius = 0)
    {
        if (regionss == null)
            return;
        if (type == RegionType.Region && regionss.Count < 3)
            return;
        if (type == RegionType.Circle && regionss.Count < 1 && radius <= 0)
            return;

        if (self.shader.name == "Unlit/CullHole")
        {
            regionss.ForEach(rf =>
            {
                if (type == RegionType.Region)
                    regions.Add(new Vector4(rf.x, rf.y, 0, 1));
                if (type == RegionType.Circle)
                    regions.Add(new Vector4(rf.x, rf.y, radius, 2));
            });
            if (type == RegionType.Region)
                regions.Add(new Vector4(0, 0, 0, 0));
            self.RefreshCull();
        }
    }

    public static void ClearRegion(this Material self, bool refresh = true)
    {
        if (self.shader.name == "Unlit/CullHole")
        {
            regions = new List<Vector4>();//(new Vector4[]{new Vector4(0,0,0,0)});
            if (refresh)
                self.RefreshCull();
        }
    }

    public enum RegionType
    {
        Region,
        Circle,
    }
}
