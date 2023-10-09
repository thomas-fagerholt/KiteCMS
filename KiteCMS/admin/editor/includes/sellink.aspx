<%@ Page %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/transitional.dtd">
<html>
	<head>
		<title>Select Link</title>
			<link rel="stylesheet" href="/admin/default.css" type="text/css"/>
		<style type="text/css">
		BODY {margin-left:10px; font-family:arial; font-size:12px; background:#fff}
		P,TD {margin-left:10px; font-family:arial; font-size:12px; background:#fff}
	</style>
		<script type="text/javascript">
<!--
var isIE = (navigator.appVersion.indexOf("MSIE") != -1);
function funcOkClick(){
	if (isIE){
		var arr = new Array();
		arr["href"] = strHref.value;
		arr["target"] = strTarget.value;
		arr["title"] = strTitle.value;
		window.returnValue = arr;
		window.close();
	} else {
		opener.funcLinkSelected(document.getElementById("strHref").value,document.getElementById("strTarget").value,document.getElementById("strTitle").value)
		window.close();
	}
}

function funcInternalOk(){
	if (document.getElementById("strPageid").value!='' && document.getElementById("strInternalHref").value=='')
		document.getElementById("strInternalHref").value=('/default.asp?pageid='+strPageid.value);
	if (isIE){
		var arr = new Array();
		arr["href"] = strInternalHref.value;
		arr["target"] = "";
		arr["title"] = strTitle.value;
		window.returnValue = arr;
		window.close();
	} else {
		opener.funcLinkSelected(document.getElementById("strInternalHref").value,"",document.getElementById("strTitleInternal").value)
		window.close();
	}
}

function funcAnchorOk(){
	if (isIE){
		var arr = new Array();
		arr["href"] = "#"+ strAnchor[strAnchor.selectedIndex].text;
		arr["target"] = "";
		window.returnValue = arr;
		window.close();
	} else {
		opener.funcLinkSelected("#"+ document.getElementById("strAnchor")[document.getElementById("strAnchor").selectedIndex].text,"","")
		window.close();
	}
 }

function funcChooseLink(){
	if (isIE){
			var arr = showModalDialog("/admin/editor/includes/chooseinternallink.aspx", "", "font-family:Verdana; font-size:10; dialogWidth:30em; dialogHeight:54em;status:no;help:no" );
			if (arr != null){
				document.getElementById('strPageid').value=arr;
				document.getElementById('strInternalHref').value='/default.aspx?pageid='+arr;
			}
	} else {
			winModalWindow = window.open("/admin/editor/includes/chooseinternallink.aspx","ModalChildChild","dependent=yes,width=250,height=400,scrollbars=yes")
			winModalWindow.focus();
	}
 }
 
function funcInternalLinkSelected(pageid){
	document.getElementById('strPageid').value=pageid;
	document.getElementById('strInternalHref').value='/default.aspx?pageid='+pageid;
}

function init(){
	var arrAnchors = new Array();
	if (isIE)
		arrAnchors = window.dialogArguments;
	else
		arrAnchors = opener.arrAnchors;

	if (isIE) {
	if (arrAnchors.length==0){
		document.getElementById("strAnchor").disabled = true;
		document.getElementById("anchorOk").disabled = true;
		}
	// dynamic fill anchor select
	for (var i = 0; i<arrAnchors.length;i++){
		if(arrAnchors[i]){
			var objNewOption = document.createElement('option');
			objNewOption.text = arrAnchors[i];
			objNewOption.value = arrAnchors[i];
			document.getElementById("strAnchor").options.add(objNewOption);
			}
		}
	} else {
		if (window.location.search.substring(1).indexOf('anchors')>=-1) {
			var arrAnchors = window.location.search.substring(1).replace('anchors=','').split(',');
			for (var i = 0; i<arrAnchors.length;i++){
			if(arrAnchors[i]!=''){
				var objNewOption = document.createElement('option');
				objNewOption.text = arrAnchors[i];
				objNewOption.value = arrAnchors[i];
				document.getElementById("strAnchor").options.add(objNewOption);
				}
			}
		} else {
			document.getElementById("strAnchor").disabled = true;
			document.getElementById("anchorOk").disabled = true;
		}
 		
	}
}
function funcChangeType(obj){
	for (var i = 0; i < obj.options.length; i++) 
		if(document.getElementById("strHref").value.indexOf(obj[i].value)==0)
			document.getElementById("strHref").value = obj[obj.selectedIndex].value + document.getElementById("strHref").value.substr(obj[i].value.length)
}
// -->
</script>
	</head>
	<body onload="init();">
		<br/>
		<table>
			<tr>
				<td colspan="2"><b><% Response.Write(KiteCMS.Functions.localText("externallink")); %></b></td>
			</tr>
			<tr>
				<td align="right"><% Response.Write(KiteCMS.Functions.localText("type")); %>:</td>
				<td>
					<select id="strType" onchange="funcChangeType(this);" class="alminput">
						<option value="http://" selected="selected">
						http:</option>
						<option value="https://">
						https:</option>
						<option value="mailto:">mailto:</option>
					</select>
				</td>
			</tr>
			<tr>
				<td align="right"><% Response.Write(KiteCMS.Functions.localText("link")); %>:</td>
				<td><input type="text" size="30" id="strHref" name="strHref" value="http://" class="alminput"/></td>
			</tr>
			<tr>
				<td align="right"><% Response.Write(KiteCMS.Functions.localText("titletag")); %>:</td>
				<td><input type="text" size="30" id="strTitle" name="strTitle" value=""/ class="alminput"></td>
			</tr>
			<tr>
				<td align="right"><% Response.Write(KiteCMS.Functions.localText("openin")); %>:</td>
				<td>
					<select id="strTarget" class="alminput">
						<option value="" selected="selected"><% Response.Write(KiteCMS.Functions.localText("samewindow")); %></option>
						<option value="_blank"><% Response.Write(KiteCMS.Functions.localText("newwindow")); %></option>
					</select>
				</td>
			</tr>
			<tr>
				<td></td>
				<td>
					<button onclick="funcOkClick();" type="button" class="almknap">
						<% Response.Write(KiteCMS.Functions.localText("ok")); %>
					</button>
					<button onclick="window.close();" type="button" class="almknap">
						<% Response.Write(KiteCMS.Functions.localText("cancel")); %>
					</button>
				</td>
			</tr>
			<tr>
				<td colspan="2"><hr/>
				</td>
			</tr>
			<tr>
				<td colspan="2"><br/>
					<b>
						<% Response.Write(KiteCMS.Functions.localText("internallink")); %>
					</b>
				</td>
			</tr>
			<tr>
				<td align="right">Pageid:</td>
				<td><input type="text" size="10" id="strPageid" name="strPageid" onblur="if(this.value!='')document.getElementById('strInternalHref').value='/default.aspx?pageid='+this.value;"
						onfocus="this.select();"/ class="alminput"> <a onclick="funcChooseLink();"><img src="/admin/editor/images/chooseLink.gif" width="22" height="23" alt="<% Response.Write(KiteCMS.Functions.localText("chooselink")); %>" border="0" align="top"/></a></td>
			</tr>
			<tr>
				<td align="right"><% Response.Write(KiteCMS.Functions.localText("link")); %>:</td>
				<td><input type="text" size="30" id="strInternalHref" class="alminput" onfocus="this.select();"/></td>
			</tr>
			<tr>
				<td align="right"><% Response.Write(KiteCMS.Functions.localText("titletag")); %>:</td>
				<td><input type="text" size="30" id="strTitleInternal" class="alminput" name="strTitleInternal" value=""/></td>
			</tr>
			<tr>
				<td></td>
				<td>
					<button onclick="funcInternalOk()" type="button" class="almknap">
						<% Response.Write(KiteCMS.Functions.localText("ok")); %>
					</button>
					<button onclick="window.close();" type="button" class="almknap">
						<% Response.Write(KiteCMS.Functions.localText("cancel")); %>
					</button>
				</td>
			</tr>
			<tr>
				<td colspan="2"><hr/>
				</td>
			</tr>
			<tr>
				<td colspan="2"><br/>
					<b>
						<% Response.Write(KiteCMS.Functions.localText("anchorlink")); %>
					</b>
				</td>
			</tr>
			<tr>
				<td align="right"><% Response.Write(KiteCMS.Functions.localText("anchor")); %>:</td>
				<td>
					<select name="strAnchor" id="strAnchor" class="alminput">
						<option value="">Vælg link</option>
					</select>
				</td>
			</tr>
			<tr>
				<td></td>
				<td>
					<button onclick="funcAnchorOk();" type="submit" id="anchorOk" class="almknap">
						<% Response.Write(KiteCMS.Functions.localText("ok")); %>
					</button>
					<button onclick="window.close();" type="button" class="almknap">
						<% Response.Write(KiteCMS.Functions.localText("cancel")); %>
					</button>
				</td>
			</tr>
		</table>
	</body>
</html>
