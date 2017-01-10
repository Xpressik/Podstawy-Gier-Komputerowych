using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum WallOwner
{
    first, second
}

public class HexCell : MonoBehaviour {

    public HexCoordinates coordinates;

    public RectTransform uiRect;

    public HexGridChunk chunk;

    public BoxCollider bc;

    public HexCellInfo info;
   
    bool isGame;
    private Color cellColor;

    public bool isWallCapsule = false;

    public bool isWallNonCapsule = false;

    private int specialIndex;

    public void SaveInfo()
    {
        this.info.cellColor = this.color;
        this.info.TransformColor();
        this.info.elevation = this.elevation;
        this.info.hasIncomingRiver = hasIncomingRiver;
        this.info.hasOutgoingRiver = hasOutgoingRiver;
        this.info.incomingRiver = incomingRiver;
        this.info.outgoingRiver = outgoingRiver;
        info.waterLevel = waterLevel;
        info.isWalled = walled;
        info.farmLevel = farmLevel;
        info.plantLevel = plantLevel;
        info.urbanLevel = urbanLevel;
    }

    void Start()
    {
        isGame = GameObject.Find("Hex Grid").GetComponent<HexGrid>().isGame;

        bc = gameObject.AddComponent<BoxCollider>();
        bc.size = new Vector3(17, 17);
    }

    void Update()
    {
      
    }

    void OnMouseEnter()
    {
        cellColor = Color;
        this.Color = new Color(1, 1, 140.0f/255.0f);
    }

    void OnMouseExit()
    {
        this.Color = cellColor;
    }

    public Color Color
    {
		get
        {
			return color;
		}
		set
        {
			if (color == value)
            {
				return;
			}
			color = value;
			Refresh();
	    	}
	}

	public int Elevation
    {
		get
        {
			return elevation;
		}
		set
        {
			if (elevation == value)
            {
				return;
			}
			elevation = value;
			Vector3 position = transform.localPosition;
			position.y = value * HexMetrics.elevationStep;
			position.y += (HexMetrics.SampleNoise(position).y * 2f - 1f) * HexMetrics.elevationPerturbStrength;
			transform.localPosition = position;

			Vector3 uiPosition = uiRect.localPosition;
			uiPosition.z = -position.y;
			uiRect.localPosition = uiPosition;

            ValidateRivers();

			Refresh();
		}
	}

	public bool HasIncomingRiver
    {
		get
        {
			return hasIncomingRiver;
		}
	}

	public bool HasOutgoingRiver
    {
		get
        {
			return hasOutgoingRiver;
		}
	}

	public bool HasRiver
    {
		get
        {
			return hasIncomingRiver || hasOutgoingRiver;
		}
	}

	public bool HasRiverBeginOrEnd
    {
		get
        {
			return hasIncomingRiver != hasOutgoingRiver;
		}
	}

	public HexDirection IncomingRiver
    {
		get
        {
			return incomingRiver;
		}
	}

	public HexDirection OutgoingRiver
    {
		get
        {
			return outgoingRiver;
		}
	}

	public Vector3 Position
    {
		get
        {
			return transform.localPosition;
		}
	}

	public float RiverSurfaceY
    {
		get
        {
			return (elevation + HexMetrics.riverSurfaceElevationOffset) * HexMetrics.elevationStep;
		}
	}

	public float StreamBedY
    {
		get
        {
			return (elevation + HexMetrics.streamBedElevationOffset) * HexMetrics.elevationStep;
		}
	}

	public Color color;

	int elevation = int.MinValue;

	public bool hasIncomingRiver, hasOutgoingRiver;
	public HexDirection incomingRiver, outgoingRiver;

	[SerializeField]
	public HexCell[] neighbors;

	public HexCell GetNeighbor (HexDirection direction)
    {
		return neighbors[(int)direction];
	}

	public void SetNeighbor (HexDirection direction, HexCell cell)
    {
		neighbors[(int)direction] = cell;
		cell.neighbors[(int)direction.Opposite()] = this;
	}

	public HexEdgeType GetEdgeType (HexDirection direction)
    {
		return HexMetrics.GetEdgeType(elevation, neighbors[(int)direction].elevation);
	}

	public HexEdgeType GetEdgeType (HexCell otherCell)
    {
		return HexMetrics.GetEdgeType(elevation, otherCell.elevation);
	}

	public bool HasRiverThroughEdge (HexDirection direction)
    {
		return hasIncomingRiver && incomingRiver == direction || hasOutgoingRiver && outgoingRiver == direction;
	}

