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
    
    // DO OKIENKA Z INFORMACJAMI O ZASOBACH NA POLU
    public string myString;
    public Text myText;
    public float fadeTime;
    
 //   public Canvas fieldEditor;
    public Button player1Button;
    public Button player2Button;
    public Button player0Button;
    public InputField supplyInput;
    public InputField contrabandInput;
    public Button S0;
    public Button S1;
    public Button S2;

    public Button C0;
    public Button C1;
    public Button C2;

    public Toggle campToggle;
    public Button quitButton;
    
    Canvas sth;
    public Field field;
    public int owner;
    public Boolean camp;
    public int supply;
    public int contraband;

    public Canvas fieldInfo;
    public Text supplyOutput;

    //public FieldInfoScript fieldInfo;

    bool isGame;
    private Color cellColor;

    public bool isWallCapsule = false;

    public bool isWallNonCapsule = false;

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

        // DO OKIENKA Z INFORMACJAMI O ZASOBACH NA POLU
        myString = "On Field";
        fadeTime = 1;
        var hexGridCanvas = GameObject.Find("Hex Grid Canvas").GetComponent<Canvas>();
        myText = GameObject.Find("Text").GetComponent<Text>();
        myText.transform.SetParent(hexGridCanvas.transform, false);
        myText.supportRichText = false;

        if (!isGame)
        {
            //sth = GameObject.Find("FieldEditor").GetComponent<Canvas>();
            sth.transform.SetParent(hexGridCanvas.transform, false);
            sth.enabled = false;
            sth.transform.Rotate(new Vector3(-0.15f, 0, 0));
            sth.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
            //fieldEditor = gameObject.GetComponentInParent<Canvas>();
            //fieldEditor = fieldEditor.GetComponent<Canvas>();
            player1Button = GameObject.Find("Player1Button").GetComponent<Button>();
            player2Button = GameObject.Find("Player2Button").GetComponent<Button>();
            player0Button = GameObject.Find("Player0Button").GetComponent<Button>();
            //supplyInput = GameObject.Find("SupplyInput").GetComponent<InputField>();
            //contrabandInput = GameObject.Find("ContrabandInput").GetComponent<InputField>();
            S0 = GameObject.Find("S0").GetComponent<Button>();
            //S0.image.color = Color.red;
            S1 = GameObject.Find("S1").GetComponent<Button>();
            S2 = GameObject.Find("S2").GetComponent<Button>();

            C0 = GameObject.Find("C0").GetComponent<Button>();
            C1 = GameObject.Find("C1").GetComponent<Button>();
            //C1.image.color = Color.red;
            C2 = GameObject.Find("C2").GetComponent<Button>();
            campToggle = GameObject.Find("CampToggle").GetComponent<Toggle>();
            //Console.WriteLine(GameObject.Find("CampToggle").ToString());
            //Debug.Log(GameObject.Find("CampToggle").ToString());
            //campToggle.transform.SetParent(sth.transform, false);
            //campToggle = campToggle.GetComponent<Toggle>();
            quitButton = GameObject.Find("QuitButton").GetComponent<Button>();
            //  fieldEditor.enabled = false;

            //Transform caretGO = supplyInput.transform.FindChild(supplyInput.transform.name + " Input Caret");
            //caretGO.GetComponent<CanvasRenderer>().SetMaterial(Graphic.defaultGraphicMaterial, Texture2D.whiteTexture);
            //caretGO = contrabandInput.transform.FindChild(supplyInput.transform.name + " Input Caret");
            //caretGO.GetComponent<CanvasRenderer>().SetMaterial(Graphic.defaultGraphicMaterial, Texture2D.whiteTexture);


            //supplyInput.ActivateInputField();
            //contrabandInput.ActivateInputField();
            //fieldInfo = GameObject.Find("FieldInfo").GetComponent<Canvas>();
            //fieldInfo = fieldInfo.GetComponent<Canvas>();
            //supplyOutput = supplyOutput.GetComponent<Text>();
            //supplyOutput.text = "test";
            //fieldInfo.enabled = false;
        }
        this.field = new Field(this);
        this.field.camp = false;
        this.field.supply = 2;
        this.field.contraband = 1;
        this.field.garrison = 0;
        this.field.playerInt = 2;    
    }

    void Update()
    {
        if (!isGame)
        {
            if (Input.GetMouseButtonDown(1))
            {
                sth.enabled = true;
                HandleInput();
                if (this.field.contraband == 0)
                {
                    this.C0.image.color = Color.red;
                    this.C1.image.color = Color.white;
                    this.C2.image.color = Color.white;
                }
                else if (this.field.contraband == 1)
                {
                    this.C0.image.color = Color.white;
                    this.C1.image.color = Color.red;
                    this.C0.image.color = Color.white;
                }
                else if (this.field.contraband == 2)
                {
                    this.C0.image.color = Color.white;
                    this.C2.image.color = Color.white;
                    this.C0.image.color = Color.red;
                }

                if (this.field.supply == 0)
                {
                    this.S0.image.color = Color.red;
                    this.S1.image.color = Color.white;
                    this.S2.image.color = Color.white;
                }
                else if (this.field.supply == 1)
                {
                    this.S0.image.color = Color.white;
                    this.S1.image.color = Color.red;
                    this.S2.image.color = Color.white;
                }
                else if (this.field.supply == 2)
                {
                    this.S0.image.color = Color.white;
                    this.S1.image.color = Color.white;
                    this.S2.image.color = Color.red;
                }

                if (this.field.playerInt == 0)
                {
                    this.player0Button.image.color = Color.red;
                    this.player1Button.image.color = Color.white;
                    this.player2Button.image.color = Color.white;
                }
                else if (this.field.playerInt == 1)
                {
                    this.player0Button.image.color = Color.white;
                    this.player1Button.image.color = Color.red;
                    this.player2Button.image.color = Color.white;
                }
                else if (this.field.playerInt == 2)
                {
                    this.player0Button.image.color = Color.white;
                    this.player1Button.image.color = Color.white;
                    this.player2Button.image.color = Color.red;
                }

                if (this.field.camp == true)
                {
                    this.campToggle.isOn = true;
                }
                else if (this.field.camp == false)
                {
                    this.campToggle.isOn = false;
                }

            }
        }

        
            if (isGame)
        {
            if (Input.GetMouseButtonDown(1))
            {
                this.fieldInfo.enabled = true;
            }
        }
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

    //void OnMouseOver()
    //{
    //    //myText.rectTransform.anchoredPosition = uiRect.anchoredPosition;
    //    var tmp = transform.position;
    //    float offset = 0;
    //    if (neighbors[5] != null && neighbors[4] != null)
    //    {
    //        float fifthNeighborYPosition = neighbors[5].transform.position.y;
    //        float fourthNeighborYPosition = neighbors[4].transform.position.y;
    //        if (fifthNeighborYPosition > tmp.y || fourthNeighborYPosition > tmp.y)
    //        {
    //            if (fifthNeighborYPosition > fourthNeighborYPosition)
    //            {
    //                offset = fifthNeighborYPosition - tmp.y;
    //            }
    //            else
    //            {
    //                offset = fourthNeighborYPosition - tmp.y;
    //            }
    //        }
    //    }
    //    myText.transform.rotation = Camera.main.transform.rotation;
    //    myText.transform.position = new Vector3(tmp.x, tmp.y + 0.5f + offset, tmp.z);
    //    myText.color = Color.black; // Color.Lerp(myText.color, Color.black, fadeTime * Time.deltaTime);
    //    if (HasRiver)
    //    {
    //        myText.text = "River!";
    //    }
    //    else 
    //    {
    //        myText.text = myString;
    //    }

    //}

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
        //SaveFieldInfo();
        sth.enabled = false;
    }

    public void SaveFieldInfo()
    {
        this.field.supply = supply;
        this.field.contraband = contraband;
        this.field.ownerInt = this.owner;
        this.field.camp = this.camp;
    }

    public void Player1ButtonClicked()
    {
        this.field.playerInt = 1;
        player1Button.image.color = Color.red;
        player2Button.image.color = Color.white;
        player0Button.image.color = Color.white;
    }

    public void Player2ButtonClicked()
    {
        this.field.playerInt = 2;
        player2Button.image.color = Color.red;
        player1Button.image.color = Color.white;
        player0Button.image.color = Color.white;
    }

    public void Player0ButtonClicked()
    {
        this.field.playerInt = 0;
        player2Button.image.color = Color.white;
        player1Button.image.color = Color.white;
        player0Button.image.color = Color.red;
    }

    public void ToggleChanged()
    {
        if (campToggle.IsActive())
        {
            this.field.camp = true;
        }
        else
        {
            this.field.camp = false;
        }
    }

    public void S0Press()
    {
        this.field.supply = 0;
        S0.image.color = Color.red;
        S1.image.color = Color.white;
        S2.image.color = Color.white;
    }

    public void S1Press()
    {
        this.field.supply = 1;
        S0.image.color = Color.white;
        S1.image.color = Color.red;
        S2.image.color = Color.white;
    }

    public void S2Press()
    {
        this.field.supply = 2;
        S0.image.color = Color.white;
        S1.image.color = Color.white;
        S2.image.color = Color.red;
    }

    public void C0Press()
    {
        this.field.contraband = 0;
        C0.image.color = Color.red;
        C1.image.color = Color.white;
        C2.image.color = Color.white;
    }

    public void C1Press()
    {
        this.field.contraband = 1;
        C0.image.color = Color.white;
        C1.image.color = Color.red;
        C2.image.color = Color.white;
    }

    public void C2Press()
    {
        this.field.contraband = 2;
        C0.image.color = Color.white;
        C1.image.color = Color.white;
        C2.image.color = Color.red;
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
}