using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Libs;
using System;
using Libs.Graph;

public class Character : MonoBehaviour {

	static Action<Character> CharacterHightlight;

	public GameObject m_keys;

	public String ActualNodeName = "";
	public String ActualStartTime = "";
	public int tickTimeout = -1;

	public Sprite standSprite;
	public Sprite finalSprite;
	public Sprite mainTalkSprite;

	public bool whisperOnRight= true;
	private EditorNode m_startNode;
	public Libs.Graph.Graph currentGraph;
	public Node currentNode = null;

    public string fileName;

    public WhisperTalkManager m_whisperTalk;
    public bool isOnBar = false;
	public bool isOnAnimation = false;
	public bool isOnDicussion = false;
    public Vector3 finalPlace;
	public Vector3 doorPlace;
	public bool gameOverAlreadyLaunch = false;

	GameTime currentGameTime;

	TextStruct textStruct;

	public bool TVisOn = false;
	public bool BubbleAlreadyDisplayed = false;
    private bool m_isWaitingForClick = false;


	public Sprite m_hoverMouse;
	public Sprite m_clicMouse;

    Character()
    {
        currentGraph = new Libs.Graph.Graph(new Node());
    }

    public Libs.Graph.GraphNode CreateGraphNode(Libs.Graph.JSONNode _node)
    {
        char hourminutdelimiter = ':';
        Debug.Log(_node.hourminut);
        string[] hourminut = _node.hourminut.Split(hourminutdelimiter);
        int day = -1;
        int hour = -1;
        int minut = -1;
        int lifetime = 0;
        System.Int32.TryParse(_node.day, out day);
        if (hourminut.Length > 1)
        {
            System.Int32.TryParse(hourminut[0], out hour);
            System.Int32.TryParse(hourminut[1], out minut);
        }
        System.Int32.TryParse(_node.lifetime, out lifetime);
        Node.eTextMiniType textMiniType = Node.eTextMiniType.DEFAULT;
        if(_node.textminitype!="")
        {
            textMiniType = (Node.eTextMiniType)System.Enum.Parse(typeof(Node.eTextMiniType), _node.textminitype, true);
        }
		Node.eMood mood = Node.eMood.DEFAULT;
		if (_node.mood != "") {
			mood = (Node.eMood)System.Enum.Parse (typeof(Node.eMood), _node.mood, true);
		}
        return new Node(
            _node.label,
            day,
            hour,
            minut,
            lifetime,
            _node.text,
            _node.minitext,
            textMiniType,
			mood
            );
    }

    public Libs.Graph.GraphEdge CreateGraphEdge(Libs.Graph.JSONEdge _edge, Libs.Graph.GraphNode from, Libs.Graph.GraphNode to)
    {
        Edge.Condition condition = new Edge.Condition(
            (Edge.Condition.ENUM)System.Enum.Parse(typeof(Edge.Condition.ENUM), _edge.type),
            _edge.label
        );
        return new Edge(from, to, condition, _edge.label);
    }


	bool IsEventOnTime(){
		return (currentNode.GetDay () == -1 ||
		(currentNode.GetDay () <= currentGameTime.day &&
				((currentNode.GetHour () * 100 + currentNode.GetMinut ()) <= (currentGameTime.hours * 100 + currentGameTime.minutes))));
	}

    // Use this for initialization
    void Start ()
	{
		currentGraph = new Libs.Graph.Graph(fileName.Replace(".json", ""), CreateGraphNode, CreateGraphEdge);

        //Print without parcour check, this can lead to infinite loop in wrong hands
		//PrintGraph(currentGraph.GetCurrentNode());

		TVEvent.m_mainTrigger += TvIsTrigger;
		m_whisperTalk.m_tickDisplayOver += DisplayWhisperStop;
		TimeManager.OnTicTriggered += OnTick;
		TimeManager.m_DayEnding += OnEndOfDay;
		m_isWaitingForClick = false;
		Character.CharacterHightlight += OnCharacterHightlight;
		UpdateOutline (false);
		//this.GetComponent<SpriteRenderer> ().sprite = standSprite;
	}

