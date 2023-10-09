<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN"  "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd">
<html>
<head>
<title>Choose color</title>
<meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1" />
<link rel="stylesheet" href="/admin/default.css" type="text/css"/>

<style type="text/css">
 body   {margin-left:10px; font-family:Verdana; font-size:12px; background:#fff; margin-top:10px; text-align:center;}
 button {width:70px}
 p      {text-align:center}
 TABLE  {cursor:hand;font-size:17px;}
 img {border:none;}
</style>

<script type="text/javascript">

<!-- Begin
var isIE = (navigator.appVersion.indexOf("MSIE") != -1);

function funcSubmit(){
	if (isIE)
		window.returnValue = document.getElementById('rgb').value;
  	else
		opener.funcColorSelected(document.getElementById('rgb').value);
  window.close();
}

addary = new Array();           //red
addary[0] = new Array(0,1,0);   //red green
addary[1] = new Array(-1,0,0);  //green
addary[2] = new Array(0,0,1);   //green blue
addary[3] = new Array(0,-1,0);  //blue
addary[4] = new Array(1,0,0);   //red blue
addary[5] = new Array(0,0,-1);  //red
addary[6] = new Array(255,1,1);
clrary = new Array(360);
for(i = 0; i < 6; i++)
for(j = 0; j < 60; j++) {
clrary[60 * i + j] = new Array(3);
for(k = 0; k < 3; k++) {
clrary[60 * i + j][k] = addary[6][k];
addary[6][k] += (addary[i][k] * 4);
   }
}
function capture() {
layobj = document.getElementById("wheel");
layobj.onmousemove = moved;
layobj.onclick = clicked;
}
function moved(e) {

y = 4 * ((document.all)?event.offsetX:e.layerX);
x = 4 * ((document.all)?event.offsetY:e.layerY);
sx = x - 512;
sy = y - 512;
qx = (sx < 0)?0:1;
qy = (sy < 0)?0:1;
q = 2 * qy + qx;
quad = new Array(-180,360,180,0);
xa = Math.abs(sx);
ya = Math.abs(sy);
d = ya * 45 / xa;
if(ya > xa) d = 90 - (xa * 45 / ya);
deg = Math.floor(Math.abs(quad[q] - d));
n = 0;
sx = Math.abs(x - 512);
sy = Math.abs(y - 512);
r = Math.sqrt((sx * sx) + (sy * sy));
if(x == 512 & y == 512) {
c = "000000";
}
else {
for(i = 0; i < 3; i++) {
r2 = clrary[deg][i] * r / 256;
if(r > 256) r2 += Math.floor(r - 256);
if(r2 > 255) r2 = 255;
n = 256 * n + Math.floor(r2);
}
c = n.toString(16);
while(c.length < 6) c = "0" + c;
}
document.getElementById("sample").style.backgroundColor = "#" + c;
return false;
}

function clicked(e) {
	document.getElementById("form").style.backgroundColor = "#" + c;
	document.getElementById("rgb").value = "#" + c;
}
//  End -->
</script>

</head>

<body onload="capture()">

<div>
<table border="0" cellpadding="0" cellspacing="0">
<tr>
<td>
<img id="wheel" src="/admin/images/colorwheel.jpg" alt="" width="256" height="256"/>
</td>
</tr>
<tr>
<td>
	<div id="sample">&nbsp;<br/>&nbsp;</div>
</td>
</tr>
<tr>
	<td style="text-align:center;">
		<div id="form" style="padding:5px;">
			<input class="alminput" type="text" name="rgb" id="rgb" size="27" onblur="document.getElementById('form').style.backgroundColor = document.getElementById('rgb').value;"/>
		</div>
</td>
</tr>
<tr>
	<td align="center">
		<button class="almknap" onclick="funcSubmit();"><% Response.Write(KiteCMS.Functions.localText("ok")); %></button>
		<button class="almknap" onclick="window.close();"><% Response.Write(KiteCMS.Functions.localText("cancel")); %></button>
	</td>
</tr>
</table>
</div>
</body>
</html>