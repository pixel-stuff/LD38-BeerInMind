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

var CharacterSheet =
{
	Settings: 0,
	NextNodeId: 0,
	CheckValue: function(value)
	{
		return value==undefined?'':value;
	},
	Build: function(formID, presentationID, data)
	{
		this.Settings = data;
		var formu="<table style=\"margin:auto;\">";
		formu+="<form>";
		formu+="<tbody>";
		formu+="<tr><td>id</td><td><input type=\"text\" name=\""+this.Settings.Html_ids_prefix+"-id\" id=\""+this.Settings.Html_ids_prefix+"-id\" value=\"new value\"/></td></tr>";
		formu+="<tr><td>label</td><td><input type=\"text\" name=\""+this.Settings.Html_ids_prefix+"-label\" id=\""+this.Settings.Html_ids_prefix+"-label\" value=\"new value\"/></td></tr>";
		formu+="<tr><td>title</td><td><input type=\"text\" name=\""+this.Settings.Html_ids_prefix+"-title\" id=\""+this.Settings.Html_ids_prefix+"-title\" value=\"new value\"/></td></tr>";
		var that = this;
		this.Settings.CharacterSheet.forEach(function(e)
		{
			formu+="<tr>";
			switch(e.type)
			{
				case "text":
					formu+="<td>"+e.label+"</td><td><input type=\"text\" id=\""+that.Settings.Html_ids_prefix+"-"+e.variable+"\" name=\""+that.Settings.Html_ids_prefix+"-"+e.variable+"\" value=\""+e.defaultvalue+"\"/></td>";
				break;
				case "combobox":
					formu+="<td>"+e.label+"</td>";
					formu+="<td>"+libgui_build_combobox(that.Settings.Html_ids_prefix+"-"+e.variable, e)+"</td>";
				break;
				case "textarea":
					formu+="<td>"+e.label+"</td><td><textarea id=\""+that.Settings.Html_ids_prefix+"-"+e.variable+"\">"+e.defaultvalue+"</textarea></td>";
				break;
			}
			formu+="</tr>";
		});
		formu+="</tbody>";
		formu+="</form>";
		formu+="</table>";
		document.getElementById(formID).innerHTML = formu;
		document.getElementById(presentationID).innerHTML = this.Settings.CharactersheetPresentation.replaceAll('{Html_ids_prefix}', this.Settings.Html_ids_prefix);
	},
	New: function(data)
	{
		data.id = ++this.NextNodeId;
	},
	Save: function(data)
	{
		data.id = document.getElementById(this.Settings.Html_ids_prefix+'-id').value;
		data.label = document.getElementById(this.Settings.Html_ids_prefix+'-label').value;
		data.title = document.getElementById(this.Settings.Html_ids_prefix+'-title').value;
		this.Settings.CharacterSheet.forEach(function(e)
		{
			if(document.getElementById('character-'+e.variable))
			{
				data[e.variable] = document.getElementById('character-'+e.variable).value;
			}
			else
			{
				alert("ERROR: Could not get 'character-"+e.variable+"' element id");
			}
		});
	},
	SetData: function(id, value, isForm)
	{
		var e = document.getElementById(id);
		if(isForm)
			e.value = value;
		else
			e.innerHTML = value;
	},
	Load: function(data, setDefault, isForm, midfix='')
	{
		if(setDefault)
		{
			data.title = "";
			this.Settings.CharacterSheet.forEach(function(e)
			{
				data[e.variable] = e.defaultvalue;
			});
		}
		if(midfix.length>0)
			midfix = "-"+midfix;
		var prefix = this.Settings.Html_ids_prefix+midfix;
		if(document.getElementById(prefix+'-id'))
			CharacterSheet.SetData(prefix+'-id', data.id, isForm);
		if(document.getElementById(prefix+'-label'))
			CharacterSheet.SetData(prefix+'-label', data.label, isForm);
		if(document.getElementById(prefix+'-title'))
			CharacterSheet.SetData(prefix+'-title', data.label, isForm);
		this.Settings.CharacterSheet.forEach(function(e)
		{
			//alert(prefix+'-'+e.variable+' '+data[e.variable]+' '+document.getElementById(prefix+'-'+e.variable));
			if(document.getElementById(prefix+'-'+e.variable))
				CharacterSheet.SetData(prefix+'-'+e.variable,  data[e.variable], isForm);
		});
	}
}