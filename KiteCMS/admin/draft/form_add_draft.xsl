<?xml version='1.0' encoding='ISO-8859-1'?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:util="urn:localText" version="1.0" exclude-result-prefixes="util">
<xsl:output omit-xml-declaration="yes"/>

<xsl:template match="/">
<head>
<link rel="stylesheet" type="text/css" href="/admin/default.css"/>
</head>

	<form method="post" action="adddraft.aspx">

	<input type="hidden" name="pageid" value="{//selectedpage}"/>
	<input type="hidden" name="action" value="add"/>

	<table cellpadding="0" cellspacing="2" border="0">
	<tr>
		<td><xsl:value-of select="util:localText('copycontent')"/></td>
		<td><input type="checkbox" name="copycontent" checked=""/></td>
	</tr>
	<tr>
		<td colspan="2"><input type="submit" class="almknap" value="{util:localText('add')}"/>
		&#160;
		<input type="button" value="{util:localText('cancel')}" class="almknap" onclick="history.go(-1)"/>
		</td>
	</tr>
	</table>
	</form>
</xsl:template>
</xsl:stylesheet>
