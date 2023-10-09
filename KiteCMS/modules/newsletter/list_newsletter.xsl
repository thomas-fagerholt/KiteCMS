<?xml version="1.0" encoding="ISO-8859-1"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:util="urn:localText" version="1.0" exclude-result-prefixes="util">
<xsl:output omit-xml-declaration="yes"/>

<xsl:param name="newsid" select="-1" />

<xsl:template match="/">
<xsl:variable name="selectedpage"><xsl:value-of select="//selectedpage"/></xsl:variable>

	<xsl:if test="$newsid=-1">
		<xsl:if test="count(/website/emailcategory[@pageid=$selectedpage]/newsitem)>0">
			<table cellpadding="0" cellspacing="2" border="0">
				<xsl:for-each select="/website/emailcategory[@pageid=$selectedpage]/newsitem">
				<xsl:sort select="@created" order="descending"/>
					<tr><td><b><a href="/default.aspx?pageid={//selectedpage}&amp;newsid={@id}"><xsl:value-of select="title"/></a></b>&#160;&#160;<xsl:value-of select="substring-before(@created,' ')"/></td></tr>
					<tr><td><xsl:value-of select="teaser"/></td></tr>
					<tr><td>&#160;</td></tr>
				</xsl:for-each>
			</table>
		</xsl:if>
	</xsl:if>
	
	<xsl:if test="$newsid!=-1">
		<table cellpadding="0" cellspacing="2" border="0">
		<xsl:for-each select="/website/emailcategory[@pageid=$selectedpage]/newsitem[@id=$newsid]">
		<xsl:sort select="@created" order="descending"/>
			<tr><td><b><xsl:value-of select="title"/></b>&#160;&#160;<xsl:value-of select="substring-before(@created,' ')"/></td></tr>
			<tr><td><xsl:value-of select="content" disable-output-escaping="yes"/></td></tr>
			<tr><td>&#160;</td></tr>
			<tr><td><a href="/default.aspx?pageid={//selectedpage}">Go back</a></td></tr>
		</xsl:for-each>
		</table>
	</xsl:if>
</xsl:template>

</xsl:stylesheet>