	void UpdateOutline(bool outline) {
		MaterialPropertyBlock mpb = new MaterialPropertyBlock();
		this.GetComponent<SpriteRenderer>().GetPropertyBlock(mpb);
		mpb.SetFloat("_Outline", outline ? 1f : 0);
		mpb.SetColor("_OutlineColor", Color.white);
		this.GetComponent<SpriteRenderer>().SetPropertyBlock(mpb);
	}

	void OnDestroy(){
		TVEvent.m_mainTrigger -= TvIsTrigger;
		m_whisperTalk.m_tickDisplayOver -= DisplayWhisperStop;
		TimeManager.OnTicTriggered -= OnTick;
		TimeManager.m_DayEnding -= OnEndOfDay;
		Character.CharacterHightlight -= OnCharacterHightlight;

		DraughtEvent.m_mainTrigger -= OnBeerReady;
		BarmanManager.m_instance.Answer -= OnAnswerRespond;
		BarClosingEvent.m_mainTrigger -= OnBarClosing;
		PolicePhoneEvent.m_mainTrigger -= OnPoliceCalled;
		TaxiPhoneEvent.m_mainTrigger -= OnTaxiCalled;
		KeysEvent.m_mainTrigger -= OnKeyTaken;
		DoorEvent.m_mainTrigger -= OnGetOut;
		FreeBeerEvent.m_mainTrigger -= OnFreeBeer;
	}


	void subcribeAll(){
		if(DraughtEvent.m_mainTrigger != null)
		foreach (Delegate d in DraughtEvent.m_mainTrigger.GetInvocationList())
			DraughtEvent.m_mainTrigger -= (d as Action);

		DraughtEvent.m_mainTrigger += OnBeerReady;


			if (BarmanManager.m_instance != null) 
			if (BarmanManager.m_instance.Answer != null)
				foreach (Delegate d in BarmanManager.m_instance.Answer.GetInvocationList())
				BarmanManager.m_instance.Answer -= (d as Action<string>);

			BarmanManager.m_instance.Answer += OnAnswerRespond;


			if(BarClosingEvent.m_mainTrigger != null)
				foreach (Delegate d in BarClosingEvent.m_mainTrigger.GetInvocationList())
					BarClosingEvent.m_mainTrigger -= (d as Action);

			BarClosingEvent.m_mainTrigger += OnBarClosing;

			if(PolicePhoneEvent.m_mainTrigger != null)
				foreach (Delegate d in PolicePhoneEvent.m_mainTrigger.GetInvocationList())
					PolicePhoneEvent.m_mainTrigger -= (d as Action);

			PolicePhoneEvent.m_mainTrigger += OnPoliceCalled;

			if(TaxiPhoneEvent.m_mainTrigger != null)
				foreach (Delegate d in TaxiPhoneEvent.m_mainTrigger.GetInvocationList())
					TaxiPhoneEvent.m_mainTrigger -= (d as Action);

			TaxiPhoneEvent.m_mainTrigger += OnTaxiCalled;

			if(KeysEvent.m_mainTrigger != null)
				foreach (Delegate d in KeysEvent.m_mainTrigger.GetInvocationList())
					KeysEvent.m_mainTrigger -= (d as Action);

			KeysEvent.m_mainTrigger += OnKeyTaken;

			if(DoorEvent.m_mainTrigger != null)
				foreach (Delegate d in DoorEvent.m_mainTrigger.GetInvocationList())
					DoorEvent.m_mainTrigger -= (d as Action);

			DoorEvent.m_mainTrigger += OnGetOut;

			if(FreeBeerEvent.m_mainTrigger != null)
				foreach (Delegate d in FreeBeerEvent.m_mainTrigger.GetInvocationList())
					FreeBeerEvent.m_mainTrigger -= (d as Action);

			FreeBeerEvent.m_mainTrigger += OnFreeBeer;

	}

