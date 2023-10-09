<?xml version='1.0' encoding='ISO-8859-1'?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:util="urn:localText" version="1.0" exclude-result-prefixes="util">
<xsl:output omit-xml-declaration="yes"/>

<xsl:param name="boxcategoryid" select="-1" />

<xsl:template match="/">

  <script type="text/javascript">
	function funcSubmit(){
	if(document.editBox.title.value==""){
		alert("<xsl:value-of select="util:localText('titlenotempty')"/>");
		document.editBox.title.focus();
		return false;
		}
	if(document.editBox.htmlstring.value==""){
		alert("<xsl:value-of select="util:localText('contentnotempty')"/>");
		document.editBox.htmlstring.focus();
		return false;
		}

	return true
	}
	</script>
	
	<form method="post" action="editboxcategory.aspx" name="editBox" onsubmit="return funcSubmit();">

	<input type="hidden" name="action" value="edit"/>
	<input type="hidden" name="boxcategoryid" value="{$boxcategoryid}"/>
	<input type="hidden" name="pageid" value="{//selectedpage}"/>

	<xsl:for-each select="/website/boxmodule/boxcategory[@id=$boxcategoryid]">	

	<table cellpadding="0" cellspacing="2" border="0">
	<tr>
		<td><xsl:value-of select="util:localText('title')"/></td>
		<td><input type="text" class="alminput" name="title" value="{title}" size="40"/></td>
	</tr>
	<tr>
		<td><xsl:value-of select="util:localText('htmlstring')"/></td>
		<td><input type="text" class="alminput" name="htmlstring" value="{htmlstring}" size="40"/></td>
	</tr>
	<tr>
		<td><xsl:value-of select="util:localText('type')"/></td>
		<td>
			<select class="alminput" name="boxType">
				<xsl:element name="option">
					<xsl:attribute name="value">1</xsl:attribute>
					<xsl:if test="@type='1'">
            <xsl:attribute name="selected">selected</xsl:attribute>
            </xsl:if>
					<xsl:value-of select="util:localText('insertabove')"/>
				</xsl:element>
				<xsl:element name="option">
					<xsl:attribute name="value">0</xsl:attribute>
					<xsl:if test="@type='0'">
            <xsl:attribute name="selected">selected</xsl:attribute>
          </xsl:if>
					<xsl:value-of select="util:localText('replace')"/>
				</xsl:element>
				<xsl:element name="option">
					<xsl:attribute name="value">2</xsl:attribute>
					<xsl:if test="@type='2'">
            <xsl:attribute name="selected">selected</xsl:attribute>
          </xsl:if>
					<xsl:value-of select="util:localText('insertbelove')"/>
				</xsl:element>
			</select>
		</td>
	</tr>
	<tr>
		<td></td>
		<td><input type="submit" class="almknap" value="{util:localText('save')}"/>
		&#160;
		<input type="button" value="{util:localText('cancel')}" class="almknap" onclick="history.go(-1)"/>
		</td>
	</tr>
	</table>
	</xsl:for-each>
	</form>

</xsl:template>
</xsl:stylesheet>
