function libgui_build_combobox(id, data)
{
	var buffer = "";
	buffer+="<select id=\""+id+"\" value=\""+data.defaultvalue+"\">";
	for(var i=0;i<data.values.length;i++)
	{
		var selected = "";
		if (data.values[i].value==data.defaultvalue)
			selected = " selected";
		buffer+="<option value=\""+data.values[i].value+"\""+selected+">"+data.values[i].label+"</option>";
	}
	buffer+="</select>";
	return buffer;
}