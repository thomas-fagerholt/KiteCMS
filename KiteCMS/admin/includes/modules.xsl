<?xml version='1.0' encoding='ISO-8859-1'?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:util="urn:localText" version="1.0" exclude-result-prefixes="util">
  <xsl:output omit-xml-declaration="yes"/>

  <xsl:param name="user" select="-1" />
  <xsl:param name="blnDraft" select="False" />

  <xsl:template match="/">


    <div id="KiteCMSAdminPageprop" class="adminSubMenu" style="display:none;top:0;left:0" onmouseout="mouseout(this.id);" onmouseover="mouseover();">
      <xsl:if test="//user[@id=$user]/access">
        <xsl:for-each select="//access/module/module[@submenu='page' and @id &lt; 2000]">
          <xsl:if test="//user[@id=$user]/access=0 or //user[@id=$user]/access=@id or method[@id=//user[@id=$user]/access]">
            <xsl:call-template name="module" />
            <ul>
              <xsl:for-each select="method">
                <xsl:call-template name="method" />
              </xsl:for-each>
            </ul>
          </xsl:if>
        </xsl:for-each>
      </xsl:if>
    </div>

    <div id="KiteCMSAdminSiteprop" class="adminSubMenu" style="display:none;top:0;left:0" onmouseout="mouseout(this.id);" onmouseover="mouseover();">
      <xsl:for-each select="//access/module/module[@submenu='site' and @id &lt; 2000]">
         <xsl:if test="//user[@id=$user]/access=0 or //user[@id=$user]/access=@id or method[@id=//user[@id=$user]/access] or //user[@id=$user]/@changepassword=1 and @id=1600">
            <xsl:call-template name="module" />
            <ul>
              <xsl:for-each select="method">
                <xsl:call-template name="method" />
              </xsl:for-each>
              <xsl:if test="//user[@id=$user]/@changepassword=1 and @id=1600">
                <li>
                  <a href="/admin/useradmin/changepassword.aspx">
                    <xsl:value-of select="util:localText('changepassword')"/>
                  </a>
                </li>
              </xsl:if>
            </ul>
        </xsl:if>
      </xsl:for-each>
    </div>

    <script type="text/javascript">
      init();

      function init() {
      if (document.all) {
      document.onkeydown = checkKey;
      }
      else {
      document.addEventListener('keypress', checkKey, true);
      document.onkeypress = checkKey;
      }
      }
      function checkKey(e) {
      if(document.all) {
      if(window.event.keyCode==123)
      {
      document.getElementById('KiteCMSAdminBarInner').style.display='block';
      document.getElementById('KiteCMSAdminBarInner').getElementsByTagName("A")[0].focus();
      }
      }
      else
      {
      if(e.keyCode=="123")
      {
      document.getElementById('KiteCMSAdminBarInner').style.display='block';
      document.getElementById('KiteCMSAdminBarInner').getElementsByTagName("A")[0].focus();
      }
      }
      }
    </script>
  </xsl:template>


  <xsl:template name="module">
    <h2>
      <xsl:value-of select="util:localText(./@name)"/>
    </h2>
  </xsl:template>

  <xsl:template name="method">
    <xsl:if test="($blnDraft = '1' and not(@id=1301)) or ($blnDraft = '0' and not(@id=1302 or @id=1303 or @id=1304 or @id=1305))">
      <xsl:if test="//user[@id=$user]/access=0 or //user[@id=$user]/access=../@id or //user[@id=$user]/access=@id">
        <li>
          <xsl:element name="a">
            <xsl:attribute name="href">
              <xsl:value-of select="./@url"/>
            </xsl:attribute>
            <xsl:if test="./@onclick!=''">
              <xsl:attribute name="onclick">
                <xsl:value-of select="./@onclick"/>
              </xsl:attribute>
            </xsl:if>
            <xsl:if test="./@onclickalert!=''">
              <xsl:attribute name="onclick">
                <xsl:value-of select="util:localText(./@onclickalert)"/>
              </xsl:attribute>
            </xsl:if>
            <xsl:value-of select="util:localText(./@name)"/>
          </xsl:element>
        </li>
      </xsl:if>
    </xsl:if>
  </xsl:template>


</xsl:stylesheet>

