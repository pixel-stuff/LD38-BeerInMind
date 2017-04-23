function libgui_build_combobox(id, data)
{
	var buffer = "";
	buffer+="<select id=\""+id+"\" name=\""+id+"\">";
	for(var i=0;i<data.values.length;i++)
	{
		var selected = "";
		if (data.values[i].value==data.defaultvalue)
			selected = " selected=\"selected\"";
		buffer+="<option"+selected+" value=\""+data.values[i].value+"\">"+data.values[i].label+"</option>";
	}
	buffer+="</select>";
	return buffer;
}