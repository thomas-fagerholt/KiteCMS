<?xml version="1.0" encoding="ISO-8859-1"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:util="urn:localText" version="1.0" exclude-result-prefixes="util">

<xsl:template match="/">
<head>
<link rel="stylesheet" type="text/css" href="/admin/default.css"/>
</head>
<table cellpadding="0" cellspacing="2" border="0">
<tr>
  <td class="adminLinkList">
    <h3><xsl:value-of select="util:localText('choosetemplate')"/></h3>
		<xsl:for-each select="//template">
		<xsl:sort select="title"/>	
			<xsl:element name="a">
			<xsl:attribute name="href">edittemplate.aspx?pageid=<xsl:value-of select="//selectedpage"/>&amp;action=edit&amp;templateid=<xsl:value-of select="@id"/></xsl:attribute>
			<xsl:value-of select="title"/>
			</xsl:element>
      <img src="/admin/images/edit.gif" width="16" height="16" alt="{util:localText('edit')}" title="{util:localText('edit')}" align="absmiddle"  border="0"/>
      <br/>
		</xsl:for-each>
		<br/>
	
		<form action="/default.aspx">
		<input type="button" value="{util:localText('cancel')}" class="almknap" onclick="history.go(-1)"/>
		</form>
	</td>
</tr>
</table>

</xsl:template>

</xsl:stylesheet>

