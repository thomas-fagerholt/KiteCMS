<?xml version="1.0" encoding="ISO-8859-1"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:util="urn:localText" version="1.0" exclude-result-prefixes="util">
<xsl:output omit-xml-declaration="yes"/>

<xsl:param name="boxcategoryid" select="-1" />

<xsl:template match="/">

<xsl:variable name="moduleid">
<xsl:value-of select="//page[@id=//selectedpage]/usetemplate"/>
</xsl:variable>

<table cellpadding="0" cellspacing="2" border="0">
<tr>
	<td>
		<xsl:for-each select="/website/boxmodule/boxcategory[@id=$boxcategoryid]">	
		<xsl:sort select="title"/>
		<xsl:if test="count(preceding-sibling::boxcategory | following-sibling::boxcategory)=0">
			<xsl:value-of select="util:localText('onlycategory')"/> 
			<xsl:element name="a">
			<xsl:attribute name="href"><xsl:value-of select="//template[@id=$moduleid]/publicurl"/>?pageid=<xsl:value-of select="//selectedpage"/></xsl:attribute>
			<xsl:value-of select="util:localText('adminpage')"/> 
			</xsl:element>
		</xsl:if>
		
		<xsl:if test="count(preceding-sibling::boxcategory | following-sibling::boxcategory)>0">
		<xsl:value-of select="util:localText('abouttodeletecategory')"/> '<xsl:value-of select="/website/boxmodule/boxcategory[@id=$boxcategoryid]/title"/>'<br/>
		<xsl:value-of select="util:localText('usedonpages')"/>&#160;<xsl:value-of select="count(/website/boxmodule/box[@boxcategoryid=$boxcategoryid])"/>&#160;<xsl:value-of select="util:localText('boxes')"/><br/>
			<xsl:if test="count(/website/boxmodule/box[@boxcategoryid=$boxcategoryid])>0">
			<xsl:value-of select="util:localText('categorytouse')"/><br/>
			<xsl:for-each select="/website/boxmodule/boxcategory">
			<xsl:sort select="title"/>
				<xsl:if test="not(@id=$boxcategoryid)">
				<xsl:element name="a">
					<xsl:attribute name="href">editboxcategory.aspx?pageid=<xsl:value-of select="//selectedpage"/>&amp;action=dodelete&amp;useboxcategoryid=<xsl:value-of select="@id"/>&amp;boxcategoryid=<xsl:value-of select="$boxcategoryid"/></xsl:attribute>
					<xsl:value-of select="title"/>
				</xsl:element>
				<br/>
				</xsl:if>
			</xsl:for-each>
			</xsl:if>
		
			<form action="/default.aspx">
			<xsl:if test="count(/website/boxmodule/box[@boxcategoryid=$boxcategoryid])=0">
				<input type="button" value="{util:localText('delete')}" class="almknap" onclick="location.href='editboxcategory.aspx?pageid={//selectedpage}&amp;action=dodelete&amp;boxcategoryid={$boxcategoryid}'"/>
				&#160;
			</xsl:if>
				<input type="button" value="{util:localText('cancel')}" class="almknap" onclick="history.go(-2)"/>
			</form>
		</xsl:if>
		</xsl:for-each>
	</td>
</tr>
</table>
	

</xsl:template>
</xsl:stylesheet>
