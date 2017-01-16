﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets
{
    public class Figure : MonoBehaviour
    {
        public int CurrentX { set; get;}
        public int CurrentY { set; get; }

        public void SetPosition(int x, int y)
        {
            CurrentX = x;
            CurrentY = y;
        }
    }
}
