<?xml version='1.0' encoding='ISO-8859-1'?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:util="urn:localText" version="1.0" exclude-result-prefixes="util">
<xsl:output omit-xml-declaration="yes"/>

<xsl:template match="/">

  <script type="text/javascript">
	function funcSubmit(){
	if(document.editBox.title.value==""){
		alert("<xsl:value-of select="util:localText('titlenotempty')"/>");
		document.editBox.title.focus();
		return false;
		}
	if(document.editBox.content.value==""){
		alert("<xsl:value-of select="util:localText('contentnotempty')"/>");
		document.editBox.title.focus();
		return false;
		}

	return true
	}
	</script>
	
	<form method="post" action="addbox.aspx" name="editBox" onsubmit="return funcSubmit();">

	<input type="hidden" name="action" value="add"/>
	<input type="hidden" name="pageid" value="{//selectedpage}"/>


	<xsl:variable name="boxcategoryid"><xsl:value-of select="@boxcategoryid"/></xsl:variable>
	<table cellpadding="0" cellspacing="2" border="0">
	<tr>
		<td><xsl:value-of select="util:localText('title')"/></td>
		<td><input type="text" class="alminput" name="title" value="{title}" size="40"/></td>
	</tr>
	<tr>
		<td><xsl:value-of select="util:localText('category')"/></td>
		<td>
			<xsl:element name="select">
				<xsl:attribute name="name">boxcategoryid</xsl:attribute>
				<xsl:attribute name="class">alminput</xsl:attribute>
				<xsl:for-each select="/website/boxmodule/boxcategory">
				<xsl:sort select="title"/>
					<xsl:element name="option">
						<xsl:if test="$boxcategoryid=@id">
						<xsl:attribute name="selected"></xsl:attribute>
						</xsl:if>
						<xsl:attribute name="value"><xsl:value-of select="@id"/></xsl:attribute>
						<xsl:value-of select="title"/>
					</xsl:element>
				</xsl:for-each>
			</xsl:element>
		</td>
	</tr>
	<tr>
		<td><xsl:value-of select="util:localText('cascade')"/></td>
		<td><input type="checkbox" value="True" name="cascade"/></td>
	</tr>
	<tr>
		<td valign="top"><xsl:value-of select="util:localText('content')"/></td>
		<td><textarea name="content" cols="80" rows="15" class="alminput"><xsl:value-of select="content"/></textarea></td>
	</tr>
	<tr>
		<td><xsl:value-of select="util:localText('xmluri')"/></td>
		<td><input type="text" class="alminput" name="xmluri" value="{xmluri}" size="81"/></td>
	</tr>
	<tr>
		<td></td>
		<td><input type="submit" class="almknap" value="{util:localText('save')}"/>
		&#160;
		<input type="button" value="{util:localText('cancel')}" class="almknap" onclick="history.go(-1)"/>
		</td>
	</tr>
	</table>

	</form>

</xsl:template>
</xsl:stylesheet>
