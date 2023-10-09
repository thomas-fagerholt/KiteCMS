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
      <a href="default.aspx?pageid={//selectedpage}&amp;action=formeditnewsletter&amp;categoryid={@id}">
        <xsl:value-of select="title"/>
        <img src="/admin/images/edit.gif" width="16" height="16" alt="{util:localText('edit')}" title="{util:localText('edit')}" align="absmiddle"  border="0"/>
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
