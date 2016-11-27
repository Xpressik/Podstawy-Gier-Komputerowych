using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets
{
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
    public class FigureModel : MonoBehaviour
    {
        public Figure Parent;
        public Vector3[] Vertices;
        public Vector3[] Normals;
        public Vector2[] UV;
        public int[] Triangles;

        void Start()
        {
            Parent = transform.parent.GetComponent<Figure>();
            Vertices = new Vector3[24];
            UV = new Vector2[24];

            var mesh = new Mesh { name = "Hex Mesh" };
            GetComponent<MeshFilter>().mesh = mesh;
            mesh.vertices = Vertices;
            mesh.uv = UV;
            mesh.triangles = Triangles;
            mesh.normals = Normals;
        }

        void Update()
        {

        }


    }
}
