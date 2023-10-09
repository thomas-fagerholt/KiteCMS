<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN"  "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd">
<html>
<head>
<title>Paste From Word</title>
<meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1" />
<link rel="stylesheet" href="/admin/default.css" type="text/css"/>

<script type="text/javascript">

var isIE = (navigator.appVersion.indexOf("MSIE") != -1);

function funcSubmit(){
//	if (isIE)
//		window.returnValue = document.getElementById('rgb').value;
//  	else
		opener.pastetext(document.getElementById('text').value);
  window.close();
}
</script>
</head>

<body>

<div>
<% Response.Write(KiteCMS.Functions.localText("pastetexthere")); %>:<br />
<textarea class="alminput" type="text" name="text" id="text" cols="40" rows="20"></textarea><br />
<button class="almknap" onclick="funcSubmit();"><% Response.Write(KiteCMS.Functions.localText("ok")); %></button>
<button class="almknap" onclick="window.close();"><% Response.Write(KiteCMS.Functions.localText("cancel")); %></button>

</div>
</body>
</html>