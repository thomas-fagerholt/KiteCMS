<?xml version='1.0' encoding='ISO-8859-1'?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:util="urn:contentFunctions" version="1.0" exclude-result-prefixes="util">
<xsl:output omit-xml-declaration="yes"/>
<xsl:template match="/">

  <p class="adminLinkList adminLinkListNoHeight">
    <xsl:for-each select="//emailcategory[@pageid=//selectedpage]">
      <b><xsl:value-of select="title"/></b><br/>
      <a href="default.aspx?pageid={//selectedpage}&amp;action=formaddarchive&amp;categoryid={@id}">
        <xsl:value-of select="util:localText('addwithoutsending')"/>
        <img src="/admin/images/plus2.gif" width="16" height="16" alt="{util:localText('addwithoutsending')}" title="{util:localText('addwithoutsending')}" align="absmiddle" border="0"/>
      </a>
      <br/>
      <br/>
      <xsl:for-each select="newsitem">
        <xsl:sort select="@created" order="descending"/>
        <a href="default.aspx?pageid={//selectedpage}&amp;action=formeditarchive&amp;newsitemid={@id}">
          <xsl:value-of select="title"/> (<xsl:value-of select="substring-before(@created,' ')"/>)
          <img src="/admin/images/edit.gif" width="16" height="16" alt="{util:localText('edit')}" title="{util:localText('edit')}" align="absmiddle"  border="0"/>
        </a>
        <br/>
      </xsl:for-each>
    </xsl:for-each>
  </p>
  <form action="/default.aspx">
      <input type="hidden" name="pageid" value="{//selectedpage}"/>
      <input type="submit" value="{util:localText('cancel')}" class="almknap"/>
    </form>
</xsl:template>
</xsl:stylesheet>
