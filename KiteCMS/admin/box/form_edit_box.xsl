<?xml version='1.0' encoding='ISO-8859-1'?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:util="urn:localText" version="1.0" exclude-result-prefixes="util">
<xsl:output omit-xml-declaration="yes"/>

<xsl:param name="boxid" select="-1" />

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
	if(document.editBox.xmluri.value!=""){	
		if(document.editBox.xmluri.value.indexOf("http")==-1){
			if(document.editBox.xmluri.value.indexOf("/")!=-1 || document.editBox.xmluri.value.indexOf("\\")!=-1){
				alert("<xsl:value-of select="util:localText('xmlurinotvalid')"/>");
				document.editBox.xmluri.focus();
				return false;
				}
			}
		}

	return true
	}
	</script>
	
	<form method="post" action="editbox.aspx" name="editBox" onsubmit="return funcSubmit();">

	<input type="hidden" name="action" value="edit"/>
	<input type="hidden" name="boxid" value="{$boxid}"/>
	<input type="hidden" name="pageid" value="{//selectedpage}"/>

	<xsl:for-each select="/website/boxmodule/box[@id=$boxid]">	

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
						<xsl:attribute name="selected">selected</xsl:attribute>
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
		<td>
			<xsl:element name="input">
			<xsl:attribute name="type">checkbox</xsl:attribute>
			<xsl:attribute name="value">True</xsl:attribute>
			<xsl:attribute name="name">cascade</xsl:attribute>
			<xsl:if test="@cascade='True'">
        <xsl:attribute name="checked">checked</xsl:attribute>
			</xsl:if>
			</xsl:element>
		</td>
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
	</xsl:for-each>
	</form>

</xsl:template>
</xsl:stylesheet>
