<?xml version='1.0' encoding='ISO-8859-1'?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:util="urn:localText" version="1.0" exclude-result-prefixes="util">
<xsl:output omit-xml-declaration="yes"/>
<xsl:template match="/">

	<p><b><xsl:value-of select="util:localText('chooseshopitem')"/></b></p>
	<p>
		<xsl:for-each select="/shop/productstore/category">
			<b><xsl:value-of select="title"/></b><br/>
			
			<xsl:variable name="categoryid"><xsl:value-of select="@id"/></xsl:variable>
			<xsl:for-each select="//shopitem[item[@id='category' and .=$categoryid]]">
				<a href="default.aspx?pageid={//selectedpage}&amp;action=formeditshopitem&amp;shopitemid={@id}"><xsl:value-of select="item[@id='title']"/></a><br/>
			</xsl:for-each>

			<xsl:for-each select="category">
				-<b><xsl:value-of select="title"/></b><br/>
				
				<xsl:variable name="subcategoryid"><xsl:value-of select="@id"/></xsl:variable>
				<xsl:for-each select="//shopitem[item[@id='category' and .=$subcategoryid]]">
					<a href="default.aspx?pageid={//selectedpage}&amp;action=formeditshopitem&amp;shopitemid={@id}"><xsl:value-of select="item[@id='title']"/></a><br/>
				</xsl:for-each>
		</xsl:for-each>
		</xsl:for-each>
	</p>
	<form action="/default.aspx">
		<input type="hidden" name="pageid" value="{//selectedpage}"/>
		<input type="submit" value="{util:localText('cancel')}" class="almknap"/>
	</form>

</xsl:template>
</xsl:stylesheet>
