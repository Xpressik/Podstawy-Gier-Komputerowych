using UnityEngine;
using UnityEngine.UI;

public class HexCell : MonoBehaviour {

    public HexCoordinates coordinates;

    public RectTransform uiRect;

    public HexGridChunk chunk;

    public BoxCollider bc;

    // DO OKIENKA Z INFORMACJAMI O ZASOBACH NA POLU
    public string myString;
    public Text myText;
    public float fadeTime;

    void Start()
    {
        bc = gameObject.AddComponent<BoxCollider>();
        bc.size = new Vector3(17, 17);

        // DO OKIENKA Z INFORMACJAMI O ZASOBACH NA POLU
        myString = "On Field";
        fadeTime = 10;
        var hexGridCanvas = GameObject.Find("Hex Grid Canvas").GetComponent<Canvas>();
        myText = GameObject.Find("Text").GetComponent<Text>();
        myText.transform.SetParent(hexGridCanvas.transform, false);
        myText.supportRichText = false;
    }
   
    void OnMouseOver()
    {
        myText.rectTransform.anchoredPosition = uiRect.anchoredPosition;
        var tmp = transform.position;
        float offset = 0;
        if (neighbors[5] != null && neighbors[4] != null)
        {
            float fifthNeighborYPosition = neighbors[5].transform.position.y;
            float fourthNeighborYPosition = neighbors[4].transform.position.y;
            if (fifthNeighborYPosition > tmp.y || fourthNeighborYPosition > tmp.y)
            {
                if (fifthNeighborYPosition > fourthNeighborYPosition)
                {
                    offset = fifthNeighborYPosition - tmp.y;
                }
                else
                {
                    offset = fourthNeighborYPosition - tmp.y;
                }
            }
        }
        myText.transform.position = new Vector3(tmp.x, tmp.y + 0.5f + offset, tmp.z);
        myText.text = myString;
        myText.color = Color.Lerp(myText.color, Color.black, fadeTime * Time.deltaTime);
    }

    void OnMouseEnter()
    {
        this.Color = new Color(1, 1, 140.0f/255.0f);
    }

    void OnMouseExit()
    {
        this.Color = Color.white;
        myText.color = Color.Lerp(myText.color, Color.clear, fadeTime * Time.deltaTime);
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

			if (hasOutgoingRiver && elevation < GetNeighbor(outgoingRiver).elevation)
            {
				RemoveOutgoingRiver();
			}
			if (hasIncomingRiver && elevation > GetNeighbor(incomingRiver).elevation)
            {
				RemoveIncomingRiver();
			}

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

	Color color;

	int elevation = int.MinValue;

	bool hasIncomingRiver, hasOutgoingRiver;
	HexDirection incomingRiver, outgoingRiver;

	[SerializeField]
	HexCell[] neighbors;

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
		if (!neighbor || elevation < neighbor.elevation)
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
		RefreshSelfOnly();

		neighbor.RemoveIncomingRiver();
		neighbor.hasIncomingRiver = true;
		neighbor.incomingRiver = direction.Opposite();
		neighbor.RefreshSelfOnly();
	}

	void Refresh ()
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
}