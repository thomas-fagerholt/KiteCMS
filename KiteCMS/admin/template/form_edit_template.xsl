<?xml version="1.0" encoding="ISO-8859-1"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:util="urn:localText" version="1.0" exclude-result-prefixes="util">

<xsl:output omit-xml-declaration="yes"/>

<xsl:param name="templateid" select="-1" />

<xsl:template match="/">

	<script type="text/javascript">
	var isIE = (navigator.appVersion.indexOf("MSIE") != -1);
	var moduleReg = /^[\w-_.]+\@[\w-_ ]+\.[\w-_ .]+$/;
	function funcSubmit(){
	if(document.editTemplate.title.value==""){
		alert("<xsl:value-of select="util:localText('titlenotempty')"/>");
		document.editTemplate.title.focus();
		return false;
	}
	if(document.editTemplate.publicurl.value==""){
		if(confirm("<xsl:value-of select="util:localText('publicurlnotempty')"/>"))
			document.editTemplate.publicurl.value="\/default.aspx";
		document.editTemplate.publicurl.focus();
		return false;
	}
	if(document.editTemplate.publicurl.value.indexOf('\\')!=-1){
		if(confirm("<xsl:value-of select="util:localText('urlnotbackslash')"/>"))
		document.editTemplate.publicurl.focus();
		return false;
	}
	if(document.editTemplate.xslurl.value==""){
		if(confirm("<xsl:value-of select="util:localText('publicxslurlnotempty')"/>"))
			document.editTemplate.xslurl.value="website.xsl";
		document.editTemplate.xslurl.focus();
		return false;
	}
	if(document.editTemplate.adminurl.value==""){
		if(confirm("<xsl:value-of select="util:localText('adminurlnotempty')"/>"))
		document.editTemplate.adminurl.value="\/admin\/default.aspx";
		document.editTemplate.adminurl.focus();
		return false;
	}
	if(document.editTemplate.usercontrol.value!="")
    if(document.editTemplate.moduleclasspublic.value!=""){
      alert("<xsl:value-of select="util:localText('notusercontrolandmoduleclass')"/>");
      document.editTemplate.usercontrol.focus();
      return false;
    }
  if(document.editTemplate.adminurl.value.indexOf('\\')!=-1){
    if(confirm("<xsl:value-of select="util:localText('urlnotbackslash')"/>"))
		document.editTemplate.adminurl.focus();
		return false;
	}
	if(document.editTemplate.moduleclasspublic.value!="")
		if (!moduleReg.test(document.editTemplate.moduleclasspublic.value)){
			alert("<xsl:value-of select="util:localText('moduleclassnotvalid')"/>");
			document.editTemplate.moduleclasspublic.focus();
			return false;
	}
	return true;
	}

	function funcColor(){
		if (isIE){
		  var arr = showModalDialog( "/admin/editor/includes/selcolor.aspx", "", "font-family:Verdana; font-size:12; dialogWidth:300px; dialogHeight:400px" );
		
		  if (arr != null) {
		    document.editTemplate.templatecolor.value=arr;
		  }
		} else {
			winModalWindow = window.open ("/admin/editor/includes/selcolor.aspx","ModalChild","dependent=yes,width=300,height=400")
			winModalWindow.focus()
		}		
	}

	function funcColorSelected(color){
	    document.editTemplate.templatecolor.value=color;
	}

	</script>
	<xsl:variable name="formaction">
		<xsl:if test="$templateid=-1">addtemplate.aspx</xsl:if>
		<xsl:if test="$templateid!=-1">edittemplate.aspx</xsl:if>
	</xsl:variable>

	<form method="post" name="editTemplate" action="{$formaction}" onsubmit="return funcSubmit();">

	<input type="hidden" name="pageid" value="{//selectedpage}"/>
	<input type="hidden" name="templateid" value="{$templateid}"/>
	<input type="hidden" name="action" value="save"/>

<table cellpadding="0" cellspacing="2" border="0">
	<tr>
		<td><xsl:value-of select="util:localText('templatetitle')"/></td>
		<td><input type="text" class="alminput" size="30" name="title" value="{//template[@id=$templateid]/title}"/></td>
	</tr>
	<tr>
		<td><xsl:value-of select="util:localText('publicurl')"/></td>
		<td><input type="text" class="alminput" size="30" name="publicurl" value="{//template[@id=$templateid]/publicurl}"/></td>
	</tr>
	<tr>
		<td><xsl:value-of select="util:localText('publicxslurl')"/></td>
		<td><input type="text" class="alminput" size="30" name="xslurl" value="{//template[@id=$templateid]/xslurl}"/></td>
	</tr>
	<tr>
		<td><xsl:value-of select="util:localText('adminurl')"/></td>
		<td><input type="text" class="alminput" size="30" name="adminurl" value="{//template[@id=$templateid]/adminurl}"/></td>
	</tr>
	<tr>
		<td><xsl:value-of select="util:localText('moduleclasspublic')"/></td>
		<td><input type="text" class="alminput" size="30" name="moduleclasspublic" value="{//template[@id=$templateid]/moduleclasspublic}"/></td>
	</tr>
	<tr>
		<td><xsl:value-of select="util:localText('moduleclassadmin')"/></td>
		<td><input type="text" class="alminput" size="30" name="moduleclassadmin" value="{//template[@id=$templateid]/moduleclassadmin}"/></td>
	</tr>
	<tr>
		<td><xsl:value-of select="util:localText('usercontrol')"/></td>
		<td><input type="text" class="alminput" size="30" name="usercontrol" value="{//template[@id=$templateid]/usercontrol}"/></td>
	</tr>
	<tr>
		<td><xsl:value-of select="util:localText('templatecolor')"/></td>
		<td><input type="text" class="alminput" size="20" style="background-color:{//template[@id=$templateid]/templatecolor}" name="templatecolor" value="{//template[@id=$templateid]/templatecolor}"/>&#160;<a href="javascript:funcColor();" title="{util:localText('choosecolor')}"><img src="/admin/Images/fgcolor.GIF" border="0" alt="{util:localText('choosecolor')}" title="{util:localText('choosecolor')}" align="absmiddle"/></a></td>
	</tr>
	<tr>
		<td></td><td><input type="submit" class="almknap" value="{util:localText('save')}"/>
		&#160;
		<input type="button" value="{util:localText('cancel')}" class="almknap" onclick="location.href='{//template[@id=//page[@id=//selectedpage]/usetemplate]/publicurl}?pageid={//selectedpage}'"/>
		</td>
	</tr>
	</table>
	</form>
</xsl:template>
</xsl:stylesheet>
