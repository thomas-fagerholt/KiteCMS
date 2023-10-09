<?xml version='1.0' encoding='ISO-8859-1'?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:util="urn:localText" version="1.0" exclude-result-prefixes="util">
<xsl:output omit-xml-declaration="yes"/>

<xsl:template match="/">

<table border="0" width="450">
<tr><td><b><xsl:value-of select="util:localText('accesstoarea')"/>:</b></td></tr>
<xsl:if test="count(//user[accesszone=//selectedaccesszone])=0">
	<tr><td><xsl:value-of select="util:localText('accesstoareanone')"/></td></tr>
</xsl:if>

<xsl:for-each select="//user[accesszone=//selectedaccesszone]">
<tr>
	<td class="adminLinkList adminLinkListNoHeight">
    <a href="default.aspx?action=doasign&amp;remove={login}&amp;pageid={//selectedpage}" title="{util:localText('remove')}">
    <img src="/admin/images/user_granted.png" width="16" height="16" alt="{util:localText('remove')}" align="absmiddle" border="0"/>
  &#160;<xsl:value-of select="login"/>
	<xsl:if test="fullname!=''">
		(<xsl:value-of select="fullname"/>)
	</xsl:if>
  </a>
  </td>
</tr>
</xsl:for-each>

<tr><td><br/><b><xsl:value-of select="util:localText('noaccesstoarea')"/>:</b></td></tr>

<xsl:for-each select="//user[not(accesszone=//selectedaccesszone)]">
<tr>
	<td class="adminLinkList adminLinkListNoHeight">
    <a href="default.aspx?action=doasign&amp;add={login}&amp;pageid={//selectedpage}" title="{util:localText('asign')}">
    <img src="/admin/images/user_denied.png" width="16" height="16" alt="{util:localText('asign')}" align="absmiddle" border="0"/>
  &#160;<xsl:value-of select="login"/>
	<xsl:if test="fullname!=''">
		(<xsl:value-of select="fullname"/>)
	</xsl:if>
  </a>
	</td>
</tr>
</xsl:for-each>

<tr><td><br/><input type="button" class="almknap" value="{util:localText('back')}" onclick="location.href='/modules/default.aspx?pageid={//selectedpage}'"/></td></tr>
</table>

</xsl:template>

</xsl:stylesheet>