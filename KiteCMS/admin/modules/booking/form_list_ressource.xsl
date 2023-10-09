<?xml version="1.0" encoding="ISO-8859-1"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:util="urn:localText" version="1.0" exclude-result-prefixes="util">
<xsl:output omit-xml-declaration="yes"/>

<xsl:template match="/">
<xsl:variable name="selectedpage"><xsl:value-of select="//selectedpage"/></xsl:variable>

<head>
<link rel="stylesheet" type="text/css" href="/admin/default.css"/>
</head>

		<xsl:for-each select="//group[item/@pageid=$selectedpage]">
		<xsl:sort select="@name"/>
			<br/><b><xsl:value-of select="@name"/></b>&#160;<a href="default.aspx?pageid={//selectedpage}&amp;action=formaddresource&amp;item={@id}">[<xsl:value-of select="util:localText('addresourcelink')"/>]</a><br/>
			<xsl:for-each select="item[@pageid=$selectedpage]">
			<xsl:sort select="@name"/>

			<span class="KiteCMSDefaultText">
			<xsl:element name="a">
			<xsl:attribute name="href">javascript:void(0);</xsl:attribute>
			<xsl:attribute name="onclick">if(confirm("<xsl:value-of select="util:localText('confirmdeleteresource')"/>")) {location.href='default.aspx?pageid=<xsl:value-of select="//selectedpage"/>&amp;action=dodeleteresource&amp;item=<xsl:value-of select="@id"/>';}</xsl:attribute>
			[<xsl:value-of select="util:localText('delete')"/>]
			</xsl:element>
			</span>	

			<xsl:value-of select="@name"/><br/>
			</xsl:for-each>
		</xsl:for-each>

		<br/><a href="default.aspx?pageid={//selectedpage}&amp;action=addgroup"><xsl:value-of select="util:localText('addgroup')"/></a>
		<br/><br/>
		<input type="button" value="{util:localText('cancel')}" class="almknap" onclick="document.location.href='/modules/default.aspx?pageid={//selectedpage}';"/>
</xsl:template>

</xsl:stylesheet>