<?xml version='1.0' encoding='ISO-8859-1'?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:util="urn:localText" version="1.0" exclude-result-prefixes="util">
<xsl:output omit-xml-declaration="yes"/>

<xsl:template match="/">

<xsl:variable name="selectedpage"><xsl:value-of select="//selectedpage"/></xsl:variable>

<table border="0" width="450">
<tr><td><b><xsl:value-of select="util:localText('boxesonpage')"/>:</b></td></tr>
<xsl:if test="count(//page[@id=$selectedpage])=0">
	<tr><td>[<xsl:value-of select="util:localText('empty')"/>]</td></tr>
</xsl:if>

<!--<xsl:for-each select="/website/boxmodule/box[@id=//page[@id=$selectedpage]/boxmodule/@boxid]">-->
<xsl:for-each select="//page[@id=$selectedpage]/boxmodule">
	<xsl:variable name="boxid"><xsl:value-of select="@boxid"/></xsl:variable>
	<xsl:variable name="position"><xsl:value-of select="position()"/></xsl:variable>
	<xsl:variable name="last"><xsl:value-of select="last()"/></xsl:variable>

	<xsl:for-each select="/website/boxmodule/box[@id=$boxid]">
	<xsl:variable name="boxcategoryid"><xsl:value-of select="@boxcategoryid"/></xsl:variable>
	<tr>
		<td class="KiteCMSDefaultText">
			<a href="attachbox.aspx?action=delete&amp;boxid={@id}&amp;pageid={$selectedpage}">
        <img src="/admin/images/delete.gif" width="16" height="16" alt="{util:localText('remove')}" title="{util:localText('remove')}" align="absmiddle" border="0"/>
      </a>&#160;<xsl:value-of select="title"/>
			(<xsl:value-of select="/website/boxmodule/boxcategory[@id=$boxcategoryid]/title"/>)
				<xsl:if test="$position!=1"><a href="attachbox.aspx?action=moveup&amp;boxid={@id}&amp;pageid={$selectedpage}"><img src="/admin/images/up.gif" alt="{util:localText('moveup')}" title="{util:localText('moveup')}" border="0"/></a></xsl:if>
				<xsl:if test="$position!=$last"><a href="attachbox.aspx?action=movedown&amp;boxid={@id}&amp;pageid={$selectedpage}"><img src="/admin/images/down.gif" alt="{util:localText('movedown')}" title="{util:localText('movedown')}" border="0"/></a></xsl:if>
		</td>
	</tr>
	</xsl:for-each>
</xsl:for-each>

<tr><td><br/><b><xsl:value-of select="util:localText('boxesnotonpage')"/>:</b></td></tr>

<xsl:for-each select="/website/boxmodule/box[not(@id=//page[@id=$selectedpage]/boxmodule/@boxid)]">
<xsl:variable name="boxcategoryid"><xsl:value-of select="@boxcategoryid"/></xsl:variable>
<tr>
	<td>
		<a href="attachbox.aspx?action=add&amp;boxid={@id}&amp;pageid={//selectedpage}">
      <img src="/admin/images/plus2.gif" width="16" height="16" alt="{util:localText('asign')}" title="{util:localText('asign')}" align="absmiddle" border="0"/>
    </a>&#160;<xsl:value-of select="title"/>
		(<xsl:value-of select="/website/boxmodule/boxcategory[@id=$boxcategoryid]/title"/>)
	</td>
</tr>
</xsl:for-each>

<xsl:variable name="moduleid">
<xsl:value-of select="//page[@id=//selectedpage]/usetemplate"/>
</xsl:variable>

<tr><td><br/><input type="button" class="almknap" value="{util:localText('back')}" onclick="location.href='{//template[@id=$moduleid]/publicurl}?pageid={//selectedpage}'"/></td></tr>
</table>

</xsl:template>

</xsl:stylesheet>