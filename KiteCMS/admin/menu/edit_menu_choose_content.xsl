<?xml version='1.0' encoding='ISO-8859-1'?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:util="urn:localText" version="1.0" exclude-result-prefixes="util">
  <xsl:output omit-xml-declaration="yes"/>

<xsl:param name="child" select="-1" />
<xsl:param name="parent" select="-1" />
<xsl:param name="pageid" select="-1" />
<xsl:param name="useUserfriendlyUrl" select="-1" />
<xsl:variable name="lcletters">abcdefghijklmnopqrstuvwxyz</xsl:variable>
<xsl:variable name="ucletters">ABCDEFGHIJKLMNOPQRSTUVWXYZ</xsl:variable>

  <xsl:include href="../data/editmenu_parameters.xsl" />

<xsl:template match="/">
  <script type="text/javascript">
    function funcSubmit(){
    for (var i = 0; i != document.getElementsByTagName("select").length; i++)
    if (document.getElementsByTagName("select")[i][document.getElementsByTagName("select")[i].selectedIndex].value == "") {
    alert("<xsl:value-of select="util:localText('mustchoose')"/>");
    document.getElementsByTagName("select")[i].focus();
    return false;
    }
    return true;
    }
  </script>

  <p><xsl:value-of select="util:localText('matchcontentholders')"/></p>
  <form method="post" action="editmenu.aspx" name="addMenu" onsubmit="return funcSubmit();">
    <input type="hidden" name="action" value="doedit"/>
    <xsl:for-each select="/website/formparameter">
      <input type="hidden" name="{@param}" value="{.}"/>
    </xsl:for-each>

    <table cellpadding="0" cellspacing="2" border="0">
      <xsl:for-each select="/website/oldtemplate/Editor">
        <xsl:variable name="currentId"><xsl:value-of select="@id"/><xsl:value-of select="@Id"/><xsl:value-of select="@iD"/><xsl:value-of select="@ID"/></xsl:variable>
        <tr>
          <td>
            <xsl:apply-templates select="@*"/>
          </td>
          <td>&#160;-->&#160;</td>
          <td>
            <select name="contentholder_{@id}{@iD}{@Id}{@ID}">
                <option value=""><xsl:value-of select="util:localText('chooseonefromlist')"/></option>
              <xsl:for-each select="/website/newtemplate/Editor">
                <xsl:variable name="thisId"><xsl:value-of select="@id"/><xsl:value-of select="@Id"/><xsl:value-of select="@iD"/><xsl:value-of select="@ID"/></xsl:variable>
                <xsl:element name="option">
                  <xsl:attribute name="value"><xsl:value-of select="$thisId"/></xsl:attribute>
                  <xsl:if test="$currentId=$thisId">
                    <xsl:attribute name="selected">selected</xsl:attribute>
                  </xsl:if>
                  <xsl:apply-templates select="@*"/>
                </xsl:element>
              </xsl:for-each>
              <option value="_delete">*** <xsl:value-of select="util:localText('delete')"/></option>
            </select>
            </td>
        </tr>
      </xsl:for-each>
      <tr>
        <td colspan="2">
          <p>&#160;</p>
          <input type="submit" class="almknap" value="{util:localText('save')}"/>
          &#160;
          <input type="button" value="{util:localText('cancel')}" class="almknap" onclick="location.href='/default.aspx?pageid={/website/formparameter[@param='pageid']}'"/>
        </td>
      </tr>
    </table>
  </form>
</xsl:template>

  <xsl:template match="@*">
    <xsl:if test="translate(name(),$ucletters,$lcletters)='id'">
      <xsl:value-of select="." />
    </xsl:if>
    <xsl:if test="translate(name(),$ucletters,$lcletters)='label'">
      &#160;(<xsl:value-of select="." />)
    </xsl:if>
  </xsl:template>

</xsl:stylesheet>
