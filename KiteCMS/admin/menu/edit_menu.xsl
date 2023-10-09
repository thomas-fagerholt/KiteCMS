<?xml version='1.0' encoding='ISO-8859-1'?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:util="urn:localText" version="1.0" exclude-result-prefixes="util">
<xsl:output omit-xml-declaration="yes"/>

<xsl:param name="child" select="-1" />
<xsl:param name="parent" select="-1" />
<xsl:param name="pageid" select="-1" />
<xsl:param name="useUserfriendlyUrl" select="-1" />

<xsl:include href="../data/editmenu_parameters.xsl" />

<xsl:template match="/">
	<script type="text/javascript">
	function funcSubmit(){
	if(document.addMenu.title.value==""){
		alert("<xsl:value-of select="util:localText('titlenotempty')"/>");
		document.addMenu.title.focus();
		return false;
		}
	<xsl:if test="$useUserfriendlyUrl='true'">
	if(document.addMenu.friendlyurl.value!="")
		if(document.addMenu.friendlyurl.value.charAt(0)!="/" || document.addMenu.friendlyurl.value.charAt(document.addMenu.friendlyurl.value.length-1)=="/" || !(document.addMenu.friendlyurl.value.indexOf(".")==-1)){
			alert("<xsl:value-of select="util:localText('friendlyurlnotok')"/>");
			document.addMenu.friendlyurl.focus();
			return false;
		}
		if(!(document.addMenu.friendlyurl.value.indexOf("\\")==-1)){
			alert("<xsl:value-of select="util:localText('friendlyurlnotbackslash')"/>");
			document.addMenu.friendlyurl.focus();
			return false;
		}
	</xsl:if>

	return true
	}
	</script>


	<form method="post" action="editmenu.aspx" name="addMenu" onsubmit="return funcSubmit();">

	<input type="hidden" name="action" value="choosecontent"/>
	<input type="hidden" name="child" value="{$child}"/>
	<input type="hidden" name="parent" value="{$parent}"/>
	<input type="hidden" name="pageid" value="{//selectedpage}"/>

	<xsl:for-each select="//page[@id=//selectedpage]">	
	<xsl:variable name="templateid"><xsl:value-of select="usetemplate"/></xsl:variable>

	<table cellpadding="0" cellspacing="2" border="0">
	<tr>
		<td><xsl:value-of select="util:localText('title')"/></td>
		<td><input type="text" class="alminput" name="title" value="{menutitle}"/></td>
	</tr>
	<tr>
		<td><xsl:value-of select="util:localText('template')"/></td>
		<td>

		<xsl:element name="select">
			<xsl:attribute name="name">template</xsl:attribute>
			<xsl:attribute name="class">alminput</xsl:attribute>
			<xsl:for-each select="//template">
			<xsl:sort select="title"/>
				<xsl:element name="option">
					<xsl:if test="$templateid=@id">
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
		<td><xsl:value-of select="util:localText('showstatus')"/>&#160;</td>
		<td>
		<select name="public" class="alminput">
			<xsl:element name="option">
				<xsl:attribute name="value">1</xsl:attribute>
				<xsl:if test="//page[@id=//selectedpage]/public=1">
				<xsl:attribute name="selected">selected</xsl:attribute>
				</xsl:if>
				<xsl:value-of select="util:localText('visible')"/>
			</xsl:element>
			<xsl:element name="option">
				<xsl:attribute name="value">0</xsl:attribute>
				<xsl:if test="//page[@id=//selectedpage]/public=0">
				<xsl:attribute name="selected"></xsl:attribute>
				</xsl:if>
				<xsl:value-of select="util:localText('notvisible')"/>
			</xsl:element>
			<xsl:element name="option">
				<xsl:attribute name="value">-1</xsl:attribute>
				<xsl:if test="//page[@id=//selectedpage]/public=-1">
				<xsl:attribute name="selected"></xsl:attribute>
				</xsl:if>
				<xsl:value-of select="util:localText('notpublic')"/>
			</xsl:element>
		</select>
		</td>
	</tr>
	<xsl:if test="$useUserfriendlyUrl='true'">
	<tr>
		<td><xsl:value-of select="util:localText('friendlyurl')"/></td>
		<td><input type="text" name="friendlyurl" class="alminput" value="{friendlyurl}"/></td>
	</tr>
	</xsl:if>
	<xsl:if test="$useUserfriendlyUrl!='true'">
		<input type="hidden" name="friendlyurl" class="alminput" value="{friendlyurl}"/>
	</xsl:if>

	<xsl:call-template name="parameters" />

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