	private void PrintGraph(Libs.Graph.GraphNode _node, List<Edge.Condition> _conditions)
	{
		Libs.Graph.GraphNode currentNode = _node;
		print(currentNode.ToString());
		foreach (Edge.Condition c in _conditions)
		{
			Libs.Graph.GraphNode transition = currentNode.Transition(c);
			if (transition != currentNode)
			{
				PrintGraph(transition, _conditions);
			}
		}
    }

    private void PrintGraph(Libs.Graph.GraphNode _node)
    {
        Libs.Graph.GraphNode currentNode = _node;
        print(currentNode.ToString()+" "+currentNode.Edges.Count);
        foreach (Edge e in currentNode.Edges)
        {
            PrintGraph(e.GetExitNode());
        }
    }



    void Update()
    {
		bool DiscussionAvoidWhiper = false;
		if (currentNode == null || currentNode != (Node)currentGraph.GetCurrentNode ()) {
			//ChangeNode
			Node oldNode = currentNode;
			currentNode = (Node)currentGraph.GetCurrentNode();
			//Discussion
			if (oldNode != null) {
				if ((currentNode.GetTextMiniType () == Node.eTextMiniType.DISCUSSION) && (oldNode.GetTextMiniType () == Node.eTextMiniType.DISCUSSION)) {
					DiscussionAvoidWhiper = true;
				}
			}

			ActualNodeName = currentNode.GetLabel (); //DEBUG
			ActualStartTime = currentNode.GetHour()+ " h " + currentNode.GetMinut();
			tickTimeout = (currentNode.GetTicksDuration ()== 1) ? 2: currentNode.GetTicksDuration ();
			BubbleAlreadyDisplayed = false;
			if (isOnDicussion) {
				BarmanManager.m_instance.Dismiss ();
				isOnDicussion = false;
			}
			m_whisperTalk.StopDisplayWhisper ();
		}

		// check StartTime
		if (IsEventOnTime()) {

			if (!isOnBar) {
				if (!isOnAnimation) {
					this.gameObject.transform.position = doorPlace;
					tickTimeout += 2;
					this.GetComponent<Animation> ().Play("EnterBar");
					isOnAnimation = true;
				}
				return;
			}

			//check Transition (ETAT)
			if (TVisOn)
				currentGraph.Transition (new Edge.Condition (Edge.Condition.ENUM.TV));
			if (!TVisOn)
				currentGraph.Transition (new Edge.Condition (Edge.Condition.ENUM.TVOFF));
			//KEY : 
			foreach (GraphEdge edge in currentNode.Edges) {
				if (edge.condition.Equals (new Edge.Condition (Edge.Condition.ENUM.KEYS))) {
					m_keys.SetActive(true);
				} 
			}

			//Special Option
			if (currentNode.GetTextMiniType () == Node.eTextMiniType.CHARACTEREXIT) {// if exitState, lancer l'animation exit
				if (!isOnAnimation) {
					this.GetComponent<Animation> ().Play("ExitBar");
					tickTimeout = 0;
					isOnAnimation = true;
				}
				return;
			}

			if (currentNode.GetTextMiniType () == Node.eTextMiniType.GAMEOVER) {// if gameover, lancer l'animation exit
				if (!gameOverAlreadyLaunch) {
					gameOverAlreadyLaunch = true;
					IronCurtainManager.m_instance.SetGameOver (currentNode.GetText ());
				}
				return;
			}



			//display text
			if (!BubbleAlreadyDisplayed && !MainTalkManager.m_instance.m_isActivate) {
				textStruct = TextManager.m_instance.GetTextStruc (currentNode.GetTextMiniType ());
				//override with node value
				if (currentNode.GetMiniText () != "" && currentNode.GetMiniText () != null) {
					textStruct.m_whisper = currentNode.GetMiniText ();
				}
				if (currentNode.GetText () != "" && currentNode.GetText () != null) {
					textStruct.m_mainTalk = currentNode.GetText ();
				}

				if (DiscussionAvoidWhiper) {
					DisplayMainTalk ();
					return;
				}
				DisplayWhisper (textStruct.m_whisper);

			}
		} else {
			// NOT ON TIME YET
			if(isOnBar){
				//tell default conversation
				if(!BubbleAlreadyDisplayed){
					textStruct = TextManager.m_instance.GetTextStruc(Node.eTextMiniType.DEFAULT);
					DisplayWhisper (textStruct.m_whisper);

				}
			}
		}
    }

