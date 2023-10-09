<?xml version="1.0" encoding="ISO-8859-1"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:util="urn:localText" version="1.0" exclude-result-prefixes="util">
<xsl:output omit-xml-declaration="yes"/>

<xsl:variable name="selectedcategory"><xsl:value-of select="//selectedcategory"/></xsl:variable>
<xsl:variable name="selectedperson"><xsl:value-of select="//selectedperson"/></xsl:variable>

<xsl:template match="/">

	<table cellpadding="2" cellspacing="0" border="0">

	<xsl:for-each select="//category[@id=$selectedcategory]/field">
		<xsl:variable name="name"><xsl:value-of select="@id"/></xsl:variable>
		<tr><td height="20px" valign="top"><label for="person{$name}"><b><xsl:value-of select="@label"/>:</b></label></td><td>
		<xsl:choose>
			<xsl:when test="@type='text'">
				<xsl:value-of select="/website/category[@id=$selectedcategory]/person[@id=$selectedperson]/item[@id=$name]"/>
			</xsl:when>
			<xsl:when test="@type='select'">
				<select id="person{$name}" name="person{$name}" disabled="true">
					<xsl:for-each select="option">
						<xsl:element name="option">
							<xsl:if test="/website/category[@id=$selectedcategory]/person[@id=$selectedperson]/item[@id=$name]=@value">
								<xsl:attribute name="selected"/>
							</xsl:if>
							<xsl:attribute name="value"><xsl:value-of select="@value"/></xsl:attribute>
							<xsl:value-of select="@label"/>
						</xsl:element>
					</xsl:for-each>
				</select>
			</xsl:when>
			<xsl:when test="@type='checkbox'">
				<xsl:element name="input">
					<xsl:attribute name="disabled">true</xsl:attribute>
					<xsl:attribute name="type">checkbox</xsl:attribute>
					<xsl:attribute name="name">person<xsl:value-of select="$name"/></xsl:attribute>
					<xsl:attribute name="id">person<xsl:value-of select="$name"/></xsl:attribute>
					<xsl:attribute name="value"><xsl:value-of select="@value"/></xsl:attribute>
					<xsl:if test="/website/category[@id=$selectedcategory]/person[@id=$selectedperson]/item[@id=$name]!=''">
						<xsl:attribute name="checked"/>
					</xsl:if>
				</xsl:element>
			</xsl:when>
		</xsl:choose>
		</td></tr>
	</xsl:for-each>
	<tr><td colspan="2"><input type="button" value="{util:localText('back')}" class="almknap" onclick="history.go(-1);"/>&#160;<input type="button" value="{util:localText('backtofrontpage')}" class="almknap" onclick="location.href='default.aspx?pageid={//selectedpage}'"/></td></tr>

	</table>
</xsl:template>
</xsl:stylesheet>