<?xml version='1.0' encoding='ISO-8859-1'?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:util="urn:localText" version="1.0" exclude-result-prefixes="util">
<xsl:output omit-xml-declaration="yes"/>

<xsl:param name="categoryid" select="-1" />

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

	<input type="hidden" name="action" value="doeditcategory"/>
	<input type="hidden" name="pageid" value="{//selectedpage}"/>
	<input type="hidden" name="categoryid" value="{$categoryid}"/>


	<table cellpadding="0" cellspacing="2" border="0">
	<tr>
		<td><xsl:value-of select="util:localText('title')"/></td>
		<td><input type="text" class="alminput" name="title" value="{//category[@id=$categoryid]/title}"/></td>
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
