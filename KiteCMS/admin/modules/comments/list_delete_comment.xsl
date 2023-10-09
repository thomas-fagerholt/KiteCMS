<?xml version="1.0" encoding="ISO-8859-1"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:util="urn:localText" version="1.0" exclude-result-prefixes="util">
  <xsl:output omit-xml-declaration="yes"/>

  <xsl:param name="dateShowNew" select="-1" />
  <xsl:param name="pageid" select="-1" />

  <xsl:template match="/">

      <p class="adminLinkList">
        <xsl:for-each select="//commentItem[@pageid=$pageid]">
          <span title="{util:localText('comment')}: {translate(item[@type='comment'],'&quot;',&quot;'&quot;)}">
            <xsl:value-of select="item[@type='header']"/>
          </span>
          &#160;<span class="debateDate">
            <xsl:value-of select="substring-before(@created,' ')"/>
            <xsl:call-template name="newdebate" />
          </span>
          &#160;
          <xsl:if test="@active=1">
            (<xsl:value-of select="util:localText('active')"/>)&#160;
          </xsl:if>
          <a href="javascript:if(confirm('{util:localText('oktodelete')}?')) {{location.href='default.aspx?action=dodeletecomment&amp;pageid={$pageid}&amp;commentid={@id}'}}">
            <span style='color:red'><xsl:value-of select="util:localText('delete')"/></span>
            <img src="/admin/images/delete.gif" width="15" height="15" alt="{util:localText('delete')}" title="{util:localText('delete')}"/>
          </a>
          <br/>
        </xsl:for-each>
      </p>
      <form action="/default.aspx">
        <input type="button" value="{util:localText('back')}" class="almknap" onclick="history.go(-1)"/>
      </form>
  </xsl:template>

  <xsl:template name="newdebate">
    <xsl:if test="(substring-before(@date,'-') &gt; substring-before($dateShowNew,'-') and substring-after(substring-before(@date,' '),'-') = substring-after(substring-before($dateShowNew,' '),'-')) or (substring-before(substring-after(substring-before(@date,' '),'-'),'-')+12*substring-after(substring-after(substring-before(@date,' '),'-'),'-') &gt; substring-before(substring-after(substring-before($dateShowNew,' '),'-'),'-')+12*substring-after(substring-after(substring-before($dateShowNew,' '),'-'),'-'))">
      <span style='color:darkred'>
        <sup>
          <xsl:value-of select="util:localText('new')"/>
        </sup>
      </span>
    </xsl:if>
  </xsl:template>

</xsl:stylesheet>