	public void RemoveIncomingRiver ()
    {
		if (!hasIncomingRiver)
        {
			return;
		}
		hasIncomingRiver = false;
		RefreshSelfOnly();

		HexCell neighbor = GetNeighbor(incomingRiver);
		neighbor.hasOutgoingRiver = false;
		neighbor.RefreshSelfOnly();
	}

	public void RemoveOutgoingRiver ()
    {
		if (!hasOutgoingRiver)
        {
			return;
		}
		hasOutgoingRiver = false;
		RefreshSelfOnly();

		HexCell neighbor = GetNeighbor(outgoingRiver);
		neighbor.hasIncomingRiver = false;
		neighbor.RefreshSelfOnly();
	}

	public void RemoveRiver ()
    {
		RemoveOutgoingRiver();
		RemoveIncomingRiver();
	}

	public void SetOutgoingRiver (HexDirection direction)
    {
		if (hasOutgoingRiver && outgoingRiver == direction)
        {
			return;
		}

		HexCell neighbor = GetNeighbor(direction);
        if (!IsValidRiverDestination(neighbor))
        {
            return;
		}

		RemoveOutgoingRiver();
		if (hasIncomingRiver && incomingRiver == direction)
        {
			RemoveIncomingRiver();
		}
		hasOutgoingRiver = true;
		outgoingRiver = direction;
        specialIndex = 0;

        RefreshSelfOnly();

		neighbor.RemoveIncomingRiver();
		neighbor.hasIncomingRiver = true;
		neighbor.incomingRiver = direction.Opposite();
        neighbor.specialIndex = 0;

        neighbor.RefreshSelfOnly();
	}

	public void Refresh ()
    {
		if (chunk)
        {
			chunk.Refresh();
			for (int i = 0; i < neighbors.Length; i++)
            {
				HexCell neighbor = neighbors[i];
				if (neighbor != null && neighbor.chunk != chunk)
                {
					neighbor.chunk.Refresh();
				}
			}
		}
	}

	void RefreshSelfOnly ()
    {
		chunk.Refresh();
	}

    public int UrbanLevel
    {
        get
        {
            return urbanLevel;
        }
        set
        {
            if (urbanLevel != value)
            {
                urbanLevel = value;
                RefreshSelfOnly();
            }
        }
    }

    public int FarmLevel
    {
        get
        {
            return farmLevel;
        }
        set
        {
            if (farmLevel != value)
            {
                farmLevel = value;
                RefreshSelfOnly();
            }
        }
    }

    public int PlantLevel
    {
        get
        {
            return plantLevel;
        }
        set
        {
            if (plantLevel != value)
            {
                plantLevel = value;
                RefreshSelfOnly();
            }
        }
    }

    int urbanLevel, farmLevel, plantLevel;

    public bool Walled
    {
        get
        {
            return walled;
        }
        set
        {
            if (walled != value)
            {
                walled = value;
                Refresh();
            }
        }
    }

    bool walled;

    public HexMesh Wall
    {
        get
        {
            return wall;
        }
        set
        {
            if(wall != value)
            {
                wall = value;
                RefreshSelfOnly();
            }
        }
    }

    HexMesh wall;

    public int WaterLevel
    {
        get
        {
            return waterLevel;
        }
        set
        {
            if (waterLevel == value)
            {
                return;
            }
            waterLevel = value;
            ValidateRivers();
            Refresh();
        }
    }

    int waterLevel;

    public bool IsUnderwater
    {
        get
        {
            return waterLevel > elevation;
        }
    }


    public float WaterSurfaceY
    {
        get
        {
            return
                (waterLevel + HexMetrics.waterElevationOffset) *
                HexMetrics.elevationStep;
        }
    }

    bool IsValidRiverDestination(HexCell neighbor)
    {
        return neighbor && (
            elevation >= neighbor.elevation || waterLevel == neighbor.elevation
        );
    }

    void ValidateRivers()
    {
        if (
            hasOutgoingRiver &&
            !IsValidRiverDestination(GetNeighbor(outgoingRiver))
        )
        {
            RemoveOutgoingRiver();
        }
        if (
            hasIncomingRiver &&
            !GetNeighbor(incomingRiver).IsValidRiverDestination(this)
        )
        {
            RemoveIncomingRiver();
        }
    }

    public int SpecialIndex
    {
        get
        {
            return specialIndex;
        }
        set
        {
            if (specialIndex != value && !HasRiver)
            {
                specialIndex = value;
                RefreshSelfOnly();
            }
        }
    }

    public bool IsSpecial
    {
        get
        {
            return specialIndex > 0;
        }
    }
}