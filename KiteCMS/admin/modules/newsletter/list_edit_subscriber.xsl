<?xml version='1.0' encoding='ISO-8859-1'?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:util="urn:contentFunctions" version="1.0" exclude-result-prefixes="util">
<xsl:output omit-xml-declaration="yes"/>
<xsl:template match="/">

  <p class="adminLinkList">
    <b>
      <xsl:value-of select="util:localText('choosecategory')"/>
    </b>
    <br/>
    <xsl:for-each select="//emailcategory[@pageid=//selectedpage]">
      <a href="default.aspx?pageid={//selectedpage}&amp;action=formeditsubscriber&amp;categoryid={@id}">
        <xsl:value-of select="title"/>
      </a>
      <br/>
    </xsl:for-each>
  </p>
			<form action="/default.aspx">
				<input type="hidden" name="pageid" value="{//selectedpage}"/>
				<input type="submit" value="{util:localText('cancel')}" class="almknap"/>
			</form>
</xsl:template>
</xsl:stylesheet>
