<?xml version='1.0' encoding='ISO-8859-1'?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:util="urn:localText" version="1.0" exclude-result-prefixes="util">
<xsl:output omit-xml-declaration="yes"/>

<xsl:template match="/">
<head>
<link rel="stylesheet" type="text/css" href="/admin/default.css"/>
</head>
<body>
	<script type="text/javascript">
	function funcSubmit(){
	if(document.addCategory.title.value==""){
		alert("<xsl:value-of select="util:localText('titlenotempty')"/>");
		document.editBox.title.focus();
		return false;
		}

	return true
	}
	</script>
	
	<form method="post" action="default.aspx" name="addCategory" onsubmit="return funcSubmit();">

	<input type="hidden" name="action" value="doaddcategory"/>
	<input type="hidden" name="pageid" value="{//selectedpage}"/>


	<table cellpadding="0" cellspacing="2" border="0">
	<tr>
		<td><xsl:value-of select="util:localText('title')"/></td>
		<td><input type="text" class="alminput" name="title" value="{title}"/></td>
	</tr>
	<tr>
		<td><xsl:value-of select="util:localText('mastercategory')"/>: </td>
		<td>
			<xsl:element name="select">
				<xsl:attribute name="name">mastercategoryid</xsl:attribute>
				<xsl:attribute name="class">alminput</xsl:attribute>
					<xsl:element name="option">
						<xsl:attribute name="value"></xsl:attribute>
						<xsl:value-of select="util:localText('newmastercategory')"/>
					</xsl:element>
				<xsl:for-each select="/shop/productstore/category">
				<xsl:sort select="title"/>
					<xsl:element name="option">
						<xsl:attribute name="value"><xsl:value-of select="@id"/></xsl:attribute>
						<xsl:value-of select="title"/>
					</xsl:element>
				</xsl:for-each>
			</xsl:element>
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

	</form>
</body>
</xsl:template>
</xsl:stylesheet>
