<?xml version="1.0" encoding="ISO-8859-1"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:util="urn:localText" version="1.0" exclude-result-prefixes="util">
<xsl:output omit-xml-declaration="yes"/>

<xsl:variable name="selectedcategory"><xsl:value-of select="//selectedcategory"/></xsl:variable>
<xsl:variable name="selectedperson"><xsl:value-of select="//selectedperson"/></xsl:variable>
<xsl:variable name="selectedpage"><xsl:value-of select="//selectedpage"/></xsl:variable>

<xsl:template match="/">

<table cellpadding="2" cellspacing="0" border="1">

<tr>
	<xsl:for-each select="//category[@id=$selectedcategory]/field">
		<td style="background-color:#ccc;"><xsl:value-of select="@label"/></td>
	</xsl:for-each>
</tr>
<xsl:for-each select="//category[@id=$selectedcategory]/person">
	<xsl:variable name="personid"><xsl:value-of select="@id"/></xsl:variable>
	<tr>
		<xsl:for-each select="//category[@id=$selectedcategory]/field">
			<xsl:variable name="fieldid"><xsl:value-of select="@id"/></xsl:variable>
			<td><xsl:value-of select="//person[@id=$personid]/item[@id=$fieldid]"/></td>
		</xsl:for-each>
	</tr>
</xsl:for-each>
</table>
</xsl:template>
</xsl:stylesheet>