	void DisplayWhisper(string text, bool displayOnRight = true)
    {
		BubbleAlreadyDisplayed = true;
		if(textStruct.m_mainTalk != null && textStruct.m_mainTalk != "" )
       	 m_isWaitingForClick = true;
		m_whisperTalk.StartDisplayWhisper(text,whisperOnRight);
    }

	void DisplayWhisperStop(){
		BubbleAlreadyDisplayed = false;
	}

    public void OnCharacEnter()
    {
        //PLAY DING DING SOUND
		this.GetComponent<SpriteRenderer> ().sprite = standSprite;
		textStruct = TextManager.m_instance.GetTextStruc(Node.eTextMiniType.CHARACTERENTRY);
		DisplayWhisper (textStruct.m_whisper);
		//DisplayWhisper(TextManager.m_instance.GetTextStruc(Node.eTextMiniType.CHARACTERENTRY).m_whisper);
    }

    public void OnEnterFinished()
    {
        this.gameObject.transform.position = finalPlace;
		this.GetComponent<SpriteRenderer> ().sprite = finalSprite;
		isOnBar = true;
		isOnAnimation = false;
    }

	public void OnGoToDoor(){
		this.GetComponent<SpriteRenderer> ().sprite = standSprite;
		this.gameObject.transform.position = doorPlace;
		textStruct = TextManager.m_instance.GetTextStruc(Node.eTextMiniType.CHARACTEREXIT);
		DisplayWhisper (textStruct.m_whisper);
		//DisplayWhisper (TextManager.m_instance.GetTextStruc(Node.eTextMiniType.CHARACTEREXIT).m_whisper);

	}

	public void OnLeaveBar(){
		m_whisperTalk.StopDisplayWhisper();
		currentGraph.Transition (new Edge.Condition (Edge.Condition.ENUM.TIMEOUT));
		isOnAnimation = false;
		isOnBar = false;
		this.gameObject.transform.position = new Vector3 (100f, 100f, 0f);
	}

    public void OnMouseUp()
    {
		if (MainTalkManager.m_instance.m_isActivate || UIClickManager.m_instance.m_isActivate || IronCurtainManager.m_instance.m_isActivate || BarmanManager.m_instance.m_isActive)
			return;
		
        if (m_isWaitingForClick)
        {
            m_isWaitingForClick = false;

			DisplayMainTalk ();

        }
    }

	void DisplayMainTalk() {
	
		if (textStruct.m_mainTalk != "" && textStruct.m_mainTalk != null) {
			Cursor.SetCursor (m_hoverMouse.texture, Vector2.zero, CursorMode.ForceSoftware);
			m_whisperTalk.StopDisplayWhisper();
			BubbleAlreadyDisplayed = false;

			MainTalkManager.m_instance.StartDisplayAnimation(textStruct.m_mainTalk,mainTalkSprite,this.name);

			if (currentNode.GetTextMiniType () == Node.eTextMiniType.DISCUSSION) {// if exitState, lancer l'animation exit
				isOnDicussion = true;
				string answer1 = "";
				string answer2 = "";
				foreach (GraphEdge edge in currentNode.Edges) {
					Edge e = (Edge)edge;
					if (e.Text != "") {
						if (answer1 == "") {
							answer1 = e.Text;
							answer2 = e.Text;
						} else {
							answer2 = e.Text;
						}
					}
				}
				BarmanManager.m_instance.Says (answer1, answer2);
			}
			Character.CharacterHightlight (this);
			subcribeAll ();
		}
	}

