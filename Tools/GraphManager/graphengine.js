/*************************************************************************************
The MIT License (MIT)

Copyright (c) 2016 Pierre-Marie Plans

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
**************************************************************************************/

var nodes = new vis.DataSet({});
var edges = new vis.DataSet({});
var _backup_nodes = new vis.DataSet({});;
var _backup_edges = new vis.DataSet({});;
var progress = undefined;

var NODES_FIELDS = ['id', 'label', 'title'];
document.Settings.CharacterSheet.forEach(function(e) {
	NODES_FIELDS.push(e.variable);
});
var EDGES_FIELDS = ['from', 'to', 'type', 'label', 'arrows'];

// provide the data in the vis format
var data = {
	nodes: nodes,
	edges: edges
};

var network = undefined;

// init sheets
var _characterManager	= Object.create(CharacterSheet);
var _linkManager		= Object.create(LinkSheet);

// Array Remove - By John Resig (MIT Licensed)
Array.prototype.remove = function(from, to) {
  var rest = this.slice((to || from) + 1 || this.length);
  this.length = from < 0 ? this.length + from : from;
  return this.push.apply(this, rest);
};

function checkUndefined(value) {
	return value==undefined?'':value;
}
/** START FILE READER FUNCTIONS **/
function FileReaderErrorHandler(evt) {
	switch(evt.target.error.code) {
		case evt.target.error.NOT_FOUND_ERR:
			alert('File Not Found!');
		break;
		case evt.target.error.NOT_READABLE_ERR:
			alert('File is not readable');
		break;
		case evt.target.error.ABORT_ERR:
			alert('Abort!');
		break; // noop
		default:
			alert('An error occurred reading this file.');
	};
	document.getElementById("load_submenu").style.display = 'none';
}
/** END FILE READER FUNCTIONS **/

// from https://fortawesome.github.io/Font-Awesome/icons/
var FAICONS = {
	person: '\uf007',
	male: '\uf183',
	female: '\uf182',
	group: '\uf0c0',
	eye: '\uf06e',
	building: '\uf0f7',
	bomb: '\uf1e2',
	handfist: '\uf255',
	hand: '\uf256',
	handpointer: '\uf25a',
	blood: '\uf043',
	heart: '\uf08a',
	heartbeat: '\uf21e',
	secretagent: '\uf21b',
	genderless: '\uf22d',
	gendermale: '\uf222',
	genderfemale: '\uf221'
}

// from https://fortawesome.github.io/Font-Awesome/icons/
function setFAIcon(data, code, size = 20, color = '#000000') {
	data.shape = 'icon';
	data.icon = {
          face: 'FontAwesome',
          code: code,
          size: size,
          color: color
        };
}

function readXML(file) {

  // Create a connection to the file.
  var Connect = new XMLHttpRequest();
  // Define which file to open and
  // send the request.
  Connect.open("GET", file, false);
  Connect.setRequestHeader("Content-Type", "text/xml");
  Connect.send(null);
  // Place the response in an XML document.
  var TheDocument = Connect.responseXML;
  // Place the root node in an element.
  var Customers = TheDocument.childNodes[0];
  // Retrieve each customer in turn.
  for (var i = 0; i < Customers.children.length; i++)
  {
   var Customer = Customers.children[i];
   // Access each of the data values.
   var Name = Customer.getElementsByTagName("Name");
   var Age = Customer.getElementsByTagName("Age");
   var Color = Customer.getElementsByTagName(
     "FavoriteColor");
   // Write the data to the page.
   document.write("<tr><td>");
   document.write(Name[0].textContent.toString());
   document.write("</td><td>");
   document.write(Age[0].textContent.toString());
   document.write("</td><td>");
   document.write(Color[0].textContent.toString());
   document.write("</td></tr>");
  }

}

