using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HexCell : MonoBehaviour {

    public HexCoordinates coordinates;

    public RectTransform uiRect;

    public HexGridChunk chunk;

    public BoxCollider bc;

    public HexCellInfo info;

    // DO OKIENKA Z INFORMACJAMI O ZASOBACH NA POLU
    public string myString;
    public Text myText;
    public float fadeTime;
    
 //   public Canvas fieldEditor;
    public Button player1Button;
    public Button player2Button;
    public InputField supplyInput;
    public InputField contrabandInput;
    public Toggle campToggle;
    public Button quitButton;
    
    Canvas sth;
    public Field field;
    public int tmp;
    public Boolean camp;

    bool isGame;
    private Color cellColor;

    public void SaveInfo()
    {
        this.info.cellColor = this.color;
        this.info.TransformColor();
        this.info.elevation = this.elevation;
        this.info.hasIncomingRiver = hasIncomingRiver;
        this.info.hasOutgoingRiver = hasOutgoingRiver;
        this.info.incomingRiver = incomingRiver;
        this.info.outgoingRiver = outgoingRiver;
    }

    void Start()
    {
        isGame = GameObject.Find("Hex Grid").GetComponent<HexGrid>().isGame;

        bc = gameObject.AddComponent<BoxCollider>();
        bc.size = new Vector3(17, 17);

        // DO OKIENKA Z INFORMACJAMI O ZASOBACH NA POLU
        myString = "On Field";
        fadeTime = 1;
        var hexGridCanvas = GameObject.Find("Hex Grid Canvas").GetComponent<Canvas>();
        myText = GameObject.Find("Text").GetComponent<Text>();
        myText.transform.SetParent(hexGridCanvas.transform, false);
        myText.supportRichText = false;

        if (!isGame)
        {
            sth = GameObject.Find("FieldEditor").GetComponent<Canvas>();
            sth.transform.SetParent(hexGridCanvas.transform, false);
            sth.enabled = false;
            sth.transform.Rotate(new Vector3(-0.15f, 0, 0));
            sth.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
            //fieldEditor = gameObject.GetComponentInParent<Canvas>();
            //fieldEditor = fieldEditor.GetComponent<Canvas>();
            player1Button = GameObject.Find("Player1Button").GetComponent<Button>();
            player2Button = GameObject.Find("Player2Button").GetComponent<Button>();
            supplyInput = GameObject.Find("SupplyInput").GetComponent<InputField>();
            contrabandInput = GameObject.Find("ContrabandInput").GetComponent<InputField>();
            //campToggle = GameObject.Find("CampToggle").
            //Console.WriteLine(GameObject.Find("CampToggle").ToString());
            //Debug.Log(GameObject.Find("CampToggle").ToString());
            //campToggle.transform.SetParent(sth.transform, false);
            //campToggle = campToggle.GetComponent<Toggle>();
            quitButton = GameObject.Find("QuitButton").GetComponent<Button>();
            //  fieldEditor.enabled = false;


            supplyInput.ActivateInputField();
            contrabandInput.ActivateInputField();
        }
        this.field = new Field(this);
        this.camp = false;    
    }

    void Update()
    {
        if (!isGame)
        {
            if (Input.GetMouseButtonDown(1))
            {
                sth.enabled = true;
                HandleInput();
            }
         //   DisableObjects();
         //   sth.enabled = true;
        }

        //if (campToggle.isOn)
        //{
        //    this.camp = true;
        //}
    }

    void HandleInput()
    {
        Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(inputRay, out hit))
        {
            sth.transform.position = new Vector3(hit.point.x, hit.point.y + 10, hit.point.z);
        }
        else
        {

        }
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
        myText.transform.rotation = Camera.main.transform.rotation;
        myText.transform.position = new Vector3(tmp.x, tmp.y + 0.5f + offset, tmp.z);
        myText.color = Color.black; // Color.Lerp(myText.color, Color.black, fadeTime * Time.deltaTime);
        if (HasRiver)
        {
            myText.text = "River!";
        }
        else 
        {
            myText.text = myString;
        }
    }

    void OnMouseEnter()
    {
        cellColor = Color;
        this.Color = new Color(1, 1, 140.0f/255.0f);
    }

    void OnMouseExit()
    {
        this.Color = cellColor;
        myText.color = Color.clear;//Color.Lerp(myText.color, Color.clear, fadeTime * Time.deltaTime);
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

	public Color color;

	int elevation = int.MinValue;

	public bool hasIncomingRiver, hasOutgoingRiver;
	public HexDirection incomingRiver, outgoingRiver;

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

    public void QuitFieldEditor()
    {
        sth.enabled = false;
    }

    public void SaveFieldInfo()
    {
        this.field.supply = Int32.Parse(supplyInput.ToString());
        this.field.contraband = Int32.Parse(contrabandInput.ToString());
        this.field.ownerInt = this.tmp;
        EnableObjects();
    }

    public void Player1ButtonClicked()
    {
        this.tmp = 1;
    }

    public void Player2ButtonClicked()
    {
        this.tmp = 2;
    }

    public void ToggleChanged()
    {
        if (this.camp == false)
        {
            this.camp = true;
        }
        if (this.camp == true)
        {
            this.camp = false;
        }

    }
    
    public void DisableObjects()
    {
        HexGrid[] objects = FindObjectsOfType<HexGrid>();
        List<HexGrid> objectsToDisable = new List<HexGrid>(objects);

        foreach (HexGrid a in objectsToDisable)
        {
            a.enabled = false;
        }
    }

    public void EnableObjects()
    {
        HexGrid[] objects = FindObjectsOfType<HexGrid>();
        List<HexGrid> objectsToDisable = new List<HexGrid>(objects);

        foreach (HexGrid a in objectsToDisable)
        {
            a.enabled = true;
        }
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
}