<?xml version='1.0' encoding='ISO-8859-1'?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:util="urn:contentFunctions" version="1.0" exclude-result-prefixes="util">
<xsl:output omit-xml-declaration="yes"/>

<xsl:param name="canDelete" select="false"/>
<xsl:variable name="selectedevent"><xsl:value-of select="//selectedevent"/></xsl:variable>

<xsl:template match="/">

	<xsl:text disable-output-escaping="yes">
	<![CDATA[
	  <form action="default.aspx" target="_self" method="post" name="tempForm">
	]]>
	</xsl:text>
  <input type="hidden" name="action" value="doimportcalendar"/>
  <input type="hidden" name="pageid" value="{//selectedpage}"/>
  <p><xsl:value-of select="util:localText('importcalendardescription')"/></p>
	<textarea id="ical" name="ical" class="alminput" cols="40" rows="15"></textarea>
  <p>
			<input value="{util:localText('save')}" type="submit" class="almknap"/>&#160;
			<xsl:if test="$canDelete">
				<xsl:element name="input">
				<xsl:attribute name="type">button</xsl:attribute>
				<xsl:attribute name="value"><xsl:value-of select="util:localText('import')"/></xsl:attribute>
				<xsl:attribute name="class">almknap</xsl:attribute>
				</xsl:element>&#160;
			</xsl:if>
			<input type="button" value="{util:localText('cancel')}" class="almknap" onclick="history.go(-1);"/>
  </p>
</xsl:template>
</xsl:stylesheet>