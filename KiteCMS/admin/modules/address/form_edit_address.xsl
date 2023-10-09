<?xml version="1.0" encoding="ISO-8859-1"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:util="urn:localText" version="1.0" exclude-result-prefixes="util">
<xsl:output omit-xml-declaration="yes"/>

<xsl:variable name="selectedcategory"><xsl:value-of select="//selectedcategory"/></xsl:variable>
<xsl:variable name="selectedperson"><xsl:value-of select="//selectedperson"/></xsl:variable>

<xsl:template match="/">

	<script language="javascript" type="text/JavaScript">
	function funcSubmit(frm)
	{
	<xsl:for-each select="//category[@id=$selectedcategory]/field[translate(@required,'abcdefghijklmnopqrstuvwxyz', 'ABCDEFGHIJKLMNOPQRSTUVWXYZ')='TRUE']">
		if (frm.person<xsl:value-of select="@id"/>.value == ''){
			alert('<xsl:value-of select="util:localText('fieldmissing')"/><xsl:value-of select="@label"/>');
			frm.person<xsl:value-of select="@id"/>.focus();
			return false;}
	</xsl:for-each>
	return true;
	}
	</script>

	<table cellpadding="2" cellspacing="0" border="0">
	<form name="person" id="person" onsubmit="return funcSubmit(this);" method="post" action="default.aspx">
	<input type="hidden" name="action" value="doeditaddress"/>
	<input type="hidden" name="categoryid" value="{$selectedcategory}"/>
	<input type="hidden" name="pageid" value="{//selectedpage}"/>
	<input type="hidden" name="personid" value="{$selectedperson}"/>
	<xsl:for-each select="//category[@id=$selectedcategory]/field">
		<xsl:variable name="name"><xsl:value-of select="@id"/></xsl:variable>
		<tr><td><label for="person{$name}"><xsl:value-of select="@label"/></label></td><td>
		<xsl:choose>
			<xsl:when test="@type='text'">
				<input id="person{$name}" name="person{$name}" class="alminput" value="{/website/category[@id=$selectedcategory]/person[@id=$selectedperson]/item[@id=$name]}"/>
			</xsl:when>
			<xsl:when test="@type='select'">
				<select id="person{$name}" name="person{$name}" class="alminput">
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
	<tr><td></td><td><input value="{util:localText('save')}" type="submit" class="almknap"/>&#160;<input type="button" value="{util:localText('cancel')}" class="almknap" onclick="history.go(-1);"/></td></tr>
	</form>
	</table>
</xsl:template>
</xsl:stylesheet>