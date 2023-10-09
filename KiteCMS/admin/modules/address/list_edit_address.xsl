<?xml version="1.0" encoding="ISO-8859-1"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:util="urn:localText" version="1.0" exclude-result-prefixes="util">
<xsl:output omit-xml-declaration="yes"/>

<xsl:variable name="selectedcategory"><xsl:value-of select="//selectedcategory"/></xsl:variable>
<xsl:variable name="selectedperson"><xsl:value-of select="//selectedperson"/></xsl:variable>
<xsl:variable name="selectedpage"><xsl:value-of select="//selectedpage"/></xsl:variable>

<xsl:template match="/">

<xsl:variable name="listitem"><xsl:value-of select="//category[@id=$selectedcategory]/field[@listitem='true']/@id"/></xsl:variable>
	<table cellpadding="2" cellspacing="0" border="0">
	<xsl:for-each select="//category[@id=$selectedcategory]/person">
		<tr><td><a href="default.aspx?pageid={$selectedpage}&amp;id={@id}&amp;action=formeditaddress"><xsl:value-of select="item[@id=$listitem]"/></a></td></tr>
	</xsl:for-each>
	<tr><td><input type="button" value="{util:localText('cancel')}" class="almknap" onclick="history.go(-1);"/></td></tr>
	</table>
</xsl:template>
</xsl:stylesheet>