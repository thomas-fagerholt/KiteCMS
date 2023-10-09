<?xml version="1.0" encoding="ISO-8859-1"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:util="urn:localText" version="1.0" exclude-result-prefixes="util">
<xsl:output omit-xml-declaration="yes"/>
<xsl:template match="/">

<table cellpadding="0" cellspacing="2" border="0">
<tr>
  <td class="adminLinkList delete">
    <xsl:value-of select="util:localText('choosetemplatedelete')"/>
		<p class="KiteCMSDefaultText adminLinkList">
		<xsl:for-each select="//template">
		<xsl:sort select="title"/>
			<xsl:variable name="id"><xsl:value-of select="@id"/></xsl:variable>
			<a href="deletetemplate.aspx?pageid={//selectedpage}&amp;action=choose&amp;templateid={@id}" class="delete">
				<xsl:value-of select="title"/>
			</a>
      (<xsl:value-of select="count(//page[usetemplate=$id])"/>&#160;<xsl:value-of select="util:localText('pages')"/>)
      <br/>
		</xsl:for-each>
		</p>
		<form action="/default.aspx">
		<input type="button" value="{util:localText('cancel')}" class="almknap" onclick="history.go(-1)"/>
		</form>
	</td>
</tr>
</table>

</xsl:template>
</xsl:stylesheet>
