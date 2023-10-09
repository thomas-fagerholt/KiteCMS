<?xml version="1.0" encoding="ISO-8859-1"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
  <xsl:output omit-xml-declaration="yes" doctype-public="-//W3C//DTD XHTML 1.0 Transitional//EN" doctype-system="http://www.w3.org/TR/xhtml1/DTD/transitional.dtd" method="xml" />
  <xsl:template match="/">
    <xsl:for-each select="//page[@id=//selectedpage]" xml:space="preserve">
		<html xml:lang="da" lang="da"> 
				<xsl:variable name="moduleid">
				<xsl:value-of select="usetemplate" />
				</xsl:variable>
			<head>
			<title>Redirect</title>
			</head>
			<body>
				<div style="padding:100px">
					<Editor id="redirecturl" hidden="true" hideinadmin="false" label="Url som siden skal redirecte til:" />
				</div>
			</body>
		</html>
	</xsl:for-each>
  </xsl:template>
</xsl:stylesheet>