var options = {
	manipulation: {
		enabled: true,
		initiallyActive: false,
		addNode: function (data, callback) {
			// filling in the popup DOM elements
			document.getElementById('operation').innerHTML = "Add Node";
			_characterManager.Load(data, true, true);
			document.getElementById('saveButton').onclick = saveData.bind(this, data, callback);
			document.getElementById('cancelButton').onclick = clearPopUp.bind();
			// $("#editor").parent().show();
			$("#editor").dialog("open").on( "dialogclose", function( event, ui ) { clearPopUp.bind(); } );
			$("#editor").parent().show();
		},
		editNode: function (data, callback) {
			// filling in the popup DOM elements
			document.getElementById('operation').innerHTML = "Edit Node";
			_characterManager.Load(data, false, true);
			document.getElementById('saveButton').onclick = saveData.bind(this, data, callback);
			document.getElementById('cancelButton').onclick = cancelEdit.bind(this,callback);
			// $("#editor").parent().show();
			$("#editor").dialog("open").on( "dialogclose", function( event, ui ) { cancelEdit.bind(this,callback); } );
			$("#editor").parent().show();
		},
		addEdge: function (data, callback) {
			// filling in the popup DOM elements
			document.getElementById('EdgeOperation').innerHTML = "Add Edge";
			_linkManager.Load(data, true);
			document.getElementById('EdgeSaveButton').onclick = EdgeSaveData.bind(this, data, callback);
			document.getElementById('EdgeCancelButton').onclick = EdgeClearPopUp.bind();
			// $("#EdgeEditor").parent().show();
			$("#EdgeEditor").dialog("open").on( "dialogclose", function( event, ui ) { EdgeClearPopUp.bind(this,callback); } );
			$("#EdgeEditor").parent().show();
			
		},
		editEdge: function (data, callback) {
			// filling in the popup DOM elements
			var oldData = edges.get(data.id, {fields: ['type', 'label', 'arrows']});
			document.getElementById('EdgeOperation').innerHTML = "Edit Edge";
			_linkManager.Load(oldData, false);
			document.getElementById('EdgeSaveButton').onclick = EdgeSaveData.bind(this, data, callback);
			document.getElementById('EdgeCancelButton').onclick = EdgeCancelEdit.bind(this, callback);
			// $("#EdgeEditor").parent().show();
			$("#EdgeEditor").dialog("open").on( "dialogclose", function( event, ui ) { EdgeCancelEdit.bind(this,callback); } );
			$("#EdgeEditor").parent().show();
			
		},
		deleteNode: true,
		deleteEdge: true,
		controlNodeStyle:{
		  // all node options are valid.
		}
	},
	groups: document.Settings.Groups,
	layout: {
		randomSeed: undefined,
		improvedLayout:true,
		hierarchical: {
			enabled:false,
			levelSeparation: 100,
			direction: 'UD',   // UD, DU, LR, RL
			sortMethod: 'directed' // hubsize, directed
		}
	},
	nodes:{
		borderWidth: 1,
		borderWidthSelected: 2,
		brokenImage:undefined,
		color: {
			border: '#2B7CE9',
			background: '#97C2FC',
			highlight: {
				border: '#2B7CE9',
				background: '#D2E5FF'
			},
			hover: {
				border: '#2B7CE9',
				background: '#D2E5FF'
			}
		},
		fixed: {
			x:false,
			y:false
		},
		font: {
			color: '#343434',
			size: 14, // px
			face: 'arial',
			background: 'none',
			strokeWidth: 0, // px
			strokeColor: '#ffffff',
			align: 'horizontal'
		},
		group: undefined,
		hidden: false,
		icon: {
			face: 'FontAwesome',
			code: undefined,
			size: 50,  //50,
			color:'#2B7CE9'
		},
		image: undefined,
		label: undefined,
		labelHighlightBold: true,
		level: undefined,
		mass: 1,
		physics: true,
		scaling: {
			min: 10,
			max: 30,
			label: {
				enabled: false,
				min: 14,
				max: 30,
				maxVisible: 30,
				drawThreshold: 5
			},
			customScalingFunction: function (min,max,total,value) {
				if (max === min) {
					return 0.5;
				}else {
					var scale = 1.0 / (max - min);
					return Math.max(0,(value - min)*scale);
				}
			}
		},
		shadow:{
			enabled: false,
			color: 'rgba(0,0,0,0.5)',
			size:10,
			x:5,
			y:5
		},
		shape: 'circle',
		shapeProperties: {
			borderDashes: false, // only for borders
			borderRadius: 6,     // only for box shape
			useImageSize: false,  // only for image and circularImage shapes
			useBorderWithImage: false  // only for image shape
		},
		size: 25,
		title: undefined,
		value: undefined,
		x: undefined,
		y: undefined
	},
	interaction:{
		dragNodes:true,
		dragView: true,
		hideEdgesOnDrag: false,
		hideNodesOnDrag: false,
		hover: false,
		hoverConnectedEdges: true,
		keyboard: {
			enabled: true,
			speed: {x: 10, y: 10, zoom: 0.02},
			bindToWindow: true
		},
		multiselect: false,
		navigationButtons: false,
		selectable: true,
		selectConnectedEdges: true,
		tooltipDelay: 300,
		zoomView: true
	}
};

