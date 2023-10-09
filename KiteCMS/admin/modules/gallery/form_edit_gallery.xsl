<?xml version="1.0" encoding="ISO-8859-1"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:util="urn:localText" version="1.0" exclude-result-prefixes="util">
<xsl:output omit-xml-declaration="yes"/>

<xsl:param name="pageid" select="//selectedpage" />

<xsl:template match="/">
<head>
<link REL="stylesheet" TYPE="text/css" HREF="/admin/default.css"/>
</head>

<script type="text/javascript">
function funcsubmit(){
	var numberReg = /^[1-9][0-9]*$/;
	var numberProcentReg = /^([1-9][0-9]*|[1-9][0-9]?%|100%)$/;
//	var numberProcentReg = /^([1-9][0-9]*)|([1-9][0-9]?%)|(100%)$/;
	var colorReg = /^[0-9a-f]{6}$/;
	if(document.formedit.imagefolder.value == ""){
		alert('<xsl:value-of select="util:localText('imagefoldernotempty')"/>');
		document.formedit.imagefolder.focus();
		return false;
	}
	if(document.formedit.imagefolder.value.charAt(0)!="/" || !(document.formedit.imagefolder.value.indexOf(".")==-1)){
		alert("<xsl:value-of select="util:localText('urlnotok')"/>");
		document.formedit.imagefolder.focus();
		return false;
	}
	if(!numberReg.test(document.formedit.width.value)){
		alert('<xsl:value-of select="util:localText('notanumber')"/>');
		document.formedit.width.focus();
		return false;
	}
	if(!numberReg.test(document.formedit.height.value)){
		alert('<xsl:value-of select="util:localText('notanumber')"/>');
		document.formedit.height.focus();
		return false;
	}
	if(!numberReg.test(document.formedit.thumbwidth.value)){
		alert('<xsl:value-of select="util:localText('notanumber')"/>');
		document.formedit.thumbwidth.focus();
		return false;
	}
	if(!numberReg.test(document.formedit.thumbheight.value)){
		alert('<xsl:value-of select="util:localText('notanumber')"/>');
		document.formedit.thumbheight.focus();
		return false;
	}
document.formedit.submit();
}
</script>

<form action="default.aspx" method="post" name="formedit" onsubmit="return funcsubmit();">
<input type="hidden" name="action" value="doeditgallery"/>
<input type="hidden" name="pageid" value="{//selectedpage}"/>

<table border="0" width="450">
<tr>
	<td width="250"><xsl:value-of select="util:localText('imagefolder')"/>:</td>
	<td width="200"><input type="text" name="imagefolder" class="alminput" size="25" maxlength="250" value="{//gallery[@pageid=$pageid]/imagefolder}"/></td>
</tr>
<tr>
	<td width="250"><xsl:value-of select="util:localText('max')"/>&#160;<xsl:value-of select="util:localText('width')"/>:</td>
	<td width="200"><input type="text" name="width" class="alminput" size="10" maxlength="250" value="{//gallery[@pageid=$pageid]/width}"/></td>
</tr>
<tr>
	<td width="250"><xsl:value-of select="util:localText('max')"/>&#160;<xsl:value-of select="util:localText('height')"/>:</td>
	<td width="200"><input type="text" name="height" class="alminput" size="10" maxlength="250" value="{//gallery[@pageid=$pageid]/height}"/></td>
</tr>
<tr>
	<td width="250"><xsl:value-of select="util:localText('thumbwidth')"/>:</td>
	<td width="200"><input type="text" name="thumbwidth" class="alminput" size="10" maxlength="250" value="{//gallery[@pageid=$pageid]/thumbwidth}"/></td>
</tr>
<tr>
	<td width="250"><xsl:value-of select="util:localText('thumbheight')"/>:</td>
	<td width="200"><input type="text" name="thumbheight" class="alminput" size="10" maxlength="250" value="{//gallery[@pageid=$pageid]/thumbheight}"/></td>
</tr>
<tr>
	<td width="250"><xsl:value-of select="util:localText('textinlist')"/>:</td>
  <td width="200">
    <xsl:element name="input">
      <xsl:attribute name="type">checkbox</xsl:attribute>
      <xsl:attribute name="name">textinlist</xsl:attribute>
      <xsl:if test="//gallery[@pageid=$pageid]/textinlist='true'">
        <xsl:attribute name="checked">checked</xsl:attribute>
      </xsl:if>
    </xsl:element>
  </td>
</tr>
<tr>
	<td></td>
	<td>
	<input type="submit" class="almknap" value="{util:localText('save')}" onclick="return funcsubmit();return false;"/>&#160;
	<input type="button" class="almknap" value="{util:localText('cancel')}" onclick="document.location.href='/modules/default.aspx?pageid={//selectedpage}';return false;"/>&#160;
	</td>
</tr>
</table>



</form>
</xsl:template>

</xsl:stylesheet>