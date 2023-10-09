<?xml version='1.0' encoding='ISO-8859-1'?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:util="urn:contentFunctions" version="1.0" exclude-result-prefixes="util">
<xsl:output omit-xml-declaration="yes"/>
<xsl:template match="/">

<h5><xsl:value-of select="util:localText('choosenewsletter')"/></h5>
		<xsl:for-each select="//emailcategory[@pageid=//selectedpage]">

        <h5><xsl:value-of select="title"/>:&#160;
          <span class="adminLinkList">          <a href="default.aspx?pageid={//selectedpage}&amp;action=formsendnewsletter&amp;categoryid={@id}">
          <xsl:value-of select="util:localText('senddirect')"/>
          <img src="/admin/images/send_new.gif" width="16" height="14" alt="{util:localText('senddirect')}" title="{util:localText('senddirect')}" align="absmiddle"  border="0"/>
        </a>
          </span>

        </h5>
        <p class="adminLinkList">
        <xsl:for-each select="newsitem">
          <xsl:sort select="@created" order="descending"/>
          <b><xsl:value-of select="title"/>
          </b> (<xsl:value-of select="substring-before(@created,' ')"/>) -
          <xsl:if test="@sentdate">
            <xsl:value-of select="util:localText('sent')"/>&#160;<xsl:value-of select="@sentdate"/>
          </xsl:if>
          <xsl:if test="not(@sentdate)">
            <a href="default.aspx?pageid={//selectedpage}&amp;action=formsendnewsletter&amp;newsitemid={@id}">
              <xsl:value-of select="util:localText('send')"/>
              <img src="/admin/images/send.gif" width="16" height="13" alt="{util:localText('send')}" title="{util:localText('send')}" align="absmiddle"  border="0"/>
            </a>
          </xsl:if>
          <br/>
        </xsl:for-each>
      </p>
    </xsl:for-each>

			<form action="/default.aspx">
				<input type="hidden" name="pageid" value="{//selectedpage}"/>
				<input type="submit" value="{util:localText('cancel')}" class="almknap"/>
			</form>
</xsl:template>
</xsl:stylesheet>
