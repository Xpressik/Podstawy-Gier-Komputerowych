using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets
{
    public class Figure : MonoBehaviour
    {
        //public Vector2 FigurePosition;
        public int CurrentX { set; get;}
        public int CurrentY { set; get; }
        //   public FigureModel FigureModel { get; set; }
        //   public Armie armie;
        public Color color { get; set; }
        public void SetPosition(int x, int y)
        {
            CurrentX = x;
            CurrentY = y;
        }

        public void InitializeModel()
        {
    //        var figure = new GameObject();
    //        figure.AddComponent<FigureModel>();
    //        FigureModel = figure.GetComponent<FigureModel>();
    //        figure.transform.parent = transform;
    //        figure.transform.localPosition = new Vector3(0, 0, 0);
    //        figure.GetComponent<Renderer>().material.shader = Shader.Find("Diffuse");
    //        figure.GetComponent<Renderer>().material.mainTexture = Resources.Load("textures/hex") as Texture2D;
        }
    }
}