// Manipulation tools
function clearPopUp() {
	document.getElementById('saveButton').onclick = null;
	document.getElementById('cancelButton').onclick = null;
	//document.getElementById('editor').style.display = 'none';
    $( '.ui-dialog' ).hide( "drop");
}

function cancelEdit(callback) {
	clearPopUp();
	callback(null);
}

function saveData(data,callback) {
	_characterManager.Save(data);
	data = updateNode(data);
	clearPopUp();
	callback(data);
}

// Manipulation tools
function EdgeClearPopUp() {
	document.getElementById('EdgeSaveButton').onclick = null;
	document.getElementById('EdgeCancelButton').onclick = null;
    $( '.ui-dialog' ).hide( "drop");
}

function EdgeCancelEdit(callback) {
	EdgeClearPopUp();
	callback(null);
}

function EdgeSaveData(data,callback) {
	_linkManager.Save(data);
	updateEdge(data);
	EdgeClearPopUp();
	if (data.from == data.to) {
		var r = confirm("Do you want to connect the node to itself?");
		if (r == true) {
			callback(data);
		}
	}else {
		callback(data);
	}
}

function updateNode(data) {
	var g = parseInt(data.gen);
	data.borderWidth = (14-g);
	switch(data.condition) {
		case 'dead':
			data.color = {border: '#000000', background: '#000000'};
			data.font = '12px arial red';
		break;
	}
	nodes.update(data);
}

function updateEdge(data) {
	var color = "#000000";
	var type = data.type;
	var width = 1;
	document.Settings.Edges.types.values.forEach(function(e)
	{
		if(type==e.value)
		{
			color = e.color;
			width = e.width;
		}
	});
	data.color = {color: color, hover: color};
	data.width = width;
	edges.update(data);
	return;
}

/*function redrawHierarchical() {
	network = new vis.Network(container, data, options);
	network.setOptions(
	{
		layout: {
			randomSeed: undefined,
			improvedLayout:true,
			hierarchical: {
				enabled:true,
				levelSeparation: 150,
				direction: 'UD',   // UD, DU, LR, RL
				sortMethod: 'directed' // hubsize, directed
			}
		}
	});
	network.redraw();
}*/
function redrawNormal() {
	network = new vis.Network(container, data, options);
	network.redraw();
}

/*network.on("click", function (params) {
	params.event = "[original event]";
});
network.on("doubleClick", function (params) {
	params.event = "[original event]";
});
network.on("oncontext", function (params) {
	params.event = "[original event]";
});
network.on("dragStart", function (params) {
	params.event = "[original event]";
});
network.on("dragging", function (params) {
	params.event = "[original event]";
});
network.on("dragEnd", function (params) {
	params.event = "[original event]";
});
network.on("zoom", function (params) {
});
network.on("showPopup", function (params) {
});
network.on("hidePopup", function () {
	console.log('hidePopup Event');
});
network.on("select", function (params) {
	console.log('select Event:', params);
});
network.on("selectNode", function (params) {
	console.log('selectNode Event:', params);
});
network.on("selectEdge", function (params) {
	console.log('selectEdge Event:', params);
});
network.on("deselectNode", function (params) {
	console.log('deselectNode Event:', params);
});
network.on("deselectEdge", function (params) {
	console.log('deselectEdge Event:', params);
});
network.on("hoverNode", function (params) {
	console.log('hoverNode Event:', params);
});
network.on("hoverEdge", function (params) {
	console.log('hoverEdge Event:', params);
});
network.on("blurNode", function (params) {
	console.log('blurNode Event:', params);
});
network.on("blurEdge", function (params) {
	console.log('blurEdge Event:', params);
});*/

