<?xml version="1.0" encoding="ISO-8859-1"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
  <xsl:output omit-xml-declaration="yes" />
  <xsl:template match="/">
    <xsl:for-each select="//page[@id=//selectedpage]">
      <xsl:if test="public!=-1">
        <xsl:value-of disable-output-escaping="yes" select="html" />
        <xsl:comment>modulecontent</xsl:comment>
      </xsl:if>
    </xsl:for-each>
  </xsl:template>
</xsl:stylesheet>