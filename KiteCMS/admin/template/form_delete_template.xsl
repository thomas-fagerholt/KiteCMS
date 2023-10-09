<?xml version="1.0" encoding="ISO-8859-1"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:util="urn:localText" version="1.0" exclude-result-prefixes="util">
<xsl:output omit-xml-declaration="yes"/>

<xsl:param name="templateid" select="-1" />

<xsl:template match="/">

<xsl:variable name="moduleid">
<xsl:value-of select="//page[@id=//selectedpage]/usetemplate"/>
</xsl:variable>

<table cellpadding="0" cellspacing="2" border="0">
<tr>
	<td>
		<xsl:for-each select="//template[@id=$templateid]">	
		<xsl:sort select="title"/>
		<xsl:if test="count(preceding-sibling::template | following-sibling::template)=0">
			<xsl:value-of select="util:localText('onlytemplate')"/> 
			<xsl:element name="a">
			<xsl:attribute name="href"><xsl:value-of select="//template[@id=$moduleid]/publicurl"/>?pageid=<xsl:value-of select="//selectedpage"/></xsl:attribute>
			<xsl:value-of select="util:localText('adminpage')"/> 
			</xsl:element>
		</xsl:if>
		
		<xsl:if test="count(preceding-sibling::template | following-sibling::template)>0">
		<xsl:value-of select="util:localText('abouttodelete')"/> '<xsl:value-of select="//template[@id=$templateid]/title"/>'<br/>
		<xsl:value-of select="util:localText('usedonpages')"/>&#160;<xsl:value-of select="count(//page[usetemplate=$templateid])"/>&#160;<xsl:value-of select="util:localText('pages')"/><br/>
			<xsl:if test="count(//page[usetemplate=$templateid])>0">
			<xsl:value-of select="util:localText('templatetouse')"/><br/>
			<xsl:for-each select="//template">
			<xsl:sort select="title"/>
				<xsl:if test="not(@id=$templateid)">
				<xsl:element name="a">
					<xsl:attribute name="href">deletetemplate.aspx?pageid=<xsl:value-of select="//selectedpage"/>&amp;action=delete&amp;usetemplateid=<xsl:value-of select="@id"/>&amp;templateid=<xsl:value-of select="$templateid"/></xsl:attribute>
					<xsl:value-of select="title"/>
				</xsl:element>
				<br/>
				</xsl:if>
			</xsl:for-each>
			</xsl:if>
		
			<form action="/default.aspx">
			<xsl:if test="count(//page[usetemplate=$templateid])=0">
				<input type="button" value="{util:localText('deletetemplate')}" class="almknap" onclick="location.href='deletetemplate.aspx?pageid={//selectedpage}&amp;action=delete&amp;templateid={$templateid}'"/>
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