/******************************
 * SAVE
 ******************************/

function save() {
	var	json = JSON.stringify(
		{
			nodes: nodes.get({fields: NODES_FIELDS}),
			edges: edges.get({fields: EDGES_FIELDS})
		}
	);
	var blob = new Blob([json], {type: "application/json"});
	var url  = URL.createObjectURL(blob);

	var a = document.createElement('a');
	a.download    = "network.json";
	a.href        = url;
	a.textContent = "Download backup.json";
	document.getElementById("body").appendChild(a);
	a.click();
	/*// update link to new 'url'
	link.download    = "network.json";
	link.href        = url;*/
	document.getElementById("body").removeChild(a);
}

/************************
 * LOAD
 ************************/
 
function load() {
	document.getElementById("load_submenu").style.display = 'block';
}
var reader;

function abortRead() {
	if(reader!=undefined)
		reader.abort();
	document.getElementById("load_submenu").style.display = 'none';
}

function updateProgress(evt) {
	// evt is an ProgressEvent.
	if (evt.lengthComputable) {
		var percentLoaded = Math.round((evt.loaded / evt.total) * 100);
		// Increase the progress bar length.
		if (percentLoaded < 100) {
			progress.style.width = percentLoaded + '%';
			progress.textContent = percentLoaded + '%';
		}else{
			document.getElementById("load_submenu").style.display = 'none';
		}
	}
}

function handleFileSelect(evt) {
	// Reset progress indicator on new file selection.
	progress.style.width = '0%';
	progress.textContent = '0%';

	reader = new FileReader();
	reader.onerror = FileReaderErrorHandler;
	reader.onprogress = updateProgress;
	reader.onabort = function(e) {
		alert('File read cancelled');
	};
	reader.onloadstart = function(e) {
		document.getElementById('progress_bar').className = 'loading';
	};
	reader.onload = function(e) {
		// Ensure that the progress bar displays 100% at the end.
		progress.style.width = '100%';
		progress.textContent = '100%';
		setTimeout("document.getElementById('progress_bar').className='';", 2000);
		var json = JSON.parse(reader.result);
		nodes.clear();
		edges.clear();
		nodes.add(json.nodes);
		edges.add(json.edges);
		document.getElementById("load_submenu").style.display = 'none';
		edges.forEach(updateEdge);
		nodes.forEach(updateNode);
	}
	reader.readAsText(evt.target.files[0]);
}

/*************************************
 * SELECT A NETWORK VIEW
 *************************************/
 
function backup() {
	_backup_nodes.clear();
	_backup_edges.clear();
	_backup_nodes.add(nodes.get({fields: NODES_FIELDS}));
	_backup_edges.add(edges.get({fields: EDGES_FIELDS}));
}

/**
 * Gives a node's network with some depth
 **/
