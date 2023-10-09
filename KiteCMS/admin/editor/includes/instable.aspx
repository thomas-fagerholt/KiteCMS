<%@ Page CodeBehind="instable.aspx.cs" Language="c#" AutoEventWireup="false" Inherits="KiteCMS.Admin.instable" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN"  "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd">
<html>
<head>
<title>Insert table</title>
<meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1" />
<link rel="stylesheet" href="/admin/default.css" type="text/css"/>

<style type="text/css">
 body   {margin-left:10px; font-family:verdana; font-size:12px; background:#fff}
 button {width:70px}
 table  {font-family:verdana; font-size:12px}
 p      {text-align:center}
</style>

<script type="text/javascript">
<!--
function IsDigit()
{
  return ((event.keyCode >= 48) && (event.keyCode <= 57))
}
// -->
</script>


<script type="text/javascript">
<!--
var isIE = (navigator.appVersion.indexOf("MSIE") != -1);
function funcOk(){
	if (isIE){
		var arr = new Array();
		arr["NumRows"] = NumRows.value;
		arr["NumCols"] = NumCols.value;
		arr["cellpad"] = cellpad.value;
		arr["cellspace"] = cellspace.value;
		arr["tableborder"] = tableborder.value;
		arr["tablesummary"] = tablesummary.value;
		arr["tablecaption"] = tablecaption.value;
		arr["tablehtml"] = "";
		window.returnValue = arr;
		window.close();
	} else {
		opener.funcInsertTableSelected(document.getElementById("NumRows").value,document.getElementById("NumCols").value,document.getElementById("cellpad").value,document.getElementById("cellspace").value,document.getElementById("tableborder").value,'',document.getElementById("tablesummary").value,document.getElementById("tablecaption").value);
		window.close();
	}
}
function funcUserOk(){
	if (isIE){
		var arr = new Array();
		arr["tablehtml"] = arrTables[document.getElementById("usertable").value];
		window.returnValue = arr;
	} else {
		opener.funcInsertTableSelected('','','','','',arrTables[document.getElementById("usertable")[document.getElementById("usertable").selectedIndex].value],'','');
	}
	window.close();
}
// -->
</script>

</head>

<body>

<table cellspacing="10" cellpadding="0" border="0">
 <tr>
  <td><% Response.Write(KiteCMS.Functions.localText("numberrows")); %>:</td>
  <td><input id="NumRows" type="text" size="3" name="NumRows" class="alminput"/></td>
  <td><% Response.Write(KiteCMS.Functions.localText("numbercols")); %>:</td>
  <td><input id="NumCols" type="text" size="3" name="NumCols" class="alminput"/></td>
 </tr>
 <tr>
  <td colspan="4"><strong><% Response.Write(KiteCMS.Functions.localText("tableattributes")); %>:</strong></td>
 </tr>
 <tr>
  <td>&nbsp;Cellpadding:</td>
  <td><input type="text" size="3" name="cellpad" id="cellpad" class="alminput"/></td>
  <td>&nbsp;Border:</td>
  <td><input type="text" size="3" name="tableborder" id="tableborder" class="alminput"/></td>
 </tr>
 <tr>
  <td>&nbsp;Cellspacing:</td>
  <td colspan="3"><input type="text" size="3" name="cellspace" id="cellspace" class="alminput"/></td>
 </tr>
 <tr>
  <td colspan="4"><strong><% Response.Write(KiteCMS.Functions.localText("tabletexts")); %>:</strong></td>
 </tr>
 <tr>
  <td>&nbsp;<% Response.Write(KiteCMS.Functions.localText("tablesummary")); %>:</td>
  <td colspan="3"><input type="text" size="30" name="tablesummary" id="tablesummary" class="alminput"/></td>
 </tr>
 <tr>
  <td>&nbsp;<% Response.Write(KiteCMS.Functions.localText("tablecaption")); %>:</td>
  <td colspan="3"><input type="text" size="30" name="tablecaption" id="tablecaption" class="alminput"/></td>
 </tr>
</table>

<p>
<button class="almknap" onclick="funcOk();" type="submit"><% Response.Write(KiteCMS.Functions.localText("ok")); %></button>
<button class="almknap" onclick="window.close();"><% Response.Write(KiteCMS.Functions.localText("cancel")); %></button>
</p>
<div>
	<asp:Literal id="userdefinedTables" Runat="server"></asp:Literal>
</div>
</body>
</html>