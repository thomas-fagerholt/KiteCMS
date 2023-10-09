<%@ Page %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN"  "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd">
<html>
<head>
<title>Abbreviation tag</title>
<meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1" />
<link rel="stylesheet" href="/admin/default.css" type="text/css"/>

<style type="text/css">
 body   {margin-left:10px; font-family:Verdana; font-size:12px; background:#fff}
 p      {text-align:center}
</style>


<script type="text/javascript">
<!--
var isIE = (navigator.appVersion.indexOf("MSIE") != -1);
function funcOk(){
  if (document.getElementById('type_abbr').checked)
    seltype = "abbr";
  else
    seltype = "acronym";
	if (isIE){
    var arr = new Array();
    arr["value"] = document.getElementById("abbr").value;
    arr["type"] = seltype;
		window.returnValue = arr;
		window.close();
	} else {
		opener.funcAbbrSelected(document.getElementById("abbr").value, seltype);
		window.close();
	}
}
// -->
</script>

</head>

<body>
<p>
<% Response.Write(KiteCMS.Functions.localText("writeabbr")); %>:<br/>
<input type="text" size="40" id="abbr"/>
<br/>
<input type="radio" name="type" id="type_abbr" class="alminput" value="abbr" <% if (Request.Browser.Browser == "IE" && Request.Browser.MajorVersion < 7) {Response.Write("disabled='true'");}%>><label for="type_abbr"><% Response.Write(KiteCMS.Functions.localText("abbr")); %></label>
<input type="radio" name="type" id="type_acronym" class="alminput" value="acronym" checked="true"><label for="type_acronym"><% Response.Write(KiteCMS.Functions.localText("acronym")); %></label>
<br/>

<button onclick="funcOk();" class="almknap" type="submit"><% Response.Write(KiteCMS.Functions.localText("ok")); %></button>
<button onclick="window.close();" class="almknap"><% Response.Write(KiteCMS.Functions.localText("cancel")); %></button>
</p>
</body>
</html>