function networkView(nodeId, depth) {
	depth = parseInt(depth);
	backup();
	/*var _nodes = network.getConnectedNodes(nodeID);
	_nodes.forEach(function(item, index) {
		_nodes[index] = {id:item, walked: false};
	});*/
	var _nodes = new Array({id: nodeId, walked: false});
	// for each level of research
	for(var l=0;l<depth;++l) {
		var _nodes_level = new Array();
		// we will look for all the nodes where we haven't looked yet
		for(var n=0;n<_nodes.length;++n) {
			if(!_nodes[n].walked) {
				_nodes[n].walked = true;
				var connected_nodes = network.getConnectedNodes(_nodes[n].id);
				connected_nodes.sort();
				/*for(var i=0;i<connected_nodes.length-1;++i) {
					if(connected_nodes[i]==connected_nodes[i+1]) {
						connected_nodes.remove(i+1);
						i--;
					}
				}*/
				//alert(connected_nodes);
				// once we get each node connections
				for(var i=0;i<connected_nodes.length;++i) {
					var isIn = false;
					// we will check we haven't this one yet
					for(var j=0;j<_nodes.length;++j) {
						//alert(connected_nodes[i]+'=='+_nodes[j].id+' -> '+(connected_nodes[i] == _nodes[j].id));
						if(connected_nodes[i] == _nodes[j].id) {
							isIn = true;
							break;
						}
					}
					// finally, if we haven't this one, we add it to the nodes pool
					// and we mark them to not looked ('walked')
					if(!isIn) {
						_nodes_level.push({id: connected_nodes[i], walked: false});
					}
				}
			}
		}
		//alert(JSON.stringify(_nodes)+' concat '+JSON.stringify(_nodes_level));
		// then the nodes pool is added to our nodes
		_nodes = _nodes.concat(_nodes_level);
		//alert(JSON.stringify(_nodes));
		// cleaning the clones once and for all
		_nodes.sort(function(a, b) { return a.id<b.id; });
		for(var i=0;i<_nodes.length-1;++i) {
			if(_nodes[i].id==_nodes[i+1].id) {
				_nodes.remove(i+1);
				i--;
			}
		}
	}
	_nodes.forEach(function(item, index) {
		_nodes[index] = item.id;
	});
	var N = nodes.get(_nodes, {fields: NODES_FIELDS});
	nodes.clear();
	nodes.add(N);
}

function networkNormal() {
	nodes.clear();
	nodes.add(_backup_nodes.get({fields: NODES_FIELDS}));
	edges.clear();
	edges.add(_backup_edges.get({fields: EDGES_FIELDS}));
	nodes.forEach(updateEdge);
	edges.forEach(updateEdge);
}

/*************************************
 * HIDE/SHOW GRAPH - FOR PLAYERS
 *************************************/

function showhide() {
	$('#mynetwork').toggle('fade');
	$('#button_showhide span').toggle('fade');
}

/*************************************
 * ABOUT ME
 *************************************/

function about_us() {
	$("#about_us").dialog({"title":"About us"}).on( "dialogclose", function( event, ui ) { $( '.ui-dialog' ).hide( "drop"); } );
}

/*************************************
 * NETWORK SETUP
 *************************************/
$(document).ready(function(){

	_characterManager.Build('charactersheet', 'charactersheet-presentation', document.Settings);
	_linkManager.Build('edge-form', document.Settings);
	// create a network
	var container = document.getElementById('mynetwork');
	// initialize your network!
	network = new vis.Network(container, data, options);
	// initialize elements
	progress = document.querySelector('.percent');
	
	$("#editor").dialog();
	$("#EdgeEditor").dialog();
	$('.ui-dialog').hide();

	document.getElementById('files').addEventListener('change', handleFileSelect, false);
	 
	network.on("selectNode", function(params) {
		var node = nodes.get(params.nodes[0], {fields: NODES_FIELDS});
		$("#nodeInfo").dialog().on( "dialogclose", function( event, ui ) { $( '.ui-dialog' ).hide( "drop"); } );
		_characterManager.Load(node, false, false, 'info');
		document.getElementById(document.Settings.Html_ids_prefix+'-info-connections').innerHTML = '';
		for(var i = 0; i< params.edges.length; ++i) {
			var vE = edges.get(''+params.edges[i]);
			document.getElementById(document.Settings.Html_ids_prefix+'-info-connections').innerHTML+=
			Link.Format(nodes, vE);//formatEdge(vE);
		}
	});
	/*network.on("click", function(params) {
		//document.getElementById('nodeInfo').style.display = 'none';
	});*/
	nodes.on("update", function (event, properties, senderId) {
		console.log('Update event:', event, properties);
	});
	edges.on("*", function(event, properties, senderId) {
		console.log('Edge event:', event, properties);
	});
});