<%@ Page CodeBehind="chooseinternallink.aspx.cs" Language="c#" AutoEventWireup="false" Inherits="KiteCMS.Admin.chooseinternallink" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN"  "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd">
<html>
<head>
<title>Choose internal link</title>
<meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1" />

<link rel="stylesheet" type="text/css" href="/admin/default.css"/>

<script type="text/javascript">
var isIE = (navigator.appVersion.indexOf("MSIE") != -1);
function funcLinkChosen(arr){
	if (isIE){
	  window.returnValue = arr;
	  window.close();
	} else {
		opener.funcInternalLinkSelected(arr)
		window.close();
	}
}

function funcMediaCancel(){
	if (isIE)
		window.returnValue = '';
	parent.window.close();
}
</script>
</head>

<body>
<div style="padding:10px">
	<asp:Literal id="content" Runat="server"></asp:Literal>
</div>
</body>
</html>
