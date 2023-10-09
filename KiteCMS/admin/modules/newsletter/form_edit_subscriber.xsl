<?xml version='1.0' encoding='ISO-8859-1'?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:util="urn:contentFunctions" version="1.0" exclude-result-prefixes="util">
<xsl:output omit-xml-declaration="yes"/>

<xsl:template match="/">
<xsl:variable name="selectedemailtype"><xsl:value-of select="//selectedemailtype"/></xsl:variable>

  <table border="0" width="450">
    <tr>
      <td colspan="3" height="20">
        <xsl:value-of select="util:localText('countsubscibers')"/>: <xsl:value-of select="count(//subscribe[@category=$selectedemailtype])"/>
      </td>
    </tr>
    <tr>
      <td></td>
      <td>
        <b>
          <xsl:value-of select="util:localText('name')"/>
        </b>
      </td>
      <td>
        <b>
          <xsl:value-of select="util:localText('email')"/>
        </b>
      </td>
      <xsl:for-each select="/website/field[@listitem='true']">
      <td>
        <b>
          <xsl:value-of select="@id"/>
        </b>
      </td>
      </xsl:for-each>
    </tr>

    <xsl:for-each select="//emailcategory[@id=$selectedemailtype and @pageid=//selectedpage]">

      <xsl:for-each select="//subscribe[@category=$selectedemailtype]">
			<xsl:variable name="nodeset" select="."/> 
        <tr>
          <td class="KiteCMSDefaultText">
            <a href="javascript:void(0):" onclick="if(confirm('{util:localText('confirmremove')} &quot;{email}&quot;?')) {{location.href='default.aspx?pageid={//selectedpage}&amp;email={email}&amp;action=removesubscriber&amp;categoryid={$selectedemailtype}';}}">
              <img src="/admin/images/delete.gif" width="16" height="16" alt="{util:localText('remove')}" title="{util:localText('remove')}" align="absmiddle" border="0"/>
            </a>
          </td>

          <td>
            <xsl:value-of select="name"/>
          </td>
          <td>
            <xsl:value-of select="email"/>
          </td>
		      <xsl:for-each select="/website/field[@listitem='true']">
						<xsl:variable name="id" select="@id"/>
		      <td>
		          <xsl:value-of select="$nodeset/child::*[local-name()=$id]"/>
		      </td>
		      </xsl:for-each>
        </tr>
      </xsl:for-each>
    </xsl:for-each>
    <tr>
      <td colspan="3">
        <br/>
        <form action="/default.aspx">
          <input type="button" class="almknap" value="{util:localText('back')}" onclick="document.location.href='/modules/default.aspx?pageid={//selectedpage}';return false;"/>&#160;
        </form>
      </td>
    </tr>
  </table>
</xsl:template>

</xsl:stylesheet>