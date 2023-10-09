<%@ Page %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN"  "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd">
<html>
<head>
<title>Choose internal link</title>
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
	if (isIE){
		window.returnValue = SelAnchor.value;
		window.close();
	} else {
		opener.funcAnchorSelected(document.getElementById("SelAnchor").value);
		window.close();
	}
}
// -->
</script>

</head>

<body>
<p>
<% Response.Write(KiteCMS.Functions.localText("writeanchor")); %>:<br/>
<input type="text" size="20" id="SelAnchor" class="alminput"/>
<br/>

<button onclick="funcOk();" type="submit" class="almknap"><% Response.Write(KiteCMS.Functions.localText("ok")); %></button>
<button onclick="window.close();" class="almknap"><% Response.Write(KiteCMS.Functions.localText("cancel")); %></button>
</p>
</body>
</html>