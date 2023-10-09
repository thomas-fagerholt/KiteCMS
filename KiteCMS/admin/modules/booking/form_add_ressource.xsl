<?xml version="1.0" encoding="ISO-8859-1"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:util="urn:localText" version="1.0" exclude-result-prefixes="util">
<xsl:output omit-xml-declaration="yes"/>

<xsl:template match="/">
<head>
<link rel="stylesheet" type="text/css" href="/admin/default.css"/>
</head>

<script type="text/javascript">
function funcsubmit(){
	if(document.ressource.title.value==''){
		alert('<xsl:value-of select="util:localText('titlenotempty')"/>');
		document.ressource.title.focus();
		return false;
	}
	document.ressource.submit();
}
</script>

<form action="default.aspx" method="post" name="ressource" onsubmit="return funcsubmit();">
<input type="hidden" name="pageid" value="{//selectedpage}"/>
<input type="hidden" name="item" value="{//selecteditem}"/>
<input type="hidden" name="action" value="doaddresource"/>

<table border="0" width="450">
<tr>
	<td colspan="2"><xsl:value-of select="util:localText('addresourcetogroup')"/> <b><xsl:value-of select="//group[@id=//selecteditem]/@name"/></b></td>
</tr>
<tr>
	<td><xsl:value-of select="util:localText('title')"/>:</td>
	<td>
	<input type="text" name="title" class="alminput"/>
	</td>
</tr>
<tr>
	<td></td>
	<td>
	<input type="button" class="almknap" value="{util:localText('save')}" onclick="return funcsubmit();return false;"/>&#160;
	<input type="button" class="almknap" value="{util:localText('cancel')}" onclick="history.go(-1);return false;"/>&#160;
	</td>
</tr>
</table>
</form>

</xsl:template>

</xsl:stylesheet>