	public void OnMouseDown()
	{
		//Debug.Log ("DOWN : " + IronCurtainManager.m_instance.m_isActivate + " / " + UIClickManager.m_instance.m_isActivate + " / " + IronCurtainManager.m_instance.m_isActivate + " / " + BarmanManager.m_instance.m_isActive);
		if (MainTalkManager.m_instance.m_isActivate || UIClickManager.m_instance.m_isActivate || IronCurtainManager.m_instance.m_isActivate || BarmanManager.m_instance.m_isActive)
			return;

		if (m_isWaitingForClick) {
			Cursor.SetCursor (m_clicMouse.texture, Vector2.zero, CursorMode.ForceSoftware);
		}
	}


	void OnMouseEnter()
	{
		//Debug.Log ("ENTER : " + IronCurtainManager.m_instance.m_isActivate + " / " + UIClickManager.m_instance.m_isActivate + " / " + IronCurtainManager.m_instance.m_isActivate + " / " + BarmanManager.m_instance.m_isActive);
		if (MainTalkManager.m_instance.m_isActivate || UIClickManager.m_instance.m_isActivate || IronCurtainManager.m_instance.m_isActivate || BarmanManager.m_instance.m_isActive)
			return;

		if (m_isWaitingForClick) {
			Cursor.SetCursor (m_hoverMouse.texture, Vector2.zero, CursorMode.ForceSoftware);
		} else {
			Cursor.SetCursor (null, Vector2.zero, CursorMode.ForceSoftware);
		}
	}

	void OnMouseExit()
	{
		Cursor.SetCursor (null, Vector2.zero, CursorMode.ForceSoftware);
	}

	void TvIsTrigger(bool isOn){
		TVisOn = isOn;
	}

	void OnBeerReady(){
		currentGraph.Transition(new Edge.Condition(Edge.Condition.ENUM.BEER));
	}

	void OnBarClosing(){
		currentGraph.Transition(new Edge.Condition(Edge.Condition.ENUM.BARCLOSED));
	}

	void OnPoliceCalled(){
		currentGraph.Transition(new Edge.Condition(Edge.Condition.ENUM.PHONE_POLICE));
	}

	void OnTaxiCalled() {
		currentGraph.Transition (new Edge.Condition (Edge.Condition.ENUM.PHONE_TAXI));
	}

	void OnKeyTaken() {
		currentGraph.Transition (new Edge.Condition (Edge.Condition.ENUM.KEYS));
		m_keys.SetActive( false);
	}

	void OnGetOut() {
		currentGraph.Transition (new Edge.Condition (Edge.Condition.ENUM.DOOR));
	}

	void OnFreeBeer() {
		currentGraph.Transition (new Edge.Condition (Edge.Condition.ENUM.FREEBEER));
	}

	void OnAnswerRespond(string response){
		isOnDicussion = false;
		currentGraph.Transition (response);
	}

	void OnTick(GameTime gametime){
		currentGameTime = gametime;
		if (isOnBar && IsEventOnTime()) {
			if (!isOnAnimation)
				tickTimeout--;
			if (tickTimeout <= 0) {
				if (currentNode.GetTextMiniType () == Node.eTextMiniType.DISCUSSION) {
					foreach (GraphEdge edge in currentNode.Edges) {
						if (edge.condition.Equals (new Edge.Condition (Edge.Condition.ENUM.TIMEOUT))) {
							currentGraph.Transition (new Edge.Condition (Edge.Condition.ENUM.TIMEOUT));
							return;
						} 
					}
					currentGraph.Transition (new Edge.Condition (Edge.Condition.ENUM.OTHER));
				} else {
				//Normal Stuff
					currentGraph.Transition (new Edge.Condition (Edge.Condition.ENUM.TIMEOUT));
				}
			}
		}
	}

	void OnEndOfDay() {
		if (isOnBar) {
			if (!isOnAnimation) {
				this.GetComponent<Animation> ().Play ("ExitBar");
				isOnAnimation = true;
			}
		}
	}

	void OnCharacterHightlight(Character cha) {
		if (this == cha) {
			UpdateOutline (true);
		} else {
			UpdateOutline (false);
		}
	}


}
