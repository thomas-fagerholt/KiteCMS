<?xml version='1.0' encoding='ISO-8859-1'?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:util="urn:localText" version="1.0" exclude-result-prefixes="util">
<xsl:output omit-xml-declaration="yes"/>

<xsl:template name="parameters">

	<tr>
		<td>parameter</td>
		<td><input type="text" name="querystring" class="alminput" value="{parameters/querystring}"/></td>
	</tr>

	<tr>
		<td>Html-title</td>
		<td><input type="text" name="htmltitle" class="alminput" size="50" value="{parameters/htmltitle}"/></td>
	</tr>
	<tr>
		<td>Metatag description</td>
		<td><input type="text" name="metadescription" class="alminput" size="50" value="{parameters/metadescription}"/></td>
	</tr>

</xsl:template>
</xsl:stylesheet>
