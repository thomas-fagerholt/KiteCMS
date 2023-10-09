<?xml version="1.0" encoding="ISO-8859-1" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:util="urn:localText" xmlns:access="urn:userHasAccess" version="1.0" exclude-result-prefixes="util access">
  <xsl:output omit-xml-declaration="yes"/>

  <xsl:template match="/">
    <xsl:variable name="fewFirstLevel">
      <xsl:choose>
        <xsl:when test="count(/website/page)&lt;=3">2</xsl:when>
        <xsl:otherwise>1</xsl:otherwise>
      </xsl:choose>
    </xsl:variable>
    <xsl:variable name="selectedpage">
      <xsl:value-of select="//selectedpage"/>
    </xsl:variable>

    <h4>
      <xsl:value-of select="util:localText('chooselocation')"/>
    </h4>
    <p class="chooseLink">
      <xsl:for-each select="//page">
        <!-- insert page before another -->
        <xsl:for-each select="ancestor::page">
          <img align="absmiddle" src="/admin/images/dottedVertical.gif" width="20px" height="18px" alt=""/>
        </xsl:for-each>
        <img align="absmiddle" src="/admin/images/dottedCross.gif" width="20px" height="18px"/>
        <xsl:choose>
          <xsl:when test="count(//canAddMenuRoot)>0 or count(ancestor::page/canAddMenu)>0">
            <a href="addmenu.aspx?pageid={$selectedpage}&amp;action=addinfo&amp;child={@id}">
              <img align="top" src="/admin/images/newMenu1.gif" alt="{util:localText('addmenu')} {util:localText('here')}" width="11" height="18px" border="0"/></a>
          </xsl:when>
          <xsl:otherwise>
            -
          </xsl:otherwise>
        </xsl:choose>
        <xsl:if test="count(ancestor::page)&lt;$fewFirstLevel">
          &#160;<img align="absmiddle" src="/admin/images/space.gif" width="500px" height="1px" class="dottedSpace"/>
        </xsl:if>
        <br/>

        <xsl:for-each select="ancestor::page">
          <img align="absmiddle" src="/admin/images/dottedVertical.gif" width="20px" height="18px" alt=""/>
        </xsl:for-each>
        <img align="absmiddle" src="/admin/images/dottedCross.gif" width="20px" height="18px"/>

        <xsl:value-of select="menutitle"/>

        <xsl:if test="count(child::page)=0 and (count(//canAddMenuRoot)>0 or count(ancestor-or-self::page/canAddMenu)>0)">
          <a href="addmenu.aspx?pageid={$selectedpage}&amp;action=addinfo&amp;parent={@id}">
            <img align="top" src="/admin/images/space.gif" width="4px" height="4px" border="0"/>
            <img align="top" src="/admin/images/newMenu2.gif" alt="{util:localText('add')} {util:localText('subitem')}" width="11" height="18px" border="0"/>
          </a>
        </xsl:if>
        <br/>

        <!-- insert page as last in menu without children -->
        <xsl:if test="count(following-sibling::page)=0 and position()!=last()">
          <xsl:if test="count(child::page)=0">
            <xsl:for-each select="ancestor::page">
              <img align="absmiddle" src="/admin/images/dottedVertical.gif" width="20px" height="18px" alt=""/>
            </xsl:for-each>
            <xsl:choose>
              <xsl:when test="count(//canAddMenuRoot)>0 or count(ancestor::page/canAddMenu)>0">
                <img align="absmiddle" src="/admin/images/dottedCorner.gif" width="20px" height="18px"/>
                <a href="addmenu.aspx?pageid={$selectedpage}&amp;action=addinfo&amp;child=-2&amp;parent={../@id[.!=0]}">
                  <img align="top" src="/admin/images/newMenu1.gif" alt="{util:localText('addmenu')} {util:localText('here')}" width="11" height="18px" border="0"/>
                </a>
              </xsl:when>
              <xsl:otherwise>
                -
              </xsl:otherwise>
            </xsl:choose>
            <br/>

            <!-- insert page as last in menu with children -->
            <xsl:for-each select="ancestor::page">
              <xsl:sort select="position()" order="descending"/>
              <xsl:if test="count(following-sibling::page)=0 and position() &lt; last()">
                <xsl:for-each select="ancestor::page">
                  <img align="absmiddle" src="/admin/images/dottedVertical.gif" width="20px" height="18px" alt=""/>
                </xsl:for-each>
                <xsl:choose>
                  <xsl:when test="count(//canAddMenuRoot)>0 or count(ancestor::page/canAddMenu)>0">
                    <img align="absmiddle" src="/admin/images/dottedCorner.gif" width="20px" height="18px"/>
                    <a href="addmenu.aspx?pageid={$selectedpage}&amp;action=addinfo&amp;child=-2&amp;parent={../@id[.!=0]}">
                      <img align="top" src="/admin/images/newMenu1.gif" alt="{util:localText('addmenu')} {util:localText('here')}" width="11" height="18px" border="0"/>
                    </a>
                  </xsl:when>
                  <xsl:otherwise>
                    -
                  </xsl:otherwise>
                </xsl:choose>
                <br/>
              </xsl:if>
            </xsl:for-each>
          </xsl:if>
        </xsl:if>
      </xsl:for-each>

      <img align="absmiddle" src="/admin/images/dottedCorner.gif" width="20px" height="18px"/>
      <xsl:choose>
        <xsl:when test="count(//canAddMenuRoot)>0 or count(ancestor::page/canAddMenu)>0">
          <a href="addmenu.aspx?pageid={$selectedpage}&amp;action=addinfo&amp;child=-2&amp;parent=">
            <img align="top" src="/admin/images/newMenu1.gif" alt="{util:localText('addmenu')} {util:localText('here')}" width="11" height="18px" border="0"/>
          </a>
        </xsl:when>
        <xsl:otherwise>
          -
        </xsl:otherwise>
      </xsl:choose>
      &#160;<img align="absmiddle" src="/admin/images/space.gif" width="500px" height="1px" class="dottedSpace"/>
    </p>
    <br/>
    <form action="/modules/default.aspx">
      <input type="button" value="{util:localText('cancel')}" class="almknap" onclick="history.go(-1)"/>
    </form>

  </xsl:template>

</xsl:stylesheet>