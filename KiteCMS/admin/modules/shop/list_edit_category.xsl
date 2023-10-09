<?xml version='1.0' encoding='ISO-8859-1'?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:util="urn:localText" version="1.0" exclude-result-prefixes="util">
<xsl:output omit-xml-declaration="yes"/>
<xsl:template match="/">

	<p><b><xsl:value-of select="util:localText('choosecategory')"/></b></p>
	<p>
		<xsl:for-each select="/shop/productstore/category">
			<a href="default.aspx?pageid={//selectedpage}&amp;action=formeditcategory&amp;categoryid={@id}"><xsl:value-of select="title"/></a><br/>
			<xsl:for-each select="category">
				- <a href="default.aspx?pageid={//selectedpage}&amp;action=formeditcategory&amp;categoryid={@id}"><xsl:value-of select="title"/></a><br/>
			</xsl:for-each>
		</xsl:for-each>
	</p>
	<form action="/default.aspx">
		<input type="hidden" name="pageid" value="{//selectedpage}"/>
		<input type="submit" value="{util:localText('cancel')}" class="almknap"/>
	</form>

</xsl:template>
</xsl:stylesheet>
