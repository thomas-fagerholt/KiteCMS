<?xml version="1.0" encoding="utf-8" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:util="urn:localText" xmlns:access="urn:userHasAccess" version="1.0" exclude-result-prefixes="util access">
  <xsl:output omit-xml-declaration="yes"/>

  <xsl:param name="hasRoot" select="-1" />
  <xsl:variable name="selectedpage">
    <xsl:value-of select="//selectedpage"/>
  </xsl:variable>

  <xsl:template match="/">
    <xsl:variable name="fewFirstLevel">
      <xsl:choose>
        <xsl:when test="count(/website/page)&lt;=3">2</xsl:when>
        <xsl:otherwise>1</xsl:otherwise>
      </xsl:choose>
    </xsl:variable>

    <script type="text/javascript">
      function funcSubmit(parent, child){
      document.copymenu.parent.value = parent;
      document.copymenu.child.value = child;

      document.copymenu.submit();
      }
    </script>

    <form action="copymenu.aspx" name="copymenu">
      <input type="hidden" name="pageid" value="{$selectedpage}"/>
      <input type="hidden" name="action" value="docopy"/>
      <input type="hidden" name="child" value=""/>
      <input type="hidden" name="parent" value=""/>

      <table cellpadding="0" cellspacing="2" border="0">
        <tr>
          <td>
            <xsl:value-of select="util:localText('includesubpages')"/>
          </td>
          <td>
            <input type="checkbox" name="includesubpages" checked="checked" value="true"/>
          </td>
        </tr>
        <tr>
          <td>
            <xsl:value-of select="util:localText('includecontent')"/>
          </td>
          <td>
            <input type="checkbox" name="includecontent" checked="checked" value="true"/>
          </td>
        </tr>
        <tr>
          <td colspan="2">
            <h4>
              <xsl:value-of select="util:localText('choosenewlocation')"/>&#160;<xsl:value-of select="util:localText('thecopy')"/> '<xsl:value-of select="//page[@id=$selectedpage]/menutitle"/>':
            </h4>
            <p class="chooseLink">

              <xsl:for-each select="//page">
                <xsl:variable name="prev">
                  <xsl:value-of select="preceding-sibling::page[position()=1]/@id"/>
                </xsl:variable>
                <xsl:variable name="countAncestor" xml:space="default">
                  <xsl:value-of select="count(ancestor-or-self::page[@id=$selectedpage])"/>
                </xsl:variable>

                <xsl:choose>
                  <xsl:when test="$countAncestor=0 and count(//page[@id=$prev]/ancestor-or-self::page[@id=$selectedpage])=0">
                    <xsl:if test="@userhasaccess='true'">
                      <xsl:for-each select="ancestor::page">
                        <img align="absmiddle" src="/admin/images/dottedVertical.gif" width="20px" height="18px" alt=""/>
                      </xsl:for-each>
                      <img align="absmiddle" src="/admin/images/dottedCross.gif" width="20px" height="18px"/>
                      <xsl:element name="a">
                        <xsl:attribute name="style">cursor: pointer</xsl:attribute>
                        <xsl:attribute name="onclick">
                          funcSubmit(-1,<xsl:value-of select="@id"/>)
                        </xsl:attribute>
                        <img align="absmiddle" src="/admin/images/newMenu1.gif" alt="{util:localText('here')}" width="11" height="18px"/>
                      </xsl:element>
                    </xsl:if>
                  </xsl:when>
                  <xsl:otherwise>
                    <img align="absmiddle" src="/admin/images/space.gif" width="11px" height="1px" alt=""/>
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

                <xsl:if test="$countAncestor>0">
                  <b>
                    <xsl:value-of select="menutitle"/>
                  </b>
                </xsl:if>
                <xsl:if test="$countAncestor=0">
                  <xsl:value-of select="menutitle"/>
                </xsl:if>

                <xsl:if test="count(child::page)=0 and $countAncestor=0">
                  <xsl:if test="@userhasaccess='true'">
                    <xsl:element name="a">
                      <xsl:attribute name="style">cursor: pointer</xsl:attribute>
                      <xsl:attribute name="onclick">
                        funcSubmit(<xsl:value-of select="@id"/>,-1)
                      </xsl:attribute>
                      <xsl:attribute name="title">nyt underpunkt</xsl:attribute>
                      <img align="absmiddle" src="/admin/images/space.gif" width="4px" height="4px"/>
                      <img align="absmiddle" src="/admin/images/newMenu2.gif" alt="{util:localText('add')} {util:localText('subitem')}" width="11" height="18px"/>
                    </xsl:element>
                  </xsl:if>
                </xsl:if>

                <!-- indsaet som sidste paa et niveau -->
                <xsl:for-each select="ancestor::page">
                  <xsl:sort select="position()" order="descending"/>
                  <xsl:if test="count(child::page)=0">
                    z
                    <xsl:if test="@userhasaccess='true'">
                      <br/>
                      <xsl:for-each select="ancestor::page">
                        <img align="absmiddle" src="/admin/images/dottedVertical.gif" width="20px" height="18px" alt=""/>
                      </xsl:for-each>
                      <xsl:element name="a">
                        <xsl:attribute name="style">cursor: pointer</xsl:attribute>
                        <xsl:attribute name="onclick">
                          <xsl:choose>
                            <xsl:when test="not(../@id)">funcSubmit(-1,-1);</xsl:when>
                            <xsl:otherwise>
                              funcSubmit(<xsl:value-of select="../@id"/>,-1);
                            </xsl:otherwise>
                          </xsl:choose>
                        </xsl:attribute>
                        <img align="absmiddle" src="/admin/images/newMenu1.gif" alt="{util:localText('here')}" width="11" height="18px"/>
                      </xsl:element>
                    </xsl:if>
                  </xsl:if>
                </xsl:for-each>
                <br/>

                <!-- insert page as last in menu without children -->
                <xsl:if test="$countAncestor=0">
                  <xsl:if test="count(following-sibling::page)=0 and position()!=last()">
                    <xsl:if test="count(child::page)=0">
                      <xsl:for-each select="ancestor::page">
                        <img align="absmiddle" src="/admin/images/dottedVertical.gif" width="20px" height="18px" alt=""/>
                      </xsl:for-each>
                      <img align="absmiddle" src="/admin/images/dottedCorner.gif" width="20px" height="18px"/>
                      <xsl:if test="@userhasaccess='true'">
                        <xsl:element name="a">
                          <xsl:attribute name="style">cursor: pointer</xsl:attribute>
                          <xsl:attribute name="onclick">
                            <xsl:choose>
                              <xsl:when test="not(../@id)">funcSubmit(-1,-1);</xsl:when>
                              <xsl:otherwise>
                                funcSubmit(<xsl:value-of select="../@id"/>,-1);
                              </xsl:otherwise>
                            </xsl:choose>
                          </xsl:attribute>
                          <img align="absmiddle" src="/admin/images/newMenu1.gif" alt="{util:localText('movemenu')} {util:localText('here')}" width="11" height="18px"/>
                        </xsl:element>
                      </xsl:if>
                      <br/>
                    </xsl:if>
                  </xsl:if>
                </xsl:if>

                <xsl:variable name="lastNodeId">
                  <xsl:value-of select="@id"/>
                </xsl:variable>

                <!-- insert page as last in menu with children -->
                <xsl:if test="count(following-sibling::page)=0 and position() &lt; last() and count(child::page)=0">

                  <xsl:for-each select="ancestor::page[count(following-sibling::page)=0]">
                    <xsl:sort select="position()" order="descending"/>
                    <xsl:if test="$countAncestor=0">

                      <xsl:variable name="match">
                        <xsl:for-each select="current()/descendant::page[count(following-sibling::page)=0]">
                          <xsl:if test="//page[@id=$lastNodeId]/following::page[@id=current()/@id]">
                            <xsl:copy-of select="."/>
                          </xsl:if>
                        </xsl:for-each>
                      </xsl:variable>

                      <xsl:if test="$match=''">
                        <xsl:for-each select="ancestor::page">
                          <img align="absmiddle" src="/admin/images/dottedVertical.gif" width="20px" height="18px" alt=""/>
                        </xsl:for-each>
                        <xsl:if test="count(ancestor::page)&gt;=$fewFirstLevel">
                          <img align="absmiddle" src="/admin/images/dottedCorner.gif" width="20px" height="18px"/>
                        </xsl:if>
                        <xsl:if test="@userhasaccess='true' and position() != last()">
                          <xsl:element name="a">
                            <xsl:attribute name="style">cursor: pointer</xsl:attribute>
                            <xsl:attribute name="onclick">
                              <xsl:choose>
                                <xsl:when test="not(../@id)">funcSubmit(-1,-1);</xsl:when>
                                <xsl:otherwise>
                                  funcSubmit(<xsl:value-of select="../@id"/>,-1);
                                </xsl:otherwise>
                              </xsl:choose>
                            </xsl:attribute>
                            <img align="absmiddle" src="/admin/images/newMenu1.gif" alt="{util:localText('movemenu')} {util:localText('here')}" width="11" height="18px"/>
                          </xsl:element>
                        </xsl:if>
                        <br/>
                      </xsl:if>
                    </xsl:if>
                  </xsl:for-each>
                </xsl:if>

              </xsl:for-each>
            </p>
          </td>
        </tr>
        <tr>
          <td colspan="2">
            <input type="button" value="{util:localText('cancel')}" class="almknap" onclick="history.go(-1)"/>
          </td>
        </tr>
      </table>
    </form>

  </xsl:template>

</xsl:stylesheet>



