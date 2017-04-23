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
String.prototype.replaceAll = function(search, replacement) {
    var target = this;
    return target.replace(new RegExp(search, 'g'), replacement);
};

var LinkSheet =
{
	Settings: 0,
	CheckValue: function(value)
	{
		return value==undefined?'':value;
	},
	Build: function(formID, data)
	{
		this.Settings = data;
		formu="";
		formu+=libgui_build_combobox("edge-type", document.Settings.Edges.types);
		formu+=libgui_build_combobox("edge-arrows", document.Settings.Edges.arrows);
		formu+="<br/><input id=\"edge-label\" value=\"new value\">";
		
		document.getElementById(formID).innerHTML = formu;
	},
	Save: function(data)
	{
		data.type = document.getElementById('edge-type').value;
		data.arrows = document.getElementById('edge-arrows').value;
		// strange, data provided by editEdge seems to not give label but gives custom value type
		data.label = document.getElementById('edge-label').value;
	},
	Load: function(data, setDefault)
	{
		if(setDefault)
		{
			data.type = this.Settings.Edges.types.defaultvalue;
			data.arrows = this.Settings.Edges.arrows.defaultvalue;
		}
		document.getElementById('edge-type').value = checkUndefined(data.type);
		document.getElementById('edge-arrows').value = checkUndefined(data.arrows);
		// strange, data provided by editEdge seems to not give label but gives custom value type
		document.getElementById('edge-label').value = checkUndefined(data.label);
	}
}

var Link = {};
Link.Format = function(nodes, edge)
{
	var from = nodes.get(edge.from);
	var to = nodes.get(edge.to);
	var type = '';
	var color = '';
	
	var viewtype = '<span style="color: #000000;">Known/Neutral</span>';
	var type = edge.type;
	document.Settings.Edges.types.values.forEach(function(e)
	{
		if(type==e.value)
		{
			viewtype = '<span style="'+e.style+'">'+e.label+'</span>';
		}
	});
	var leftarrow = ' -- ';
	var rightarrow = ' -- ';
	
	if (edge.arrows=='middle'){
	leftarrow = '--\>';
	rightarrow = ' \>-- ';
	}
	if (edge.arrows=='to'){
	leftarrow = ' -- ';
	rightarrow = ' --\> ';
	}
	if (edge.arrows=='from'){
	leftarrow = ' \<-- ';
	rightarrow = ' -- ';
	}
	if (edge.arrows=='to;from'){
	leftarrow = ' \<-- ';
	rightarrow = ' --\> ';
	}
	
	return '<hr/>'+from.label+leftarrow+viewtype +rightarrow+to.label+' ';
}
